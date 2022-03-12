using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
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
        private static List<BoardTask> Tasks = new List<BoardTask>();

        private readonly ILogger<TaskController> _logger;
        private readonly ITimeService _timeService;

        public TaskController(ILogger<TaskController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Array> Get()
        {
            return null;
        }
    }

}