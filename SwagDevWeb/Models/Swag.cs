using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SwagDevWeb.Models
{
    public class Swag
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public double UnitCost { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }

    }
}