using System.Collections.Generic;
using System.Linq;
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
    }
}