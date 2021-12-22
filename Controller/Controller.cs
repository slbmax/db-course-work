using System;
using Model;
using ViewLib;
using System.Collections.Generic;
using Visualization;
using System.IO;
using System.Diagnostics;
namespace ControllerLib
{
    public class Controller
    {
        private Service _service;
        public Controller()
        {
            _service = new Service();
        }
        public void InsertFootwear(string pairName, string pairBrand, int pairCost, string pairOrigin)
        {
            if(pairCost <= 0)
            {
                View.PrintError("Price can`t be less than 0");
                return;
            }
            Footwear fw = new Footwear()
            {
                name = pairName,
                brand = pairBrand,
                cost = pairCost,
                country_of_origin = pairOrigin
                
            };
            try
            {
                fw.id = _service.footwearRepository.Insert(fw);
                View.ShowInsertResult(typeof(Footwear).Name, fw.id);
            }
            catch
            {
                View.PrintError($"Cannot insert footwear");
            }
        }
        public void DeleteFootwear(int id)
        {
            Footwear fw = _service.footwearRepository.GetById(id);
            if(fw == null)
            {
                View.PrintError($"Cannot delete unexisting product");
                return;
            }
            try
            {
                _service.orderItemRepository.DeleteByProductId(id);
                _service.reviewRepository.DeleteByProductId(id);
                _service.footwearRepository.DeleteById(id);
                View.ShowDeleteResult(typeof(Footwear).Name, fw.id);
            }
            catch
            {
                View.PrintError($"Cannot delete footwear");
            }
        }
        public void UpdateFootwear(int fw_id,string pairName, string pairBrand, int pairCost, string pairOrigin)
        {
            Footwear oldFw = _service.footwearRepository.GetById(fw_id);
            if(oldFw==null)
            {
                View.PrintError($"Cannot update -- footwear with id [{fw_id}] does not exist");
                return;
            }
            if(pairCost <= 0)
            {
                View.PrintError("Price can`t be less than 0");
                return;
            }
            Footwear fw = new Footwear()
            {
                id = fw_id,
                name = pairName,
                brand = pairBrand,
                cost = pairCost,
                country_of_origin = pairOrigin
            };
            try
            {
                _service.footwearRepository.Update(fw);
                View.ShowUpdateResult(typeof(Footwear).Name, fw.id);
            }
            catch (Exception ex)
            {
                View.PrintError($"{ex.Message}");
            }
        }
        public void InsertOrder(int client_id, string pm, List<int> items, DateTime date)
        {
            if(items.Count == 0)
            {
                View.PrintError("Order must contain items");
                return;
            }
            Client client = _service.clientRepository.GetById(client_id);
            if(client==null)
            {
                View.PrintError($"Cannot insert -- client with id [{client_id}] does not exist");
                return;
            }
            foreach(int id in items)
            {
                Footwear oldFw = _service.footwearRepository.GetById(id);
                if(oldFw==null)
                {
                    View.PrintError($"Cannot insert -- footwear with id [{id}] does not exist");
                    return;
                }
            }
            Order order = new Order()
            {
                client_id = client_id,
                checkout_date = date,
                payment_method = pm
            };
            try
            {
                order.id = _service.orderRepository.Insert(order);
                foreach(int id in items)
                {
                    OrderItem oi = new OrderItem()
                    {
                        product_id = id,
                        order_id = order.id
                    };
                    _service.orderItemRepository.Insert(oi);
                }
                View.ShowInsertResult(typeof(Order).Name, order.id);
            }
            catch (Exception ex)
            {
                View.PrintError($"{ex.Message}");
            }
        }
        public void DeleteOrder(int id)
        {
            Order o = _service.orderRepository.GetById(id);
            if(o == null)
            {
                View.PrintError($"Cannot delete unexisting order");
                return;
            }
            try
            {
                _service.orderItemRepository.DeleteByOrderId(id);
                _service.orderRepository.DeleteById(id);
                View.ShowDeleteResult(typeof(Order).Name, id);
            }
            catch
            {
                View.PrintError($"Cannot delete order");
            }
        }
        public void InsertClient(string name, DateTime date, string email)
        {
            if(date.Year > DateTime.Now.Year - 16 || date < new DateTime(1950,1,1))
            {
                Console.WriteLine("Something is wrong with the client age");
                return;
            }
            Client cl = new Client()
            {
              fullname = name,
              birthday_date = date,
              email = email  
            };
            try
            {
                cl.id = _service.clientRepository.Insert(cl);
                View.ShowInsertResult(typeof(Client).Name, cl.id);
            }
            catch
            {
                View.PrintError($"Cannot insert footwear");
            }
        }
        public void InsertReview(string cont, int rat, DateTime date, int client_id, int prod_id)
        {
            if(rat <= 0 || rat > 10)
            {
                View.PrintError($"Rating should be in range from 1 to 10");
                return;
            }
            Client c = _service.clientRepository.GetById(client_id);
            if(c == null)
            {
                View.PrintError($"Cannot insert -- client with id [{client_id}] does not exist");
                return;
            }
            Footwear fw = _service.footwearRepository.GetById(prod_id);
            if(fw==null)
            {
                View.PrintError($"Cannot insert -- footwear with id [{prod_id}] does not exist");
                return;
            }
            Review review = new Review()
            {
                content = cont,
                rating = rat,
                created_at = date,
                client_id = client_id,
                product_id = prod_id
            };
            try
            {
                review.id = _service.reviewRepository.Insert(review);
                View.ShowInsertResult(typeof(Review).Name, review.id);
            }
            catch
            {
                View.PrintError($"Cannot insert review");
            }
        }
        public void DeleteClient(int id)
        {
            Client c = _service.clientRepository.GetById(id);
            if(c == null)
            {
                View.PrintError($"Cannot delete unexisting client");
                return;
            }
            try
            {
                _service.orderRepository.DeleteByClientId(id);
                _service.reviewRepository.DeleteByClientId(id);
                _service.clientRepository.DeleteById(id);
                View.ShowDeleteResult(typeof(Client).Name, id);
            }
            catch (Exception ex)
            {
                View.PrintError($"{ex.Message}");
            }
        }
        public void UpdateClient(int id, string name, DateTime date, string email)
        {
            Client c = _service.clientRepository.GetById(id);
            if(c == null)
            {
                View.PrintError($"Cannot update unexisting client");
                return;
            }
            if(date.Year > DateTime.Now.Year - 16 || date < new DateTime(1950,1,1))
            {
                Console.WriteLine("Something is wrong with the client age");
                return;
            }
            Client cl = new Client()
            {
                id = id,
                fullname = name,
                birthday_date = date,
                email = email  
            };
            try
            {
                _service.clientRepository.Update(cl);
                View.ShowUpdateResult(typeof(Client).Name, id);
            }
            catch
            {
                View.PrintError($"Cannot update client");
            }
        }
        public void DeleteReview(int id)
        {
            Review r = _service.reviewRepository.GetById(id);
            if(r == null)
            {
                View.PrintError($"Cannot delete unexisting review");
                return;
            }
            try
            {
                _service.reviewRepository.DeleteById(id);
                View.ShowDeleteResult(typeof(Review).Name, id);
            }
            catch (Exception ex)
            {
                View.PrintError(ex.Message);
            } 
        }
        public void GetFootwearById(int id)
        {
            Footwear fw = _service.footwearRepository.GetById(id);
            if(fw==null)
            {
                View.PrintError($"Cannot get entity -- footwear with id [{id}] does not exist");
                return;
            }
            View.PrintEntity<Footwear>(fw); 
        }
        public void GetOrderById(int id)
        {
            Order or = _service.orderRepository.GetById(id);
            if(or == null)
            {
                View.PrintError($"Cannot get entity -- order with id [{id}] does not exist");
                return;
            }
            List<int> fwIds = _service.orderItemRepository.GetProductIdsByOrderId(id);
            List<Footwear> fw = new List<Footwear>();
            foreach(int fwId in fwIds)
            {
                fw.Add(_service.footwearRepository.GetById(fwId));
            }
            View.PrintEntity<Order>(or);
            View.PrintEntities<Footwear>(fw);
            
        }
        public void GetOrderByClientId(int id)
        {
            Client c = _service.clientRepository.GetById(id);
            if(c == null)
            {
                View.PrintError($"Client with id {id} does not exist");
                return;
            }
            List<Order> orders = _service.orderRepository.GetByClientId(id);
            if(orders.Count == 0)
            {
                View.PrintInfo($"Client with id {id} do not have any orders yet");
                return;
            }
            View.PrintEntities<Order>(orders);
        }
        public void GetReviewsByClientId(int id)
        {
            Client c = _service.clientRepository.GetById(id);
            if(c == null)
            {
                View.PrintError($"Client with id {id} does not exist");
                return;
            }
            List<Review> rw = _service.reviewRepository.GetByClientId(id);
            View.PrintEntities<Review>(rw);
        }
        public void GetReviewsByProductId(int id)
        {
            Footwear f = _service.footwearRepository.GetById(id);
            if(f == null)
            {
                View.PrintError($"Footwear with id {id} does not exist");
                return;
            }
            List<Review> rw = _service.reviewRepository.GetByProductId(id);
            View.PrintEntities<Review>(rw);
        }
        public void GetFootwearByName(string name)
        {
            View.PrintEntities<Footwear>(_service.footwearRepository.GetByName(name));
        }
        public void GetClientById(int id)
        {
            Client c = _service.clientRepository.GetById(id);
            if(c==null)
            {
                View.PrintError($"Cannot get entity -- client with id [{id}] does not exist");
                return;
            }
            View.PrintEntity<Client>(c); 
        }
        public void GetClientByFullName(string name)
        {
            View.PrintEntities<Client>(_service.clientRepository.GetByFullName(name));
        }
        public void UpdateReview(int review_id, string content, int rating)
        {
            Review reviewOld = _service.reviewRepository.GetById(review_id);
            if(reviewOld == null)
            {
                View.PrintError($"Cannot update -- review with id [{review_id}] does not exist");
                return;
            }
            if(rating <=0 || rating > 10)
            {
                View.PrintError("Rating of the film can be only from 1 to 10");
                return;
            }
            Review review = new Review()
            {
                id = reviewOld.id,
                content = content,
                rating = rating,
                created_at = DateTime.Now,
                client_id = reviewOld.client_id,
                product_id = reviewOld.product_id
            };
            try
            {
                _service.reviewRepository.Update(review);   
                View.ShowUpdateResult(typeof(Review).Name, review.id);
            }
            catch (Exception ex)
            {
                View.PrintError($"{ex.Message}");
            }
        }
        public void GetFootwearByRangeCost(string pn,int lp, int hp)
        {
            if(lp > hp || hp<=0)
            {
                View.PrintError("Invalid cost range");
                return;
            }
            View.PrintEntities<Footwear>(_service.footwearRepository.GetByNameAndRangeCost(pn,lp,hp));
        }
        public void GetFootwearRatings(int id)
        {
            Footwear fw = _service.footwearRepository.GetById(id);
            if(fw==null)
            {
                View.PrintError($"Footwear with id [{id}] does not exist");
                return;
            }
            List<int> ratings = _service.reviewRepository.GetRatingsByProductId(id);
            View.FootwearRatingsChart(ratings, fw.name+ " by " + fw.brand);
        }
        public void GenerateProductReport(int id)
        {
            Footwear fw = _service.footwearRepository.GetById(id);
            if(fw==null)
            {
                View.PrintError($"Cannot generate report -- footwear with id [{id}] does not exist");
                return;
            }
            Review highestRatingRev = _service.reviewRepository.GetHighestRatingReview(id);
            List<int> ratings = _service.reviewRepository.GetRatingsByProductId(id);
            if(highestRatingRev == null)
            {
                Visualization.ReportGenerator.SetValues(fw,null,null,null,null,0,ratings);
                Visualization.ReportGenerator.Run();
                return;
            }
            Review lowestRatingRev = _service.reviewRepository.GetLowestRatingReview(id);
            Client highestRatingClient = _service.clientRepository.GetById(highestRatingRev.client_id);
            Client lowestRatingClient = _service.clientRepository.GetById(lowestRatingRev.client_id);
            double rating = _service.reviewRepository.GetFootwearRating(id);

            Visualization.ReportGenerator.SetValues(fw,highestRatingRev,lowestRatingRev,highestRatingClient,lowestRatingClient,rating,ratings);
            Visualization.ReportGenerator.Run();
        }
        public void GenerateHashIndexChart()
        {
            int clientsCount = _service.clientRepository.GetCount();
            int footwearCount = _service.footwearRepository.GetCount();
            if(clientsCount == 0 || footwearCount == 0)
            {
                View.PrintError("There arent enough entities to conduct test.");
                return;
            }
            Stopwatch sw = new Stopwatch();
            double[] withoutHash = new double[2];
            double[] withHash = new double[2];
            string fullname = _service.clientRepository.GetRandomFullname();

            //clients table
            _service.clientRepository.DropFullnameIndex();
            sw.Start();
            _service.clientRepository.GetByFullName(fullname);
            sw.Stop();

            withoutHash[0]=sw.ElapsedMilliseconds;
            
            sw.Reset();
            _service.clientRepository.CreateFullnameIndex();

            sw.Start();
            _service.clientRepository.GetByFullName(fullname);
            sw.Stop();

            withHash[0] = sw.ElapsedMilliseconds;
            //clients table
            sw.Reset();
            //footwear table
            string name = _service.footwearRepository.GetRandomName();
            _service.footwearRepository.DropFullnameIndex();
            sw.Start();
            _service.footwearRepository.GetByName(name);
            sw.Stop();

            withoutHash[1]=sw.ElapsedMilliseconds;

            sw.Reset();
            _service.footwearRepository.CreateFullnameIndex();

            sw.Start();
            _service.footwearRepository.GetByName(name);
            sw.Stop();
            withHash[1]=sw.ElapsedMilliseconds;
            //footwear table

            View.HashIndexChart(withoutHash,withHash);
        }
        public void GenerateBtreeIndexChart()
        {
            int reviewsCount = _service.reviewRepository.GetCount();
            int footwearCount = _service.footwearRepository.GetCount();
            if(reviewsCount == 0 || footwearCount == 0)
            {
                View.PrintError("There arent enough entities to conduct test.");
                return;
            }
            Stopwatch sw = new Stopwatch();
            double[] withoutHash = new double[2];
            double[] withHash = new double[2];
            DateTime dt = new DateTime(2019,1,1);
            //reviews table

            _service.reviewRepository.DropBtrIndex();

            sw.Start();
            _service.reviewRepository.GetByGoodRatingAndTimeSpan(dt);
            sw.Stop();

            withoutHash[0]=sw.ElapsedMilliseconds;

            sw.Reset();
            _service.reviewRepository.CreateBtrIndex();

            sw.Start();
            _service.reviewRepository.GetByGoodRatingAndTimeSpan(dt);
            sw.Stop();

            withHash[0] = sw.ElapsedMilliseconds;
            //reviews table
            sw.Reset();
            //footwear table
            _service.footwearRepository.DropCostIndex();

            sw.Start();
            _service.footwearRepository.GetByRangeCost(200,300);
            sw.Stop();

            withoutHash[1]=sw.ElapsedMilliseconds;

            sw.Reset();
            _service.footwearRepository.CreateCostIndex();

            sw.Start();
            _service.footwearRepository.GetByRangeCost(200,300);
            sw.Stop();
            withHash[1]=sw.ElapsedMilliseconds;


            //footwear table
            View.BTreeIndexChart(withoutHash,withHash);
        }
        public void GenerateFootwearAgeCategoriesChart(int id)
        {
            Footwear fw = _service.footwearRepository.GetById(id);
            if(fw == null)
            {
                View.PrintError($"Footwear with id [{id}] doesn`t exist.");
                return;
            }
            List<double> ageGroups = _service.footwearRepository.GetClientsAge(id);
            View.FootwearAgeCategoriesChart(ageGroups, fw.name + " by " + fw.brand);
        }
        public void GenerateIncomesStatisticsChart(int year)
        {
            List<double> incomes = _service.orderRepository.GetIncomesByYear(year);
            if(incomes.Count == 0)
            {
                View.PrintError($"There aren`t information about incomes in {year}");
                return;
            }
            View.IncomesStatisticsChart(incomes, year);
        }

    }
}