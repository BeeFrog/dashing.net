using dashing.net.Authentication;
using dashing.net.common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dashing.net.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index(string view)
        {
            if (string.IsNullOrWhiteSpace(view))
            {
                return View();
            }
            else
            {
                object model = GetViewData(view);
                return View(view, model);
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]        
        public ActionResult Login(string authID)
        {
            var authentication = new SimpleAuthentication();
            if (authentication.Validate(this.Request.Form))
            {
                authentication.AuthenticateUser(Response);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Login");
        }



        private object GetViewData(string view)
        {
            try {
                var processor = Jobs.Get().Where(j => j is IDataRetriever).Cast<IDataRetriever>().Where(d => d.ProcessorWidgetIds.Contains(view, StringComparer.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (processor != null)
                {
                    var model = processor.GetData(view);
                    return model;
                }
            }
            // TODO: Something, Anything but this! Maybe log it?
            catch {}

            return null;
        }

        [Authorize]
        [HttpPost]
        public ActionResult SendMessage()
        {
            string widgetID = string.Empty;

            dynamic expando = new ExpandoObject();
            var items = expando as IDictionary<string, object>;

            var form = this.Request.Form;
            foreach (var key in form.Keys)
            {
                if (key.ToString() == "id")
                {
                    widgetID = form[key.ToString()];
                }
                else
                {
                    items[key.ToString()] = form[key.ToString()];
                }
            }

            if (widgetID != null)
            {
                // Find a data processor
                var processors = Jobs.Get().Where(j => j is IDataProcessor).Cast<IDataProcessor>();
                var processor = processors.FirstOrDefault(p => p.ProcessorWidgetIds.Contains(widgetID, StringComparer.CurrentCultureIgnoreCase));
                if (processor != null)
                {
                    processor.ProcessData(widgetID, items);
                    return View("Index");
                }
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(items);
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            //if (model.AuthToken == System.Configuration.ConfigurationManager.AppSettings.Get("AuthToken"))
            //{            
                dashing.net.streaming.Dashing.SendMessage(model);
            //}
            
            return View("Index");
        }

    }
}