using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot_Discord_CSharp.Dto
{
    class ConfigDto
    {
        [JsonProperty("TOKEN")]
        public string Token { get; private set; }
        [JsonProperty("PREFIX")]
        public string Prefix { get; private set; }
    }
}
