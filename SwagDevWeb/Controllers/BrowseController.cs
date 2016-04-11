using SwagDevWeb.DAL;
using SwagDevWeb.Models;
using SwagDevWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.SharePoint.Client.Taxonomy;

namespace SwagDevWeb.Controllers
{
    public class BrowseController : Controller
    {
        private SwagDBContext db = new SwagDBContext();
        private List<Term> terms;

        // GET: Browse
        public ActionResult Index()
        {
            //SwagDevWeb.Utilities.StaticMethods.initCart(SwagDevWeb.Utilities.StaticMethods.saveUserName(HttpContext, this), this, Session);
            return View(db.Swags.ToList());
        }


        /*
            Apparently Model binding like this is no longer best practice. Better to use TryUpdateModel()
        */

        [ValidateAntiForgeryToken]
        public ActionResult CheckOut([Bind(Include= "Order,CartItems")] Cart cart)
        {

            // Need to check to make sure the quantity is available before checkout

            string userName;
            if (Session["UserName"] != null)
            {
                userName = Session["UserName"].ToString();
            }
            else
            {
                userName = SwagDevWeb.Utilities.StaticMethods.saveUserName(HttpContext, this);
            }

            if (ModelState.IsValid)
            {
                Order order = new Order()
                {
                    AccountExecutive = userName,
                    Client = cart.Order.Client,
                    Date = DateTime.Today,
                    DateNeeded = cart.Order.DateNeeded,
                    MeetingDate = DateTime.Parse(cart.Order.MeetingDate.ToString()),
                    OrderLines = new List<OrderLine>()
                };

                foreach(var cartItem in cart.CartItems)
                {
                    OrderLine orderLine = new OrderLine() 
                    {
                        SwagID = cartItem.SwagID,
                        Quantity = cartItem.Quantity
                    };
                    Swag tempSwag = db.Swags.Find(cartItem.SwagID);
                    tempSwag.Quantity -= cartItem.Quantity;
                    db.Entry(tempSwag).State = System.Data.Entity.EntityState.Modified;
                    order.OrderLines.Add(orderLine);
                    db.CartItems.Remove(db.CartItems.Find(cartItem.ID));
                }

                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Cart()
        {
            string userName;
            if (Session["UserName"] != null)
            {
                userName = Session["UserName"].ToString();
            }
            else
            {
                userName = SwagDevWeb.Utilities.StaticMethods.saveUserName(HttpContext, this);
            }
            
            SwagDevWeb.ViewModels.Cart cart = new Cart();

            cart.CartItems = db.CartItems.Where(c => c.UserName == userName).ToList();

            return View(cart);
        }

        public JsonResult AutoCompleteClients(string term)
        {
            // There should be a better way to do this. 
            // don't like the idea of populating the terms everytime this function is called
            // but each action gets a new controller context

            terms = PopulateTerms();
            if(this.terms == null)
            {
                return null;
            }
            else
            {
                var results = (from t in this.terms
                               where t.Name.ToLower().Contains(term.ToLower())
                               select new { t.Name }).Distinct();
                return Json(results, JsonRequestBehavior.AllowGet);
            }
        }

        private List<Term> PopulateTerms()
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            Uri HostUrl = spContext.SPHostUrl;
            

            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    TaxonomySession taxonomySession = TaxonomySession.GetTaxonomySession(clientContext);
                    clientContext.Load(taxonomySession);
                    clientContext.ExecuteQuery();

                    if (taxonomySession != null)
                    {
                        TermStore termstore = taxonomySession.GetDefaultKeywordsTermStore();

                        //This is where our active clients list lives. 
                        //We could pull from a DB but since the client names are already on SharePoint why not
                        TermGroup termgroup = termstore.Groups.GetByName("ConnectWise");
                        TermSet termset = termgroup.TermSets.GetByName("Clients");

                        clientContext.Load(termset.Terms);
                        clientContext.ExecuteQuery();
                        return termset.Terms.ToList();
                    }
                }
            }
            return null;
        }


        [HttpPost]
        public int addToCart(int itemID, int qty)
        {
            // Need to account for user turning off javascript or inform user that Javascript is required

            string userName;
            if (Session["UserName"] != null )
	        {
                userName = Session["UserName"].ToString();
	        }
            else
            {
                userName = SwagDevWeb.Utilities.StaticMethods.saveUserName(HttpContext, this);
            }

            CartItem item = new CartItem()
            {
                SwagID = itemID,
                Quantity = qty,
                UserName = userName
            };

            db.CartItems.Add(item);
            db.SaveChanges();

            var items = db.CartItems.Where(c => c.UserName == userName);  //.Sum(itm => (int?)itm.Quantity);

            int newQty = 0;

            if (items.Count() > 0)
            {
                newQty = (int)items.Sum(itm => (int?)itm.Quantity);
            }

            Session["CartQuant"] = newQty;

            return newQty;  
        }


        //[AcceptVerbs(HttpVerbs.Delete)]
        [HttpPost]
        public ActionResult removeFromCart(int itemID)
        {
            string userName;
            if (Session["UserName"] != null )
	        {
                userName = Session["UserName"].ToString();
	        }
            else
            {
                userName = SwagDevWeb.Utilities.StaticMethods.saveUserName(HttpContext, this);
            }

            try
            {
                db.CartItems.Remove(db.CartItems.Find(itemID));
                db.SaveChanges();
            }
            catch(Exception e)
            {
                throw new Exception("oh no");
            }

            var items = db.CartItems.Where(c => c.UserName == userName);  //.Sum(itm => (int?)itm.Quantity);

            int newQty = 0;

            if(items.Count() > 0)
            {
                newQty = (int)items.Sum(itm => (int?)itm.Quantity);
            }

            Session["CartQuant"] = newQty;

            return Json ( new
                { success = true,
                  cartQuant = newQty}) ;  
            
        }


    }
}