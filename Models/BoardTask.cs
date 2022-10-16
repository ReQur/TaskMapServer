namespace dotnetserver.Models
{
    public class IBoardTask
    {
        public uint taskId { get; set; }
        public uint next_task_id { get; set; }
        public uint boardId { get; set; }
        public uint userId { get; set; }

        public string createdDate { get; set; }

        public string taskLabel { get; set; }
        public string taskText { get; set; }
        public string color { get; set; }
        public uint state { get; set; }

        public double x { get; set; }
        public double y { get; set; }
    }

    public class BoardTask : IBoardTask
    {

    }

}