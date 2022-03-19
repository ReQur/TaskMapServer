using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace dotnetserver.Models
{
    public class IBoardTask
    {
        public uint taskId { get; set; }
        public uint boardId { get; set; }
        public uint userId { get; set; }

        public DateTime createdDate { get; set; }

        public string label { get; set; }
        public string text { get; set; }
        public Color color { get; set; }

        public uint state { get; set; }
        public Dictionary<string, int> coordinates { get; set; }

    }

    public class BoardTask : IBoardTask
    {
        public BoardTask(uint _taskId, uint _boardId, uint _userId, Dictionary<string, int> _coordinates)
        {
            taskId = _taskId;
            boardId = _boardId;
            userId = _userId;
            coordinates = _coordinates;
        }
        public BoardTask()
        { }
    }
}