using System;
using System.Threading.Tasks;
using Flurl.Http;
using Goblin.Application.Core;
using Goblin.Application.Core.Abstractions;
using Goblin.Application.Core.Results.Failed;
using Goblin.Application.Core.Results.Success;
using Goblin.Narfu;
using Goblin.Narfu.Schedule;
using Serilog;

namespace Goblin.Application.Vk.Extensions
{
    public static class NarfuExtensions
    {
        public static async Task<IResult> GetExamsWithResult(this StudentsSchedule students, int group)
        {
            try
            {
                var lessons = await students.GetExams(group);
                return new SuccessfulResult
                {
                    Message = lessons.ToString()
                };
            }
            catch(FlurlHttpException)
            {
                return new FailedResult(DefaultErrors.NarfuSiteIsUnavailable);
            }
            catch(Exception ex)
            {
                Log.ForContext<NarfuApi>().Fatal(ex, "Ошибка при получении экзаменов");
                return new FailedResult(DefaultErrors.NarfuUnexpectedError);
            }
        }

        public static async Task<IResult> GetScheduleAtDateWithResult(this StudentsSchedule students, int group, DateTime date)
        {
            try
            {
                var schedule = await students.GetScheduleAtDate(group, date);
                return new SuccessfulResult
                {
                    Message = schedule.ToString()
                };
            }
            catch(FlurlHttpException)
            {
                return new FailedResult(DefaultErrors.NarfuSiteIsUnavailable);
            }
            catch(Exception ex)
            {
                Log.ForContext<NarfuApi>().Fatal(ex, "Ошибка при получении расписания на день");
                return new FailedResult(DefaultErrors.NarfuUnexpectedError);
            }
        }
    }
}