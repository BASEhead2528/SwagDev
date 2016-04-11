using SwagDevWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SwagDevWeb.ViewModels
{
    public class Cart
    {
        public Order Order { get; set; }

        [UIHint("CartItemsList")]
        public List<CartItem> CartItems { get; set; }
    }
}