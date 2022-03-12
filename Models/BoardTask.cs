using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace dotnetserver.Models
{
    public class BoardTask
    {
        public uint taskId;
        public uint boardId;
        public uint userId;

        public DateTime createdDate;

        public string label;
        public string text;
        public Color color;

        public uint state;
        public Dictionary<string, int> coordinates;

        public BoardTask(uint _taskId, uint _boardId, uint _userId, Dictionary<string, int> _coordinates)
        {
            taskId = _taskId;
            boardId = _boardId;
            userId = _userId;
            coordinates = _coordinates;
        }
        public BoardTask()
        {}
    }
}