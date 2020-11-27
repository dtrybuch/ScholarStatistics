using ScholarStatistics.Helpers;
using ScholarStatistics.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScholarStatistics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryDetailPage : ContentPage
    {
        public Category Category { get; set; }
        private readonly ChartHelper chartHelper = new ChartHelper();
        public CategoryDetailPage(Category category)
        {
            Category = category;
            InitializeComponent();
            var categoryEntries = chartHelper.GetCategoryDaysPercentageEntry(category);
            Chart1.Chart = chartHelper.FillDonutChart(Chart1.Chart, categoryEntries);
            webViewIndicator.IsVisible = true;
            webViewIndicator.IsRunning = true;
            webView.Source = Path.Combine(WebAPIHelper.GetAPIBaseURL(), $"heatmap/{category.CategoryId}");
            BindingContext = this;
        }
        private void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            IsBusy = false;
            webViewIndicator.IsVisible = false;
            webViewIndicator.IsRunning = false;
        }
    }
}