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
    public class BookedTicketServices
    {
        FirebaseClient firebase = new FirebaseClient("https://likefly-5ec61-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseClient storage = new FirebaseClient("gs://likefly-5ec61.appspot.com");

        public List<BookedTicketServices> bookedTickets;

        public BookedTicketServices() { }
        public async Task<List<BookedTicket>> GetAllBookedTicket()
        {
            return (await firebase
              .Child("BookedTickets")
              //   .OrderBy("bookTime")
              .OnceAsync<BookedTicket>()).Select(item => new BookedTicket
              {
                  Id = item.Object.Id,
                  Flight = item.Object.Flight,
                  Name = item.Object.Name,
                  Birthday = item.Object.Birthday,
                  Contact = item.Object.Contact,
                  Email = item.Object.Email,
                  Cmnd = item.Object.Cmnd,
                  Address = item.Object.Address,
                  BookTime = item.Object.BookTime,
                  Invoice = item.Object.Invoice,
                  IsCancel = item.Object.IsCancel,
                  
              }).ToList();
        }
        public async Task AddBookedTicket(BookedTicket bookedTicket)
        {
            await firebase
              .Child("BookedTickets")
              .PostAsync(new BookedTicket()
              {
                  Id = bookedTicket.Id,
                  Flight = new Flight { Id = bookedTicket.Flight.Id },
                  Name = bookedTicket.Name,
                  Birthday = bookedTicket.Birthday,
                  Contact = bookedTicket.Contact,
                  Email = bookedTicket.Email,
                  Cmnd = bookedTicket.Cmnd,
                  Address = bookedTicket.Address,
                  BookTime = bookedTicket.BookTime,
                  Invoice = bookedTicket.Invoice,
                  IsCancel = bookedTicket.IsCancel,
                  
              });
        }

        public async Task UpdateBookedTicket(BookedTicket bookedTicket)
        {
            var toUpdateTour = (await firebase
                 .Child("BookedTickets")
                 .OnceAsync<BookedTicket>()).Where(a => a.Object.Id == bookedTicket.Id).FirstOrDefault();

            await firebase
              .Child("BookedTickets")
              .Child(toUpdateTour.Key)
              .PutAsync(new BookedTicket
              {
                  Id = bookedTicket.Id,
                  Flight = new Flight { Id = bookedTicket.Flight.Id },
                  Name = bookedTicket.Name,
                  Birthday = bookedTicket.Birthday,
                  Contact = bookedTicket.Contact,
                  Email = bookedTicket.Email,
                  Cmnd = bookedTicket.Cmnd,
                  Address = bookedTicket.Address,
                  BookTime = bookedTicket.BookTime,
                  Invoice = bookedTicket.Invoice,
                  IsCancel = bookedTicket.IsCancel,
              });

        }
        public async Task DeleteBookedTicket(string id)
        {
            var toDelete = (await firebase
              .Child("BookedTickets")
              .OnceAsync<BookedTicket>()).Where(a => a.Object.Id == id).FirstOrDefault();
            await firebase.Child("BookedTickets").Child(toDelete.Key).DeleteAsync();
        }

        public string GenerateTicketId(int length = 10)
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

        public double countBookFlightRegulation(Flight flight)
        {
            string[] tourStartTime = flight.StartDate.Split('/');

            string[] splitYear = tourStartTime[2].Split(' ');
            DateTime time = new DateTime(
                int.Parse(splitYear[0]),
                int.Parse(tourStartTime[1]),
                int.Parse(tourStartTime[0])
                );

            DateTime currentTime = DateTime.Now.AddDays(0);
            TimeSpan interval = time.Subtract(currentTime);
            double count = interval.Days;
            return count;
        }
    }
}

