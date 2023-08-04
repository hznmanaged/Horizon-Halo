using Horizon.Halo.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Horizon.Halo.Controllers
{
    public class AssetsController
    {
        private const string PATH = "api/Asset";
        private readonly HaloAPIClient Client;
        public AssetsController(HaloAPIClient client)
        {
            this.Client = client;
        }

        public async Task<IEnumerable<Asset>> Get(string search = "", int pageSize = 30, int page = 1, string order = "ticket_id")
        {
            var token = await Client.Authenticate("all");

            using (var restclient = new RestClient(token.AccessToken))
            {
                var builder = new UriBuilder(StringTools.CombineURI(Client.URL, PATH));
                var queryParameters = new NameValueCollection()
                {
                    //{ "search", search },
                    { "pageinate", "true" },
                    { "page_size", pageSize.ToString() },
                    { "page_no", page.ToString() },
                    //{ "order", order },
                    //{ "includeinactive", "false" },
                };
                builder.Query = queryParameters.ToQueryString();

                string targetUrl = builder.Uri.ToString();

                var response = await restclient.GetAsync(targetUrl);

#if NET40_OR_GREATER
                response.EnsureSuccessStatusCode();
#elif NETSTANDARD
                await response.EnsureSuccessStatusCodeWithBody();
#else
#error Target Framework not supported
#endif

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<AssetSearchResponse>(content);

                return data.Assets;
            }
        }


        public async Task<Asset> GetByTag(string tag)
        {
            var token = await Client.Authenticate("all");

            using (var restclient = new RestClient(token.AccessToken))
            {
                string targetUrl = StringTools.CombineURI(Client.URL, PATH) + "?Search=" + tag;

                var response = await restclient.GetAsync(targetUrl);
                
#if NET40_OR_GREATER
                response.EnsureSuccessStatusCode();
#elif NETSTANDARD
                await response.EnsureSuccessStatusCodeWithBody();
#else
#error Target Framework not supported
#endif

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<AssetSearchResponse>(content);

                return data.Assets.FirstOrDefault();
            }
        }
        public async Task<Asset> GetByID(int id)
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
                var data = JsonSerializer.Deserialize<Asset>(content);

                return data;
            }
        }

    }
}
