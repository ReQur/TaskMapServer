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

        public string createdDate { get; set; }

        public string taskLabel { get; set; }
        public string taskText { get; set; }
        public string color { get; set; }
        public string coordinates { get; set; }

        public uint state { get; set; }
    }

    public class BoardTask : IBoardTask
    {

    }

}