using System;
using System.Collections.Generic;
using System.Text;

namespace ScholarStatistics.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double DifferenceBetweenPublicationsInDays { get; set; }
        public float RatioPublications { get; set; }
        public string MainCountry { get; set; }
        public int CountOfMondays { get; set; }
        public int CountOfTuesdays { get; set; }
        public int CountOfWednesdays { get; set; }
        public int CountOfThursdays { get; set; }
        public int CountOfFridays { get; set; }
        public int CountOfSaturdays { get; set; }
        public int CountOfSundays { get; set; }
        public double PercentageOfMondays { get; set; }
        public double PercentageOfTuesdays { get; set; }
        public double PercentageOfWednesdays { get; set; }
        public double PercentageOfThursdays { get; set; }
        public double PercentageOfFridays { get; set; }
        public double PercentageOfSaturdays { get; set; }
        public double PercentageOfSundays { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
