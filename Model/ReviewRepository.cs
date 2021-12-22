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
        public void Update(Review review)
        {
            int id = review.id;
            var local = context.reviews.Find(id);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(review).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void DeleteById(int id)
        {
            context.reviews.Remove(context.reviews.Find(id));
            context.SaveChanges();
        }
        public int Insert(Review review)
        {
            context.reviews.Add(review);
            context.SaveChanges();
            return review.id;
        }
        public void DeleteByClientId(int id)
        {
            context.reviews.RemoveRange(context.reviews.Where(x => x.client_id == id));
            context.SaveChanges();
        }

        public void DeleteByProductId(int id)
        {
            context.reviews.RemoveRange(context.reviews.Where(x => x.product_id == id));
            context.SaveChanges();
        }
        public Review GetById(int id) 
        {
            return context.reviews.Find(id);
        }
        public List<Review> GetByClientId(int id)
        {
            return context.reviews.Where(x => x.client_id == id).ToList();
        }
        public List<Review> GetByProductId(int id)
        {
            return context.reviews.Where(x => x.product_id == id).ToList();
        }
        public List<int> GetRatingsByProductId(int id)
        {
            return context.reviews.Where(x => x.product_id == id).Select(o => o.rating).ToList();
        }
        public Review GetHighestRatingReview(int prod_id)
        {
            try
            {
                return context.reviews.Where(x => x.product_id == prod_id).OrderByDescending(o => o.rating).First();
            }
            catch
            {
                return null;
            }
        }
        public Review GetLowestRatingReview(int prod_id)
        {
            try
            {
                return context.reviews.Where(x => x.product_id == prod_id).OrderBy(o => o.rating).First();
            }
            catch
            {
                return null;
            }
        }
        public double GetFootwearRating(int fw_id)
        {
            return context.reviews.Where(x => x.product_id == fw_id).Average(o => o.rating);
        }



        public int GetCount()
        {
            return context.reviews.Count<Review>();
        }
        public void DropBtrIndex()
        {
            string sql = "DROP INDEX IF EXISTS rw_btr";
            context.Database.ExecuteSqlRaw(sql);
        }
        public void CreateBtrIndex()
        {
            string sql = "CREATE INDEX IF NOT EXISTS rw_btr ON reviews USING btree(rating, created_at)";
            context.Database.ExecuteSqlRaw(sql);
        }
        public List<Review> GetByGoodRatingAndTimeSpan(DateTime dt)
        {
            return context.reviews.Where(x => x.rating > 6 && x.created_at > dt).ToList();
        }
    }
}