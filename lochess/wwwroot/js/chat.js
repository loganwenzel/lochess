/* SECTION: signalr connection */
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

// Plan for implementing this with the chess data: send the FEN string as an object by using a GetFEN chessboardjs function, then setting the page to that string
// Cant figure out how to combine the files Index.js and chat.js - is it even necessary tho?

//TODO: MAKE THESE WORK

// Function for getting the FEN string and object positions
function getPosition() {
    console.log('Current position as an Object:')
    console.log(board.position())

    console.log('Current position as a FEN string:')
    console.log(board.fen())
}

// Function for setting the position from an FEN string
function setPosition(FEN) {
    board.position(FEN)
}
