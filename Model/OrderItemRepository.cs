using System.Linq;
using System.Collections.Generic;
namespace Model
{
    public class OrderItemRepository
    {
        private ServiceContext context;
        public OrderItemRepository(ServiceContext c)
        {
            this.context = c;
        }
        public void Insert(OrderItem oi)
        {
            context.order_items.Add(oi);
            context.SaveChanges();
        }
        public void DeleteByOrderId(int id)
        {
            context.order_items.RemoveRange(context.order_items.Where(x => x.order_id == id));
            context.SaveChanges();
        }
        public void DeleteByProductId(int id)
        {
            context.order_items.RemoveRange(context.order_items.Where(x => x.product_id == id));
            context.SaveChanges();
        }
        public List<int> GetProductIdsByOrderId(int id)
        {
            return context.order_items.Where(x => x.order_id == id).Select(o => o.product_id).ToList();
        }
    }
}