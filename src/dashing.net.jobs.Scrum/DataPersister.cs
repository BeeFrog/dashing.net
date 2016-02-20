namespace dashing.net.jobs.Scrum
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class DataPersister
    {
        public string Get(string key)
        {
            var path = this.GetPath(key);
            if (path !=null && System.IO.File.Exists(path))
            {
                var data = File.ReadAllText(path);
                return data;
            }

            return string.Empty;
        }

        public void Set(string key, string data)
        {
            var path = this.GetPath(key);
            if (path != null)
            {
                File.WriteAllText(path, data);
            }
        }

        protected string GetPath(string key)
        {
            // System.Web.HttpContext.Current?.Server?.MapPath("~/App_Data");
            var folder = DllFolder(); 
            if (folder != null)
            {
                var path = System.IO.Path.Combine(folder, "scrum-" + key);
                return path;
            }
            return null;
        }

        protected string DllFolder()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
}
}
