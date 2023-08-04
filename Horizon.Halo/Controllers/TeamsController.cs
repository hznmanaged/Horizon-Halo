using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Horizon.Halo.Controllers
{
    public class TeamsController
    {
        private const string PATH = "api/Team";
        private readonly HaloAPIClient Client;
        public TeamsController(HaloAPIClient client)
        {
            this.Client = client;
        }

        public async Task<IEnumerable<Team>> Search()
        {
            var token = await Client.Authenticate("all");

            using (var restclient = new RestClient(token.AccessToken))
            {
                string targetUrl = StringTools.CombineURI(Client.URL,PATH);



                var response = await restclient.GetAsync(targetUrl);

#if NET40_OR_GREATER
                response.EnsureSuccessStatusCode();
#elif NETSTANDARD
                await response.EnsureSuccessStatusCodeWithBody();
#else
#error Target Framework not supported
#endif

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<Team>>(content);

                return data;
            }
        }
    }
}
