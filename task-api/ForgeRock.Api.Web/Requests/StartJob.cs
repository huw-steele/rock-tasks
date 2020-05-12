using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeRock.Api.Web.Requests
{
    public class StartJob
    {
        public string Name { get; set; }

        public List<StartJobStep> Steps { get; set; }
    }

    public class StartJobStep
    {
        public string Name { get; set; }
        public string Task { get; set; }
    }
}
