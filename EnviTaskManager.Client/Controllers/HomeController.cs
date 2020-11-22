using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EnviTaskManagerClient.Models;
using System.Net.Http;
using System.Net;

using Microsoft.Extensions.Options;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Threading;
using EnviTaskManagerClient.Services;
using Microsoft.AspNetCore.Authorization;

namespace EnviTaskManagerClient.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AppSettings AppSettings { get; set; }
        private Services.ITaskService _service;


        public HomeController(ILogger<HomeController> logger,
                              IOptions<AppSettings> settings,
                              Services.ITaskService service)
        {
            _logger = logger;
            AppSettings = settings.Value;
            _service = service;
        }


        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult TaskStatus(int taskId, int customerId)
        {
            //https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-http-api
            var item = new TaskItem { TaskType = taskId, CustomerId = customerId };
            var result = _service.GetTaskStatus ( item);
      
            return Json(result);
        }

        [Authorize]
        [HttpGet]
        public ActionResult TaskStart(int taskId,  int customerId)
        {
            //https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-http-api
            var item = new TaskItem { TaskType = taskId, CustomerId = customerId };
            var result = _service.TaskStart(item);

            return Json(result);
        }
        [Authorize]
        [HttpGet]
        public ActionResult TaskTerminate(int taskId, int customerId)
        {
            var item = new TaskItem { TaskType = taskId, CustomerId = customerId };
            var result = _service.TaskTerminate(item);

            return Json(result);
        }
        [Authorize]
        [HttpGet]
        public ActionResult TaskScheduler(int taskId, int customerId, int hour)
        {
            var item = new TaskItem { TaskType = taskId, CustomerId = customerId, MonitorHour= hour };
            var result = _service.TaskScheduler(item);

            return Json(result);
        }

  
  
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
