﻿using System.Threading.Tasks;
using Goblin.Bot.Enums;
using Goblin.Bot.Models;
using Goblin.Domain.Entities;
using Goblin.Persistence;
using Microsoft.EntityFrameworkCore;
using Vk.Models.Messages;

namespace Goblin.Bot.Commands
{
    public class MuteErrors : ICommand
    {
        public string Name { get; } = "Мут";
        public string Decription { get; } = "Выключить ошибки при написании неправильной команды";
        public string Usage { get; } = "Мут";
        public string[] Allias { get; } = { "мут" };
        public CommandCategory Category { get; } = CommandCategory.Common;
        public bool IsAdmin { get; } = false;

        private readonly BotDbContext _db;

        public MuteErrors(BotDbContext db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Execute(Message msg, BotUser user)
        {
            user.IsErrorsDisabled = !user.IsErrorsDisabled;
            await _db.SaveChangesAsync();

            return new CommandResponse
            {
                Text = "Ошибки " + (user.IsErrorsDisabled ? "выключены" : "включены")
            };
        }

        public (bool Success, string Text) CanExecute(Message msg, BotUser user)
        {
            return (true, "");
        }
    }
}