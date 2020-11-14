using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace ScholarStatistics.Models
{
    public class Affiliation
    {
        public int AffiliationId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public Position Position {
            get {
                return new Position(Lattitude, Longitude); 
            }
        }
    }
}
