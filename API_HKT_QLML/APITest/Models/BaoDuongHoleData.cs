using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class BaoDuongHoleData
    {
        public int Hole_Id { get; set; }
        public string Maintian_Pic { get; set; }
        public int Maintain_Status { get; set; }
        public string Description { get; set; }
        public string Hole_Name { get; set; }
        public int Period_Id { get; set; }
        public string base_64_image { get; set; }
    }
}