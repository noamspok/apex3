using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using Ex3.Models;
using MazeLib;

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
        }

        public void SendMessage(string senderUserName, string recipientUserName, string text)
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
            SendMessage(username, rival, "Rival:" + username);
            SendMessage( rival, username,"Rival:"+rival);
            SendMaze(username,(model.GetGames(name).ToJSON()));
            
        }
        public void GetGames(string user)
        {
            SendGames(user, games.ToArray());
        }

        public void SendGames(string user, string[] text) {
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

    }
    
}