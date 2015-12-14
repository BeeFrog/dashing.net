using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Threading;
using dashing.net.common;
using dashing.net.streaming;

namespace dashing.net.jobs.Examples
{
    [Export(typeof(IJob))]
    public class Sample : IJob
    {
        private readonly Random _rand;

        public int CurrentValuation { get; private set; }
        public int LastValuation { get; private set; }
        
        public Lazy<Timer> Timer { get; }

        public Sample()
        {
            if (ConfigurationManager.AppSettings["EnableExampleJobs"] != "true") return;

            _rand = new Random();

            CurrentValuation = _rand.Next(100);

            Timer = new Lazy<Timer>(() => new Timer(SendMessage, null, TimeSpan.Zero, TimeSpan.FromSeconds(2)));
        }

        protected void SendMessage(object message)
        {
            LastValuation = CurrentValuation;
            
            CurrentValuation = _rand.Next(100);

            Dashing.SendMessage(new {current = CurrentValuation, last = LastValuation, id = "sample"});
        }
    }
}
