using Microcharts;
using ScholarStatistics.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<ChartEntry> GetCategoriesDaysEntry(List<Category> categories, bool isMax)
        {
            var result = new List<ChartEntry>();
            Dictionary<string, int> days = new Dictionary<string, int>();
            if(isMax)
                days = GetCategoriesCountInDays(categories, (category) => GetMaxDay(category));
            else
                days = GetCategoriesCountInDays(categories, (category) => GetMinDay(category));
            foreach (var day in days)
            {
                byte[] bytes = new byte[3];
                rnd.NextBytes(bytes);
                var red = bytes[0];
                var green = bytes[1];
                var blue = bytes[2];
                result.Add(new ChartEntry((float)day.Value)
                {
                    Label = day.Key,
                    ValueLabel = day.Value.ToString(),
                    Color = new SKColor(red, green, blue)
                });
            }
            return result;
        }
        private Dictionary<string,int> GetCategoriesCountInDays(List<Category> categories, Func<Category, string> comparator)
        {
            var result = new Dictionary<string, int>();
            foreach (var day in categories[0].CountOfDays)
            {
                result.Add(day.Key, 0);
            }
            foreach (var category in categories)
            {
                var maxDay = comparator(category);
                result[maxDay]++;
            }
            return result;
        }
        private string GetMaxDay(Category category)
        {
           var max = new Tuple<string, int>("Monday", category.CountOfMondays);
            foreach (var day in category.CountOfDays)
            {
                max = GetHigherValue(max, day.Key, day.Value);
            }
            return max.Item1;
        }
        private string GetMinDay(Category category)
        {
            var min = new Tuple<string, int>("Monday", category.CountOfMondays);
            foreach (var day in category.CountOfDays)
            {
                min = GetLowerValue(min, day.Key, day.Value);
            }
            return min.Item1;
        }
        private Tuple<string, int> GetHigherValue(Tuple<string, int> max, string day, int value)
        {
            if(max.Item2 < value)
                max = new Tuple<string, int>(day, value);
            return max;
        }
        private Tuple<string, int> GetLowerValue(Tuple<string, int> min, string day, int value)
        {
            if (min.Item2 > value)
                min = new Tuple<string, int>(day, value);
            return min;
        }
        public List<ChartEntry> GetCategoryDaysPercentageEntry(Category category)
        {
            var result = new List<ChartEntry>();
            foreach (var day in category.CountOfDays)
            {
                AddPercentageValueToChartEntry(result, day.Key, day.Value);
            }
            
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
