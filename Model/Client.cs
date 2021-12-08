using System.Collections.Generic;
namespace Model
{
    public class Client
    {
        public int id {get; set; }
        public string fullname {get; set; }
        public System.DateTime birthday_date {get; set; }
        public string email {get; set; }

        public List<Review> reviews{get; set; }
        public List<Order> orders{get; set; }


        public override string ToString()
        {
            return string.Format($"[{id} {fullname} [{birthday_date.ToShortDateString()}]] [{email}]");
            
        }
    }
}