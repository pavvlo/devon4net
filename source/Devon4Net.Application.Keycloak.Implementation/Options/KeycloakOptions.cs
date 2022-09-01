namespace Devon4Net.Application.Keycloak.Implementation.Options
{
    public class KeycloakOptions
    {
        public string Realm { get; set; }

        public string Url { get; set; }

        public string ClientId { get; set; }

        public string Password { get; set; }

        public string Audience { get { return ClientId; } }

        public string Authority { get { return Url + "realms/" + Realm; } }
    }
}
