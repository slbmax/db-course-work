using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
namespace Model
{
    public class OrderRepository
    {
        private ServiceContext context;
        public OrderRepository(ServiceContext c)
        {
            this.context = c;
        }
        public void GenerateOrders(int amount)
        {
            string query = @$"CREATE TRIGGER order_trigger
                            AFTER INSERT ON orders
                            FOR EACH ROW EXECUTE PROCEDURE order_items_gen();";
            context.Database.ExecuteSqlRaw(query);

            int[] clientIds = context.clients.Select(o => o.id).ToArray();
            Random rand = new Random();
            
            for(int i = 0; i < amount; i++)
            {
                query = @$"INSERT INTO orders (client_id, checkout_date, payment_method)
                            SELECT
                            {clientIds[rand.Next(0,clientIds.Length)]},
                            timestamp '2016-01-01' + random() * (timestamp '2021-12-12' - timestamp '2016-01-01'),
                            random_choice(array['cash', 'check', 'debit card', 'credit card', 'mobile payment'])";
                context.Database.ExecuteSqlRaw(query);
            }

            query = "DROP TRIGGER order_trigger ON orders";
            context.Database.ExecuteSqlRaw(query);
        }
        public int Insert(Order order)
        {
            context.orders.Add(order);
            context.SaveChanges();
            return order.id;
        }
        public void DeleteById(int id)
        {
            context.orders.Remove(context.orders.Find(id));
            context.SaveChanges();
        }
        public void Update(Order order)
        {
            int id = order.id;
            var local = context.orders.Find(id);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(order).State = EntityState.Modified;
            context.SaveChanges();
        }
        public Order GetById(int id)
        {
            return context.orders.Find(id);
        }
        public void DeleteByClientId(int id)
        {
            List<Order> orders = GetByClientId(id);
            foreach(Order order in orders)
            {
                context.order_items.RemoveRange(context.order_items.Where(x => x.order_id == order.id));
            }
            context.orders.RemoveRange(orders);
            context.SaveChanges();
        }
        public List<Order> GetByClientId(int id)
        {
            return context.orders.Where(x => x.client_id == id).ToList();
        }
        public List<double> GetIncomesByYear(int year)
        {
            List<double> result = new List<double>();
            var connection = context.Database.GetDbConnection();
            try
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    for(int i = 1; i<12; i++)
                    {
                        command.CommandText = @$"SELECT SUM(footwear.cost) FROM footwear,orders,order_items 
                        WHERE orders.checkout_date >= timestamp '{year}-{i}-1' AND orders.checkout_date < timestamp '{year}-{i+1}-1'
                        AND orders.id = order_items.order_id AND order_items.product_id = footwear.id";
                        result.Add(Convert.ToDouble(command.ExecuteScalar()));
                    }
                    command.CommandText = @$"SELECT SUM(footwear.cost) FROM footwear,orders,order_items
                    WHERE orders.checkout_date >= timestamp '{year}-12-1' AND orders.checkout_date < timestamp '{year+1}-1-1'
                    AND orders.id = order_items.order_id AND order_items.product_id = footwear.id";
                    result.Add(Convert.ToDouble(command.ExecuteScalar()));
                }
            }
            catch (System.Exception e) { Console.WriteLine(e.Message);}
            finally { connection.Close(); }
            return result;
        }
        public int GetCount()
        {
            return context.orders.Count<Order>();
        }
    }
}