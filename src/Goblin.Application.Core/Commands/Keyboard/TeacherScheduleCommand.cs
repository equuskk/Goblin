using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using Goblin.Application.Core.Abstractions;
using Goblin.Application.Core.Results.Failed;
using Goblin.Application.Core.Results.Success;
using Goblin.Domain.Entities;
using Goblin.Narfu;
using Newtonsoft.Json;

namespace Goblin.Application.Core.Commands.Keyboard
{
    public class TeacherScheduleCommand : IKeyboardCommand
    {
        public string Trigger => "teacherSchedule";
        private readonly NarfuApi _narfuApi;

        public TeacherScheduleCommand(NarfuApi narfuApi)
        {
            _narfuApi = narfuApi;
        }

        public async Task<IResult> Execute(IMessage msg, BotUser user)
        {
            if(string.IsNullOrWhiteSpace(msg.Payload))
            {
                return new FailedResult("Невозожно получить ID преподавателя.");
            }

            var id = int.Parse(JsonConvert.DeserializeObject<Dictionary<string, string>>(msg.Payload)[Trigger]);
            try
            {
                var schedule = await _narfuApi.Teachers.GetLimitedSchedule(id);

                return new SuccessfulResult
                {
                    Message = schedule.ToString()
                };
            }
            catch(FlurlHttpException)
            {
                return new FailedResult(DefaultErrors.NarfuSiteIsUnavailable);
            }
            catch(Exception)
            {
                return new FailedResult(DefaultErrors.NarfuUnexpectedError);
            }
        }
    }
}