﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Goblin.Models.Keyboard;

namespace Goblin.Bot.Commands
{
    public class RandomCommand : ICommand
    {
        public string Name { get; } = "Рандом *smth*, *smth*, *smth*....";
        public string Decription { get; } = "Выбирает один из нескольких вариантов.";
        public string Usage { get; } = "Рандом 1, 2, 3,4 или 5";
        public List<string> Allias { get; } = new List<string> { "рандом" };
        public Category Category { get; } = Category.Common;
        public bool IsAdmin { get; } = false;

        public string Message { get; set; }
        public Keyboard Keyboard { get; set; }

        public async Task Execute(string param, int id = 0)
        {
            var forRandom = Split(param);

            var index = GetRandom(0, forRandom.Length);
            Message = $"Я выбираю {forRandom[index]}";
        }

        public bool CanExecute(string param, int id = 0)
        {
            if (string.IsNullOrEmpty(param))
            {
                Message = $"Пример использования команды: {Usage}";
                return false;
            }

            var forRandom = Split(param);
            if (forRandom.Length < 2)
            {
                Message = "Введи два или более параметроы";
                return false;
            }
            return true;
        }

        private int GetRandom(int start, int end)
        {
            //todo: шо за магическое число
            return new Random(DateTime.Now.Millisecond * 3819).Next(start, end);
        }

        private string[] Split(string str) => str.Split(new[] { ",", ", ", " или " }, StringSplitOptions.None);
    }
}
