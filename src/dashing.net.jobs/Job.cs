using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using dashing.net.common;

namespace dashing.net.jobs
{
    public abstract class Job : IJob
    {
        public Lazy<Timer> Timer { get; }

        protected Job(TimeSpan interval)
        {
            Timer = new Lazy<Timer>(() => new Timer(SendMessage, null, TimeSpan.Zero, interval));
        }

        protected abstract void SendMessage(object state);
    }
}
