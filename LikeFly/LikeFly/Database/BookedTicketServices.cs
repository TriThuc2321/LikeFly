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
                  id = item.Object.id,
                  flight = item.Object.flight,
                  name = item.Object.name,
                  birthday = item.Object.birthday,
                  contact = item.Object.contact,
                  email = item.Object.email,
                  cmnd = item.Object.cmnd,
                  address = item.Object.address,
                  bookTime = item.Object.bookTime,
                  invoice = item.Object.invoice,
                  isCancel = item.Object.isCancel

              }).ToList();
        }
        public async Task AddBookedTicket(BookedTicket bookedTicket)
        {
            /*await firebase
              .Child("BookedTickets")
              .PostAsync(new BookedTicket()
              {
                  id = bookedTicket.id,
                  flight = new Flight() { id = bookedTicket.flight.id },
                  name = bookedTicket.name,
                  birthday = bookedTicket.birthday,
                  contact = bookedTicket.contact,
                  email = bookedTicket.email,
                  cmnd = bookedTicket.cmnd,
                  address = bookedTicket.address,
                  bookTime = bookedTicket.bookTime,
                  invoice = bookedTicket.invoice,
                  isCancel = bookedTicket.isCancel
              });*/
        }


        public async Task UpdateBookedTicket(BookedTicket bookedTicket)
        {
            /*var toUpdateTour = (await firebase
                 .Child("BookedTickets")
                 .OnceAsync<BookedTicket>()).Where(a => a.Object.id == bookedTicket.id).FirstOrDefault();

            await firebase
              .Child("BookedTickets")
              .Child(toUpdateTour.Key)
              .PutAsync(new BookedTicket
              {
                  id = bookedTicket.id,
                  flight = new Flight() { id = bookedTicket.flight.id },
                  name = bookedTicket.name,
                  birthday = bookedTicket.birthday,
                  contact = bookedTicket.contact,
                  email = bookedTicket.email,
                  cmnd = bookedTicket.cmnd,
                  address = bookedTicket.address,
                  bookTime = bookedTicket.bookTime,
                  invoice = bookedTicket.invoice,
                  isCancel = bookedTicket.isCancel
              });*/

        }
        public async Task DeleteBookedTicket(string id)
        {
            var toDelete = (await firebase
              .Child("BookedTickets")
              .OnceAsync<BookedTicket>()).Where(a => a.Object.id == id).FirstOrDefault();
            await firebase.Child("BookedTickets").Child(toDelete.Key).DeleteAsync();
        }

        public string GenerateTicketId()
        {
            int length = 15;
            var List = DataManager.Ins.ListBookedTickets;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

            int i = 0;
            while (i < List.Count())
            {
                if (List[i].id == randomString)
                {
                    i = -1;
                    randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
                }
                i++;
            }
            return randomString;
        }


        //public double countBookTourRegulation(Flight flight)
        //{
        //    string[] tourStartTime = flight.startTime.Split('/');

        //    string[] splitYear = tourStartTime[2].Split(' ');
        //    DateTime time = new DateTime(
        //        int.Parse(splitYear[0]),
        //        int.Parse(tourStartTime[0]),
        //        int.Parse(tourStartTime[1])
        //        );

        //    DateTime currentTime = DateTime.Now.AddDays(0);
        //    TimeSpan interval = time.Subtract(currentTime);
        //    double count = interval.Days;
        //    return count;
        //}
    }
}

