using dashing.net.common;
using dashing.net.streaming;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dashing.net.jobs.Scrum
{
    [Export(typeof(IJob))]
    public class Job : IJob, IDataProcessor, IDataRetriever
    {
        private DataPersister persister;

        public Lazy<Timer> Timer { get; private set; }

        public string[] ProcessorWidgetIds
        {
            get { return new string[] { "burndown", "sprintgoal" }; }
        }

        public Job() : this(new DataPersister())
        {
        }

        public Job(DataPersister persister)
        {
            this.persister = persister;
            Timer = new Lazy<Timer>(() => new Timer(SendMessage, null, TimeSpan.Zero, TimeSpan.FromMinutes(1)));
        }

        public void SendMessage(object message)
        {
            var sprintGoal = this.persister.Get("sprintgoal");
            if (!string.IsNullOrWhiteSpace(sprintGoal))
            {
                Dashing.SendMessage(new { id = "sprintgoal", text = sprintGoal });
            }

            // Now send the burndown
            var data = this.GetData("burndown");
            if (data != null)
            {
                Dashing.SendMessage(new { id = "burndown", points = data });
            }

        }

        public void ProcessData(string widgetId, IDictionary<string, object> data)
        {
            //Extract the data and convert it into my object
            switch (widgetId) {
                case "burndown":
                    ProcessBurnDownData(data);
                    break;
                case "sprintgoal":
                    ProcessSprintGoalData(data);
                    break;
            }
        }

        private void ProcessSprintGoalData(IDictionary<string, object> data)
        {
            string sprintGoal = this.TryGetValue(data, "text");

            // Persist and save the data
            this.persister.Set("sprintgoal", sprintGoal);
            Dashing.SendMessage(new { id = "sprintgoal", text = sprintGoal });
        }

        private string TryGetValue(IDictionary<string, object> data, string key)
        {
            object value;
            data.TryGetValue(key, out value);
            return value + "";
        }

        private void ProcessBurnDownData(IDictionary<string, object> data)
        {
            var points = new List<Point>();

            for(int i = 1; i < 11; i++)
            {
                string y = this.TryGetValue(data, "d" + i);
                int Y;
                int.TryParse(y, out Y);
                points.Add(new Point() { X = i, Y = Y });
            }

            // Save the data
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(points);
            this.persister.Set("burndown", json);

            // Send it
            Dashing.SendMessage(new { id = "burndown", points = points.Select(m => new { x = m.X, y = m.Y }) });
        }

        public object GetData(string widgetId)
        {
            var json = this.persister.Get(widgetId);

            
            if (!string.IsNullOrWhiteSpace(json) && widgetId == "burndown")
            {
                var points = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                return points;
            }
            else if(widgetId == "sprintgoal")
            {
                return json;
            }

            return null;
        }

        class Point
        {
            [Newtonsoft.Json.JsonProperty("x")]
            public int X { get; set; }

            [Newtonsoft.Json.JsonProperty("y")]
            public int Y { get; set; }
        }
    }
}
