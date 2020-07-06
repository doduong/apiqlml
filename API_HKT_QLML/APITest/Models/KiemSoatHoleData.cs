using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class KiemSoatHoleData
    {
        public int Hole_Id { get; set; }
        public string Inspect_Pic { get; set; }

        public int Inspect_Status { get; set; }

        public int Ok_Status { get; set; }
        public string Description { get; set; }
        public int Inspect_Count { get; set; }
        public int isks { get; set; }
        public string Hole_Name { get; set; }
        public int Period_Id { get; set; }
        public string base_64_image { get; set; }
    }
}