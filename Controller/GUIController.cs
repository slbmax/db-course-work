using System;
using System.Collections.Generic;
namespace ControllerLib
{
    public static class GUIController
    {
        public static void RunConsoleInterface(Controller controller)
        { 
            Console.WriteLine("Hello!");
            while(true)
            {
                Console.WriteLine("\nSelect the entity to work with:\n1.Footwear\n2.Order"
                +"\n3.Client\n4.Review\n\n\n5.[Chart] Ratings of certain product\n"+
                "6.[Chart] Age groups of product\n7.[Chart] Hash index chart\n"+
                "8.[Chart] B-tree index chart\n9.[Chart] Incomes statistics (of the special year)"+
                "\n10.Generate product report\n11.Exit");
                int option;
                bool num = int.TryParse(Console.ReadLine(), out option);
                if(!num){Console.WriteLine("\n--Wrong option--\n"); continue;}
                switch(option)
                {
                    case 1:
                        ProcessFootwearOptions(controller);
                        break;
                    case 2:
                        ProcessOrderOptions(controller);
                        break;
                    case 3:
                        ProcessClientOptions(controller);
                        break;
                    case 4:
                        ProcessReviewOptions(controller);
                        break;
                    case 5:
                        ProcessFootwearRatings(controller);
                        break;
                    case 6:
                        controller.GenerateFootwearAgeCategoriesChart(GetIdOfEntity("footwear","get age groups"));
                        break;
                    case 7:
                        controller.GenerateHashIndexChart();
                        break;
                    case 8:
                        controller.GenerateBtreeIndexChart();
                        break;
                    case 9:
                        ProcessIncomesStatisticsChart(controller);
                        break;
                    case 10:
                        controller.GenerateProductReport(GetIdOfEntity("footwear", "generate report"));
                        break;
                    case 11:
                        return;
                    default:
                        Console.WriteLine("\n--Wrong option--\n");
                        break;
                }   
            }
        }
        public static void ProcessFootwearOptions(Controller controller)
        {
            while(true)
            {
                Console.WriteLine("\nSelect the option:\n1.Insert new pair of footwear\n2.Delete existing product"+
                "\n3.Update existing product\n4.Get footwear by id\n5.Get footwear by pair name"
                + "\n6.Get footwear by pair name in the cost range\n7.Exit footwear options");
                string option = Console.ReadLine();
                switch(option)
                {
                    case "1":
                        ProcessInsertFootwear(controller);
                        break;
                    case "2":
                        controller.DeleteFootwear(GetIdOfEntity("footwear","delete it"));
                        break;
                    case "3":
                        ProcessUpdateFootwear(controller);
                        break;
                    case "4":
                        controller.GetFootwearById(GetIdOfEntity("footwear", "search it"));
                        break;
                    case "5":
                        Console.WriteLine("Enter pair name:");
                        string name = Console.ReadLine();
                        controller.GetFootwearByName(name);
                        break;
                     case "6":
                        ProcessCostRangeFootwear(controller);
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("\n--Wrong entity option--\n");
                        break;
                } 
            }
        }
        public static void ProcessInsertFootwear(Controller controller)
        {
            Console.WriteLine("\nEnter footwear info:");
            Console.WriteLine("Name:");
            string pairName = Console.ReadLine();
            Console.WriteLine("Brand:");
            string pairBrand = Console.ReadLine();
            int pairCost = 0;
            while(true)
            {
                Console.WriteLine("Price:");
                bool c = int.TryParse(Console.ReadLine(), out pairCost);
                if(c) break;
                else Console.WriteLine("Error: Price should be integer");
            }
            Console.WriteLine("Country of origin:");
            string pairOrigin = Console.ReadLine();
            controller.InsertFootwear(pairName,pairBrand,pairCost,pairOrigin);
        }
        static int GetIdOfEntity(string entity, string process)
        {
            int id = 0;
            while(true)
            {
                Console.WriteLine($"\nEnter {entity} id to {process}: ");
                bool idParse = int.TryParse(Console.ReadLine(), out id);
                if(idParse) break;
                else Console.WriteLine("id should be integer");
            }
            return id;
        }
        public static void ProcessUpdateFootwear(Controller controller)
        {
            Console.WriteLine("\nEnter footwear info:");
            int product_id = GetIdOfEntity("footwear", "update");
            Console.WriteLine("New name:");
            string pairName = Console.ReadLine();
            Console.WriteLine("New brand:");
            string pairBrand = Console.ReadLine();
            int pairCost = 0;
            while(true)
            {
                Console.WriteLine("New price:");
                bool c = int.TryParse(Console.ReadLine(), out pairCost);
                if(c) break;
                else Console.WriteLine("Error: Price should be integer");
            }
            Console.WriteLine("New country of origin:");
            string pairOrigin = Console.ReadLine();
            controller.UpdateFootwear(product_id, pairName, pairBrand, pairCost, pairOrigin);
        }

        public static void ProcessCostRangeFootwear(Controller controller)
        {
            Console.WriteLine("Enter pair name:");
            string pn = Console.ReadLine();
            int lp = 0;
            while(true)
            {
                Console.WriteLine("Lowest price:");
                bool c = int.TryParse(Console.ReadLine(), out lp);
                if(c) break;
                else Console.WriteLine("Error: Price should be integer");
            }
            int hp = 0;
            while(true)
            {
                Console.WriteLine("Highest price:");
                bool c = int.TryParse(Console.ReadLine(), out hp);
                if(c) break;
                else Console.WriteLine("Error: Price should be integer");
            }
            controller.GetFootwearByRangeCost(pn,lp,hp);
        }

        public static void ProcessOrderOptions(Controller controller)
        {
            while(true)
            {
                Console.WriteLine("\nSelect the option:\n1.Create new order\n2.Delete existing order\n3.Get order by id"+
                "\n4.Get orders of certain client\n6.Exit order options");
                string option = Console.ReadLine();
                switch(option)
                {
                    case "1":
                        ProcessInsertOrder(controller);
                        break;
                    case "2":
                        controller.DeleteOrder(GetIdOfEntity("order","delete it"));
                        break;
                    case "3":
                        controller.GetOrderById(GetIdOfEntity("order", "search it"));
                        break;
                    case "4":
                        controller.GetOrderByClientId(GetIdOfEntity("client", "find his orders"));
                        break;
                    case "5":
                        
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("\n--Wrong entity option--\n");
                        break;
                } 
            }
            
        }
        public static void ProcessInsertOrder(Controller controller)
        {
            Console.WriteLine("\nEnter order info:");
            int client_id = GetIdOfEntity("client","insert order");
            Console.WriteLine("Payment method:");
            string pm = Console.ReadLine();
            List<int> order_items = new List<int>();
            while(true)
            {
                int id = 0;
                Console.WriteLine("Enter id of the product to put in the order (type '-1' to confirm order):");
                bool c = int.TryParse(Console.ReadLine(), out id);
                if(c && id==-1) 
                    break;
                else if (c)
                    order_items.Add(id);
                else Console.WriteLine("Error: id should be integer");
            }
            controller.InsertOrder(client_id, pm, order_items, DateTime.Now);
        }

        public static void ProcessReviewOptions(Controller controller)
        {
            while(true)
            {
                Console.WriteLine("\nSelect the option:\n1.Create new review\n2.Delete existing review"+
                "\n3.Update existing review\n4.Get reviews by client id\n5.Get reviews about footwear (by footwear id)"+
                "\n6.Exit order options");
                string option = Console.ReadLine();
                switch(option)
                {
                    case "1":
                        ProcessInsertReview(controller);
                        break;
                    case "2":
                        controller.DeleteReview(GetIdOfEntity("review","delete it"));
                        break;
                    case "3":
                        ProcessUpdateReview(controller);
                        break;
                    case "4":
                        controller.GetReviewsByClientId(GetIdOfEntity("client","search reviews"));
                        break;
                    case "5":
                        controller.GetReviewsByProductId(GetIdOfEntity("product","search reviews"));
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("\n--Wrong entity option--\n");
                        break;
                } 
            }
        }
        public static void ProcessInsertReview(Controller controller)
        {
            Console.WriteLine("\nEnter review info:");
            Console.WriteLine("Content:");
            string rCont = Console.ReadLine();
            int rating = 0;
            while(true)
            {
                Console.WriteLine("Rating (from 1 to 10):");
                bool c = int.TryParse(Console.ReadLine(), out rating);
                if(c) break;
                else Console.WriteLine("Error: rating should be integer");
            }
            int client_id = 0;
            while(true)
            {
                Console.WriteLine("Author id(client):");
                bool c = int.TryParse(Console.ReadLine(), out client_id);
                if(c) break;
                else Console.WriteLine("Error: id should be integer");
            }
            int product_id= 0;
            while(true)
            {
                Console.WriteLine("Product id:");
                bool c = int.TryParse(Console.ReadLine(), out product_id);
                if(c) break;
                else Console.WriteLine("Error: id should be integer");
            }
            controller.InsertReview(rCont, rating, DateTime.Now, client_id, product_id);
        }
        static void ProcessUpdateReview(Controller controller)
        {
            int review_id = GetIdOfEntity("review", "update");
            Console.WriteLine("\nEnter new review content: ");
            string content = Console.ReadLine();
            int rating = 0;
            while(true)
            {
                Console.WriteLine("\nEnter new reviews rating (1-10): ");
                bool rParse = int.TryParse(Console.ReadLine(), out rating);
                if(rParse) break;
                else Console.WriteLine("rating should be integer");
            }
            controller.UpdateReview(review_id, content, rating);
        }

        public static void ProcessClientOptions(Controller controller)
        {
            while(true)
            {
                Console.WriteLine("\nSelect the option:\n1.Insert new client\n2.Delete existing client"+
                "\n3.Update existing client\n4.Get client by id\n5.Get client by fullname\n6.Exit client options");
                string option = Console.ReadLine();
                switch(option)
                {
                    case "1":
                        ProcessInsertClient(controller);
                        break;
                    case "2":
                        controller.DeleteClient(GetIdOfEntity("client","delete it"));
                        break;
                    case "3":
                        ProcessUpdateClient(controller);
                        break;
                    case "4":
                        controller.GetClientById(GetIdOfEntity("client","search it"));
                        break;
                    case "5":
                        Console.WriteLine("Enter full name of client: ");
                        string name = Console.ReadLine();
                        controller.GetClientByFullName(name);
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("\n--Wrong entity option--\n");
                        break;
                } 
            }
        }
        public static void ProcessInsertClient(Controller controller)
        {
            Console.WriteLine("\nEnter client info:");
            Console.WriteLine("Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Email:");
            string email = Console.ReadLine();
            DateTime birthDate;
            while(true)
            {
                Console.Write("Birth date: ");
                if(DateTime.TryParse(Console.ReadLine(), out birthDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: birth date must be in date format. Try again.");
                }
            }
            controller.InsertClient(name, birthDate, email);
        }
        public static void ProcessUpdateClient(Controller controller)
        {
            Console.WriteLine("\nEnter new client info:");
            int client_id = GetIdOfEntity("client", "update");
            Console.WriteLine("New name:");
            string name = Console.ReadLine();
            Console.WriteLine("New Email:");
            string email = Console.ReadLine();
            DateTime birthDate;
            while(true)
            {
                Console.Write("Birth date: ");
                if(DateTime.TryParse(Console.ReadLine(), out birthDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: birth date must be in date format. Try again.");
                }
            }
            controller.UpdateClient(client_id, name, birthDate, email);
        }

        public static void ProcessFootwearRatings(Controller controller)
        {
            int id = GetIdOfEntity("product","check product ratings");
            controller.GetFootwearRatings(id);
        }
        public static void ProcessIncomesStatisticsChart(Controller controller)
        {
            int year = 0;
            while(true)
            {
                Console.WriteLine($"\nEnter the year to check order stats: ");
                bool idParse = int.TryParse(Console.ReadLine(), out year);
                if(idParse && year > 0) break;
                else Console.WriteLine("year should be real integer");
            }
            controller.GenerateIncomesStatisticsChart(year);
        }
    }
}
