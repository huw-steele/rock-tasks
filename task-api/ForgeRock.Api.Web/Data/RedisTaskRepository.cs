using ForgeRock.Api.Web.Domain;
using ForgeRock.Api.Web.Domain.Models;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeRock.Api.Web.Data
{
    public class RedisTaskRepository : ITaskRepository
    {
        private readonly IRedisClient _client;

        public RedisTaskRepository(IRedisClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public void AddTask(WorkerTask newTask)
        {
            _client.As<WorkerTask>().Store(newTask);
        }

        public IList<WorkerTask> LoadAllTasks()
        {
            return _client.As<WorkerTask>().GetAll();
        }
    }
}
