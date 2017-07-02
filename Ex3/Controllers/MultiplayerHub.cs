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
            games.Add("1");
            games.Add("2");
            games.Add("3");
            connectedUsers[UserName] = Context.ConnectionId;
            JObject obj = new JObject();
            obj["games"] = JToken.FromObject(games);
            Clients.All.gotGames(obj);
        }

        public void Start(string senderUserName, string recipientUserName, string text)
        {
            string recipientId = connectedUsers[recipientUserName];
            if (recipientId == null)
                return;
            Clients.Client(recipientId).gotMessage( text);
        }

        public void GenerateGame(string name,string username, int rows, int columns)
        {
            
            Maze maze= model.GenerateGame(name, rows, columns);
            gameGenerator[name] = username;
            games.Add(name);
            SendMaze(username, (model.GetGames(name).ToString()));
        }
        public void JoinGame(string name, string username) {

            string rival = gameGenerator[name];
            Start(username, rival, username);
            Start( rival, username,rival);
            SendMaze(username,(model.GetGames(name).ToJSON()));
            
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

        public void SendGames(string user, JObject text) {
            string recipientId = connectedUsers[user];
            if (recipientId == null)
                return;
            Clients.Client(recipientId).gotGames(text);
        }
        public void SendMaze(string user, string text)
        {
            string recipientId = connectedUsers[user];
            if (recipientId == null)
                return;
            Clients.Client(recipientId).gotMaze(text);
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