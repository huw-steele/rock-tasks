using ForgeRock.Api.Web.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeRock.Api.Web.Domain
{
    public interface IJobRepository
    {
        void SaveJob(Job job);

        IList<Job> GetJobs();

        Job GetJob(Guid id);
    }
}
