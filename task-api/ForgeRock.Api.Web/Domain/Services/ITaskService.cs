using ForgeRock.Api.Web.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeRock.Api.Web.Domain.Services
{
    public interface ITaskService
    {
        IList<WorkerTask> GetTaskList();

        WorkerTask RegisterTask(string taskName);
    }
}
