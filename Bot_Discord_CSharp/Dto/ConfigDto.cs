using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot_Discord_CSharp.Dto
{
    class ProfilesDto
    {
        [JsonProperty("profiles")]
        public Bot_Discord_CSharpDto Bot_Discord_CSharp { get; private set; }
}

    class Bot_Discord_CSharpDto
    {
        [JsonProperty("Bot_Discord_CSharp")]
        public EnvironmentVariablesDto EnvironmentVariables { get; private set; }
    }

    class EnvironmentVariablesDto
    {
        [JsonProperty("environmentVariables")]
        public SecretsDto Secrets { get; private set; }
    }

    class SecretsDto
    {
        [JsonProperty("TOKEN")]
        public string Token { get; private set; }
        [JsonProperty("PREFIX")]
        public string Prefix { get; private set; }
    }
}
