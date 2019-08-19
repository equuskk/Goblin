using System;
using System.Threading.Tasks;
using Goblin.Application.Abstractions;
using Goblin.Application.Extensions;
using Goblin.Application.Results;
using Goblin.Domain.Entities;
using VkNet.Model;

namespace Goblin.Application.Commands.Text
{
    public class ChooseCommand : ITextCommand
    {
        public bool IsAdminCommand => false;
        public string[] Aliases => new[] { "выбери", "рандом" };
        
        public Task<IResult> Execute(Message msg, BotUser user)
        {
            var param = string.Join(' ', msg.GetCommandParameters());
            var split = Split(param);
            
            if(split.Length < 2)
            {
                return Task.FromResult<IResult>(new FailedResult($"Введите два или более предложений, разделенных следующими символами: ',' и 'или'"));
            }

            var random = GetRandom(0, split.Length);

            return Task.FromResult<IResult>(new SuccessfulResult
            {
                Message = $"Я выбираю это: {split[random]}"
            });
        }
        
        private int GetRandom(int start, int end)
        {
            return new System.Random(DateTime.Now.Millisecond).Next(start, end);
        }

        private string[] Split(string str)
        {
            return str.Split(new[] { ",", ", ", " или " }, StringSplitOptions.None);
        }
    }
}