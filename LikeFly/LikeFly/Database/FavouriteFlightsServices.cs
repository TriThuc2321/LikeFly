using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeFly.Database
{
    public class FavouriteFlightsServices
    {
        FirebaseClient firebase = new FirebaseClient("https://likefly-5ec61-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseStorage storage = new FirebaseStorage("likefly-5ec61.appspot.com");

        public List<FavouriteFlight> favouriteFlights;

        public FavouriteFlightsServices() { }
        public async Task<List<FavouriteFlight>> GetAllFavourite()
        {
            return (await firebase
             .Child("Favourites")
             .OnceAsync<FavouriteFlight>()).Select(item => new FavouriteFlight
             {
                 id = item.Object.id,
                 flight = item.Object.flight,
                 email = (string)item.Object.email,
                  //.Where(email => email.ToString() == DataManager.Ins.CurrentUser.email),
              }).ToList();

        }
        public async Task AddFavouriteFlight(FavouriteFlight favourite)
        {
            await firebase
              .Child("Favourites")
              .PostAsync(new FavouriteFlight()
              {
                  id = favourite.id,
                  flight = new Flight { id = favourite.flight.id },
                  email = favourite.email,
              });
        }

        public async Task DeleteFavoriteFlight(string id)
        {
            var toDelete = (await firebase
              .Child("Favourites")
              .OnceAsync<FavouriteFlight>()).Where(a => a.Object.id == id).FirstOrDefault();
            await firebase.Child("Favourites").Child(toDelete.Key).DeleteAsync();
        }
    }
}
