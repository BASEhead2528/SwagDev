using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SwagDevWeb.Models;

namespace SwagDevWeb.DAL
{
    public class SwagDBContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Swag> Swags { get; set; }
    }
}