using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Goblin.Application.Core.Abstractions;
using Goblin.Application.Core.Results.Failed;
using Goblin.Application.Core.Results.Success;
using Goblin.Domain.Entities;
using Newtonsoft.Json;
using Serilog;

namespace Goblin.Application.Core
{
    public class CommandsService
    {
        private readonly IEnumerable<IKeyboardCommand> _keyboardCommands;
        private readonly ILogger _logger;
        private readonly IEnumerable<ITextCommand> _textCommands;

        public CommandsService(IEnumerable<ITextCommand> textCommands,
                               IEnumerable<IKeyboardCommand> keyboardCommands)
        {
            _textCommands = textCommands;
            _keyboardCommands = keyboardCommands;
            _logger = Log.ForContext<CommandsService>();
        }

        public async Task<IResult> ExecuteCommand(IMessage msg, BotUser user)
        {
            if(!string.IsNullOrWhiteSpace(msg.Payload))
            {
                return await ExecuteKeyboardCommand(msg, user);
            }

            return await ExecuteTextCommand(msg, user);
        }

        private async Task<IResult> ExecuteTextCommand(IMessage msg, BotUser user)
        {
            _logger.Debug("Обработка текстовой команды");
            var cmdName = msg.CommandName;

            foreach(var command in _textCommands)
            {
                var isAllowed = command.IsAdminCommand && user.IsAdmin;
                if(!command.Aliases.Contains(cmdName) || !isAllowed)
                {
                    continue;
                }

                _logger.Debug("Выполнение команды {0}", command.GetType());
                var result = await command.Execute(msg, user);
                if(result is FailedResult failedExecuteResult)
                {
                    _logger.Debug("Команда вернула {0} результат", failedExecuteResult.GetType());
                    return failedExecuteResult;
                }

                _logger.Debug("Команда выполнена успешно");

                return result as SuccessfulResult;
            }

            return new CommandNotFoundResult();
        }

        private async Task<IResult> ExecuteKeyboardCommand(IMessage msg, BotUser user)
        {
            _logger.Debug("Обработка команды с клавиатуры");
            var record = JsonConvert.DeserializeObject<Dictionary<string, string>>(msg.Payload).FirstOrDefault();
            foreach(var command in _keyboardCommands)
            {
                if(!record.Key.Contains(command.Trigger))
                {
                    continue;
                }

                _logger.Debug("Выполнение команды с клавиатуры {0}", command.GetType());
                return await command.Execute(msg, user);
            }

            return new CommandNotFoundResult();
        }
    }
}