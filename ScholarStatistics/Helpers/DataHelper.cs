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

        public List<EntryPair> GetCategoriesCount(SyndicationItem item, List<EntryPair> categoriesCounts)
        {
            foreach (var category in item.Categories)
            {
                if (categoriesCounts.Where(item => item.Name == SubjectClassifications.GetSubject(category.Name)).Count() > 0)
                {
                    categoriesCounts.FirstOrDefault(item => item.Name == SubjectClassifications.GetSubject(category.Name)).Count++;
                }
                else
                {
                    categoriesCounts.Add(new EntryPair(SubjectClassifications.GetSubject(category.Name)));
                }
            }
            return categoriesCounts;
        }
        public List<EntryPair> GetAuthorsCount(SyndicationItem item, List<EntryPair> authorsCounts)
        {
            foreach (var author in item.Authors)
            {
                var names = author.Name.Split(" ");
                if (names.Count() > 1)
                {
                    if (authorsCounts.Where(item => item.Name.Contains(names[1])).Count() > 0)
                    {
                        authorsCounts.FirstOrDefault(item => item.Name.Contains(names[1])).Count++;
                    }
                    else
                    {
                        authorsCounts.Add(new EntryPair(author.Name));
                    }
                }
            }
            return authorsCounts;
        }
    }
}
