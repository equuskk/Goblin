﻿using System.Threading.Tasks;
using Goblin.Bot.Enums;
using Goblin.Bot.Models;
using Goblin.Domain.Entities;
using Narfu;
using Vk.Models.Messages;

namespace Goblin.Bot.Commands
{
    public class FindTeacher : ICommand
    {
        public string Name { get; } = "Найти *часть ФИО*";
        public string Description { get; } = "Найти преподавателя по части его ФИО";
        public string Usage { get; } = "Найти деменков";
        public string[] Aliases { get; } = { "найти" };
        public CommandCategory Category { get; } = CommandCategory.Safu;
        public bool IsAdmin { get; } = false;

        private readonly NarfuService _service;

        public FindTeacher(NarfuService service)
        {
            _service = service;
        }

        public Task<CommandResponse> Execute(Message msg, BotUser user)
        {
            var canExecute = CanExecute(msg, user);
            if(!canExecute.Success)
            {
                return Task.FromResult(new CommandResponse
                {
                    Text = canExecute.Text
                });
            }

            var text = _service.Teachers.FindByName(msg.GetParamsAsArray()[0]);
            if(text.Length >= 4095)
            {
                text =
                    "Ошибка. Найдено слишком много преподавателей.\n" +
                    "Пожалуйста, укажите более точные данные (например, введите фамилию и имя преподавателя).";
            }

            return Task.FromResult(new CommandResponse
            {
                Text = text
            });
        }

        public (bool Success, string Text) CanExecute(Message msg, BotUser user)
        {
            var param = msg.GetParamsAsArray()[0];
            if(param.Length < 4)
            {
                return (false, "Ошибка. Введите больше 4х символов в ФИО");
            }

            var teachers = _service.Teachers.FindByName(param);
            var found = !string.IsNullOrEmpty(teachers);
            if(!found)
            {
                var text = "Ошибка. Данного преподавателя нет в списке";

                return (false, text);
            }

            return (true, "");
        }
    }
}