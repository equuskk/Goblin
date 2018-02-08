﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Goblin.Models;
using Newtonsoft.Json;

namespace Goblin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string Test([FromBody] dynamic body)
        {
            var EventType = body["type"].ToString();
            switch (EventType)
            {
                case "confirmation":
                    return "***REMOVED***";

                case "message_new":
                    break;

                case "group_join":
                    //send message
                    break;
                case "group_leave":
                case "message_deny":
                    //delete from db, send message
                    break;
            }
            //var aa = JsonConvert.DeserializeObject<dynamic>(Json as string);
            return "ok";
        }
    }
}
