using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForgeRock.Api.Web.Domain.Models;
using ForgeRock.Api.Web.Domain.Services;
using ForgeRock.Api.Web.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ForgeRock.Api.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
        }

        [HttpPost]
        public ActionResult Start([FromBody] StartJob startJob)
        {
            var job = new Job(startJob.Name);
            startJob.Steps.ForEach(s => job.Steps.Add(new JobStep(s.Name, s.Task)));
            _jobService.StartJob(job);            
            return Ok(new { job.Id });
        }

        [HttpPost("{jobId:guid}/steps/{stepId:guid}/complete")]
        public ActionResult Progress([FromRoute]Guid jobId, [FromRoute]Guid stepId)
        {
            _jobService.ProgressJob(jobId, stepId);
            return Ok();
        }

        [HttpGet("{jobId:guid}")]
        public ActionResult Get([FromRoute]Guid jobId)
        {
            return Ok(_jobService.GetJob(jobId));
        }
    }
}