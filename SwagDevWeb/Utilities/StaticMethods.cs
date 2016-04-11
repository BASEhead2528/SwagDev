using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint.Client;
using System.Drawing;
using System.Web.Mvc;
using SwagDevWeb.DAL;
using System.Diagnostics;

namespace SwagDevWeb.Utilities
{
    public static class StaticMethods
    {
        // Check if a group is in a set of groups
        public static bool isInGroup(IEnumerable<object> groups, string group)
        {
            foreach(Group grp in groups)
            {
                if (grp.Title == group)
                {
                    return true;
                }
            }
            return false;
        }

        // Check if a group is in a set of groups
        public static bool isInGroup(GroupCollection groups, string group)
        {
            foreach (Group grp in groups)
            {
                if (grp.Title == group)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isValidImageFile(HttpPostedFileBase file)
        {
            try
            {
                using(Bitmap image = new Bitmap(file.InputStream))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        [SharePointContextFilter]
        public static string saveUserName(HttpContextBase httpContext, Controller controller)
        {
            User spUser = null;

            var spContext = SharePointContextProvider.Current.GetSharePointContext(httpContext);
            Uri HostUrl = spContext.SPHostUrl;

            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    spUser = clientContext.Web.CurrentUser;

                    clientContext.Load(spUser, user => user.Title);

                    clientContext.ExecuteQuery();

                    controller.ViewBag.UserName = spUser.Title;

                    httpContext.Session.Add("UserName", spUser.Title);

                    return spUser.Title;
                }
            }

            return null;
        }

        public static void initCart(string user, Controller controller, HttpSessionStateBase session)
        {
            SwagDBContext db = new SwagDBContext();
            int? qty = db.CartItems.Where(c => c.UserName == user).Sum(item => (int?)item.Quantity);



            controller.ViewBag.CartQuantity = qty != null ? qty : 0;
            session.Add("CartQuant", qty != null ? qty : 0);
        }
    }
}