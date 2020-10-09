using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;

namespace ScholarStatistics
{

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        private Random rnd = new Random();
        private int max_results = 100;
        private string labelText;
        public string LabelText
        {
            get { return labelText; }
            set
            {
                labelText = value;
                OnPropertyChanged(nameof(LabelText)); // Notify that there was a change on this property
            }
        }

        private double progress;
        public double Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged(nameof(Progress)); // Notify that there was a change on this property
            }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        void OnSearchButtonPressed(object sender, EventArgs e)
        {
            try
            {
                SearchBar searchBar = (SearchBar)sender;
                //HttpResponseMessage response = client.GetAsync("https://export.arxiv.org/api/query?search_query=all:electron&start=0&max_results=10").Result;
                //LabelText = response.Content.ReadAsStringAsync().Result;
                Task.Run(async () => await ShowCharts(searchBar.Text));
            }
            catch(Exception ex)
            {
                LabelText = ex.Message;
            }
        }
        async Task ShowCharts(string searchText)
        {
            Atom10FeedFormatter formatter = new Atom10FeedFormatter();
            BindingContext = this;
            await UpdateSearchProgressBar(0.2);
            using (XmlReader reader = XmlReader.Create("https://export.arxiv.org/api/query?search_query=" + searchText + "&max_results=" + max_results))
            {
                formatter.ReadFrom(reader);
                if (formatter.Feed.Items.Count() == 0)
                {
                    await UpdateSearchProgressBar(1.0);
                    LabelText = "Not Found";
                }

                await Task.Run(async () => await FillCharts(formatter));
            }
        }
        async Task FillCharts(Atom10FeedFormatter formatter)
        {
            Chart1.Chart = new BarChart();
            Chart1.Chart = new DonutChart();
            await UpdateSearchProgressBar(0.0f);
            int itemNumber = 0;
            var categoriesCounts = new List<EntryPair>();
            var authorsCounts = new List<EntryPair>();
            foreach (SyndicationItem item in formatter.Feed.Items)
            {
                Debug.WriteLine(SearchProgressBar.Progress);

                await UpdateSearchProgressBar((double)itemNumber++ / formatter.Feed.Items.Count());

                categoriesCounts = GetCategories(item, categoriesCounts);
                authorsCounts = GetAuthors(item, authorsCounts);
            }
            var categoryEntries = GetEntry(categoriesCounts);
            var authorEntries = GetEntry(authorsCounts);
            var dependency = DependencyService.Get<INativeFont>();
            if (categoryEntries.Count > 0)
            {
                Chart1.Chart = new BarChart()
                {
                    Entries = categoryEntries,
                    LabelTextSize = dependency.GetNativeSize(10)

                };
            }
            if(authorEntries.Count > 0)
            {
                Chart2.Chart = new DonutChart()
                {
                    Entries = authorEntries,
                    LabelTextSize = dependency.GetNativeSize(5),
                    LabelMode = LabelMode.LeftAndRight,
                    GraphPosition = GraphPosition.Center

                };
            }
            await UpdateSearchProgressBar(1.0f);
        }
        List<ChartEntry> GetEntry(List<EntryPair> pairs)
        {
            var result = new List<ChartEntry>();
            foreach (var pair in pairs)
            {
                if (pair.Name == "") continue;
                byte[] bytes = new byte[3];
                rnd.NextBytes(bytes);
                var red = bytes[0];
                var green = bytes[1];
                var blue = bytes[2];
                result.Add(new ChartEntry(pair.Count)
                {
                    Label = pair.Name,
                    ValueLabel = pair.Count.ToString(),
                    Color = new SKColor(red, green, blue)
                });
            }
            return result;
        }
        List<EntryPair> GetCategories(SyndicationItem item, List<EntryPair> categoriesCounts)
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
        List<EntryPair> GetAuthors(SyndicationItem item, List<EntryPair> authorsCounts)
        {
            foreach (var author in item.Authors)
            {
                var names = author.Name.Split(" ");
                if(names.Count() > 1)
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
        async Task UpdateSearchProgressBar(double value)
        {
            await Task.Run(() => Progress = value);
        }

    }

}
