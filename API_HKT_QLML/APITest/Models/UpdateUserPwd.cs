using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class UpdateUserPwd
    {
        public int UserId { get; set; }
        public string OldPwd { get; set; }
        public string NewPwd { get; set; }
    }
}