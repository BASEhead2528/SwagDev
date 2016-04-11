using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SwagDevWeb.Controllers;
using Microsoft.SharePoint.Client;

namespace SwagDevWeb.Filters
{
    public class SwagAuthorize : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public SwagAuthorize(params string[] roles)
        {
            this.allowedroles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            User spUser = null;
            IEnumerable<object> userGroups = null;

            //if the session already contains the groups then use them else get them
            if (httpContext.Session["UserGroups"] == null)
            {
                var spContext = SharePointContextProvider.Current.GetSharePointContext(httpContext);

                using (var clientContext = spContext.CreateUserClientContextForSPHost())
                {
                    if (clientContext != null)
                    {
                        spUser = clientContext.Web.CurrentUser;
                        clientContext.Load(spUser, user => user.Groups);
                        clientContext.ExecuteQuery();
                    }
                }
                httpContext.Session["UserGroups"] = spUser.Groups;
                userGroups = spUser.Groups;
            }
            else
            {
                userGroups = httpContext.Session["UserGroups"] as IEnumerable<object>;
            }

            foreach (Group grp in userGroups)
            {
                if (Array.IndexOf(allowedroles, grp.Title) != -1)
                {
                    return true;
                }
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult { ViewName = "NotAllowed" };
        }
    }
}