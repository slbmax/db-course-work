using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
namespace Model
{
    public class FootwearRepository
    {
        private ServiceContext context;
        public FootwearRepository(ServiceContext c)
        {
            this.context = c;
        }
        public void AddBulkData(List<Footwear> footwear)  
        {  
            context.AddRange(footwear);
            context.SaveChanges();
        }  
        public int GetCount()
        {
            return context.footwear.Count<Footwear>();
        }
        public int Insert(Footwear fw)
        {
            context.footwear.Add(fw);
            context.SaveChanges();
            return fw.id;
        }
        public void DeleteById(int id)
        {
            context.footwear.Remove(context.footwear.Find(id));
            context.SaveChanges();
        }
        public void Update(Footwear fw)
        {
            int id = fw.id;
            var local = context.footwear.Find(id);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(fw).State = EntityState.Modified;
            context.SaveChanges();
        }
        public Footwear GetById(int id)
        {
            return context.footwear.Find(id);
        }
        public List<Footwear> GetByName(string name)
        {
            return context.footwear.Where(x => x.name == name).ToList();
        }
        public List<Footwear> GetByNameAndRangeCost(string name, int l, int h)
        {
            return context.footwear.Where(x => x.cost >= l && x.cost <= h && x.name == name).ToList();
        }
        public List<double> GetClientsAge(int garmentId)
        {
            List<double> result = new List<double>();
            var connection = context.Database.GetDbConnection();
            try
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @$"SELECT Count(*) FROM clients, orders, order_items WHERE order_items.product_id = {garmentId} 
                    AND orders.id = order_items.order_id AND clients.id = orders.client_id AND clients.birthday_date >= timestamp '{DateTime.Now.Year - 18}-{DateTime.Now.Month}-{DateTime.Now.Day}'";
                    result.Add(Convert.ToDouble(command.ExecuteScalar()));

                    command.CommandText = @$"SELECT Count(*) FROM clients, orders, order_items WHERE order_items.product_id = {garmentId} 
                    AND orders.id = order_items.order_id AND clients.id = orders.client_id AND clients.birthday_date < timestamp '{DateTime.Now.Year - 18}-{DateTime.Now.Month}-{DateTime.Now.Day}'
                    AND clients.birthday_date >= timestamp '{DateTime.Now.Year - 25}-{DateTime.Now.Month}-{DateTime.Now.Day}'";
                    result.Add(Convert.ToDouble(command.ExecuteScalar()));

                    command.CommandText = @$"SELECT Count(*) FROM clients, orders, order_items WHERE order_items.product_id = {garmentId} 
                    AND orders.id = order_items.order_id AND clients.id = orders.client_id AND clients.birthday_date < timestamp '{DateTime.Now.Year - 25}-{DateTime.Now.Month}-{DateTime.Now.Day}'
                    AND clients.birthday_date >= timestamp '{DateTime.Now.Year - 40}-{DateTime.Now.Month}-{DateTime.Now.Day}'";
                    result.Add(Convert.ToDouble(command.ExecuteScalar()));

                    command.CommandText = @$"SELECT Count(*) FROM clients, orders, order_items WHERE order_items.product_id = {garmentId} 
                    AND orders.id = order_items.order_id AND clients.id = orders.client_id AND clients.birthday_date < timestamp '{DateTime.Now.Year - 40}-{DateTime.Now.Month}-{DateTime.Now.Day}'
                    AND clients.birthday_date >= timestamp '{DateTime.Now.Year - 50}-{DateTime.Now.Month}-{DateTime.Now.Day}'";
                    result.Add(Convert.ToDouble(command.ExecuteScalar()));

                    command.CommandText = @$"SELECT Count(*) FROM clients, orders, order_items WHERE order_items.product_id = {garmentId} 
                    AND orders.id = order_items.order_id AND clients.id = orders.client_id AND clients.birthday_date < timestamp '{DateTime.Now.Year - 50}-{DateTime.Now.Month}-{DateTime.Now.Day}'";
                    result.Add(Convert.ToDouble(command.ExecuteScalar()));
                }
            }
            catch (System.Exception e) { Console.WriteLine(e.Message);}
            finally { connection.Close(); }
            return result;   
        }

        public void DropFullnameIndex()
        {
            string sql = "DROP INDEX IF EXISTS fw_hash";
            context.Database.ExecuteSqlRaw(sql);
        }
        public void CreateFullnameIndex()
        {
            string sql = "CREATE INDEX IF NOT EXISTS fw_hash ON footwear USING hash(name)";
            context.Database.ExecuteSqlRaw(sql);
        }
        public string GetRandomName()
        {
            Random rand = new Random();
            int toSkip = rand.Next(1, context.clients.Count());
            return context.footwear.Skip(toSkip).Take(1).First().name;            
        }
        public void DropCostIndex()
        {
            string sql = "DROP INDEX IF EXISTS fw_btr";
            context.Database.ExecuteSqlRaw(sql);
        }
        public void CreateCostIndex()
        {
            string sql = "CREATE INDEX IF NOT EXISTS fw_btr ON footwear USING btree(cost)";
            context.Database.ExecuteSqlRaw(sql);
        }
        public List<Footwear> GetByRangeCost(int l, int h)
        {
            return context.footwear.Where(x => x.cost > l && x.cost < h).ToList();
        }
    }
}