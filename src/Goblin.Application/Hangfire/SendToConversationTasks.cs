using System;
using System.Threading.Tasks;
using Flurl.Http;
using Goblin.Application.Extensions;
using Goblin.Application.Results.Failed;
using Goblin.Application.Results.Success;
using Goblin.DataAccess;
using Goblin.Narfu;
using Goblin.OpenWeatherMap;
using Hangfire;
using Serilog;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace Goblin.Application.Hangfire
{
    public class SendToConversationTasks
    {
        private readonly BotDbContext _db;
        private readonly NarfuApi _narfuApi;
        private readonly IVkApi _vkApi;
        private readonly OpenWeatherMapApi _weatherApi;

        public SendToConversationTasks(OpenWeatherMapApi weatherApi, NarfuApi narfuApi, IVkApi vkApi, BotDbContext db)
        {
            _weatherApi = weatherApi;
            _narfuApi = narfuApi;
            _vkApi = vkApi;
            _db = db;

            InitJobs();
        }

        public async Task SendToConv(long id, int group = 0, string city = "")
        {
            const int convId = 2000000000;
            id = convId + id;

            if(!string.IsNullOrWhiteSpace(city) && await _weatherApi.IsCityExists(city))
            {
                Log.Information("Отправка погоды в беседу {0}", id);
                var weather = await _weatherApi.GetDailyWeatherWithResult(city, DateTime.Today);
                if(weather is FailedResult failed)
                {
                    await _vkApi.Messages.SendError(failed.Error, id);
                }
                else
                {
                    var success = weather as SuccessfulResult;
                    await _vkApi.Messages.SendWithRandomId(new MessagesSendParams
                    {
                        PeerId = id,
                        Message = success.Message
                    });
                }
            }

            if(_narfuApi.Students.IsCorrectGroup(group))
            {
                Log.Information("Отправка расписания в беседу {0}", id);
                var schedule = await _narfuApi.Students.GetScheduleAtDateWithResult(group, DateTime.Now);
                if(schedule is FailedResult failed)
                {
                    await _vkApi.Messages.SendError(failed.Error, id);
                }
                else
                {
                    var success = schedule as SuccessfulResult;
                    await _vkApi.Messages.SendWithRandomId(new MessagesSendParams
                    {
                        PeerId = id,
                        Message = success.Message
                    });
                }
            }
        }

        public void InitJobs()
        {
            foreach(var job in _db.CronJobs)
            {
                RecurringJob.AddOrUpdate<SendToConversationTasks>(
                                                                  $"DAILY__{job.Name}",
                                                                  x => x.SendToConv(job.VkId, job.NarfuGroup,
                                                                                    job.WeatherCity),
                                                                  $"{job.Minutes} {job.Hours} * * 1-6",
                                                                  TimeZoneInfo.Local
                                                                 );
            }
        }

        public void Dummy()
        {
            //TODO: lol
        }
    }
}