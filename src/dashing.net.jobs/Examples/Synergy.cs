using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Threading;
using dashing.net.common;
using dashing.net.streaming;

namespace dashing.net.jobs.Examples
{
    [Export(typeof(IJob))]
    public class Synergy : IJob
    {
        private readonly Random _rand;

        public Lazy<Timer> Timer { get; }

        public Synergy()
        {
            if (ConfigurationManager.AppSettings["EnableExampleJobs"] != "true") return;

            _rand = new Random();

            Timer = new Lazy<Timer>(() => new Timer(SendMessage, null, TimeSpan.Zero, TimeSpan.FromSeconds(2)));
        }

        protected void SendMessage(object message)
        {
            Dashing.SendMessage(new {value = _rand.Next(100), id = "synergy"});
        }
    }
}
