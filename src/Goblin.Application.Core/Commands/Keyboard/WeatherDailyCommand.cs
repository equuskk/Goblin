using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Goblin.Application.Core.Abstractions;
using Goblin.Application.Core.Results.Failed;
using Goblin.Domain.Abstractions;
using Newtonsoft.Json;

namespace Goblin.Application.Core.Commands.Keyboard
{
    public class WeatherDailyCommand : IKeyboardCommand
    {
        public string Trigger => "weatherDaily";

        private readonly IWeatherService _weatherService;

        public WeatherDailyCommand(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IResult> Execute<T>(IMessage msg, BotUser user) where T : BotUser
        {
            if(string.IsNullOrWhiteSpace(user.WeatherCity))
            {
                return new FailedResult("Для получения погоды установите город (нужно написать следующее - установить город Москва).");
            }

            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(msg.MessagePayload);
            var isExists = dict.TryGetValue(Trigger, out var day);
            if(!isExists)
            {
                return new FailedResult("Невозможно получить значение даты");
            }

            var isCorrectDate = DateTime.TryParse(day, out var dateTime);
            if(!isCorrectDate)
            {
                return new FailedResult("Некорректное значение даты");
            }

            var weather = await _weatherService.GetDailyWeather(user.WeatherCity, dateTime);

            return weather;
        }
    }
}