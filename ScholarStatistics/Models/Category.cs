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
        public override string ToString()
        {
            return Name;
        }
    }
}
