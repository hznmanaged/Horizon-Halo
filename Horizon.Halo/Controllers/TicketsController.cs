using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Horizon.Halo.Controllers
{
    public class TicketsController
    {


        private const string PATH = "api/Tickets";
        private readonly HaloAPIClient Client;
        public TicketsController(HaloAPIClient client)
        {
            this.Client = client;
        }

        public Task<int> Post(Dictionary<string, object> ticket)
        {
            return PostInternal(new[] { ticket });
        }

        public async Task<Ticket> GetByID(int id, bool includeDetails = false)
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

                var content = await response.Content.ReadAsStringAsync();
#if NET40_OR_GREATER
                response.EnsureSuccessStatusCode();
#elif NETSTANDARD
                await response.EnsureSuccessStatusCodeWithBody();
#else
#error Target Framework not supported
#endif

                var data = JsonSerializer.Deserialize<Ticket>(content);

                return data;
            }
        }

        public Task<int> Post(IEnumerable<Dictionary<string, object>> tickets)
        {
            return PostInternal(tickets);
        }

        public Task<int> Post(Ticket ticket)
        {
            return PostInternal(new[] { ticket });
        }
        public Task<int> Post(IEnumerable<Ticket> tickets)
        {
            return PostInternal(tickets);
        }


        private async Task<int> PostInternal(IEnumerable<Object> tickets)
        {
            var token = await Client.Authenticate(Scopes.EDIT_TICKETS);
            using (var restclient = new RestClient(token.AccessToken))
            {
                string targetUrl = StringTools.CombineURI(Client.URL, PATH);



                var requestJson = JsonSerializer.Serialize(tickets);

                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await restclient.PostAsync(targetUrl, requestContent);

#if NET40_OR_GREATER
                response.EnsureSuccessStatusCode();
#elif NETSTANDARD
                await response.EnsureSuccessStatusCodeWithBody();
#else
#error Target Framework not supported
#endif

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<Ticket>(content);

                return data.ID.Value;
            }
        }
    }
}
