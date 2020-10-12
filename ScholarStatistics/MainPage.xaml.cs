using Microcharts;
using ScholarStatistics.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ScholarStatistics
{

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        public string SearchText { get; set; }
        private int max_results = 100;
        private string labelText;
        public string LabelText
        {
            get { return labelText; }
            set
            {
                labelText = value;
                OnPropertyChanged(nameof(LabelText)); 
            }
        }

        private string authorPublicationsNumber;
        public string AuthorPublicationsNumber
        {
            get { return authorPublicationsNumber; }
            set
            {
                authorPublicationsNumber = value;
                OnPropertyChanged(nameof(AuthorPublicationsNumber)); 
            }
        }

        private string authorFullName;
        public string AuthorFullName
        {
            get { return authorFullName; }
            set
            {
                authorFullName = value;
                OnPropertyChanged(nameof(AuthorFullName));
            }
        }

        private string mainAffiliation;
        public string MainAffiliation
        {
            get { return mainAffiliation; }
            set
            {
                mainAffiliation = value;
                OnPropertyChanged(nameof(MainAffiliation)); 
            }
        }

        private double progress;
        public double Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged(nameof(Progress)); 
            }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        void OnAuthorSearchButtonPressed(object sender, EventArgs e)
        {
            try
            {
                SearchBar searchBar = (SearchBar)sender;
                SearchText = searchBar.Text;
                //HttpResponseMessage response = client.GetAsync("https://export.arxiv.org/api/query?search_query=all:electron&start=0&max_results=10").Result;
                //LabelText = response.Content.ReadAsStringAsync().Result;
                Task.Run(async () => await ShowAuthorResults(searchBar.Text));
            }
            catch(Exception ex)
            {
                LabelText = ex.Message;
            }
        }
        async Task ShowAuthorResults(string searchText)
        {
            
            BindingContext = this;
            await UpdateSearchProgressBar(0.2);
            using (XmlReader reader = XmlReader.Create("https://export.arxiv.org/api/query?search_query=au:" + searchText + "&max_results=" + max_results))
            {
                var feed = SyndicationFeed.Load(reader);
                if (feed.Items.Count() == 0)
                {
                    await UpdateSearchProgressBar(1.0);
                    LabelText = "Not Found";
                }

                await Task.Run(async () => await FillCharts(feed));
            }
        }
        async Task FillCharts(SyndicationFeed feed)
        {
            var chartHelper = new ChartHelper();
            await UpdateSearchProgressBar(0.0f);
            var (affiliation, authorFullName, categoriesCounts, authorsCounts) = await GetMainData(feed);
            MainAffiliation = "";
            AuthorFullName = "";
            MainAffiliation = affiliation;
            AuthorPublicationsNumber = feed.Items.Count().ToString();
            AuthorFullName = authorFullName;
            if (MainAffiliation == "")
                MainAffiliation = "Not Found";
            BindingContext = this;
            var categoryEntries = chartHelper.GetEntry(categoriesCounts);
            var authorEntries = chartHelper.GetEntry(authorsCounts);

            Chart1.Chart = chartHelper.FillBarChart(Chart1.Chart, categoryEntries);
            Chart2.Chart = chartHelper.FillDonutChart(Chart2.Chart, authorEntries);
            Chart2.HeightRequest = chartHelper.SetChartRequestHeight(authorEntries);
            chartHelper.SetChartRequestHeight(authorEntries);
            await UpdateSearchProgressBar(1.0f);
        }
        public async Task<(string, string, List<EntryPair>, List<EntryPair>)> GetMainData(SyndicationFeed feed)
        {
            int itemNumber = 0;
            var categoriesCounts = new List<EntryPair>();
            var authorsCounts = new List<EntryPair>();
            var affiliation = "";
            var fullName = "";
            var dataHelper = new DataHelper();
            foreach (SyndicationItem item in feed.Items)
            {
                var tmpAff = dataHelper.GetAffiliation(item, SearchText);
                if (affiliation.Length < tmpAff.Length)
                    affiliation = tmpAff;
                var tmpAut = dataHelper.GetLongerFullName(item, SearchText);
                if (fullName.Length < tmpAut.Length)
                    fullName = tmpAut;
                Debug.WriteLine(SearchProgressBar.Progress);

                await UpdateSearchProgressBar((double)itemNumber++ / feed.Items.Count());

                categoriesCounts = dataHelper.GetCategoriesCount(item, categoriesCounts);
                authorsCounts = dataHelper.GetAuthorsCount(item, authorsCounts);
            }
            return (affiliation, fullName, categoriesCounts, authorsCounts);
        }

        async Task UpdateSearchProgressBar(double value)
        {
            await Task.Run(() => Progress = value);
        }

    }

}
