using System;
using ScottPlot;
using System.Collections.Generic;

namespace Visualization
{
    public static class Charts 
    {
        public static void CreateFootwearRatingsChart(List<int> ratings, string fw_name, string filePath)
        {
            var plt = new ScottPlot.Plot(600, 400);
            int pointCount = 10;
            double[] xs = DataGen.Consecutive(pointCount,1);
            double[] ys = MakeYAsixs(ratings, pointCount);
            plt.PlotBar(xs, ys,horizontal: true);
            plt.Title(fw_name + " ratings");
            plt.Grid(lineStyle: LineStyle.Solid);
            string[] labels = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"};
            plt.YTicks(xs, labels);
            plt.XTicks(ForXTicks(ratings));
            plt.SaveFig(filePath);
        }
        private static double[] MakeYAsixs(List<int> ratings, int count)
        {
            double[] newYs = new double[count];
            for(int i = 0; i < ratings.Count; i++)
            {
                int rating = ratings[i];
                newYs[rating-1] +=1;
            }
            return newYs;
        }
        private static string[] ForXTicks(List<int> ratings)
        {
            ratings.Sort();
            int max = 0;
            int current = 0;
            int currentMax = 0;
            foreach(int i in ratings)
            {
                if(current != i)
                {
                    current = i;
                    currentMax = 0;
                }
                currentMax++;
                if(currentMax>max)
                {
                    max = currentMax;
                }
            }
            string[] result = new string[max+1];
            for(int i = 0; i<result.Length; i++)
            {
                result[i] = i.ToString();
            }
            return result;
        }
        public static void CreateFootwearAgeCategoriesChart(List<double> ageGroups, string fw_name)
        {
            var plt = new ScottPlot.Plot(600, 500);
            int nulls = 0;
            foreach(double d in ageGroups)
            {
                if(d == 0)
                {
                    nulls++;
                }
            }
            if(nulls == ageGroups.Count)
            {
                throw new Exception("There aren`t any customers who bought this product.");
            }
            double[] values = ageGroups.ToArray();
            var pie = plt.AddPie(values);
            string[] labels = { "0-18", "19-25", "26-40", "41-50", "51+" };
            pie.SliceLabels = labels;
            plt.Legend();
            plt.Title(fw_name);
            pie.Explode = true;
            pie.ShowValues = true;
            plt.SaveFig(@"C:\Users\Макс\myprojects\db_coursework\data\AgeGroups.png");
        }
        public static void CreateHashIndexChart(double[] withoutHash, double[] withHash)
        {
            var plt = new ScottPlot.Plot(600, 400);
            double[][] barHeights = { withoutHash, withHash };
            string[] seriesLabels = { "Without index", "With index" };
            string[] groupLabels = { "search client by fullname", "search product by title"};
            plt.AddBarGroups(groupLabels, seriesLabels, barHeights,null);
            plt.Legend(location: Alignment.UpperRight);
            plt.SetAxisLimits(yMin: 0);
            plt.Title("Hash index");
            plt.YLabel("Milliseconds");
            plt.SaveFig(@"C:\Users\Макс\myprojects\db_coursework\data\HashChart.png");
        }
        public static void CreateBtreeIndexChart(double[] withoutHash, double[] withHash)
        {
            var plt = new ScottPlot.Plot(600, 400);
            double[][] barHeights = { withoutHash, withHash };
            string[] seriesLabels = { "Without index", "With index" };
            string[] groupLabels = { "search 'good' review in timespan", "search product in cost range"};
            plt.AddBarGroups(groupLabels, seriesLabels, barHeights,null);
            plt.Legend(location: Alignment.UpperRight);
            plt.SetAxisLimits(yMin: 0);
            plt.Title("B-tree index");
            plt.YLabel("Milliseconds");
            plt.SaveFig(@"C:\Users\Макс\myprojects\db_coursework\data\BTreeChart.png");
        }
        public static void CreateIncomesStatisticsChart(List<double> incomes, int year)
        {
            var plt = new ScottPlot.Plot(800, 400);
            double[] values = incomes.ToArray();
            double[] xs = DataGen.Consecutive(12);
            string[] labels = new string[12]{"January","February",
            "March","April","May","June","July","August","September","October","November","December"};
            plt.AddScatter(xs, values);
            plt.XTicks(xs, labels);
            plt.XAxis.TickLabelStyle(rotation: 30);
            plt.Title($"Incomes in {year}");
            plt.SaveFig(@$"C:\Users\Макс\myprojects\db_coursework\data\IncomesPer{year}.png");
        }
    }
}
