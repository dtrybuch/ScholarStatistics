using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using ScholarStatistics.iOS.Dependencies;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(NativeFont))]
namespace ScholarStatistics.iOS.Dependencies
{
    public class NativeFont : INativeFont
    {
        public float GetNativeSize(float size)
        {
            return size * (float)UIScreen.MainScreen.Scale;
        }
    }
}