using Microsoft.AspNet.SignalR;
using RT_Coffee_App.Helpers;
using RT_Coffee_App.Models;
using System;
using System.Threading.Tasks;

// QUESTIONS
// Comment ça fonctionne que chaque browser est considéré comme un client / connected device différent ? est-ce c'est parce qu'il est associé à un connectedId unique ?
// Est-ce que ça tire beaucoup de jus à l'application que le TCP socket reste ouvert pour toute la durée de l'application ?? On peut le fermer mais à ce moment plus possible de communiquer RT.

// Sends and receives messages from front end
// We covered:
//    Creating Hubs
//    Contacting clients
//    Protecting hubs
namespace RT_Coffee_App.Hubs
{
    // Authorize just like a controller class using role based auth
    // [Authorize]
    // Generic ICoffeeClient so that the Clients.Caller dynamic object is strongly typed.
    // Else the compilers compiles on any function name but it might not exist.
    public class CoffeeHub : Hub<ICoffeeClient>
    {
        private static readonly OrderChecker _orderChecker = new OrderChecker(new Random());
        //[Authorize(Roles = "admin")]
        public async Task GetUpdateForOrder(Order order)
        {
            // the logged in user can be accessed in the Hub by using the context property
            // Context.User.Identity.Name;
            // Context.User.isInRole();
            // Clients.Others is making a call to other clients except the client that initiated the request
            await Clients.Others.NewOrder(order);
            UpdateInfo result;
            do
            {
                result = _orderChecker.GetUpdate(order);
                await Task.Delay(700);
                if (!result.New) continue;

                // serialization is made by SignalR
                // de serialization on the front end is made automatically by SignalR
                // Clients.caller is making a call to the client that initiated the request
                await Clients.Caller.ReceiveOrderUpdate(result);
                await Clients.Group("allUpdateReceivers").ReceiveOrderUpdate(result);
            } while (!result.Finished);
            await Clients.Caller.Finished(order);
        }

        // When a new client connects
        // To keep track of connected client or notify other clients when a client connects
        public override Task OnConnected()
        {
            // Context is a property in the hub that gives you user information
            // Available anywhere in the hub
            // ConnectionId = unique connectionId for the client that was exchanged during the websockets handshake
            if (Context.QueryString["group"] == "allUpdates")
                Groups.Add(Context.ConnectionId, "allUpdateReceivers");
            return base.OnConnected();

            // when you want to call a specific client:
            // Clients.Client(string connectionId)

            // when calling all clients except certain clients:
            // Clients.AllExcept(params string[] excludeConnectionIds)

            // Add clients to groups:
            // When you add a client to the group for the first time it creates the group
            // Groups.Add(string connectionId, string groupName)

            // Remove client from groups:
            // Group destroyed when last client is removed
            // Groups.Remove(string connectionId, string groupName)

            // If you are using multiple hubs in your application : Groups are separate for each hub
        }
    }
}