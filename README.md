# Horizon-Halo PowerShell Module
This module is a collection of tools created to interact with the Halo PSA application.

# Requirements
- PowerShell 7.0 or greater
- .NET 6 Runtime or SDK or greater

# Setup
To install, download the latest release, extract it somewhere, and then import it with this command:

	Import-Module .\Horizon-Halo.dll

# Commands
## Import-MicrosoftProject
This command takes a Microsoft Project file and creates a Halo project from it. 

### Preparing the file
The file must be prepare in a particular manner. Structure the file with these guidelines:

- The project title itself will become the project ticket in Halo. The title of the project can be set in MS Project in File > Info > Project Information > Advanced Properties.
- Use the project start date to set the starting date used for schedule the ticket. This can be set in Project Information.
- All Microsoft Project tasks will become task tickets in Halo, as child tasks when contained within a parent task.
- If the task is marked a milestone, it will not be created as a task ticket in Halo. It will instead be created as a milestone on whatever task is its parent, and any non-milestone tasks underneath it will be assigned to the milestone.
    - A task in Halo can only have one milestone assigned to it. Nesting milestones in Microsoft Project will not result in tasks assigned to multiple milestones.
    - Halo does not support nested milestones. Nested milestones in the project file will just result in sibling milestones on the parent task.
    - A task is marked as a milestone via the Milestone column.
    - Milestones with no tickets will not be created.
- To set a budget for a task, set the Budget field to the name of the budget type it should be assigned to. The import will add up all of the budgeted time for all tasks with the same budget type, and create that budget on the project.
    - Budget types can not be auto-created, and must be created by an admin in Halo before import.
    - Only bottom-level tasks contribute to budget times.
    - The "Budget" field is not built in, and must be added as a custom field.
- The "Notes" field will be used as the project's/tasks' description.
- Start and Finish dates in the Microsoft Project file directly translate to the Halo project/task/milestone start and complete dates.
- Predecessors will produce a note in the Halo ticket description pointing to the ticket associated with the predecessor.
    - Predecessors must be above the tickets that they are set to.
- Resources assigned to a task will produce a note in the Halo ticket description listing those resources.

Once the file is prepared, save it as an XML file.

### Command usage
This is a sample of the command:

	Import-MicrosoftProject -Path ".\project.xml" -TargetClientID 1 -URL "https://halo.contoso.com/" -APIClientID "####" -APIClientSecret "####"

The command produces no output unless there is an error.

All parameters are required. The table below provides more information.

| Parameter | Description |
|---|---|
| Path | The path to the Project XML file to import. |
| TargetClientID | The Halo ID of the client to import the project for. The ID can be found by navigating to the client screen in Halo, selecting the desired client, and then checking the URL. The URL will contain clientId=##, where ## is the ID of the client. |
| URL | The root URL of the Halo instance to import into. |
| APIClientID | NOT the same as the TargetClientID. This is an API client ID generated in Halo. |
| APIClientSecret | The API client secret generated in Halo. |