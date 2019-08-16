﻿using Newtonsoft.Json;

namespace Goblin.OpenWeatherMap.Models.Daily
{
    public class DailyWeather
    {
        [JsonProperty("city")]
        public City City { get; set; }

        [JsonProperty("cod")]
        public string Cod { get; set; } //INT

        [JsonProperty("message")]
        public double Message { get; set; }

        [JsonProperty("cnt")]
        public long Cnt { get; set; }

        [JsonProperty("list")]
        public DailyWeatherListItem[] List { get; set; }
    }
}