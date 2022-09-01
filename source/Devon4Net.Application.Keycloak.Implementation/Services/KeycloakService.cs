using Devon4Net.Application.Keycloak.Implementation.Model;
using Devon4Net.Application.Keycloak.Implementation.Options;
using Newtonsoft.Json;

namespace Devon4Net.Application.Keycloak.Implementation.Services
{
    public class KeycloakService
    {
        private KeycloakOptions _options;

        public KeycloakService(KeycloakOptions options)
        {
            _options = options;
        }

        public async Task<KeyCloakToken> GetToken(string username, string password)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_options.Url);

                var parameters = new List<KeyValuePair<string, string>> {
                    new ("username", username),
                    new ("password", password),
                    new ("client_id", _options.ClientId),
                    new ("grant_type", _options.Password)
                };

                using (var content = new FormUrlEncodedContent(parameters))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    var response = await httpClient.PostAsync($"realms/{_options.Realm}/protocol/openid-connect/token", content);
                    KeyCloakToken token = JsonConvert.DeserializeObject<KeyCloakToken>(await response.Content.ReadAsStringAsync());
                    return token;
                }
            }
        }
    }
}