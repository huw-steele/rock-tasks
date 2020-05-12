using ForgeRock.Api.Web.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeRock.Api.Web.Domain.Services
{
    public interface IJobService
    {
        void StartJob(Job job);

        IList<Job> GetJobs();

        Job GetJob(Guid id);

        void ProgressJob(Guid id, Guid stepId);
    }
}
