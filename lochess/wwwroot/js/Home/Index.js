// FEN strings that can be used to set the starting position of the pieces on the board:
var ruyLopez = 'r1bqkbnr/pppp1ppp/2n5/1B2p3/4P3/5N2/PPPP1PPP/RNBQK2R'
var start = 'start'

// 'Position' objects can also be used to specify starting positions
var position = {
    d6: 'bK',
    d4: 'wP',
    e4: 'wK'
} 
// Use the 'Config' variable to define the chessboard's behaviour entirely
var config = {
    position: ruyLopez,
    showNotation: true, // This is the default
    draggable: true,
    dropOffBoard: 'snapback', // This is the default
    pieceTheme: '/chessboardjs/img/chesspieces/wikipedia/{piece}.png' // This is the default, but others can be added as folders to the wikipedia folder, for example: /chessboardjs/img/chesspieces/wikipedia/colourfulpieces/{piece}.png'
    // Many more configuration options, see: https://chessboardjs.com/examples#2082 onwards
}

// Create board
var board = Chessboard('myBoard', config)

// Get board to resize based on its parent automatically as window size changes, specified by a percentage in the style tag
$(window).resize(board.resize)


// Integration of chess.js API to only allow legal moves:
var board = null
var game = new Chess()
var $status = $('#status')
var $fen = $('#fen')
var $pgn = $('#pgn')

function onDragStart(source, piece, position, orientation) {
    // do not pick up pieces if the game is over
    if (game.game_over()) return false

    // only pick up pieces for the side to move
    if ((game.turn() === 'w' && piece.search(/^b/) !== -1) ||
        (game.turn() === 'b' && piece.search(/^w/) !== -1)) {
        return false
    }
}

function onDrop(source, target) {
    // see if the move is legal
    var move = game.move({
        from: source,
        to: target,
        promotion: 'q' // NOTE: always promote to a queen for example simplicity
    })

    // illegal move
    if (move === null) return 'snapback'

    updateStatus()
}

// update the board position after the piece snap
// for castling, en passant, pawn promotion
function onSnapEnd() {
    board.position(game.fen())
}

function updateStatus() {
    var status = ''

    var moveColor = 'White'
    if (game.turn() === 'b') {
        moveColor = 'Black'
    }

    // checkmate?
    if (game.in_checkmate()) {
        status = 'Game over, ' + moveColor + ' is in checkmate.'
    }

    // draw?
    else if (game.in_draw()) {
        status = 'Game over, drawn position'
    }

    // game still on
    else {
        status = moveColor + ' to move'

        // check?
        if (game.in_check()) {
            status += ', ' + moveColor + ' is in check'
        }
    }

    $status.html(status)
    $fen.html(game.fen())
    $pgn.html(game.pgn())
}

var config = {
    draggable: true,
    position: 'start',
    onDragStart: onDragStart,
    onDrop: onDrop,
    onSnapEnd: onSnapEnd
}
board = Chessboard('myBoard', config)

updateStatus()

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
