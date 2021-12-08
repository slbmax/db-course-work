namespace Model
{
    public class Review
    {
        public int id{get; set; }
        public string content{get; set; }
        public int rating{get; set; }
        public int product_id{get; set; }
        public int client_id{get; set; }
        public System.DateTime created_at{get; set;}
        public Client client{get; set; }
        public Footwear footwear{get; set; }

        public override string ToString()
        {
            return string.Format($"[{this.id}] \"{this.content}\" [{rating}] (Product: [{product_id}]; Client: [{client_id}])");
        }
    }
}