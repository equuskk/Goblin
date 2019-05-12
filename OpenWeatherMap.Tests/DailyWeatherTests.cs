using System;
using Xunit;

namespace OpenWeatherMap.Tests
{
    public class DailyWeatherTests : TestBase
    {
        private const string City = "�����";

        [Fact(DisplayName = "Get daily weather")]
        public async void GetWeather()
        {
            var result = await GetWeatherInfo().GetDailyWeather(City, DateTime.Today);
            Assert.Equal("Arkhangelsk", result.City.Name);
            Assert.Equal("200", result.Cod);
            Assert.NotEmpty(result.List);
        }

        [Fact(DisplayName = "Get daily weather as string")]
        public async void GetString()
        {
            var result = await GetWeatherInfo().GetDailyWeatherString(City, DateTime.Today);
            Assert.Contains("�����������", result);
            Assert.Contains("�������", result);
        }

        private WeatherInfo GetWeatherInfo()
        {
            var client = GetDailyHttpClient();
            return new WeatherInfo("test_token", client);
        }
    }
}
