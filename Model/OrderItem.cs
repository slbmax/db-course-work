namespace Model
{
    public class OrderItem
    {
        public int product_id {get; set; }
        public int order_id {get; set; }

        public Order order {get; set; }
        public Footwear footwear {get; set; }

        public override string ToString()
        {
            return string.Format(@$"Order [{order_id}] - Product [{product_id}]");
        }
    }
}