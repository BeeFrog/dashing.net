namespace dashing.net.jobs.TeamCity.Models
{
    using System.Collections.Generic;

    public class ProjectStatus
    {

        public ProjectStatus()
        {
            this.BuildTypes = new List<BuildType>();
        }

        public string Name { get; set; }
        public IList<BuildType> BuildTypes { get; set; }
    }
}
