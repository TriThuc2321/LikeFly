using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeFly.Database
{
    public class FlightServices
    {
        FirebaseClient firebase = new FirebaseClient("https://likefly-5ec61-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseClient storage = new FirebaseClient("likefly-5ec61.appspot.com");

        public FlightServices() { }
        public async Task<List<Flight>> GetAllFlights()
        {            
            return (await firebase
              .Child("Flights")
              .OnceAsync<Flight>()).Select(item => new Flight
              {
                  Id = item.Object.Id,
                  Name = item.Object.Name,
                  Duration = item.Object.Duration,
                  StartDate = item.Object.StartDate,
                  StartTime = item.Object.StartTime,
                  ImgSource = item.Object.ImgSource,
                  Description = item.Object.Description,
                  PassengerNumber = item.Object.PassengerNumber,
                  IsOccured = item.Object.IsOccured,
                  Price = item.Object.Price,
                  IntermediaryAirportList = item.Object.IntermediaryAirportList,
                  AirportStart = item.Object.AirportStart,
                  AirportEnd = item.Object.AirportEnd,
                  TicketTypes = item.Object.TicketTypes,                  
              }).ToList();
        }
        public async Task AddFlight(Flight flight)
        {
            await firebase
              .Child("Flights")
              .PostAsync(new Flight()
              {
                    Id = flight.Id,
                    Name = flight.Name,
                    Duration = flight.Duration,
                    StartDate = flight.StartDate,
                    StartTime = flight.StartTime,
                    ImgSource = flight.ImgSource,
                    Description = flight.Description,
                    PassengerNumber = flight.PassengerNumber,
                    IsOccured = flight.IsOccured,
                    Price = flight.Price,
                    IntermediaryAirportList = flight.IntermediaryAirportList,
                    AirportStart = flight.AirportStart,
                    AirportEnd = flight.AirportEnd,
                    TicketTypes = flight.TicketTypes,

              });
        }

        public async Task UpdateFlight(Flight flight)
        {
            var toUpdatePlace = (await firebase
              .Child("Flights")
              .OnceAsync<Flight>()).Where(a => a.Object.Id == flight.Id).FirstOrDefault();

            await firebase
              .Child("Flights")
              .Child(toUpdatePlace.Key)
              .PutAsync(new Flight
              {
                  Id = flight.Id,
                  Name = flight.Name,
                  Duration = flight.Duration,
                  StartDate = flight.StartDate,
                  StartTime = flight.StartTime,
                  ImgSource = flight.ImgSource,
                  Description = flight.Description,
                  PassengerNumber = flight.PassengerNumber,
                  IsOccured = flight.IsOccured,
                  Price = flight.Price,
                  IntermediaryAirportList = flight.IntermediaryAirportList,
                  AirportStart = flight.AirportStart,
                  AirportEnd= flight.AirportEnd,
                  TicketTypes = flight.TicketTypes
              });
        }
        async public Task<string> saveImage(Stream imgStream, string airportId)
        {
            string id = GenerateId();

            var stroageImage = await new FirebaseStorage("likefly-5ec61.appspot.com")
                .Child("Flights").Child(airportId)
                .Child(id + ".png")
                .PutAsync(imgStream);
            var imgurl = stroageImage;
            return imgurl;
        }
        public string GenerateId(int length = 10)
        {
            var List = DataManager.Ins.ListBookedTickets;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

            int i = 0;
            while (i < List.Count())
            {
                if (List[i].Id == randomString)
                {
                    i = -1;
                    randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
                }
                i++;
            }
            return randomString;
        }
        public async Task DeleteFile(string folderAirportId, int id)
        {
            try
            {
                await new FirebaseStorage("likefly-5ec61.appspot.com")
                 .Child("Flights")
                 .Child(folderAirportId).Child(id + ".png")
                 .DeleteAsync();
            }
            catch { }

            try
            {
                await new FirebaseStorage("likefly-5ec61.appspot.com")
                 .Child("Flights")
                 .Child(folderAirportId).Child(id + ".jpg")
                 .DeleteAsync();
            }
            catch { }

        }
        public async Task DeleteAirport(Flight flight)
        {
            var toDeleted = (await firebase
               .Child("Flights").OnceAsync<Flight>()).FirstOrDefault(p => p.Object.Id == flight.Id);

            await firebase.Child("Flights").Child(toDeleted.Key).DeleteAsync();            
            

        }

        public async Task<Flight> FindFlightById(string id)
        {
            var all = await GetAllFlights();
            await firebase
                .Child("Flights")
                .OnceAsync<Flight>();
            return all.Where(a => a.Id == id).FirstOrDefault();
        }
    }
}
