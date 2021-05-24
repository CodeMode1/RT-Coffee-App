using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RT_Coffee_App.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public string Size { get; set; }
    }
}