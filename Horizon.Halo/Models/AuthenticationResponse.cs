using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo
{
    public class AuthenticationResponse
    {

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
