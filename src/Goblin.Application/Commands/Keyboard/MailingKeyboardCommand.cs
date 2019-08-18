using System.Threading.Tasks;
using Goblin.Application.Abstractions;
using Goblin.Application.Extensions;
using Goblin.Application.Results;
using Goblin.Domain.Entities;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Keyboard;

namespace Goblin.Application.Commands.Keyboard
{
    public class MailingKeyboardCommand : IKeyboardCommand
    {
        public string Trigger => "mailingKeyboard";
        
        public Task<IResult> Execute(Message msg, BotUser user)
        {
            var isSchedule = user.SubscribeInfo.IsSchedule;
            var isWeather = user.SubscribeInfo.IsWeather;
            
            var scheduleColor = isSchedule
                                        ? KeyboardButtonColor.Negative
                                        : KeyboardButtonColor.Positive;
            var weatherColor = isWeather
                                        ? KeyboardButtonColor.Negative
                                        : KeyboardButtonColor.Positive;

            var scheduleText = isSchedule
                                       ? "Отписаться от рассылки расписания"
                                       : "Подписаться на рассылку расписания";
            var weatherText = isWeather
                                       ? "Отписаться от рассылки расписания"
                                       : "Подписаться на рассылку расписания";
            
            var kb = new KeyboardBuilder(true);
            kb.AddButton(scheduleText, "mailing", scheduleColor, "schedule");
            kb.AddLine();
            kb.AddButton(weatherText, "mailing", weatherColor, "weather");
            kb.AddReturnToMenuButton();

            return Task.FromResult<IResult>(new SuccessfulResult
            {
                Message = "Выберите действие:",
                Keyboard = kb.Build()
            });
        }
    }
}