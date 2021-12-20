using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
namespace Model
{
    public class ClientRepository
    {
        private ServiceContext context;
        public ClientRepository(ServiceContext c)
        {
            this.context = c;
        }
        public void AddBulkData(List<Client> clients)  
        {  
            context.AddRange(clients);
            context.SaveChanges();
        }
        public int GetCount()
        {
            return context.clients.Count<Client>();
        }
        public int Insert(Client client)
        {
            context.clients.Add(client);
            context.SaveChanges();
            return client.id;
        }
        public void DeleteById(int id)
        {
            context.clients.Remove(context.clients.Find(id));
            context.SaveChanges();
        }
        public void Update(Client client)
        {
            int id = client.id;
            var local = context.clients.Find(id);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(client).State = EntityState.Modified;
            context.SaveChanges();
        }
        public Client GetById(int id)
        {
            return context.clients.Find(id);
        }
        public List<Client> GetByFullName(string name)
        {
            return context.clients.Where(x => x.fullname == name).ToList();
        }
        public string GetRandomFullname()
        {
            Random rand = new Random();
            int toSkip = rand.Next(1, context.clients.Count());
            return context.clients.Skip(toSkip).Take(1).First().fullname;            
        }
        public void DropFullnameIndex()
        {
            string sql = "DROP INDEX IF EXISTS cl_hash";
            context.Database.ExecuteSqlRaw(sql);
        }
        public void CreateFullnameIndex()
        {
            string sql = "CREATE INDEX IF NOT EXISTS cl_hash ON clients USING hash(fullname)";
            context.Database.ExecuteSqlRaw(sql);
        }
    }
}