namespace dashing.net.jobs.TeamCity.Models
{
    public class BuildType
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string LastChangedBy { get; set; }
        public string LastBuildNumber { get; set; }
    }
}
