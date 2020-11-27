using Microcharts;
using Newtonsoft.Json;
using Plugin.Connectivity;
using ScholarStatistics.Helpers;
using ScholarStatistics.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ScholarStatistics
{

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class CategoryPage : TabbedPage
    {
        public ObservableCollection<Category> Categories { get; private set; } = new ObservableCollection<Category>();
        public ObservableCollection<Affiliation> Affiliations { get; private set; } = new ObservableCollection<Affiliation>();
        private readonly ChartHelper chartHelper = new ChartHelper();
        public CategoryPage()
        {
            InitializeComponent();
            GetCategoriesAndAffiliations();
            webView.Source = Path.Combine(WebAPIHelper.GetAPIBaseURL(),"heatmap");
        }

        void GetCategoriesAndAffiliations()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                var service = DependencyService.Get<IToastMessage>();
                service.LongAlert("No access to the Internet");
                return;
            }
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);
            client.Timeout = TimeSpan.FromSeconds(1000);
            using (var response = client.GetAsync(WebAPIHelper.GetAPIBaseURL() + "categories").Result)
            {
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                Categories = JsonConvert.DeserializeObject<ObservableCollection<Category>>(responseBody);
                var categoryEntries = chartHelper.GetCategoriesDifferenceDaysEntry(Categories.OrderByDescending(category => category.DifferenceBetweenPublicationsInDays).Take(20).ToList());
                var daysMaxEntries = chartHelper.GetCategoriesDaysEntry(Categories.ToList(), true);
                var daysMinEntries = chartHelper.GetCategoriesDaysEntry(Categories.ToList(), false);
                Chart1.Chart = chartHelper.FillBarChart(Chart1.Chart, categoryEntries);
                Chart2.Chart = chartHelper.FillBarChart(Chart1.Chart, daysMaxEntries);
                Chart3.Chart = chartHelper.FillBarChart(Chart2.Chart, daysMinEntries);
            }

            using (var response = client.GetAsync(WebAPIHelper.GetAPIBaseURL() + "affiliations").Result)
            {
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                Affiliations = JsonConvert.DeserializeObject<ObservableCollection<Affiliation>>(responseBody);
                BindingContext = this;
            }
        }

        async void OnCategoryTapped(object sender, ItemTappedEventArgs e)
        {
            Category category = e.Item as Category;
            await Navigation.PushAsync(new CategoryDetailPage(category));
        }

        async void Pin_InfoClicked(object sender, Xamarin.Forms.Maps.PinClickedEventArgs e)
        {

            var item = sender as Xamarin.Forms.Maps.Pin;
            var affiliation = Affiliations.Where(affiliation => affiliation.Name == item.Label).FirstOrDefault();
            await Navigation.PushAsync(new CategoryListPage(affiliation, Categories));
        }


    }

}
