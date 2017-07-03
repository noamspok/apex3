using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using Ex3.Models;
using MazeLib;
using Newtonsoft.Json.Linq;

namespace Ex3.Controllers
{
    public class MultiplayerHub : Hub
    {
        private Ex3Context db = new Ex3Context();
        private static ConcurrentDictionary<string, string> connectedUsers =
             new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, String> gameGenerator =
             new ConcurrentDictionary<string, string>();
        public static List<string> games = new List<string>();

        private SingleModel model = new SingleModel();
        public void Connect(string UserName)
        {
            
            connectedUsers[UserName] = Context.ConnectionId;
            SendGames();
        }
        

        public void GenerateGame(string name,string username, int rows, int columns)
        {
            model.GenerateGame(name, rows, columns);
            gameGenerator[name] = username;
            games.Add(name);
            SendGames();
            
        }
        public void JoinGame(string name, string username) {

            string rival = gameGenerator[name];
            string recipientId = connectedUsers[username];
            if (recipientId == null)
                return;
            string otherRecipientId = connectedUsers[rival];
            if (recipientId == null)
                return;
            Maze maze = model.GetGames(name);
            JObject obj = JObject.Parse(maze.ToJSON());
            Clients.Client(otherRecipientId).gotMaze(obj, username);
            Clients.Client(recipientId).gotMaze(obj, rival);
            games.Remove(name);
            model.DeleteGame(name);
            SendGames();
            Clients.Client(otherRecipientId).start(username);
            Clients.Client(recipientId).start(rival);
        }
        public void GetGames(string user)
        {
            string recipientId = connectedUsers[user];
            if (recipientId == null)
                return;

            JObject obj = new JObject();
            obj["games"] = JToken.FromObject(games);
            Clients.Client(recipientId).gotGames(obj);
        }

        
        public void SendGames()
        {
            JObject obj = new JObject();
            obj["games"] = JToken.FromObject(games);
            Clients.All.gotGames(obj);

        }
        public void Move(string user, string text)
        {
            string recipientId = connectedUsers[user];
            if (recipientId == null)
                return;
            Clients.Client(recipientId).move(text);
        }
    }
    
}