using RT_Coffee_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RT_Coffee_App.Controllers
{
    public class CoffeeController : Controller
    {
        private static int OrderId;

        public ActionResult Coffee()
        {
            return View();
        }

        // As soon as this method is called, I want to process the order and send a response to the caller
        // specifying the orderid
        [HttpPost]
        public int OrderCoffee(Order order)
        {
            // Using Hubs in any classes
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<CoffeeHub>();
            //hubContext.Clients.All.NewOrder(order);
            OrderId++;
            return OrderId;
        }
    }
}