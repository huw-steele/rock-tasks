using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace ForgeRock.Api.Web.Domain.Models
{
    public class Job
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public DateTime? StartedOn { get; private set; }

        public DateTime? EndedOn { get; private set; }

        public IList<JobStep> Steps { get; private set; }

        private Job()
        {
        }

        public Job(string name)
        {
            Name = string.IsNullOrWhiteSpace(name) ?
                throw new ArgumentNullException(nameof(name))
                : name;
            Id = Guid.NewGuid();
            Steps = new List<JobStep>();
        }

        public JobStep Start()
        {
            StartedOn = DateTime.UtcNow;
            var step = Steps.FirstOrDefault();
            step.Start();
            return step;
        }

        public JobStep GetStep(Guid id)
        {
            return Steps.SingleOrDefault(s => s.Id == id);
        }

        public JobStep GetNextStep()
        {
            return Steps.FirstOrDefault(s => !s.EndedOn.HasValue);
        }

        public void Finish()
        {
            EndedOn = DateTime.UtcNow;
        }
    }
}
