﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Goblin.Persistence;
using Goblin.WebUI.Extensions;
using Hangfire;
using Narfu;
using OpenWeatherMap;
using Vk;

namespace Goblin.WebUI.Hangfire
{
    public class ScheduledTasks
    {
        private readonly ApplicationDbContext _db;
        private readonly VkApi _api;
        private readonly WeatherService _weather;
        private readonly NarfuService _service;

        private const int ChunkLimit = 100; // максимум 100 ID в отправке
        private const int VkApiLimit = 20; // в секунду
        private const int ExtraDelay = 15; // милисекунд

        public ScheduledTasks(ApplicationDbContext db, VkApi api, WeatherService weather, NarfuService service)
        {
            _db = db;
            _api = api;
            _weather = weather;
            _service = service;

            InitJobs();
        }

        public async Task SendRemind()
        {
            var reminds =
                _db.Reminds
                   .ToArray()
                   .Where(x => x.Date.ToString("dd.MM.yyyy HH:mm") == DateTime.Now.ToString("dd.MM.yyyy HH:mm")); //TODO: что-то сделать с датой

            if(!reminds.Any())
            {
                return;
            }

            foreach(var remind in reminds)
            {
                await _api.Messages.Send(remind.VkId, $"Напоминаю:\n{remind.Text}");
                _db.Reminds.Remove(remind);
            }

            await _db.SaveChangesAsync();
        }

        public void SendDailyStuff()
        {
            var weatherJob = BackgroundJob.Enqueue(() => SendWeather());
            if (DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
            {
                BackgroundJob.ContinueWith(weatherJob, () => SendSchedule());
            }
        }

        public async Task SendSchedule()
        {
            var grouped = _db.GetScheduleUsers().GroupBy(x => x.Group);
            foreach(var group in grouped)
            {
                foreach(var chunk in group.Chunk(ChunkLimit))
                {
                    var ids = chunk.Select(x => x.Vk);
                    var schedule = await _service.Students.GetScheduleAsStringAtDate(DateTime.Today, group.Key);
                    await _api.Messages.Send(ids, schedule);
                    await Task.Delay((1000 / VkApiLimit) + ExtraDelay);
                }   
            }
        }

        public async Task SendWeather()
        {
            var grouped = _db.GetWeatherUsers().GroupBy(x => x.City);
            foreach(var group in grouped)
            {
                foreach(var chunk in group.Chunk(ChunkLimit))
                {
                    var ids = chunk.Select(x => x.Vk);
                    var weather = await _weather.GetDailyWeatherString(group.Key, DateTime.Today);
                    await _api.Messages.Send(ids, weather);
                    await Task.Delay((1000 / VkApiLimit) + ExtraDelay); 
                }
            }
        }

        public async Task SendToConv(int id, int group = 0, string city = "")
        {
            const int convId = 2000000000;
            id = convId + id;

            if(!string.IsNullOrWhiteSpace(city) && await _weather.CheckCity(city))
            {
                var weather = await _weather.GetDailyWeatherString(city, DateTime.Today);
                await _api.Messages.Send(id, weather);
            }

            if(_service.Students.IsCorrectGroup(group))
            {
                var schedule = await _service.Students.GetScheduleAsStringAtDate(DateTime.Now, group);
                await _api.Messages.Send(id, schedule);
            }
        }

        public void InitJobs()
        {
            //RecurringJob.AddOrUpdate<ScheduledTasks>("SendSchedule", x => x.SendSchedule(),
            //                                         "0 6 * * 1-6", TimeZoneInfo.Local);

            //RecurringJob.AddOrUpdate<ScheduledTasks>("SendWeather", x => x.SendWeather(),
            //                                         "0 7 * * *", TimeZoneInfo.Local);

            foreach(var job in _db.Jobs)
            {
                RecurringJob.AddOrUpdate<ScheduledTasks>(
                    $"DAILY__{job.JobName}",
                    x => x.SendToConv(job.Conversation, job.NarfuGroup, job.WeatherCity),
                    $"{job.Minutes} {job.Hours} * * 1-6", TimeZoneInfo.Local
                );
            }
        }

        public static void Init()
        {
            // минуты часи дни месяцы дни-недели
            RecurringJob.AddOrUpdate<ScheduledTasks>("SendRemind", x => x.SendRemind(), Cron.Minutely,
                                                     TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate<ScheduledTasks>("SendDailyStuff", x => x.SendDailyStuff(),
                                                     "30 5 * * *", TimeZoneInfo.Local);
        }

        public void Dummy()
        {
            //TODO: lol
        }
    }
}