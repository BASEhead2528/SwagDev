using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwagDevWeb.Models
{
    public class CartItem
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public int SwagID { get; set; }
        public int Quantity { get; set; }

        public virtual Swag Swag { get; set; }
    }
}