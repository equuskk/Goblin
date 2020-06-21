﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Goblin.DataAccess;
using Goblin.Domain;
using Goblin.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Goblin.WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class RemindsController : Controller
    {
        private readonly BotDbContext _db;

        public RemindsController(BotDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var data = _db.Reminds.ToArray();
            ViewData["count"] = data.Length;
            return View(data);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRemind(long peerId, string text, string dateStr, string time)
        {
            var date = DateTime.Parse($"{dateStr} {time}");
            try
            {
                await _db.Reminds.AddAsync(new Remind(peerId, text, date, ConsumerType.Vkontakte));
                await _db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Log.ForContext<RemindsController>().Error(ex, "Невозможно добавить напоминание");
            }

            return RedirectToAction("Index", "Reminds");
        }
    }
}