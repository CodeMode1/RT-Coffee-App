// WebSocket = undefined;
// EventSource = undefined;

// React to function calls initiated by the server
setupConnection = (hubProxy) => {

    // First parameter, method name on the server
    // Second parameter, function executed when the server response is received
    hubProxy.on("receiveOrderUpdate", function (updateObject) {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = `Order: ${updateObject.OrderId}: ${updateObject.Update}`;
    });

    hubProxy.on("newOrder", function (order) {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = `Somebody ordered an ${order.Product}`;
    });
};

// -- Without generated proxy object --
// When a document is ready, create a connection by calling hubConnection() function
// Once we have the connection, we can get an on the fly hub proxy by calling createHubProxy() on the connection obj.
// The hub proxy object is used to communicate with backend hub
$(document).ready(() => {
    var connection = $.hubConnection('https://localhost:44341/Coffee');
    var hubProxy = connection.createHubProxy('CoffeeHub');

    setupConnection(hubProxy);
    // opening up the connection, after having configured the request handling
    // the client always initate the communication
    // its a good idea to first create the handlers for the function calls and then open the connection
    // So youre sure youre not missing out on any messages
    // As soon as we start a connection, the negotiate request is sent to the server
    //    server responses with connectionid and other parameters for client to configure itself
    // Since WebSockets is the best available transport, the WebSockets connection will be initiated.
    // The next request does the WebSockets handshaking. ( connect )
    connection.start();
    //connection.start({ transport: 'longPolling' });
    // Now let's pretend my browser doesn't support WebSockets.
    // I can easily do that by making the WebSocket object undefined in the first line of my Wired Brain JavaScript file.
    // When I save and refresh the browser, you can see the server - sent events is chosen now.It's using eventsource.
    // And when I also make EventSource undefined, SignalR reverts to long polling.Different requests are now made that are kept open until there's an update.
    // Setting objects to undefined is kind of a brute - force solution to test what SignalR does when it picks a transport automatically.
    // If you want SignalR to use a specific transport and switch off the auto selection, you can do so by specifying a parameter when calling the start function that is used to open the connection.
    // It's an object with a transport property.The value of the property is the forced transport. 
    document.getElementById("submit").addEventListener("click",
        e => {
            e.preventDefault();
            var statusDiv = document.getElementById("status");
            statusDiv.innerHTML = "Submitting order..";

            const product = document.getElementById("product").value;
            const size = document.getElementById("size").value;

            // Ajax request to the controller when the submit button is clicked
            fetch("/Coffee/OrderCoffee",
                {
                    method: "POST",
                    body: JSON.stringify({ product, size }),
                    headers: {
                        'content-type': 'application/json'
                    }
                })
                .then(response => response.text())
                .then(id => hubProxy.invoke('getUpdateForOrder', { id, product, size })
                    .fail(error => console.log(error))
                );
        });
});