using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Horizon.Halo.Controllers
{
    public class TicketTypesController
    {
        private const string PATH = "api/TicketType";
        private readonly HaloAPIClient Client;
        public TicketTypesController(HaloAPIClient client)
        {
            this.Client = client;
        }

        public async Task<IEnumerable<TicketType>> Search(bool showInactive = false, int? clientId = null)
        {
            var token = await Client.Authenticate(Scopes.READ_TICKETS);

            using (var restclient = new RestClient(token.AccessToken))
            {
                string targetUrl = StringTools.CombineURI(Client.URL,PATH);


                var queryArgs = new Dictionary<string, object>()
                {
                    { "showinactive", showInactive },
                };
                if(clientId.HasValue)
                {
                    queryArgs["client_id"] = clientId.Value;
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
                var data = JsonSerializer.Deserialize<List<TicketType>>(content);

                return data;
            }
        }

        public async Task<TicketType> GetByID(int id, bool includeDetails = false)
        {
            var token = await Client.Authenticate(Scopes.READ_TICKETS);

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

#if NET40_OR_GREATER
                response.EnsureSuccessStatusCode();
#elif NETSTANDARD
                await response.EnsureSuccessStatusCodeWithBody();
#else
#error Target Framework not supported
#endif

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<TicketType>(content);

                return data;
            }
        }
    }
}
