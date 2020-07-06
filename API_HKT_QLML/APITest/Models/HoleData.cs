using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class HoleData
    {
        public int Id { get; set; }

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


    public int Period_Id { get; set; }

    public DateTime Maintain_Day { get; set; }

    public DateTime Inspect_Day { get; set; }

    public string Maintain_Pic { get; set; }

    public string Inspect_Pic { get; set; }

    public int Maintain_Status { get; set; }

    public int Inspect_Status { get; set; }

    public int Ok_Status { get; set; }    

    public int Inspect_Count { get; set; }

    public string description_holedata { get; set; }

    public string Hole_Latitude { get; set; }

    public string Hole_Longitude { get; set; }

    }
}