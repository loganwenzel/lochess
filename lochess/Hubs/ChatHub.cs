using lochess.Areas.Identity.Data;
using lochess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol.Plugins;
using System;
using System.ComponentModel;
using Ubiety.Dns.Core;

namespace lochess.Hubs
{
    public class ChatHub : Hub
    {
        private LochessIdentityContext context;

        private IHttpContextAccessor _httpContextAccessor;

        public ChatHub(LochessIdentityContext cc, IHttpContextAccessor httpContextAccessor)
        {
            context = cc;
            _httpContextAccessor = httpContextAccessor;
        }

        // Function for sending message
        public async Task SendMessage(string message)
        {
            // TODO: ADD A LOADING ANIMATION WHILE THIS METHOD IS RUNNING TO ACCOUNT FOR THE DELAY

            // Find the opponent's AspNetUser record based on the passed in UserName
            AspNetUser sender = context.Users.Where(a => a.Id == Context.UserIdentifier).FirstOrDefault();
            string groupName = sender.GroupName;

            // Send message to group
            await Clients.Group(groupName).SendAsync("ReceiveMessage", sender.FirstName, message);
        }

        // Function for sending the updated fenString
        public async Task SendMove(string source, string target)
        {
            // Find the opponent's AspNetUser record based on the passed in UserName
            AspNetUser sender = context.Users.Where(a => a.Id == Context.UserIdentifier).FirstOrDefault();
            string groupName = sender.GroupName;

            // Send move to group
            await Clients.Group(groupName).SendAsync("ReceiveMove", source, target);
        }

        // Function for creating a group between the sender and opponent.
        // Called when the 'Play' modal is submitted
        public async Task AddToGroup(string opponentUserName)
        {
            AspNetUser sender = context.Users.Where(a => a.Id == Context.UserIdentifier).FirstOrDefault();
            string senderUserName = sender.UserName;

            AspNetUser opponent = context.Users.Where(a => a.UserName == opponentUserName).FirstOrDefault();

            // Fetch connection ids from the Connections table for the sender and opponent
            List<string> senderConnectionIds = context.Connections.Where(a => a.UserAgent == senderUserName).Select(a => a.ConnectionId).ToList();
            List<string> opponentConnectionIds = context.Connections.Where(a => a.UserAgent == opponentUserName).Select(a => a.ConnectionId).ToList();

            // Define groupName as the sender's username underscore the opponent's username for simplicity
            string groupName = senderUserName + "_" + opponentUserName;

            // Add connection ids of the sender to the group
            foreach (string connectionId in senderConnectionIds)
            {
                await Groups.AddToGroupAsync(connectionId, groupName);
            }

            // Add connection ids of the opponent to the group
            foreach (string connectionId in opponentConnectionIds)
            {
                await Groups.AddToGroupAsync(connectionId, groupName);
            }

            // Set the group name of the sender and opponent
            context.AspNetUsers.Where(a => a.UserName == senderUserName).FirstOrDefault().GroupName = groupName;
            context.AspNetUsers.Where(a => a.UserName == opponentUserName).FirstOrDefault().GroupName = groupName;

            // Create Game Logic:
            // Only create a game if there is no currently active game with either player
            if (context.Games.Where(a => ((a.WhiteUserName == senderUserName || a.BlackUserName == senderUserName) && a.GameActive == true)
                                   || (a.WhiteUserName == opponentUserName || a.BlackUserName == opponentUserName) && a.GameActive == true).Count() == 0)
            {

                Random random = new Random();
                string whiteUserName;
                string blackUserName;

                // Let the invitation sender be white if a random number is even
                if (random.Next() % 2 == 0)
                {
                    whiteUserName = sender.UserName;
                    blackUserName = opponent.UserName;
                }
                else
                {
                    whiteUserName = sender.UserName;
                    blackUserName = opponent.UserName;
                }
                // Create the Game record, and assign white vs. black
                context.Games.Add(new Game
                {
                    WhiteUserName = whiteUserName,
                    BlackUserName = blackUserName,
                    GameActive = true
                });

                // Set the initial JS variables  
                await Clients.Group(groupName).SendAsync("GameStart", blackUserName, whiteUserName);

                context.SaveChanges();

                // Sends Message to the specified group 'groupName'
                await Clients.Group(groupName).SendAsync("ReceiveMessage", "Lochess", $"{sender.FirstName} {sender.LastName} and {opponent.FirstName} {opponent.LastName} have joined the group {groupName}.");
                await Clients.Group(groupName).SendAsync("ReceiveInvite");
            }
        }

        // Function for deleting the group that exists between the leaver and the opponent.
        // Called when the 'Leave Match' button is clicked
        public async Task RemoveFromGroup()
        {
            // Current logic for leaving groups: one person leaves a group ends the group for all the members
            AspNetUser leaver = context.Users.Where(a => a.Id == Context.UserIdentifier).FirstOrDefault();
            // Find opponent from Group Name
            AspNetUser opponent = context.Users.Where(a => a.GroupName == leaver.GroupName && a.Id != leaver.Id).FirstOrDefault();

            if (opponent != null)
            {
                string groupName = leaver.GroupName;

                // Fetch connection ids from the Connections table for the leaver and opponent
                List<string> leaverConnectionIds = context.Connections.Where(a => a.UserAgent == leaver.UserName).Select(a => a.ConnectionId).ToList();
                List<string> opponentConnectionIds = context.Connections.Where(a => a.UserAgent == opponent.UserName).Select(a => a.ConnectionId).ToList();

                // Sends Message to the specified group 'groupName'
                await Clients.Group(groupName).SendAsync("ReceiveMessage", "Lochess", $"{leaver.FirstName} {leaver.LastName} and {opponent.FirstName} {opponent.LastName} have left the group {groupName}.");
                await Clients.Group(groupName).SendAsync("MatchLeft");

                // Remove all connection ids of sender and opponent from the group
                foreach (string connectionId in leaverConnectionIds)
                {
                    await Groups.RemoveFromGroupAsync(connectionId, groupName);
                }
                foreach (string connectionId in opponentConnectionIds)
                {
                    await Groups.RemoveFromGroupAsync(connectionId, groupName);
                }

                // Set the group name of the sender and opponent to null
                context.AspNetUsers.Where(a => a.UserName == leaver.UserName).FirstOrDefault().GroupName = null;
                context.AspNetUsers.Where(a => a.UserName == opponent.UserName).FirstOrDefault().GroupName = null;

                // Update the Game record
                Game game = context.Games.Where(a => (a.WhiteUserName == leaver.UserName || a.BlackUserName == leaver.UserName) && a.GameActive == true).FirstOrDefault();
                game.GameActive = false;
                game.Result = "draw";
                // TODO: ADD IMPLEMENTATION FOR UPDATING THE PGN EACH TIME A MOVE IS MADE - Use the pgn in updatestatus()

                context.SaveChanges();
            }
        }

        public async Task GameOver(string result, string winnerColour, string pgn)
        {
            // Does not matter which player (black or white, inviter or receiver, etc) is used to query the Game
            AspNetUser player1 = context.Users.Where(a => a.Id == Context.UserIdentifier).FirstOrDefault();
            Game game = context.Games.Where(a => (a.WhiteUserName == player1.UserName || a.BlackUserName == player1.UserName) && a.GameActive == true).FirstOrDefault();
            if (game != null)
            {
                if (result == "draw")
                {
                    // Game ended in draw
                    game.GameActive = false;
                    game.Result = "draw";
                    game.Pgn = pgn;
                    context.SaveChanges();
                }
                else
                {
                    // Game was decisive
                    game.GameActive = false;
                    game.Pgn = pgn;

                    // game.Result records who won using the username
                    if (winnerColour == "black")
                    {
                        game.Result = game.BlackUserName;
                    }
                    else
                    {
                        game.Result = game.WhiteUserName;
                    }
                    context.SaveChanges();
                }
            }
        }

        // Function for adding rows to the 'Connections' table whenever there is a new connection
        public override Task OnConnectedAsync()
        {
            var name = Context.User.Identity.Name;
            List<string> userNames = context.Users.Select(a => a.UserName).ToList();
            var user = context.AspNetUsers.Where(a => a.UserName == name).FirstOrDefault();
            var connection = context.Connections.Find(Context.ConnectionId);
            if (user != null)
            {
                // Logged in user
                if (connection == null)
                {
                    // First time connecting, need to create connection row
                    context.Connections.Add(new lochess.Models.Connection
                    {
                        ConnectionId = Context.ConnectionId,
                        UserAgent = user.UserName,
                        Connected = true,
                        AspNetUserId = user.Id
                    });
                    context.SaveChanges();
                }
                else
                {
                    // ConnectionId already exists in DB, don't need to create connection row
                    connection.Connected = true;
                    context.SaveChanges();
                }
                return base.OnConnectedAsync();
            }
            else
            {
                // No logged in user -> log connection with null AspNetUserId and UserAgent
                context.Connections.Add(new lochess.Models.Connection
                {
                    ConnectionId = Context.ConnectionId,
                    Connected = true
                });
                context.SaveChanges();

                return base.OnConnectedAsync();
            }
        }

        // Function for setting the 'connected' boolean of a connection to false upon disconnection
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = context.Connections.Find(Context.ConnectionId);
            connection.Connected = false;

            // Kick user out of game when they disconnect
            RemoveFromGroup();

            context.SaveChanges();
            await base.OnDisconnectedAsync(exception);
        }
    }
}