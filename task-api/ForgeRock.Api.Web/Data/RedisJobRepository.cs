using ForgeRock.Api.Web.Domain;
using ForgeRock.Api.Web.Domain.Models;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeRock.Api.Web.Data
{
    public class RedisJobRepository : IJobRepository
    {
        private readonly IRedisClient _client;

        public RedisJobRepository(IRedisClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public IList<Job> GetJobs()
        {
            return _client.As<Job>().GetAll();
        }

        public Job GetJob(Guid id)
        {
            return _client.As<Job>().GetById(id);
        }

        public void SaveJob(Job job)
        {
            _client.As<Job>().Store(job);
        }
    }
}
