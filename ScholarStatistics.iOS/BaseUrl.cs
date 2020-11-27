using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Foundation;
using ScholarStatistics.iOS;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(BaseUrl))]
namespace ScholarStatistics.iOS
{
   public class BaseUrl : IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath;
        }
    }
}