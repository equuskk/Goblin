﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Goblin.Vk.Models
{
    public class UsersGetReponse
    {
        [JsonProperty("response")]
        public List<VkUser> Response { get; set; }
    }
    public class VkUser
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("is_closed")]
        public bool? IsClosed { get; set; }
        [JsonProperty("can_access_closed")]
        public bool? CanAccessClosed { get; set; }
        [JsonProperty("deactivated")]
        public string Deactivated { get; set; }

        public override string ToString() => $"{FirstName} {LastName}";
    }

}