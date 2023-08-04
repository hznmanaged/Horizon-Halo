using Horizon.Halo.Models;
using Horizon.Halo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Horizon.Halo.Controllers
{
    public class ProjectsController
    {


        private const string PATH = "api/Projects";
        private readonly HaloAPIClient Client;
        public ProjectsController(HaloAPIClient client)
        {
            this.Client = client;
        }

        private Task<int> Post(Dictionary<string, object> projects)
        {
            return Post(new[] { projects });
        }
        private async Task<int> Post(IEnumerable<Dictionary<string, object>> projects)
        {
            return Tools.GetIDField(await PostInternal(projects)).Value;
        }

        private async Task<int> Post(ProjectTicket project)
        {
            return await Post(new[] { project });
        }
        private async Task<int> Post(IEnumerable<ProjectTicket> projects)
        {
            return Tools.GetIDField(await PostInternal(projects)).Value;
        }


        public async Task<IEnumerable<ProjectTicket>> Search(int? clientId = null)
        {

            var content = await SearchJSON(clientId: clientId);
            var data = JsonSerializer.Deserialize<ProjectSearchResponse>(content);

            return data.Tickets;
        }

        public async Task<string> SearchJSON(int? clientId = null, int? parentId = null,
            string searchQuery = "")
        {
            var token = await Client.Authenticate(Scopes.READ_PROJECTS, Scopes.READ_TICKETS);

            using (var restclient = new RestClient(token.AccessToken))
            {
                string targetUrl = StringTools.CombineURI(Client.URL, PATH);


                var queryArgs = new Dictionary<string, object>()
                {                    
                };
                if (clientId.HasValue)
                {
                    queryArgs["client_id"] = clientId.Value;
                }
                if (parentId.HasValue)
                {
                    queryArgs["parent_id"] = parentId.Value;
                }
                if(!String.IsNullOrWhiteSpace(searchQuery))
                {
                    queryArgs["search"] = searchQuery;
                }

                var uriBuilder = new UriBuilder(targetUrl);

                uriBuilder.Query = queryArgs.ToQueryString();

                var response = await restclient.GetAsync(uriBuilder.ToString());

#if NET40_OR_GREATER
                response.EnsureSuccessStatusCode();
#elif NETSTANDARD
                await response.EnsureSuccessStatusCodeWithBody();
#else
#error Target Framework not supported
#endif

                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
        }

        public async Task<Ticket> GetByID(int id, bool includeDetails = false)
        {
            var content = await GetJSONByID(id: id, includeDetails: includeDetails);

            var data = JsonSerializer.Deserialize<ProjectTicket>(content);

            return data;
        }

        public async Task<string> GetJSONByID(int id, bool includeDetails = false)
        {
            var token = await Client.Authenticate(Scopes.READ_PROJECTS, Scopes.READ_TICKETS);

            using (var restclient = new RestClient(token.AccessToken))
            {
                string targetUrl = StringTools.CombineURI(Client.URL, PATH, id.ToString());

                var queryArgs = new Dictionary<string, object>()
                {
                    { "includedetails", includeDetails },
                };

                var uriBuilder = new UriBuilder(targetUrl);

                uriBuilder.Query = queryArgs.ToQueryString();

                var response = await restclient.GetAsync(uriBuilder.ToString());

                var content = await response.Content.ReadAsStringAsync();
#if NET40_OR_GREATER
                response.EnsureSuccessStatusCode();
#elif NETSTANDARD
                await response.EnsureSuccessStatusCodeWithBody();
#else
#error Target Framework not supported
#endif

                return content;
            }
        }

        





        private async Task<string> PostInternal(IEnumerable<Object> projects)
        {
            var token = await Client.Authenticate(Scopes.EDIT_PROJECTS, Scopes.EDIT_TICKETS);
            using (var restclient = new RestClient(token.AccessToken))
            {
                string targetUrl = StringTools.CombineURI(Client.URL, PATH);



                var requestJson = JsonSerializer.Serialize(projects);

                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await restclient.PostAsync(targetUrl, requestContent);

                var content = await response.Content.ReadAsStringAsync();
                try
                {
#if NET40_OR_GREATER
                response.EnsureSuccessStatusCode();
#elif NETSTANDARD
                    await response.EnsureSuccessStatusCodeWithBody();
#else
#error Target Framework not supported
#endif

                } catch(Exception ex)
                {
                    throw;
                }

                return content;
            }
        }


        public async Task<int> Create(string summary, string details, int clientId)
        {
            var item = new ProjectTicket()
            {
                DateOccured = DateTimeOffset.Now,
                Details = details,
                Summary = summary,
                ClientID = clientId,
                TicketTypeID = 5
            };

            return await Post(item);
        }

        public async Task<int> CreateTask(string summary, string details, int clientId, int parentId,
            DateTime? startDate = null,
            DateTime? targetDate = null,
            int? budgetTypeId = null,
            decimal? budgetTime = null)
        {
            var item = new ProjectTicket()
            {
                DateOccured = DateTimeOffset.Now,
                Details = details,
                Summary = summary,
                ClientID = clientId,
                TicketTypeID = 41,
                ParentID = parentId,
                StartDate = startDate,
                StartTime = startDate?.TimeOfDay,
                TargetDate = targetDate,
                TargetTime = targetDate?.TimeOfDay,
                BudgetTypeID = budgetTypeId,
                ProjectTimeBudget = budgetTime
            };

            return await Post(item);
        }


        /// <summary>
        /// WARNING: Will erase any existing milestones
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="milestones"></param>
        /// <returns></returns>
        public async Task CreateMilestones(int ticketId, IEnumerable<ProjectMilestone> milestones)
        {

            var lastSequence = 0;
            foreach(var milestone in milestones)
            {
                var ticketValues = new Dictionary<string, object>
                {
                    { "id", ticketId },
                };
                var milestonesData = new List<object>();


                var existingTicketDataJson = await this.GetJSONByID(id: ticketId);
                var existingMilestoneData = JsonSerializer.Deserialize<RawMilestoneData>(existingTicketDataJson);

                if(string.IsNullOrWhiteSpace(milestone.Name))
                {
                    throw new Exception("Milestones must all have names");
                }
                var sequence = lastSequence + 1;
                if(milestone.Sequence.HasValue)
                {
                    sequence = milestone.Sequence.Value;
                    if(sequence > lastSequence)
                    {
                        lastSequence = sequence;
                    }
                } else
                {
                    lastSequence = sequence;
                }
                var milestoneObject = new Dictionary<string, object>()
                {
                    { "name", milestone.Name },
                    { "sequence", sequence },
                    //TicketID = ticketId,
                    //TicketsList = tickets,
                };
                if (milestone.TicketsList.Any())
                {
                    milestoneObject["tickets_list"] = milestone.TicketsList.Select(e => new { id = e.ID });
                }
                if (milestone.StartDate.HasValue)
                {
                    milestoneObject["start_date"] = milestone.StartDate.Value;
                }
                if (milestone.TargetDate.HasValue)
                {
                    milestoneObject["target_date"] = milestone.TargetDate.Value;
                }
                milestonesData.Add(milestoneObject);

                milestonesData.AddRange(existingMilestoneData.Milestones);

                ticketValues.Add("milestones", milestonesData);

                //ticketValues.Add("attachments", Array.Empty<object>());
                ticketValues.Add("utcoffset", 300);

                await this.Post(ticketValues);
            }



        }

        /// <summary>
        /// WARNING: Will erase any existing budgets
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="budget"></param>
        /// <returns></returns>
        public async Task CreateBudgets(int ticketId, IEnumerable<ProjectBudget> budgets)
        {
            var ticketValues = new Dictionary<string, object>
            {
                { "id", ticketId },
            };

            var data = new List<object>();
            foreach (var budget in budgets)
            {
                var dataObject= new Dictionary<string, object>()
                {
                    { "budgettype_id", budget.BudgetTypeID },
                    { "hours", budget.Hours},
                    { "rate", budget.Rate},
                };
                data.Add(dataObject);
            }



            ticketValues.Add("budgets", data);

            //ticketValues.Add("attachments", Array.Empty<object>());

            await this.Post(ticketValues);
        }
    }
}
