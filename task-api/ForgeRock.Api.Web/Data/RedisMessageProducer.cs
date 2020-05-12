using ForgeRock.Api.Web.Domain;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;

namespace ForgeRock.Api.Web.Data
{
    public class RedisMessageProducer : IMessageProducer
    {
        private readonly IRedisClient _client;

        public RedisMessageProducer(IRedisClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public void Produce(Guid jobId, Guid stepId, string taskName)
        {
            // Messy here too, but for speed's sake
            var msg = new { jobId, stepId, taskName };
            _client.PublishMessage("messages", JsonConvert.SerializeObject(msg));
        }
    }
}
