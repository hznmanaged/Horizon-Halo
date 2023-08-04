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
    public class AppointmentsController
    {
        private const string PATH = "api/Appointment";
        private readonly HaloAPIClient Client;
        public AppointmentsController(HaloAPIClient client)
        {
            this.Client = client;
        }

        public async Task<IEnumerable<Appointment>> GetAppointments(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var token = await Client.Authenticate("all");

            using (var restclient = new RestClient(token.AccessToken))
            {
                var urlBuilder = new UriBuilder(StringTools.CombineURI(Client.URL, PATH));
                var queryParameters = new NameValueCollection()
                {
                    { "start_date", startDate.ToUniversalTime().ToString() },
                    { "end_date", endDate.ToUniversalTime().ToString() },
                };
                urlBuilder.Query = queryParameters.ToQueryString();

                string targetUrl = urlBuilder.Uri.ToString();

                var response = await restclient.GetAsync(targetUrl);
                var content = await response.Content.ReadAsStringAsync();

#if NET40_OR_GREATER
                response.EnsureSuccessStatusCode();
#elif NETSTANDARD
                await response.EnsureSuccessStatusCodeWithBody();
#else
#error Target Framework not supported
#endif

                var data = JsonSerializer.Deserialize<IEnumerable<Appointment>>(content);

                return data;
            }
        }
      

    }
}
