using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeRock.Api.Web.Domain.Models
{
    public class WorkerTask
    {     

        public string Name { get; private set; }

        private WorkerTask()
        {
        }

        public WorkerTask(string name)
        {
            Name = string.IsNullOrWhiteSpace(name) ? 
                throw new ArgumentNullException(nameof(name)) : 
                name;
        }
    }
}
