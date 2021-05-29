using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.History
{
    public class HistoryList
    {
        [JsonProperty("data")]
        public List<History> Data { get; set; }
    }
}
