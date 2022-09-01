using Newtonsoft.Json;

namespace Devon4Net.Application.Keycloak.Implementation.Model
{
    public class KeyCloakToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int TokenExpirationTime { get; set; }

        [JsonProperty("refresh_expires_in")]
        public int RefreshTokenExpirationTime { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("not-before-policy")]
        public int NotBeforePolicy { get; set; }

        [JsonProperty("session_state")]
        public string SessionState { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
