﻿using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LikeFly.Database
{
    public class UsersServices
    {
        FirebaseClient firebase = new FirebaseClient("https://likefly-5ec61-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseClient storage = new FirebaseClient("gs://likefly-5ec61.appspot.com");
        public UsersServices()
        {

        }
        async public Task<string> saveImage(Stream imgStream)
        {
            var stroageImage = await new FirebaseStorage("gs://likefly-5ec61.appspot.com")
                .Child("ProfilePic")
                .Child("test.png")
                .PutAsync(imgStream);
            var imgurl = stroageImage;
            return imgurl;
        }

        async public Task addUser(User user)
        {
            await firebase
              .Child("Users")
              .PostAsync(new User()
              {
                  id = user.id,
                  name = user.name,
                  birthday = user.birthday,
                  contact = user.contact,
                  cmnd = user.cmnd,
                  profilePic = user.profilePic
              });
        }
    }
}
