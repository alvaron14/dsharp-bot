using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot_Discord_CSharp.Dto
{
    class ConfigDto
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("Prefix")]
        public string Prefix { get; private set; }
    }
}
