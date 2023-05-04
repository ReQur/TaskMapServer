namespace dotnetserver.Models
{
    public class IBoard
    {
        public uint boardId { get; set; }
        public uint userId { get; set; }

        public string createdDate { get; set; }

        public string boardName { get; set; }
        public string boardDescription { get; set; }
    }

    public class Board : IBoard
    {

    }

    public class SharedInfoBoard : Board
    {
        public string accessRights { get; set; }
        public bool isShared{ get; set; }

    }

    public class ShareRequest
    {
        public uint boardId { get; set; }
        public string accessRights { get; set; }
        public IEnumerable<uint> userIdList { get; set; }
    }

}
