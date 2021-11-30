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
    public class FlightServices
    {
        FirebaseClient firebase = new FirebaseClient("https://likefly-5ec61-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseClient storage = new FirebaseClient("likefly-5ec61.appspot.com");

        public List<Flight> places;
        public FlightServices() { }
        public async Task<List<Flight>> GetAllFlights()
        {
            return (await firebase
              .Child("Flights")
              .OnceAsync<Flight>()).Select(item => new Flight
              {
                  id = item.Object.id,
                  country = item.Object.country,
                  title = item.Object.title,
                  imgSource = item.Object.imgSource,
                  description = item.Object.description
              }).ToList();
        }
        public async Task AddFlight(Flight flight)
        {
            await firebase
              .Child("Flights")
              .PostAsync(new Flight()
              {
                  id = flight.id,
                  country = flight.country,
                  title = flight.title,
                  imgSource = flight.imgSource,
                  description = flight.description
              });
        }
    }
}
