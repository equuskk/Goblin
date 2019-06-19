﻿using System.Threading.Tasks;
using Goblin.Bot.Enums;
using Goblin.Bot.Models;
using Goblin.Domain.Entities;
using Goblin.Persistence;
using Vk.Models.Messages;

namespace Goblin.Bot.Commands
{
    public class UnsetMailing : ICommand
    {
        public string Name { get; } = "Отписка *расписание ИЛИ погода*";
        public string Decription { get; } = "Отписка от рассылки расписания или погоды";
        public string Usage { get; } = "Отписка погода";
        public string[] Allias { get; } = { "отписка" };
        public CommandCategory Category { get; } = CommandCategory.Common;
        public bool IsAdmin { get; } = false;

        private readonly BotDbContext _db;

        public UnsetMailing(BotDbContext db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Execute(Message msg, BotUser user)
        {
            var canExecute = CanExecute(msg, user);
            if(!canExecute.Success)
            {
                return new CommandResponse
                {
                    Text = canExecute.Text
                };
            }

            var param = msg.GetParamsAsArray()[0].ToLower();
            var text = "";
            if(param == "погода")
            {
                user.Weather = false;
                text = "Ты отписался от рассылки погоды :с";
            }
            else if(param == "расписание")
            {
                user.Schedule = false;
                text = "Ты отписался от рассылки расписания :с";
            }
            else
            {
                text = $"Ошибка. Можно отписаться от рассылки погоды или расписания (выбрано - {param})";
            }

            if(_db.ChangeTracker.HasChanges())
            {
                await _db.SaveChangesAsync();
            }

            return new CommandResponse
            {
                Text = text
            };
        }

        public (bool Success, string Text) CanExecute(Message msg, BotUser user)
        {
            if(string.IsNullOrEmpty(msg.GetParams()))
            {
                return (false, "А от чего отписаться? Укажи 'погода' либо 'расписание'");
            }

            return (true, "");
        }
    }
}