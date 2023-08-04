using Horizon.Halo.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Horizon.Halo.Controllers
{
    public class SitesController
    {
        private const string PATH = "api/Site";
        private readonly HaloAPIClient Client;
        public SitesController(HaloAPIClient client)
        {
            this.Client = client;
        }

        public async Task<IEnumerable<Site>> Search(int? clientId = null)
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
                    { "count", 900 },
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
                var data = JsonSerializer.Deserialize<SitesResponse>(content);

                return data.Sites;
            }
        }

        public async Task<Site> GetByID(int id)
        {
            var token = await Client.Authenticate("all");

            using (var restclient = new RestClient(token.AccessToken))
            {
                string targetUrl = StringTools.CombineURI(Client.URL, PATH, id.ToString());                

                var response = await restclient.GetAsync(targetUrl);

#if NET40_OR_GREATER
                response.EnsureSuccessStatusCode();
#elif NETSTANDARD
                await response.EnsureSuccessStatusCodeWithBody();
#else
#error Target Framework not supported
#endif

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<Site>(content);

                return data;
            }
        }

    }
}
