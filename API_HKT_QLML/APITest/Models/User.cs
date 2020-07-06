using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class User
    {
        public int id { get; set; }

        public int user_type_id { get; set; }

        public string name { get; set; }

        public string pass { get; set; }

        public string fullname { get; set; }

        public int status { get; set; }

        public int user_type_name { get; set; }

    }
}