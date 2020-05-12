using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForgeRock.Api.Web.Domain.Services;
using ForgeRock.Api.Web.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ForgeRock.Api.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var taskList = _taskService.GetTaskList();
            return Ok(taskList);
        }

        [HttpPost]
        public IActionResult Register([FromBody] RegisterWorkerTask model)
        {
            _ = _taskService.RegisterTask(model.Name);
            return Ok();
        }
    }
}
