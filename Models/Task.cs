using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace dotnetserver.Models
{
    public class Task
    {
        public uint TaskId;
        public uint BoardId;
        public uint UserId;

        public DateTime CreatedDate;

        public string Label;
        public string Text;
        public Color Color;

        public string State;
        public Tuple<uint, uint> Coordinates;
    }
}