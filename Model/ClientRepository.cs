using System.Collections.Generic;
using System.Linq;
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
    }
}