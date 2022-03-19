using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using dotnetserver.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace dotnetserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;

        public TaskController(ILogger<TaskController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> GetTasks(string boardId)
        {
            //return Newtonsoft.Json.JsonConvert.SerializeObject(TaskService.GetBoardTasks(boardId));
            var res = await TaskService.GetTasks();
            return JsonConvert.SerializeObject(res.ToArray());
        }
    }

}