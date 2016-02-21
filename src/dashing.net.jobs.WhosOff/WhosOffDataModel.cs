namespace dashing.net.jobs.WhosOff
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class WhosOffDataModel
    {
        public WhosOffDataModel()
        {
            this.items = new List<Item>();
            this.title = "Whos Off";
            this.updatedAtMessage = string.Format("last updtaed at: {0:dd/MM/yyyy}", DateTime.Now);
        }

        public string updatedAtMessage { get; set; }
        public string title { get; set; }

        public IList<Item> items { get; private set; }

        public class Item
        {
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public double DaysOff { get; set; }

            public string DisplayText { get { return string.Format("{0:dd/MM/yyyy} ({1} days)", StartDate, DaysOff); } }

            public bool OffToday { get { return DateTime.Now >= this.StartDate; } }
        }
    }
}
