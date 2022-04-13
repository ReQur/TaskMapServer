using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;


namespace dotnetserver.Models
{
    public class IUser
    {
        public uint userId { get; set; }
        public string email { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string md5PasswordHash { get; set; }
    }

    public class TbUser : IUser
    {

    }

}
