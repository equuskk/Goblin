﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Goblin.Helpers;
using Goblin.Models.Keyboard;
using Microsoft.EntityFrameworkCore;

namespace Goblin.Bot.Commands
{
    public class ScheduleCommand : ICommand
    {
        public string Name { get; } = "Раписание *день*.*месяц*";
        public string Decription { get; } = "Возвращает расписание на указанную дату. День и месяц обязательно должны содержать 2 цифры.";
        public string Usage { get; } = "Расписание 01.02";
        public List<string> Allias { get; } = new List<string> {"расписание"};
        public Category Category { get; } = Category.SAFU;
        public bool IsAdmin { get; } = false;

        public string Message { get; set; }
        public Keyboard Keyboard { get; set; }

        public async Task Execute(string param, int id = 0)
        {
            var user = await DbHelper.Db.Users.FirstAsync(x => x.Vk == id);
            DateTime time;
            if (param == "")
            {
                time = DateTime.Now;
            }
            else
            {
                var dayAndMonth = param.Split('.').Select(int.Parse).ToList(); // [Day, Month]
                time = new DateTime(2018, dayAndMonth[1], dayAndMonth[0]);
            }

            Message = await ScheduleHelper.GetScheduleAtDate(time, user.Group);
        }

        public bool CanExecute(string param, int id = 0)
        {
            var user = DbHelper.Db.Users.First(x => x.Vk == id);
            if (user.Group == 0)
            {
                Message = "Для начала установи группу командой 'устгр'";
                return false;
            }

            if (param == "")
            {
                return true;
            }

            var test = param.Split('.').Select(int.Parse).ToList();
            DateTime time;
            //TODO: DateTime.TryParseExact?
            try
            {
                time = new DateTime(DateTime.Now.Year, test[1], test[0]);
            }
            catch
            {
                Message = "Неправильная дата";
                return false;
            }

            return true;
        }
    }
}