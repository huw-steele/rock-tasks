using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeRock.Api.Web.Domain.Models
{
    public class JobStep
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Task { get; private set; }

        public DateTime? StartedOn { get; private set; }
        public DateTime? EndedOn { get; private set; }

        private JobStep()
        {
        }

        public JobStep(string name, string task)
        {
            Name = string.IsNullOrWhiteSpace(name) ?
                throw new ArgumentNullException(nameof(name)) :
                name;

            Task = string.IsNullOrWhiteSpace(task) ?
                throw new ArgumentNullException(nameof(task)) :
                task;

            Id = Guid.NewGuid();
        }

        public void Start()
        {
            StartedOn = DateTime.UtcNow;
        }

        public void Complete()
        {
            EndedOn = DateTime.UtcNow;
        }
    }
}
