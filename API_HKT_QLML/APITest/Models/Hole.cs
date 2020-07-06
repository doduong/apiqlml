using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class Hole
    {
        public int Hole_Id { get; set; }

        public int Hole_Route { get; set; }

        public string Hole_Name { get; set; }

        public string Hole_Address { get; set; }

        public int Street_Id { get; set; }

        public int HoleStatus_Id { get; set; }

        public int HoleSize_Id { get; set; }

        public int HoleType_Id { get; set; }

        public string Description { get; set; }

        public string HoleType_Name { get; set; }
        public string HoleSize_Name { get; set; }

        public string HoleStatus_Name { get; set; }

        public string Street_Name { get; set; }
        public string Maintain_Name { get; set; }
        public string Inspect_Name { get; set; }
        



    }
}