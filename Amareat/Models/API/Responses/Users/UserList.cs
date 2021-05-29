using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.Users
{
    public class UserList
    {
        [JsonProperty("data")]
        public List<User> Data { get; set; }
    }
}
