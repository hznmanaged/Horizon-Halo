using Horizon.Halo.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Horizon.Halo.Controllers
{
    public class BudgetTypesController
    {
        private const string PATH = "api/BudgetType";
        private readonly HaloAPIClient Client;
        public BudgetTypesController(HaloAPIClient client)
        {
            this.Client = client;
        }

        public async Task<IEnumerable<BudgetType>> GetAll()
        {
            var token = await Client.Authenticate("all");

            using (var restclient = new RestClient(token.AccessToken))
            {
                string targetUrl = StringTools.CombineURI(Client.URL, PATH);


                var response = await restclient.GetAsync(targetUrl);

#if NET40_OR_GREATER
                response.EnsureSuccessStatusCode();
#elif NETSTANDARD
                await response.EnsureSuccessStatusCodeWithBody();
#else
#error Target Framework not supported
#endif

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<IEnumerable<BudgetType>>(content);

                return data;
            }
        }

    }
}
