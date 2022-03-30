using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;


namespace dotnetserver.Models
{
    public class Coordinates
    {
        public uint x { get; set; }
        public uint y { get; set; }
    }

    public class IBoardTask
    {
        public uint taskId { get; set; }
        public uint boardId { get; set; }
        public uint userId { get; set; }

        public string createdDate { get; set; }

        public string taskLabel { get; set; }
        public string taskText { get; set; }
        public string color { get; set; }

        public uint state { get; set; }
        public Coordinates coordinates { get; set; }

        public string stringCoordinates
        {
            get { return JsonConvert.SerializeObject(coordinates); }
        }

    }

    public class BoardTask : IBoardTask
    {
        public BoardTask(uint _taskId, uint _boardId, uint _userId, string _coordinates)
        {
            taskId = _taskId;
            boardId = _boardId;
            userId = _userId;
            //coordinates = _coordinates;
        }
        public BoardTask()
        { }
    }

    public class BoardTaskT : BoardTask
    {
        public string stringCoordinates
        {
            get { return JsonConvert.SerializeObject(coordinates); }
        }
    }


}