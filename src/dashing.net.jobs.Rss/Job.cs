using dashing.net.common;
using HigLabo.Net.Rss;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using dashing.net.streaming;

namespace dashing.net.jobs.Rss
{
    [Export(typeof(IJob))]
    public class Job : IJob
    {
        private int timesUpdated = 0;
        private Random randomGenerator = new Random(DateTime.Now.Millisecond);
        private IEnumerable<RssItem> feedItems;
        private string feedUrl;

        public Lazy<Timer> Timer { get; private set; }

        public Job()
        {
            this.feedUrl = System.Configuration.ConfigurationManager.AppSettings.GetValues("rss-feed")?.FirstOrDefault();
            Timer = new Lazy<Timer>(() => new Timer(SendMessage, null, TimeSpan.Zero, TimeSpan.FromMinutes(1)));
        }

        public void SendMessage(object message)
        {
            var feed = this.GetFeed()?? CreateFakeFeedItem();
            Dashing.SendMessage(new { id = "rss", item_title = feed.Title, item_description = feed.Description });
        }

        private RssItem CreateFakeFeedItem()
        {
            return new RssItem { Description = "You have not configured the RSS feed yet.", Title = "Feed Error!" };
        }

        public RssItem GetFeed()
        {
            // To save on hitting bandwidth let's only hit the server 1 in 10 times
            if (timesUpdated > 10)
            {
                timesUpdated = 0;
            }

            RssItem item = null;

            if (timesUpdated < 1)
            {
                var client = new RssClient();
                var feed = client.GetRssFeed(new Uri(this.feedUrl));
                this.feedItems = feed?.Items;

                item = feed?.Items?.FirstOrDefault();
            }
            else
            {
                if (this.feedItems != null)
                {
                    int i = this.randomGenerator.Next(this.feedItems.Count() - 1);
                    item = this.feedItems.Skip(i).FirstOrDefault();
                }
            }

            timesUpdated++;
            return item;            
        }
    }
}
