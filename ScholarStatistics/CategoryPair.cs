using System;
using System.Collections.Generic;
using System.Text;

namespace ScholarStatistics
{
    public class EntryPair
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public EntryPair(string name, int count = 1)
        {
            Name = name;
            Count = count;
        }
    }
}
