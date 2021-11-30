using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using System.Linq;

namespace LikeFly.Database
{
    public class NotificationServices
    {
        FirebaseClient firebase = new FirebaseClient("https://likefly-5ec61-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseStorage storage = new FirebaseStorage("likefly-5ec61.appspot.com");
        public List<Notification> ListAllNoti { get; set; }
  
        public NotificationServices() {}
        public async Task<List<Notification>> GetAllNotification()
        {
            ListAllNoti = (await firebase
              .Child("Notification")
              .OnceAsync<Notification>()).Select(item => new Notification
              {
                  id = item.Object.id,
                  reciever = item.Object.reciever,
                  isVisible = item.Object.isVisible,
                  flightId = item.Object.flightId,
                  title = item.Object.title,
                  isChecked = item.Object.isChecked,
                  body = item.Object.body,
                  when = item.Object.when,

              }).ToList();
            ListAllNoti.Reverse();

            return ListAllNoti;
        }

        //Add a noti into DB
        public async Task SendNoti(string id, string reciever, string body, string title, string flightId)
        {
            await firebase
              .Child("Notification")
              .PostAsync(new Notification()
              {
                  id = id,            
                  reciever = reciever,
                  flightId = flightId,
                  isVisible = "True",
                  body = body,
                  isChecked = "False",
                  title = title,
                  when = DateTime.Now,
              });
        }

        public async Task UpdateNoti(Notification notification)
        {
            var toUpdateNotification = (await firebase
              .Child("Notification")
              .OnceAsync<Notification>()).Where(a => a.Object.id == notification.id).FirstOrDefault();

            await firebase
              .Child("Notification")
              .Child(toUpdateNotification.Key)
              .PutAsync(new Notification
              {
                  id = notification.id,
                  isChecked = notification.isChecked,
                  reciever = notification.reciever,
                  isVisible = notification.isVisible,
                  flightId = notification.flightId,
                  body = notification.body,
                  when = notification.when,
                  title = notification.title,
              });
        }
    }
}
