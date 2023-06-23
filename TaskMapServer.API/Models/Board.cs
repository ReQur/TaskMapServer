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

    public static class BoardPermissions
    {
        public static readonly string[] Values = { "read-only", "edit-access", "administrating" };

        public static bool canRead(string accessRights)
        {
            return Values.Contains(accessRights);
        }
        public static bool canEdit(string accessRights)
        {
            return Values.Contains(accessRights) && accessRights != "read-only";
        }
        public static bool canAdministrate(string accessRights)
        {
            return accessRights == "administrating";
        }
    }
}
