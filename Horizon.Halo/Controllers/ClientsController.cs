using Horizon.Halo.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Horizon.Halo.Controllers
{
    public class ClientsController
    {
        private const string PATH = "api/Client";
        private readonly HaloAPIClient Client;
        public ClientsController(HaloAPIClient client)
        {
            this.Client = client;
        }

        public async Task<IEnumerable<Client>> Search()
        {
            var token = await Client.Authenticate("all");

            using (var restclient = new RestClient(token.AccessToken))
            {
                string targetUrl = StringTools.CombineURI(Client.URL, PATH);


                var queryArgs = new Dictionary<string, object>()
                {
                    { "pageinate", false},
                    { "includeinactive", false},
                    { "includeactive", true },
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
                var data = JsonSerializer.Deserialize<ClientsResponse>(content);

                return data.Clients;
            }
        }

    }
}
