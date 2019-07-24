﻿using System;
using System.Threading.Tasks;
using Goblin.Bot.Enums;
using Goblin.Bot.Models;
using Goblin.Domain.Entities;
using Vk.Models.Messages;

namespace Goblin.Bot.Commands
{
    public class Random : ICommand
    {
        public string Name { get; } = "Рандом *smth*, *smth*, *smth*....";
        public string Description { get; } = "Выбирает один из нескольких вариантов.";
        public string Usage { get; } = "Рандом 1, 2, 3,4 или 5";
        public string[] Aliases { get; } = { "рандом" };
        public CommandCategory Category { get; } = CommandCategory.Common;
        public bool IsAdmin { get; } = false;

        public Task<CommandResponse> Execute(Message msg, BotUser user)
        {
            var canExecute = CanExecute(msg, user);
            if(!canExecute.Success)
            {
                return Task.Run(() => new CommandResponse
                {
                    Text = canExecute.Text
                });
            }

            var forRandom = Split(msg.GetParams());
            var index = GetRandom(0, forRandom.Length);
            return Task.Run(() => new CommandResponse
            {
                Text = $"Я выбираю следующее: {forRandom[index]}"
            });
        }

        public (bool Success, string Text) CanExecute(Message msg, BotUser user)
        {
            var param = msg.GetParams();
            if(string.IsNullOrEmpty(param))
            {
                return (false, $"Ошибка. Пример использования команды: {Usage}");
            }

            var forRandom = Split(param);
            if(forRandom.Length < 2)
            {
                return (false, $"Введи два или более параметра ({Usage})");
            }

            return (true, "");
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