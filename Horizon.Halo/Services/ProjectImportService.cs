using Horizon.Halo;
using Horizon.Halo.Models;
using Horizon.Schemas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Horizon.Halo.Services
{
    
    public class ProjectImportService
    {

        public Horizon.Schemas.Project Parse(Stream input)
        {
            var serializer = new XmlSerializer(typeof(Horizon.Schemas.Project));
            return (Horizon.Schemas.Project)serializer.Deserialize(input);
        }

        public void Validate(Horizon.Schemas.Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            if (project.Tasks?.IsEmpty() ?? false)
            {
                throw new Exception("No tasks found on project");
            }
            for (var i = 0; i < project.Tasks.Count; i++)
            {
                var task = project.Tasks[i];

                var parent = GetParent(project.Tasks, task);

                if (string.IsNullOrWhiteSpace(task.Name))
                {
                    throw new Exception($"Task ID {task.ID} has no name. All tasks must have names.");
                }

                if (!task.Milestone)
                {
                    if (string.IsNullOrWhiteSpace(task.Notes))
                    {
                        //throw new Exception($"Task {task.Name} has no notes. All tasks must have notes.");
                    }
                }

                if (task.PredecessorLink != null && task.PredecessorLink.Any())
                {
                    var predecessor = GetPredecessor(project.Tasks, task);
                    if (predecessor == null) {
                        throw new Exception($"Task {task.Name} specifies a predecessor that is not present.");
                    }
                }


                if (task.Milestone)
                {

                    while (parent?.Milestone ?? false)
                    {
                        //throw new Exception($"The parent of milestone task {task.Name} is itself a milestone. Milestones must not be directly nested.");
                        parent = GetParent(project.Tasks, parent);
                    }
                    if (parent == null)
                    {
                        throw new Exception($"Task {task.Name} does not have a parent. Milestones must not be root tasks.");
                    }
                }

                if(task.Start>task.Finish)
                {
                    throw new Exception($"Task {task.Name}'s start date/time is before the finidh date/time.");
                }
            }
        }

        private bool HasChildren(List<ProjectTask> tasks, ProjectTask parentTask) {
            int parentIndex = tasks.IndexOf(parentTask);
            if (parentIndex == -1)
            {
                throw new Exception("Could not find parentTask in tasks");
            }

            var level = parentTask.OutlineLevelInteger;

            if((parentIndex+1) == tasks.Count)
            {
                // Last item in list, no children
                return false;
            }

            var nextItem = tasks[parentIndex+1];

            if(nextItem.OutlineLevelInteger > level)
            {
                // Next item has a greater level, indicating it is a child
                return true;
            }

            // Subsequent item has same or less level, indicating no children
            return false;
        }

        private ProjectTask GetPredecessor(List<ProjectTask> tasks, ProjectTask task)
            => tasks.FirstOrDefault(e => e.UID == task.PredecessorLink.First().PredecessorUID);

        private ProjectTask GetParent(List<ProjectTask> tasks, ProjectTask childTask)
        {
            if (childTask.OutlineLevelInteger.GetValueOrDefault(0) == 0)
            {
                return null;
            }

            int childIndex = tasks.IndexOf(childTask);
            if(childIndex==-1)
            {
                throw new Exception("Could not find task in tasks");
            }

            var level = childTask.OutlineLevelInteger;

            for(int i = childIndex; i>=0; i--)
            {
                if (tasks[i].OutlineLevelInteger == childTask.OutlineLevelInteger-1)
                {
                    return tasks[i];
                }
            }
            return null;
        }

        private readonly Regex BudgetTimeFormat = new Regex(@"^PT(\d+)H(\d+)M(\d+)S$");

        private string GetBudgetName(
            ProjectExtendedAttribute budgetAttribute,
            ProjectTask task)
        {
            if (budgetAttribute == null)
                return String.Empty;

            return task.ExtendedAttribute?
                        .FirstOrDefault(e => e.FieldID == budgetAttribute.FieldID)?.Value;
        }


        private decimal ParseDuration(string input)
        {
            if (!BudgetTimeFormat.IsMatch(input))
            {
                throw new Exception($"Unknown duration value: {input}");
            }
            var m = BudgetTimeFormat.Match(input);
            //PT0H30M0S
            return ((Convert.ToDecimal(m.Groups[1].Value) * 60 * 60) +
                (Convert.ToDecimal(m.Groups[2].Value) * 60) +
                Convert.ToDecimal(m.Groups[3].Value)) / (60 * 60);
        }
        public async Task<int> Import(Stream input, HaloAPIClient halo, int clientId)
        {
            var project = Parse(input);
            Validate(project);
            int? projectId = null;

            var milestones = new Dictionary<string,ProjectMilestone>();

            var ticketMapping = new Dictionary<string, int>();
            var budgets = new Dictionary<string,decimal>(StringComparer.OrdinalIgnoreCase);

            var budgetTypes = await halo.BudgetTypes.GetAll();

            // Pre-calculate budgets
            var budgetAttribute = project.ExtendedAttributes.FirstOrDefault(e => e.Alias == "Budget");
            if (budgetAttribute != null) {
                for (var i = 0; i < project.Tasks.Count; i++)
                {
                    var task = project.Tasks[i];
                    var budget = GetBudgetName(budgetAttribute, task);

                    if(!String.IsNullOrWhiteSpace(budget))
                    {
                        budget = budget.Trim();
                        if (!budgets.ContainsKey(budget))
                        {
                            if (!budgetTypes.Any(e => e.Name.ToLower() == budget.ToLower()))
                            {
                                throw new Exception($"Budget type {budget} is not recognized");
                            }
                            budgets[budget] = 0;
                        }

                        if (!HasChildren(project.Tasks, task))
                        {
                            budgets[budget] += ParseDuration(task.Duration);
                        }
                    }
                }
            }
            

            for (var i = 0; i <project.Tasks.Count;i++) 
            {
                var task = project.Tasks[i];
                var level = task.OutlineLevelInteger;
                var taskName = task.Name;


                var parent = GetParent(project.Tasks, task);

                if(task.Milestone)
                {
                    while (parent?.Milestone ?? false)
                    {
                        //throw new Exception($"The parent of milestone task {task.Name} is itself a milestone. Milestones must not be directly nested.");
                        parent = GetParent(project.Tasks, parent);
                    }
                    if (parent == null)
                    {
                        throw new Exception($"Task {taskName} does not have a parent. Milestones must not be root tasks.");
                    }
                    milestones[task.UID] = new ProjectMilestone()
                    {
                        TicketID = ticketMapping[parent.UID],
                        Name = task.Name,
                        StartDate = task.StartSpecified ? task.Start : null,
                        TargetDate = (task.FinishSpecified && task.Start < task.Finish) ? task.Finish : null
                    };
                }
                else
                {
                    ProjectTask milestoneTask = null;
                    if(parent?.Milestone??false)
                    {
                        // Parent is a milestone, get parent
                        milestoneTask = parent;
                        parent = GetParent(project.Tasks, milestoneTask);

                        while(parent.Milestone)
                        {
                            //throw new Exception($"The parent of milestone task {milestoneTask.Name} is itself a milestone. Milestones must not be directly nested.");
                            parent = GetParent(project.Tasks, parent);
                        }
                        if (parent == null)
                        {
                            throw new Exception($"Task {milestoneTask.Name} does not have a parent. Milestones must not be root tasks.");
                        }

                    }

                    int ticketId;
                    var details = new StringBuilder();


                    if(task.PredecessorLink != null && task.PredecessorLink.Any())
                    {
                        var predecessor = GetPredecessor(project.Tasks, task);
                        //var predecessorTicketId = ticketMapping[predecessor.UID];
                        details.AppendLine($"This task is preceded by \"{predecessor.Name}\"");
                        details.AppendLine();
                    }

                    var assignments = project.Assignments.Where(e => e.TaskUID == task.UID).ToArray();
                    if (assignments.Any())
                    {
                        var assignedResourceIds = assignments.Select(e => e.ResourceUID).ToArray();
                        var resourceNames = project.Resources.Where(e => assignedResourceIds.Contains(e.UID)).Select(e => e.Name);

                        if (resourceNames.Any())
                        {
                            details.AppendLine($"Resources: {String.Join(", ", resourceNames)}");
                            details.AppendLine();
                        }
                    }

                    if(String.IsNullOrWhiteSpace(task.Notes))
                    {
                        details.AppendLine(task.Name);
                    }
                    else
                    {
                        details.AppendLine(task.Notes);
                    }

                    if (String.IsNullOrWhiteSpace(details.ToString())) {
                        details.AppendLine("No notes found");
                    }

                    

                    if (parent == null)
                    {
                        // Root level element, create project from this
                        ticketId = await halo.Projects.Create(summary: taskName, details: details.ToString(), clientId: clientId);

                        projectId = ticketId;
                        // Now we create all the budgets
                        if (budgets.Any()) { 
                            await halo.Projects.CreateBudgets(ticketId: ticketId,
                                budgets: budgets.Select(b=> new ProjectBudget()
                                {
                                    Hours = b.Value,
                                    BudgetTypeID = budgetTypes.First(e => e.Name.ToLower() == b.Key.ToLower()).ID.Value,
                                    Rate = budgetTypes.First(e => e.Name.ToLower() == b.Key.ToLower()).DefaultRate,
                                }));
                        }
                    }
                    else
                    {
                        var parentTicketId = ticketMapping[parent.UID];

                        var budget = GetBudgetName(budgetAttribute, task);
                        int? budgetTypeId = null;
                        decimal? budgetTime = null;
                        if(!String.IsNullOrWhiteSpace(budget))
                        {
                            budgetTypeId = budgetTypes.First(e => e.Name.ToLower() == budget.ToLower()).ID;
                            budgetTime = ParseDuration(task.Duration);
                        }

                        // Child element, create task.
                        ticketId = await halo.Projects.CreateTask(summary: taskName, 
                            details: details.ToString(), clientId: clientId, parentTicketId,
                            startDate: task.StartSpecified ? task.Start : null,
                            targetDate: task.FinishSpecified ? task.Finish : null,
                            budgetTypeId: budgetTypeId,
                            budgetTime: budgetTime);
                    }

                    ticketMapping[task.UID] = ticketId;

                    if(milestoneTask!=null)
                    {
                        var milestone = milestones[milestoneTask.UID];
                        if (milestone.TicketID==ticketId)
                        {
                            Console.WriteLine();
                        }
                        // Add ticket to milestone list
                        if(!milestone.TicketsList.Any(e=>e.ID== ticketId))
                        {
                            milestone.TicketsList.Add(new IDObject() { ID = ticketId });
                        }
                        else
                        {
                            Console.WriteLine();
                        }
                    }

                }
            }


            var ticketWithMilestones = milestones.Values.Select(e => e.TicketID).Distinct();

            foreach(var ticketWithMilestone in ticketWithMilestones)
            {
                var ticketMilestones = milestones.Values
                    .Where(e => e.TicketID == ticketWithMilestone
                                    && e.TicketsList.Any()).ToArray();
                await halo.Projects.CreateMilestones(
                    ticketId: ticketWithMilestone,
                    milestones: ticketMilestones);
            }

            if(!projectId.HasValue)
            {
                throw new Exception("No project ID found");
            }

            return projectId.Value;
        }
    }
}
