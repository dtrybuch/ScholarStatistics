using Newtonsoft.Json;
using ScholarStatistics.Helpers;
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
        public ObservableCollection<Category> AllCategories { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public CategoryListPage(Affiliation affiliation, ObservableCollection<Category> allCategories)
        {
            Affiliation = affiliation;
            AllCategories = allCategories;
            InitializeComponent();
            GetCategories();
        }

        public void GetCategories()
        {
            Categories = new ObservableCollection<Category>(AllCategories.Where(allcategory => Affiliation.CategoriesUsingInThisAffiliationFK.Contains(allcategory.CategoryId)));
            BindingContext = this;
        }
    }
}