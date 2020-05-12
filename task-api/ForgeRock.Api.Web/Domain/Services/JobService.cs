using ForgeRock.Api.Web.Domain;
using ForgeRock.Api.Web.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeRock.Api.Web.Domain.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMessageProducer _messageProducer;

        public JobService(IJobRepository jobRepository, IMessageProducer messageProducer)
        {
            _jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
        }

        public Job GetJob(Guid id)
        {
            return _jobRepository.GetJob(id);
        }

        public IList<Job> GetJobs()
        {
            return _jobRepository.GetJobs();
        }

        public void ProgressJob(Guid id, Guid stepId)
        {
            Console.WriteLine("Progress Job called");
            // This is grotesque; I'd rather wrap the behaviour more neatly but trying to save time
            var job = _jobRepository.GetJob(id);
            job.GetStep(stepId)?.Complete();
            var nextStep = job.GetNextStep();            
            if (nextStep == null)
            {
                job.Finish();
            }
            else
            {
                Console.WriteLine("Next step found"); 
                nextStep.Start();
                _messageProducer.Produce(job.Id, nextStep.Id, nextStep.Task);
            }
            _jobRepository.SaveJob(job);
        }

        public void StartJob(Job job)
        {
            // Lacking transactionality, but that's been a common bugbear for me...
            var currentStep = job.Start();
            _jobRepository.SaveJob(job);
            if (currentStep == null) return;
            _messageProducer.Produce(job.Id, currentStep.Id, currentStep.Task);
        }
    }
}
