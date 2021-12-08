using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Model
{
    public class OrderRepository
    {
        private ServiceContext context;
        public OrderRepository(ServiceContext c)
        {
            this.context = c;
        }
        public void GenerateOrders(int amount)
        {
            string query = @$"CREATE TRIGGER order_trigger
                            AFTER INSERT ON orders
                            FOR EACH ROW EXECUTE PROCEDURE order_items_gen();
                            
                            INSERT INTO orders (client_id, checkout_date, payment_method)
                            SELECT
                            random_client_id(),
                            timestamp '2016-01-01' + random() * (timestamp '2021-12-12' - timestamp '2016-01-01'),
                            random_choice(array['cash', 'check', 'debit card', 'credit card', 'mobile payment'])
                            FROM generate_series(1, {amount});
                            
                            DROP TRIGGER order_trigger ON orders";
            context.Database.ExecuteSqlRaw(query);
        }
    }
}