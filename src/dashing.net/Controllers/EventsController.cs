namespace dashing.net.Controllers
{
    using dashing.net.common;
    using dashing.net.Infrastructure;
    using dashing.net.streaming;
    using Microsoft.AspNet.SignalR;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    public class EventsController : Hub
    {
        private static readonly BlockingCollection<string> MessageQueue = new BlockingCollection<string>();

        public EventsController()
        {
            Dashing.SendMessage = SendMessage;

            Task.Factory.StartNew(ProcessQueue);

            Jobs.Start();
        }

        private void SendMessage(dynamic message)
        {
            var updatedAt = TimeHelper.ElapsedTimeSinceEpoch();

            if (message.GetType() == typeof(JObject))
            {
                message.updatedAt = updatedAt;
            }
            else
            {
                message = JsonHelper.Merge(message, new { updatedAt });
            }

            var serialized = JsonConvert.SerializeObject(message);

            MessageQueue.TryAdd(serialized);
        }

        private void ProcessQueue()
        {
            foreach (var message in MessageQueue.GetConsumingEnumerable())
            {
                Clients.All.sendMessage(message);
            }
        }
    }
}
