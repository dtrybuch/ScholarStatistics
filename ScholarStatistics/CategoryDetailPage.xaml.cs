using ScholarStatistics.Models;
using System;
using System.Collections.Generic;
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
        public CategoryDetailPage(Category category)
        {
            Category = category;
            InitializeComponent();
            BindingContext = this;
        }
    }
}