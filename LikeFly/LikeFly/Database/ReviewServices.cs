using Firebase.Database;
using Firebase.Database.Query;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeFly.Database
{
    public class ReviewServices
    {
        FirebaseClient firebase = new FirebaseClient("https://likefly-5ec61-default-rtdb.asia-southeast1.firebasedatabase.app/");

        public async Task<List<Review>> GetAllReviews()
        {
            return (await firebase
              .Child("Reviews")
              .OnceAsync<Review>()).Select(item => new Review
              {
                  email = item.Object.email,
                  message = item.Object.message,
                  time = item.Object.time,
                  starNumber = item.Object.starNumber,
              }).ToList();
        }
        public async Task Add(Review review)
        {
            await firebase
              .Child("Reviews")
              .PostAsync(new Review()
              {
                  email = review.email,
                  message = review.message,
                  time = review.time,
                  starNumber = review.starNumber,
              });
        }

        //public async Task UpdateReview(Review review)
        //{
        //    var toUpdateReview = (await firebase
        //         .Child("Reviews")
        //         .OnceAsync<Review>()).Where(a => a.Object.tourId == review.tourId && a.Object.email == review.email).FirstOrDefault();

        //    await firebase
        //      .Child("Reviews")
        //      .Child(toUpdateReview.Key)
        //      .PutAsync(new Review
        //      {
        //          email = review.email,
        //          message = review.message,
        //          time = review.time,
        //          starNumber = review.starNumber,
        //      });

        //}
    }
}
