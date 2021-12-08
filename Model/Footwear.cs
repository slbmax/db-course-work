using System.Collections.Generic;
namespace Model
{
    public class Footwear
    {
        public int id {get; set; }
        public string name {get; set; }
        public string brand {get; set; }
        public int cost {get; set; }
        public List<Review> reviews{get; set; }
        public List<Order> orders{get; set; }
        public List<OrderItem> order_items{get; set; }
        public string country_of_origin {get; set; }
        public override string ToString()
        {
            return string.Format(@$"[{id}] {name} -- {brand} (Made in {country_of_origin}) - {cost}$");
        }
    }
}