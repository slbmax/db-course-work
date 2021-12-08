namespace Model
{
    public class OrderItemRepository
    {
        private ServiceContext context;
        public OrderItemRepository(ServiceContext c)
        {
            this.context = c;
        }
    }
}