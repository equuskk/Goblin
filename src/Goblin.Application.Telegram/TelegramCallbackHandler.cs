﻿using System.Threading.Tasks;
using AutoMapper;
using Goblin.Application.Core;
using Goblin.Application.Core.Results.Failed;
using Goblin.Application.Telegram.Converters;
using Goblin.Application.Telegram.Models;
using Goblin.Domain.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Goblin.Application.Telegram
{
    public class TelegramCallbackHandler
    {
        private readonly TelegramBotClient _botClient;
        private readonly CommandsService _commandsService;
        private readonly IMapper _mapper;

        public TelegramCallbackHandler(TelegramBotClient botClient, CommandsService commandsService, IMapper mapper)
        {
            _botClient = botClient;
            _commandsService = commandsService;
            _mapper = mapper;
        }

        public async Task Handle(Update update)
        {
            if(update.Type == UpdateType.Message)
            {
                await HandleMessage(update.Message);
            }
            else if(update.Type == UpdateType.CallbackQuery)
            {
                // await HandleKeyboard(update.CallbackQuery);
            }
        }

        // private async Task HandleKeyboard(CallbackQuery callbackQuery)
        // {
        // }

        private async Task HandleMessage(Message message)
        {
            var msg = _mapper.Map<TelegramMessage>(message);
            var user = new BotUser(1, "Архангельск", 351917);
            var result = await _commandsService.ExecuteCommand(msg, user);

            if(result is FailedResult failed)
            {
                if(result is CommandNotFoundResult && !user.IsErrorsEnabled)
                {
                    // если команда не найдена, и у пользователя отключены ошибки
                    return;
                }

                await _botClient.SendTextMessageAsync(msg.MessageChatId, failed.Message);
            }
            else
            {
                await _botClient.SendTextMessageAsync(msg.MessageChatId, result.Message,
                                                      replyMarkup: KeyboardConverter.FromCoreToTg(result.Keyboard));
            }
        }
    }
}