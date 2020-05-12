using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeRock.Api.Web.Domain
{
    public interface IMessageProducer
    {
        void Produce(Guid jobId, Guid stepId, string taskName);
    }
}
