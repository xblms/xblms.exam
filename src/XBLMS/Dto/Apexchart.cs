using System.Collections.Generic;

namespace XBLMS.Dto
{
    public class Apexchart
    {
        public static List<Series_Data> GetSeries(List<KeyValuePair<string,List<int>>> list)
        {
            var result=new List<Series_Data>();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    result.Add(new Series_Data
                    {
                        Name = item.Key,
                        Data = item.Value
                    });
                }
            }
            return result;
        }
        public class Series_Data
        {
            public string Name { get; set; }
            public List<int> Data { get; set; }
        }
        public class Series_Data_Group
        {
            public string Name { get; set; }
            public string Group { get; set; }
            public List<int> Data { get; set; }
        }

        public static ChartOptions GetChartOptions(List<string> xList)
        {
            var result = new ChartOptions();
            result.Xaxis.Categories = xList;
            return result;
        }
        public class ChartOptions
        {
            //public ChartOptions_tooltip Tooltip = new ChartOptions_tooltip();
            public ChartOptions_chart Chart = new ChartOptions_chart();
            public ChartOptions_plotOptions PlotOptions = new ChartOptions_plotOptions();
            public ChartOptions_stroke Stroke = new ChartOptions_stroke();
            public ChartOptions_title Title = new ChartOptions_title();
            public ChartOptions_xaxis Xaxis = new ChartOptions_xaxis();
            //public ChartOptions_yaxis Yaxis = new ChartOptions_yaxis();
            public ChartOptions_fill Fill = new ChartOptions_fill();
            public ChartOptions_legend Legend = new ChartOptions_legend();
        }
        public class ChartOptions_tooltip
        {
            public bool Enabled { get; set; } = false;
        }

        public class ChartOptions_chart
        {
            public string Type { get; set; } = "bar";
            public int Height { get; set; } = 350;
            public bool Stacked { get; set; } = true;
            public ChartOptions_chart_toolbar Toolbar = new ChartOptions_chart_toolbar();
        }
        public class ChartOptions_chart_toolbar
        {
            public bool Show { get; set; } = false;
        }
        public class ChartOptions_plotOptions
        {
            public ChartOptions_plotOptions_bar Bar = new ChartOptions_plotOptions_bar();
        }
        public class ChartOptions_plotOptions_bar
        {
            public bool Horizontal { get; set; } = true;
            public ChartOptions_plotOptions_dataLabels DataLabels = new ChartOptions_plotOptions_dataLabels();
        }
        public class ChartOptions_plotOptions_dataLabels
        {
            public ChartOptions_plotOptions_dataLabels_total Total = new ChartOptions_plotOptions_dataLabels_total();
        }
        public class ChartOptions_plotOptions_dataLabels_total
        {
            public bool Enabled { get; set; } = true;
            public int OffsetX { get; set; } = 0;
            public ChartOptions_plotOptions_dataLabels_total_style Style = new ChartOptions_plotOptions_dataLabels_total_style();
        }
        public class ChartOptions_plotOptions_dataLabels_total_style
        {
            public string FontSize { get; set; } = "13px";
            public int FontWeight { get; set; } = 900;
        }
        public class ChartOptions_stroke
        {
            public int Width { get; set; }
            public List<string> colors { get; set; } = new List<string> { "#fff" };
        }
        public class ChartOptions_title
        {
            //public string Text { get; set; } = "chat title";
        }
        public class ChartOptions_xaxis
        {
            public List<string> Categories { get; set; }
        }
        public class ChartOptions_yaxis
        {
            public ChartOptions_yaxis_title Title = new ChartOptions_yaxis_title();
        }
        public class ChartOptions_yaxis_title
        {
            //public string Text { get; set; } = null;
        }
        public class ChartOptions_fill
        {
            public int Opacity { get; set; } = 1;
        }
        public class ChartOptions_legend
        {
            public string Position { get; set; } = "top";
            public string HorizontalAlign { get; set; } = "left";
            public int OffsetX { get; set; } = 40;
        }
    }
}
