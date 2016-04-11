using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SwagDevWeb.Filters;
using System.Diagnostics;

namespace SwagDevWeb.Controllers
{
    [SwagAuthorize("SWAG_ADMIN", "SWAG_INV_MANAGER", "SWAG_MRK_MANAGER", "SWAG_USER")]
    public class HomeController : Controller
    {
        public Uri HostUrl { get; set; }


        public ActionResult Index()
        {
            SwagDevWeb.Utilities.StaticMethods.initCart(SwagDevWeb.Utilities.StaticMethods.saveUserName(HttpContext, this), this, Session);
            return View();
        }

        public ActionResult Back()
        {
            return Redirect(SharePointContextProvider.Current.GetSharePointContext(HttpContext).SPHostUrl.ToString());
        }

        [SwagAuthorize("SWAG_ADMIN")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
