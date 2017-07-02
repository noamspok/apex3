using Ex3.Models;
using MazeLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Ex3.Controllers
{
    public class MultiController : ApiController
    {
        private SingleModel singleModel = new SingleModel();

        // GET: api/Multi/5
        public JObject Get(string game)
        {
            return JObject.Parse(singleModel.GetGames(game).ToJSON());
        }

        // POST: api/Multi
        public JObject PostGame(MazeInfo maze)
        {
            Maze retMaze = singleModel.GenerateGame(maze.Name, maze.Rows, maze.Cols);
            JObject obj = JObject.Parse(retMaze.ToJSON());
            return obj;
        }

        // PUT: api/Multi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Multi/5
        public void Delete(int id)
        {
        }
    }
}
