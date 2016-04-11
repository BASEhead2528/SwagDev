using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SwagDevWeb.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string AccountExecutive { get; set; }
        public string Client { get; set; }
        public DateTime Date { get; set; }

        [DataType(DataType.Date)]
        public DateTime MeetingDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateNeeded { get; set; }

        public bool Filled { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}