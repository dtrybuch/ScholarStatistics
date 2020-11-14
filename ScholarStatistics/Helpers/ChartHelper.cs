using Microcharts;
using ScholarStatistics.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ScholarStatistics.Helpers
{
    public class ChartHelper
    {
        private INativeFont NativeFont => DependencyService.Get<INativeFont>();
        private Random rnd = new Random();
        public BarChart FillBarChart(Chart chart, List<ChartEntry> data)
        {
            if (data.Count > 0)
            {
                chart = new BarChart()
                {
                    Entries = data,
                    LabelTextSize = NativeFont.GetNativeSize(10)

                };
            }
            return (BarChart) chart;
        }
        public DonutChart FillDonutChart(Chart chart, List<ChartEntry> data)
        {
            float labelSize = 1;
            if (data.Count < 10)
                labelSize = NativeFont.GetNativeSize(15);
            if(data.Count >= 10 && data.Count < 50)
                labelSize = NativeFont.GetNativeSize(10);
            if (data.Count > 0)
            {
                chart = new DonutChart()
                {
                    Entries = data,
                    LabelTextSize = labelSize,
                    LabelMode = LabelMode.RightOnly,
                    GraphPosition = GraphPosition.AutoFill

                };
            }
            return (DonutChart) chart;
        }
        public List<ChartEntry> GetCategoriesDifferenceDaysEntry(List<Category> categories)
        {
            var result = new List<ChartEntry>();
            foreach (var category in categories)
            {
                if (category.Name == "") continue;
                byte[] bytes = new byte[3];
                rnd.NextBytes(bytes);
                var red = bytes[0];
                var green = bytes[1];
                var blue = bytes[2];
                result.Add(new ChartEntry((float)category.DifferenceBetweenPublicationsInDays)
                {
                    Label = category.Name,
                    ValueLabel = category.DifferenceBetweenPublicationsInDays.ToString("0.00"),
                    Color = new SKColor(red, green, blue)
                });
            }
            return result;
        }

        public List<ChartEntry> GetCategoryDaysPercentageEntry(Category category)
        {
            var result = new List<ChartEntry>();
            AddPercentageValueToChartEntry(result, "Monday", category.PercentageOfMondays);
            AddPercentageValueToChartEntry(result, "Tuesday", category.PercentageOfTuesdays);
            AddPercentageValueToChartEntry(result, "Wednesday", category.PercentageOfWednesdays);
            AddPercentageValueToChartEntry(result, "Thursday", category.PercentageOfThursdays);
            AddPercentageValueToChartEntry(result, "Friday", category.PercentageOfFridays);
            AddPercentageValueToChartEntry(result, "Saturday", category.PercentageOfSaturdays);
            AddPercentageValueToChartEntry(result, "Sunday", category.PercentageOfSundays);
            return result;
        }

        private void AddPercentageValueToChartEntry(List<ChartEntry> entries, string day, double value)
        {
            byte[] bytes = new byte[3];
            rnd.NextBytes(bytes);
            var red = bytes[0];
            var green = bytes[1];
            var blue = bytes[2];
            entries.Add(new ChartEntry((float)value)
            {
                Label = day,
                ValueLabel = Math.Round(value).ToString() + "%",
                Color = new SKColor(red, green, blue)
            });
        }

        public float SetChartRequestHeight(List<ChartEntry> data)
        {
            if (data.Count < 10)
                return 300;
            if(data.Count >= 10 && data.Count < 25)
                return 600;
            if (data.Count >= 25 && data.Count <= 70)
                return 1000;
            return 1300;
        }
    }
}
