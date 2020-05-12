using ForgeRock.Api.Web.Domain;
using ForgeRock.Api.Web.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeRock.Api.Web.Domain.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public IList<WorkerTask> GetTaskList()
        {
            return _taskRepository.LoadAllTasks();
        }

        public WorkerTask RegisterTask(string taskName)
        {
            var newTask = new WorkerTask(taskName);
            _taskRepository.AddTask(newTask);
            return newTask;
        }
    }
}
