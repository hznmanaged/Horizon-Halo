using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Horizon.Halo
{
    internal class RestClient: HttpClient
    {
        private string _authToken;

        public RestClient(string authToken)
        {
            _authToken = authToken;
            this.SetHeaders();
        }

        protected virtual void SetHeaders()
        {
            this.DefaultRequestHeaders.Accept.Clear();
            this.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //this.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            if (!string.IsNullOrWhiteSpace(_authToken))
            {
                this.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _authToken);
            }
        }

    }
}
