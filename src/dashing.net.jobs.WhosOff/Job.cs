using dashing.net.common;
using dashing.net.streaming;
using DDay.iCal;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dashing.net.jobs.WhosOff
{
    [Export(typeof(IJob))]
    public class Job :IJob
    {
        private string feelUrl;
        private int refreshInterval;

        public Lazy<Timer> Timer { get; private set; }

        public Job()
        {
            this.feelUrl = System.Configuration.ConfigurationManager.AppSettings.GetValues("whosoff-icalFeed")?.FirstOrDefault();

            var refreshStr = System.Configuration.ConfigurationManager.AppSettings.GetValues("whosoff-refreshInterval")?.FirstOrDefault();            
            if (!int.TryParse(refreshStr, out this.refreshInterval))
            {
                this.refreshInterval = 3;
            }

            var data = new WhosOffDataModel();
            data.items.Add(new WhosOffDataModel.Item { DaysOff = 0, StartDate = "", Name = "Feed URL:" + this.feelUrl });
            Dashing.SendMessage(new { id = "whosoff", items = data.items });

            Timer = new Lazy<Timer>(() => new Timer(SendMessage, null, TimeSpan.Zero, TimeSpan.FromMinutes(this.refreshInterval)));            
        }

        public void SendMessage(object message)
        {
            var data = GetData();
            Dashing.SendMessage(new { id = "whosoff", items = data.items });
        }

        public WhosOffDataModel GetData()
        {
            var uri = new Uri(this.feelUrl);
            var model = new WhosOffDataModel();
            try
            {
                var cal = iCalendar.LoadFromUri(uri);
                var events = from c in cal where c.Events != null select c.Events.Where(e=>e.End.Date >= DateTime.UtcNow.Date);

                // Convert to a presentation Object
                model = this.ToModel(events);
            }
            catch (Exception ex)
            {
                model.updatedAtMessage = "Error updating calender: " + ex.ToString();
            }

            return model;
        }

        private WhosOffDataModel ToModel(IEnumerable<IEnumerable<IEvent>> events)
        {
            var model = new WhosOffDataModel();
            model.title = "Whos Off";
            model.updatedAtMessage = DateTime.Now.ToLongDateString();

            foreach (var cal in events)
            {
                foreach (var subItem in cal)
                {
                    var item = new WhosOffDataModel.Item()
                    {
                        StartDate = subItem.Start.ToString("dd/MM/yyyy"),
                        EndDate = subItem.End.ToString("dd/MM/yyyy"),
                        DaysOff = Math.Round((subItem.End.Value - subItem.Start.Value).TotalDays, 2),
                        Name = subItem.Summary,
                    };                                       

                    model.items.Add(item);
                }
            }

            return model;
        }
    }
}
