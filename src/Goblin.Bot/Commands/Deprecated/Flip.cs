﻿//using System;
//using System.Threading.Tasks;
//using Goblin.Bot.Enums;
//using Goblin.Bot.Models;
//using Vk.Models.Messages;

//namespace Goblin.Bot.Commands.Deprecated
//{
//    public class Flip : ICommand
//    {
//        public string Name { get; } = "Монета";
//        public string Description { get; } = "Подбрасывает монету и выдаёт орёл/решка";
//        public string Usage { get; } = "Монета";
//        public string[] Aliases { get; } = { "монета" };
//        public CommandCategory Category { get; } = CommandCategory.Common;
//        public bool IsAdmin { get; } = false;

//        public Task<CommandResponse> Execute(Message msg, BotUser user)
//        {
//            var choices = new[] { "Орёл", "Решка" };

//            var a = GetRandom(0, 1);
//            return Task.Run(() => new CommandResponse
//            {
//                Text = choices[a]
//            });
//        }

//        public (bool Success, string Text) CanExecute(Message msg, BotUser user)
//        {
//            return (true, "");
//        }

//        public static int GetRandom(int start, int end)
//        {
//            return new System.Random(DateTime.Now.Millisecond).Next(start, end);
//        }
//    }
//}