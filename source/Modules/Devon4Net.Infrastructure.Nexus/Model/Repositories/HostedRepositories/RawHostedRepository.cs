﻿using Devon4Net.Infrastructure.Nexus.Model.Repositories.Base;
using System.Text.Json.Serialization;

namespace Devon4Net.Infrastructure.Nexus.Model.Repositories.HostedRepositories
{
    public class RawHostedRepository : NexusRepositoryHosted
    {
        [JsonPropertyName("raw")]
        public Raw Raw { get; set; }
    }
}
