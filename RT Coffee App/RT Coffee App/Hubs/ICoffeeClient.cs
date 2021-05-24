using RT_Coffee_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RT_Coffee_App.Hubs
{
    // Interface that allows to define the signatures of the methods to call on the front end.
    // So that the Clients.Caller (dynamic object) is strongly typed.
    public interface ICoffeeClient
    {
        Task NewOrder(Order order);
        Task ReceiveOrderUpdate(UpdateInfo info);
        Task Finished(Order order);
    }
}