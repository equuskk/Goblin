using System;
using System.Globalization;
using System.Threading.Tasks;
using Goblin.Application.Abstractions;
using Goblin.Application.Extensions;
using Goblin.Application.Results;
using Goblin.DataAccess;
using Goblin.Domain.Entities;
using VkNet.Model;

namespace Goblin.Application.Commands.Text
{
    public class AddRemindCommand : ITextCommand
    {
        private readonly BotDbContext _db;
        public bool IsAdminCommand => false;
        public string[] Aliases => new[] { "напомни" };

        public AddRemindCommand(BotDbContext db)
        {
            _db = db;
        }

        public async Task<IResult> Execute(Message msg, BotUser user)
        {
            var param = string.Join(' ', msg.GetCommandParameters());
            var all = param.Split(' ', 3);

            if(all.Length != 3)
            {
                return new FailedResult("Укажите дату, время и текст напоминания (11.11.2011 11:11 текст)");
            }

            if(all[0] == "завтра")
            {
                var d = DateTime.Now.AddDays(1);
                all[0] = $"{d.Day}.{d.Month}.{d.Year}";
            }
            else if(all[0] == "сегодня")
            {
                var d = DateTime.Now;
                all[0] = $"{d.Day}.{d.Month}.{d.Year}";
            }

            var isCorrectTime = ParseTime(all[0], all[1], out var dateTime);
            if(!isCorrectTime)
            {
                return new FailedResult("Некорректная дата или время");
            }

            if(dateTime <= DateTime.Now)
            {
                return new FailedResult("Дата напоминания меньше текущей");
            }

            _db.Reminds.Add(new Remind(user.VkId, all[2], dateTime));
            await _db.SaveChangesAsync();

            return new SuccessfulResult
            {
                Message = $"Окей. {dateTime:f} напомню следующее:\n{all[2]}"
            };
        }

        private bool ParseTime(string date, string time, out DateTime dateTime)
        {
            var isCorrect = DateTime.TryParseExact($"{date} {time}",
                                                   new[]
                                                   {
                                                       "dd.MM.yyyy HH:mm", "d.MM.yyyy HH:mm",
                                                       "dd.M.yyyy HH:mm", "d.M.yyyy HH:mm",
                                                       "dd.MM.yyyy H:mm", "d.MM.yyyy H:mm",
                                                       "dd.M.yyyy H:mm", "d.M.yyyy H:mm",
                                                       "dd.MM.yyyy HH:m", "d.MM.yyyy HH:m",
                                                       "dd.M.yyyy HH:m", "d.M.yyyy HH:m",
                                                       "dd.MM.yyyy H:m", "d.MM.yyyy H:m",
                                                       "dd.M.yyyy H:m", "d.M.yyyy H:m"
                                                   },
                                                   null, DateTimeStyles.AssumeLocal, out var res);
            dateTime = res;
            return isCorrect;
        }
    }
}