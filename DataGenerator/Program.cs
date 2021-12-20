using System;
using Model;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
namespace DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            DatasetsFilePathes dataFiles = new DatasetsFilePathes()
            {
                fullnames = @".\..\data\generator\names.txt",
                countries = @".\..\data\generator\countries.txt",
                brands = @".\..\data\generator\brands.txt",
                fwnames = @".\..\data\generator\fnames.txt",
                emails = @".\..\data\generator\emails.txt"      
            };
            Service service = new Service();
            bool exit = false;
            while(!exit)
            {
                Console.WriteLine($"Enter the entity that you want to generate:"+
                "\n1.Footwear\n2.Client\n3.Order\n4.Review\n5.Exit");
                string input = Console.ReadLine(); 
                switch (input)
                {
                    case "1":
                        ProcessFootwearGen(dataFiles,service.footwearRepository);
                        break;
                    case "2":
                        ProcessClientGen(dataFiles, service.clientRepository);
                        break;
                    case "3":
                        ProcessOrderGen(service);
                        break;
                    case "4":
                        ProcessReviewGen(service);
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.Error.WriteLine("Error: invalid input");
                        break;
                }
                
            }
        }
        public static void ProcessFootwearGen(DatasetsFilePathes dataFiles, FootwearRepository repo)
        {
            int amount = GetAmountOfEntities();
            Console.WriteLine("Generating footwear...");
            string[] names = File.ReadAllText(dataFiles.fwnames).Split("\r\n");
            string[] brands = File.ReadAllText(dataFiles.brands).Split("\r\n");
            string[] countries = File.ReadAllText(dataFiles.countries).Split("\r\n");
            Random rand = new Random();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            List<Footwear> entities = new List<Footwear>();
            for(int i = 0; i < amount; i++)
            {
                entities.Add(new Footwear()
                {
                    name = names[rand.Next(0,names.Length)],
                    brand = brands[rand.Next(0,brands.Length)],
                    cost = rand.Next(10,700),
                    country_of_origin = countries[rand.Next(0,countries.Length)]
                });
            }
            repo.AddBulkData(entities);
            Console.WriteLine("[Gen. Footwear] Elapsed: "+sw.Elapsed);
            sw.Stop();
        }
        public static void ProcessClientGen(DatasetsFilePathes dataFiles, ClientRepository repo)
        {
            int amount = GetAmountOfEntities();
            Console.WriteLine("Generating clients...");
            string[] names = File.ReadAllText(dataFiles.fullnames).Split("\r\n");
            string[] emails = File.ReadAllText(dataFiles.emails).Split("\r\n");
            DateTime startDate = new DateTime(1965,1,1);
            DateTime finishDate = new DateTime(2003,6,1);
            TimeSpan range = finishDate - startDate;
            Random rand = new Random();
            List<Client> entities = new List<Client>();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            for(int i = 0; i < amount; i++)
            {
                TimeSpan randDate = new TimeSpan((long)(rand.NextDouble() * range.Ticks));
                entities.Add(new Client()
                {
                    fullname = names[rand.Next(0,names.Length)],
                    birthday_date = startDate + randDate,
                    email = emails[rand.Next(0,emails.Length)]
                });
            }
            repo.AddBulkData(entities);
            Console.WriteLine("[Gen. Clients] Elapsed: "+sw.Elapsed);
            sw.Stop();
        }
        public static void ProcessOrderGen(Service service)
        {
            int count = service.clientRepository.GetCount();
            if(count == 0)
            {
                Console.WriteLine("There are no clients to generate orders");
                return;
            }
            count = service.footwearRepository.GetCount();
            if(count == 0)
            {
                Console.WriteLine("There are no products to generate orders");
                return;
            }
            int amount = GetAmountOfEntities();
            Console.WriteLine("Generating orders and order items...");
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            service.orderRepository.GenerateOrders(amount);
            Console.WriteLine("[Gen. Orders] Elapsed: "+sw.Elapsed);
            sw.Stop();
        }
        public static void ProcessReviewGen(Service service)
        {
            int count = service.clientRepository.GetCount();
            if(count == 0)
            {
                Console.WriteLine("There are no clients to generate reviews");
                return;
            }
            count = service.footwearRepository.GetCount();
            if(count == 0)
            {
                Console.WriteLine("There are no products to generate reviews");
                return;
            }
            int amount = GetAmountOfEntities();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            service.reviewRepository.GenerateReviews(amount);
            Console.WriteLine("[Gen. Reviews] Elapsed:"+sw.Elapsed);
            sw.Stop();
        }
        public static int GetAmountOfEntities()
        {
            while(true)
            {
                Console.WriteLine("Enter the amount of the entities:");
                int amount;
                if(!int.TryParse(Console.ReadLine(), out amount))
                {
                    Console.Error.WriteLine("Error: invalid input");
                    continue;
                }
                if(amount <= 0)
                {
                    Console.Error.WriteLine("Error: invalid amount");
                    continue;
                }
                return amount;
            }
        }
    }
    struct DatasetsFilePathes
    {
        public string fullnames;
        public string countries;
        public string brands;
        public string fwnames;
        public string emails;
    }
    
}
