using System.Collections.Generic;
namespace Model
{
    public class Order
    {
        public int id {get; set; }
        public int client_id {get; set; }
        public System.DateTime checkout_date {get; set; }
        public string payment_method {get; set; }


        public List<Footwear> footwear{get; set; }
        public List<OrderItem> order_items{get; set; }
        public Client client {get; set; }

        public override string ToString()
        {
            return string.Format(@$"[{id}] Client id: {client_id}; payment method: "+
            "{payment_method}; checkout date: {checkout_date.ToShortDateString()} ");
        }
    }
}