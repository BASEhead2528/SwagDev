using SwagDevWeb.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SwagDevWeb.Controllers
{
    public class AjaxController : Controller
    {
        SwagDBContext db = new SwagDBContext();

        public int CartInfo()
        {

            Debug.WriteLine("In controller");

            string userName;
            if (Session["UserName"] != null)
            {
                userName = Session["UserName"].ToString();
            }
            else
            {
                userName = SwagDevWeb.Utilities.StaticMethods.saveUserName(HttpContext, this);
            }
            return db.CartItems.Where(c => c.UserName == userName).Sum(c => c.Quantity);
        }
    }
}