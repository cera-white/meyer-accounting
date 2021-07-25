using IdentitySample.Models;
using MeyerAccountingV2.EF;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IdentitySample
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new MyPropertyActionFilter(), 0);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }

    public class MyPropertyActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            using (MeyerAccountingEntities db = new MeyerAccountingEntities())
            {
                filterContext.Controller.ViewBag.BusinessHours = db.BusinessHours.ToList();
                filterContext.Controller.ViewBag.Address = string.Format("{0}<br />{1}, {2} {3}",
                    db.BusinessInfoes.FirstOrDefault(x => x.Name == "Address").Value,
                    db.BusinessInfoes.FirstOrDefault(x => x.Name == "City").Value,
                    db.BusinessInfoes.FirstOrDefault(x => x.Name == "State").Value,
                    db.BusinessInfoes.FirstOrDefault(x => x.Name == "Zip").Value);
                filterContext.Controller.ViewBag.Phone = db.BusinessInfoes.FirstOrDefault(x => x.Name == "Phone").Value;
                filterContext.Controller.ViewBag.Mobile = db.BusinessInfoes.FirstOrDefault(x => x.Name == "Mobile").Value;
                filterContext.Controller.ViewBag.Fax = db.BusinessInfoes.FirstOrDefault(x => x.Name == "Fax").Value;
                filterContext.Controller.ViewBag.Email = db.BusinessInfoes.FirstOrDefault(x => x.Name == "Email").Value;
                filterContext.Controller.ViewBag.Company = db.BusinessInfoes.FirstOrDefault(x => x.Name == "Company").Value;
                filterContext.Controller.ViewBag.SiteKeywords = db.BusinessInfoes.FirstOrDefault(x => x.Name == "Site Keywords").Value;
                filterContext.Controller.ViewBag.SiteDescription = db.BusinessInfoes.FirstOrDefault(x => x.Name == "Site Description").Value;
                filterContext.Controller.ViewBag.Logo = db.BusinessInfoes.FirstOrDefault(x => x.Name == "Logo").Value;
                filterContext.Controller.ViewBag.Facebook = db.BusinessInfoes.FirstOrDefault(x => x.Name == "Facebook").Value;
            }
        }
    }
}
