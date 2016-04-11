using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwagDevWeb.Models
{
    public class OrderLine
    {
        public int ID { get; set; }
        public int SwagID { get; set; }
        public int OrderID { get; set; }
        public int Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Swag Swag { get; set; }
    }
}