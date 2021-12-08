using EFCore.BulkExtensions;
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks; 
using Microsoft.EntityFrameworkCore; 
namespace Model
{
    public class ReviewRepository
    {
        private ServiceContext context;
        public ReviewRepository(ServiceContext c)
        {
            this.context = c;
        }
        public void GenerateReviews(int amount)
        {
            string query = @$"INSERT INTO reviews (content, rating, created_at, client_id, product_id)
                            SELECT
                            random_string(25),
                            trunc(random()*10+1)::int,
                            timestamp '2016-01-01' + random() * (timestamp '2021-10-10' - timestamp '2016-01-01'),
                            random_client_id(),
                            random_footwear_id()
                            FROM generate_series(1, {amount})";
            context.Database.ExecuteSqlRaw(query);
        }
    }
}