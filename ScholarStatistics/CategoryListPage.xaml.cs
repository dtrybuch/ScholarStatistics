using Newtonsoft.Json;
using ScholarStatistics.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScholarStatistics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryListPage : ContentPage
    {
        public Affiliation Affiliation { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public CategoryListPage(Affiliation affiliation)
        {
            Affiliation = affiliation;
            InitializeComponent();
            GetCategory();
        }

        public void GetCategory()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                var service = DependencyService.Get<IToastMessage>();
                service.LongAlert("No access to the Internet");
                return;
            }
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient client = new HttpClient(clientHandler);
            using (var response = client.GetAsync("https://10.0.2.2:44320/affiliations/" + Affiliation.AffiliationId).Result)
            {
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                Categories = JsonConvert.DeserializeObject<ObservableCollection<Category>>(responseBody);
                BindingContext = this;
            }
        }
    }
}