using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace dotnetserver.Models
{
    public class IBoard
    {
        public uint boardId { get; set; }
        public uint userId { get; set; }

        public string createdDate { get; set; }

        public string boardName { get; set; }
        public string boardDescription { get; set; }

        public string state { get; set; }
    }

    public class Board : IBoard
    {

    }

}
