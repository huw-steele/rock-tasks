using ForgeRock.Api.Web.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeRock.Api.Web.Domain
{
    public interface ITaskRepository
    {
        IList<WorkerTask> LoadAllTasks();
        void AddTask(WorkerTask newTask);
    }
}
