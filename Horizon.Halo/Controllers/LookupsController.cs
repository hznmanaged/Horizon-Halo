using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Horizon.Halo.Controllers
{
    public class LookupsController
    {
        private const string PATH = "api/Lookup";
        private readonly HaloAPIClient Client;
        public LookupsController(HaloAPIClient client)
        {
            this.Client = client;
        }

        public async Task<IEnumerable<LookupValue>> GetByID(int id)
        {
            var token = await Client.Authenticate("all");

            using (var restclient = new RestClient(token.AccessToken))
            {
                string targetUrl = StringTools.CombineURI(Client.URL, PATH);


                var queryArgs = new Dictionary<string, object>()
                {
                    { "showall", true},
                    { "lookupid", id},
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
                var data = JsonSerializer.Deserialize<IEnumerable<LookupValue>>(content);

                return data;
            }
        }

    }
}
