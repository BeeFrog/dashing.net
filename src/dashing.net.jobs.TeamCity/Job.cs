using dashing.net.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.ComponentModel.Composition;
using dashing.net.streaming;
using dashing.net.jobs.TeamCity.Models;

/// <summary>
/// A Job which connects to TeamCity to returieve the latest status of your builds.
/// To configure add in the following app settings into the web.config
/// 
/// teamcity-hostname The host name of the server running team city. e.g. buildmachine.mydomain.com:8080
/// teamcity-username The username to auythenticate with. 
/// teamcity-password The password for the above username.
/// </summary>
namespace dashing.net.jobs.TeamCity
{
    [Export(typeof(IJob))]
    public class TeamCityJob : IJob
    {
        public Lazy<Timer> Timer { get; private set; }

        public TeamCityJob()
        {
            Timer = new Lazy<Timer>(() => new Timer(SendMessage, null, TimeSpan.Zero, TimeSpan.FromMinutes(2)));
        }

        public void SendMessage(object message)
        {
            var projectStatus = GetCurrentProjectStatus();
                        
            Dashing.SendMessage(new { id = "teamcity", items = projectStatus });
        }

        public IEnumerable<ProjectStatus> GetCurrentProjectStatus()
        {
            var url = System.Configuration.ConfigurationManager.AppSettings.GetValues("teamcity-hostname")?.FirstOrDefault();
            var username = System.Configuration.ConfigurationManager.AppSettings.GetValues("teamcity-username")?.FirstOrDefault();
            var pass = System.Configuration.ConfigurationManager.AppSettings.GetValues("teamcity-password")?.FirstOrDefault();            

            // https://github.com/y-gagar1n/TeamCitySharp
            var client = new TeamCitySharp.TeamCityClient(url);
            client.Connect(username, pass);
            var projects = client.Projects.All();

            var convertedProjects = this.ConvertProjects(client, projects);

            return convertedProjects;
        }

        private IEnumerable<ProjectStatus> ConvertProjects(TeamCitySharp.TeamCityClient client, List<TeamCitySharp.DomainEntities.Project> projects)
        {
            var projectStatus = new List<ProjectStatus>();
            
            var configs = client.BuildConfigs.All();
            foreach (var config in configs)
            {
                var status = client.Builds.ByBuildConfigId(config.Id).FirstOrDefault();

                if (status != null)
                {
                    var project = projectStatus.FirstOrDefault(p => p.Name == config.ProjectName);
                    if(project == null)
                    {
                        project = new ProjectStatus() { Name = config.ProjectName };
                        projectStatus.Add(project);
                    }

                    var item = new BuildType()
                    {
                        Name = config.Name,                        
                        Status = status.Status,
                        LastBuildNumber = status.Number,
                    };
                    
                    var lastChange = client.Changes.LastChangeDetailByBuildConfigId(config.Id);
                    if (lastChange != null)
                    {
                        item.LastChangedBy = lastChange.Username;
                        var date = lastChange.Date;
                    }

                    project.BuildTypes.Add(item);
                }

            }

            return projectStatus;
        }
    }
}
