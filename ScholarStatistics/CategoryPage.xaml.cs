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
    public partial class CategoryPage : ContentPage
    {
        public ObservableCollection<Category> Categories { get; private set; } = new ObservableCollection<Category>();
        public CategoryPage()
        {
            InitializeComponent();
            GetCategories();
        }

        void GetCategories()
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
            using (var response = client.GetAsync("https://10.0.2.2:44320/categories").Result)
            {
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                //CategoriesView.ItemsSource = Categories;
                Categories = JsonConvert.DeserializeObject<ObservableCollection<Category>>(responseBody);
                BindingContext = this;
            }
        }

        async void OnCategoryTapped(object sender, ItemTappedEventArgs e)
        {
            Category category = e.Item as Category;
            await Navigation.PushAsync(new CategoryDetailPage(category));
        }
    }

}
