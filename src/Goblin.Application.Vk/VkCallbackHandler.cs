using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Goblin.Application.Core;
using Goblin.Application.Core.Abstractions;
using Goblin.Application.Vk.Converters;
using Goblin.Application.Vk.Extensions;
using Goblin.Application.Vk.Models;
using Goblin.Application.Vk.Options;
using Goblin.DataAccess;
using Goblin.Domain.Entities;
using Microsoft.Extensions.Options;
using Serilog;
using VkNet.Abstractions;
using VkNet.Enums;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.GroupUpdate;
using VkNet.Model.RequestParams;

namespace Goblin.Application.Vk
{
    public class VkCallbackHandler
    {
        private readonly CommandsService _commandsService;
        private readonly BotDbContext _db;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly VkOptions _options;
        private readonly IVkApi _vkApi;

        public VkCallbackHandler(CommandsService commandsService, BotDbContext db, IVkApi vkApi, IOptions<VkOptions> options,
                                 IMapper mapper)
        {
            _commandsService = commandsService;
            _db = db;
            _vkApi = vkApi;
            _mapper = mapper;
            _options = options.Value;
            _logger = Log.ForContext<VkCallbackHandler>();
        }

        public async Task Handle(GroupUpdate upd)
        {
            if(upd.Secret != _options.SecretKey)
            {
                _logger.Warning("Пришло событие с неправильным секретным ключом ({0})", upd.Secret);
                return;
            }

            _logger.Debug("Обработка события с типом {0}", upd.Type);

            if(upd.Type == GroupUpdateType.MessageNew)
            {
                var msg = _mapper.Map<VkMessage>(upd.MessageNew.Message);
                await MessageNew(msg, upd.MessageNew.ClientInfo);
            }
            else if(upd.Type == GroupUpdateType.GroupLeave)
            {
                await GroupLeave(upd.GroupLeave);
            }
            else if(upd.Type == GroupUpdateType.GroupJoin)
            {
                await GroupJoin(upd.GroupJoin);
            }
            else
            {
                _logger.Fatal("Обработчик для события {0} не найден", upd.Type);
                throw new ArgumentOutOfRangeException(nameof(upd.Type), "Отсутствует обработчик события");
            }

            _logger.Information("Обработка события {0} завершена", upd.Type);
        }

        private async Task MessageNew(VkMessage message, ClientInfo clientInfo)
        {
            _logger.Debug("Обработка сообщения");
            await _commandsService.ExecuteCommand<VkBotUser>(message, OnSuccess, OnFailed);
            _logger.Information("Обработка сообщения завершена");

            async Task OnSuccess(IResult res)
            {
                await _vkApi.Messages.SendWithRandomId(new MessagesSendParams
                {
                    Message = res.Message,
                    Keyboard = KeyboardConverter.FromCoreToVk(res.Keyboard, clientInfo.InlineKeyboard),
                    PeerId = message.MessageChatId
                });
            }

            async Task OnFailed(IResult res)
            {
                await _vkApi.Messages.SendWithRandomId(new MessagesSendParams
                {
                    Message = res.ToString(),
                    PeerId = message.MessageChatId
                });
            }
        }

        public async Task GroupLeave(GroupLeave leave)
        {
            const string groupLeaveMessage = "Очень жаль, что ты решил отписаться от группы 😢\n" +
                                             "Если тебе что-то не понравилось или ты не разобрался с ботом, то всегда можешь написать " +
                                             "администрации об этом через команду 'админ *сообщение*' (подробнее смотри в справке).";

            _logger.Information("Пользователь id{0} покинул группу", leave.UserId);
            var admins = _db.VkBotUsers.Where(x => x.IsAdmin).Select(x => x.Id);
            var vkUser = (await _vkApi.Users.GetAsync(new[] { leave.UserId.Value })).First();
            var userName = $"{vkUser.FirstName} {vkUser.LastName}";
            await _vkApi.Messages.SendToUserIdsWithRandomId(new MessagesSendParams
            {
                Message = $"@id{leave.UserId} ({userName}) отписался :С",
                UserIds = admins
            });

            if(leave.IsSelf.HasValue && leave.IsSelf.Value)
            {
                try
                {
                    await _vkApi.Messages.SendWithRandomId(new MessagesSendParams
                    {
                        Message = groupLeaveMessage,
                        PeerId = leave.UserId.Value
                    });
                }
                catch
                {
                    // ignored
                }
            }
        }

        public async Task GroupJoin(GroupJoin join)
        {
            const string groupJoinMessage = "Спасибо за подписку! ❤\n" +
                                            "Если у тебя возникнут вопросы, то ты всегда можешь связаться с администрацией бота " +
                                            "при помощи команды 'админ *сообщение*' (подробнее смотри в справке)";

            _logger.Information("Пользователь id{0} вступил в группу", join.UserId);
            var admins = _db.VkBotUsers.Where(x => x.IsAdmin).Select(x => x.Id);
            var vkUser = (await _vkApi.Users.GetAsync(new[] { join.UserId.Value })).First();
            var userName = $"{vkUser.FirstName} {vkUser.LastName}";
            await _vkApi.Messages.SendToUserIdsWithRandomId(new MessagesSendParams
            {
                Message = $"@id{join.UserId} ({userName}) подписался!",
                UserIds = admins
            });

            if(join.JoinType.HasValue && join.JoinType == GroupJoinType.Join)
            {
                try
                {
                    await _vkApi.Messages.SendWithRandomId(new MessagesSendParams
                    {
                        Message = groupJoinMessage,
                        PeerId = join.UserId.Value
                    });
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}