﻿namespace dotnetserver.Models
{
    public class IUser
    {
        public uint userId { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string md5PasswordHash { get; set; }
        public string avatar { get; set; }
        public uint lastBoardId { get; set; }

    }

    public class TbUser : IUser
    {

    }

}
