using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Base
{
    public class BaseApiErrorResponse
    {
        [JsonProperty("codeReason")]
        public string CodeReason { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
