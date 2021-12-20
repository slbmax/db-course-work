using System;
using System.Collections.Generic;
using Visualization;
namespace ViewLib
{
    public class View
    {
        public static void PrintError(string message)
        {
            Console.WriteLine("\n--- --- --- Error --- --- ---");
            Console.WriteLine(message);
            Console.WriteLine("--- --- --- Error --- --- ---\n");
        }
        public static void PrintInfo(string message)
        {
            Console.WriteLine(message);
        }
        public static void ShowInsertResult(string entity, int id)
        {
            Console.WriteLine($"\n--- --- --- Inserting --- --- ---");
            Console.WriteLine($"{entity} was succesfully added with id: [{id}]");
            Console.WriteLine($"--- --- --- Inserting --- --- ---\n");
        }
        public static void ShowDeleteResult(string entity, int id)
        {
            Console.WriteLine($"\n--- --- --- Deleting --- --- ---");
            Console.WriteLine($"{entity} with id: [{id}] was succesfully deleted ");
            Console.WriteLine($"--- --- --- Deleting --- --- ---\n");
        }
        public static void ShowUpdateResult(string entity, int id)
        {
            Console.WriteLine($"\n--- --- --- Updating --- --- --- ");
            Console.WriteLine($"{entity} with id: [{id}] was succesfully updated ");
            Console.WriteLine($"--- --- --- Updating --- --- ---\n");
        }
        public static void PrintEntity<T>(T entity)
        {
            Console.WriteLine($"\n--- --- --- {typeof(T).Name} --- --- ---");
            Console.WriteLine(entity.ToString());
            Console.WriteLine($"--- --- --- {typeof(T).Name} --- --- ---\n");
        }
        public static void PrintEntities<T>(List<T> entities)
        {
            if(entities.Count == 0)
            {
                Console.WriteLine("There aren`t any results of the search");
                return;
            }
            else
            {
                Console.WriteLine($"There are {entities.Count} {typeof(T).Name}s found:");
            }
            foreach(T ent in entities)
            {
                PrintEntity<T>(ent);
            }
        }
        public static void FootwearRatingsChart(List<int> ratings, string fw_name)
        {
            Charts.CreateFootwearRatingsChart(ratings,fw_name,@"C:\Users\Макс\myprojects\db_coursework\data\RatingsChart.png");
            Console.WriteLine(@"--- Chart was saved as 'RatingsChart' in db_coursework\data ---");
        }
        public static void FootwearAgeCategoriesChart(List<double> ageGroups, string fw_name)
        {
            try
            {
                Charts.CreateFootwearAgeCategoriesChart(ageGroups, fw_name);
                Console.WriteLine(@"--- Chart was saved as 'AgeGroups' in db_coursework\data ---");
            }
            catch(Exception ex)
            {
                PrintError(ex.Message);
            }
        }
        public static void HashIndexChart(double[] without, double[] with)
        {
            Charts.CreateHashIndexChart(without,with);
            Console.WriteLine(@"--- Chart was saved as 'HashChart' in db_coursework\data ---");
        }
        public static void BTreeIndexChart(double[] without, double[] with)
        {
            Charts.CreateBtreeIndexChart(without,with);
            Console.WriteLine(@"--- Chart was saved as 'BTreeChart' in db_coursework\data ---");
        }
        public static void IncomesStatisticsChart(List<double> incomes, int year)
        {
            Charts.CreateIncomesStatisticsChart(incomes, year);
            Console.WriteLine(@"--- Chart was saved as 'IncomesChart' in db_coursework\data ---");
        }

    }
}
