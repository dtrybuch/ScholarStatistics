using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ScholarStatistics.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(BaseUrl))]
namespace ScholarStatistics.Droid
{
    public class BaseUrl : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/";
        }
    }
}