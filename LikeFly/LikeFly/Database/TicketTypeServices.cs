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
    public class TicketTypeServices
    {
        FirebaseClient firebase = new FirebaseClient("https://likefly-5ec61-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseStorage storage = new FirebaseStorage("likefly-5ec61.appspot.com");

        public TicketTypeServices()
        {
        }
        public async Task<List<TicketType>> GetAllTickets()
        {
            int a = 0;
            return (await firebase
              .Child("TicketTypes")
              .OnceAsync<TicketType>()).Select(item => new TicketType
              {
                  Id = item.Object.Id,
                  Name = item.Object.Name,
                  Percent = item.Object.Percent,
                  IsUsed = item.Object.IsUsed
              }).ToList();
        }
        public async Task AddTicket(TicketType ticket)
        {
            await firebase
              .Child("TicketTypes")
              .PostAsync(new TicketType()
              {
                  Id = ticket.Id,
                  Name = ticket.Name,
                  Percent = ticket.Percent,
                  IsUsed = ticket.IsUsed
              });
        }
        public async Task UpdateTicketType(TicketType ticket)
        {
            var toUpdatePlace = (await firebase
              .Child("TicketTypes")
              .OnceAsync<Airport>()).Where(a => a.Object.Id == ticket.Id).FirstOrDefault();

            await firebase
              .Child("TicketTypes")
              .Child(toUpdatePlace.Key)
              .PutAsync(new TicketType
              {
                  Id = ticket.Id,
                  Name = ticket.Name,
                  Percent = ticket.Percent,
                  IsUsed = ticket.IsUsed
              });
        }
        public async Task DeleteTicketType(TicketType ticket)
        {
            var toDeleted = (await firebase
               .Child("TicketTypes").OnceAsync<TicketType>()).FirstOrDefault(p => p.Object.Id == ticket.Id);

            await firebase.Child("Airports").Child(toDeleted.Key).DeleteAsync();

        }
    }
}
