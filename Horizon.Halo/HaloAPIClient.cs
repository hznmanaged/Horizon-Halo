using Horizon.Halo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Horizon.Halo
{
    public class HaloAPIClient: IDisposable
    {
        public readonly AssetsController Assets;
        public readonly AppointmentsController Appointments;
        public readonly BudgetTypesController BudgetTypes;
        public readonly ClientsController Clients;
        public readonly LookupsController Lookups;
        public readonly ProjectsController Projects;
        public readonly SitesController Sites;
        public readonly TeamsController Teams;
        public readonly TicketsController Tickets;
        public readonly TicketTypesController TicketTypes;
        public readonly UsersController Users;

        internal readonly string ClientID;
        internal readonly string ClientSecret;
        internal readonly string URL;


        public HaloAPIClient(string url, string clientID, string clientSecret)
        {
            this.URL = url;            
            this.ClientID = clientID;
            this.ClientSecret = clientSecret;
            this.Teams = new TeamsController(this);
            this.Tickets = new TicketsController(this);
            this.TicketTypes = new TicketTypesController(this);
            this.Clients = new ClientsController(this);
            this.Sites = new SitesController(this);
            this.Lookups = new LookupsController(this);
            this.Assets = new AssetsController(this);
            this.Appointments = new AppointmentsController(this);
            this.Users = new UsersController(this);
            this.Projects = new ProjectsController(this);
            this.BudgetTypes = new BudgetTypesController(this);
        }

        /// <summary>
        /// This function can only retrieve lookups for known fields.
        /// </summary>
        /// <param name="field_name"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LookupValue>> GetLookupValuesForField(string field_name)
        {
            switch(field_name)
            {
                case "team_id":
                    return (await this.Teams.Search()).Select(e=>new LookupValue() { ID  = e.ID, Name = e.Name});
                case "site_id":
                    return (await this.Sites.Search()).Select(e => new LookupValue() { ID = e.ID, Name = $"{e.Name} ({e.ClientName})" });
                case "client_id":
                    return (await this.Clients.Search()).Select(e => new LookupValue() { ID = e.ID, Name = e.Name });
                case "tickettype_id":
                    return (await this.TicketTypes.Search()).Select(e => new LookupValue() { ID = e.ID, Name = e.Name });
                case "impact":
                    return (await this.Lookups.GetByID(id: (int)Lookup.Impact));
                case "urgency":
                    return (await this.Lookups.GetByID(id: (int)Lookup.Urgency));
                default:
                    throw new NotSupportedException("Field not recognized: " + field_name);
            }
        }

        public async Task<AuthenticationResponse> Authenticate(params string[] scopes)
        {
            using (var httpClient = new HttpClient())
            {


                string targetUrl = this.URL + "auth/token";

                var uriBuilder = new UriBuilder(targetUrl);

                var args = new Dictionary<string, string>()
            {
                { "grant_type", "client_credentials" },
                { "client_id", ClientID },
                { "client_secret", ClientSecret },
                { "scope", String.Join(" ", scopes) },
            };

                var uri = uriBuilder.ToString();

                var requestContent = new StringContent(args.ToQueryString(), Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = await httpClient.PostAsync(uri, requestContent);

#if NET40_OR_GREATER
                response.EnsureSuccessStatusCode();
#elif NETSTANDARD
                await response.EnsureSuccessStatusCodeWithBody();
#else
#error Target Framework not supported
#endif

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<AuthenticationResponse>(content);

                return data;
            }

        }

        public void Dispose()
        {
            // Nothing to do
        }
    }
}
