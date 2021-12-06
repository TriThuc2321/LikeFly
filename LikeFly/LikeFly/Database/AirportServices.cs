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
    public class AirportServices
    {
        FirebaseClient firebase = new FirebaseClient("https://likefly-5ec61-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseStorage storage = new FirebaseStorage("likefly-5ec61.appspot.com");

        public List<Airport> places;
        public AirportServices()
        {
        }
        public async Task<List<Airport>> GetAllAirport()
        {
            return (await firebase
              .Child("Airports")
              .OnceAsync<Airport>()).Select(item => new Airport
              {
                  Id = item.Object.Id,
                  Name = item.Object.Name,
                  ImgSource = item.Object.ImgSource,
                  Province = item.Object.Province,
              }).ToList();
        }
        public async Task AddAirport(Airport airport)
        {
            await firebase
              .Child("Airports")
              .PostAsync(new Airport()
              {
                  Id = airport.Id,
                  Name = airport.Name,
                  ImgSource = airport.ImgSource,
                  Province = airport.Province,
              });
        }
        public async Task UpdateAirport(Airport airport)
        {
            var toUpdatePlace = (await firebase
              .Child("Airports")
              .OnceAsync<Airport>()).Where(a => a.Object.Id == airport.Id).FirstOrDefault();

            await firebase
              .Child("Airports")
              .Child(toUpdatePlace.Key)
              .PutAsync(new Airport
              {
                  Id = airport.Id,
                  Name = airport.Name,
                  ImgSource = airport.ImgSource,
                  Province = airport.Province,
              });
        }
        async public Task<string> saveImage(Stream imgStream, string airportId, int id)
        {
            var stroageImage = await new FirebaseStorage("likefly-5ec61.appspot.com")
                .Child("Airports").Child(airportId)
                .Child(id + ".png")
                .PutAsync(imgStream);
            var imgurl = stroageImage;
            return imgurl;
        }

        public async Task DeleteFile(string folderAirportId, int id)
        {
            try
            {
                await new FirebaseStorage("likefly-5ec61.appspot.com")
                 .Child("Airports")
                 .Child(folderAirportId).Child(id + ".png")
                 .DeleteAsync();
            }
            catch { }

            try
            {
                await new FirebaseStorage("likefly-5ec61.appspot.com")
                 .Child("Airports")
                 .Child(folderAirportId).Child(id + ".jpg")
                 .DeleteAsync();
            }
            catch { }

        }

        public async Task DeleteAirport(Airport airport)
        {
            var toDeleted = (await firebase
               .Child("Airports").OnceAsync<Airport>()).FirstOrDefault(p => p.Object.Id == airport.Id);

            await firebase.Child("Airports").Child(toDeleted.Key).DeleteAsync();

            await DeleteFile(airport.Id, 0);

        }
        
    }
}
