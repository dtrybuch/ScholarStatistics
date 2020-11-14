using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml.Linq;

namespace ScholarStatistics.Helpers
{
    public class DataHelper
    {
        public string GetAffiliation(SyndicationItem item, string authorName)
        {
            foreach (var author in item.Authors)
            {
                if (author.Name.Contains(authorName, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (SyndicationElementExtension extension in author.ElementExtensions)
                    {
                        XElement ele = extension.GetObject<XElement>();
                        if (ele.Name.LocalName == "affiliation") return ele.Value;
                    }
                }
            }
            return "";
        }

        public string GetLongerFullName(SyndicationItem item, string authorName)
        {
            foreach (var author in item.Authors)
            {
                if (author.Name.Contains(authorName, StringComparison.OrdinalIgnoreCase) && author.Name.Length > authorName.Length)
                    return new string(author.Name);
            }
            return new string(authorName);
        }
    }
}
