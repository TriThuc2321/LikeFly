﻿using Firebase.Database;
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
    public class RuleServices
    {
        FirebaseClient firebase = new FirebaseClient("https://likefly-5ec61-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseStorage storage = new FirebaseStorage("likefly-5ec61.appspot.com");
        public List<Rule> Rule { get; set; }

        public RuleServices()
        {
        }
        public async Task<List<Rule>> GetRule()
        {
            Rule = (await firebase
             .Child("Rule")
             .OnceAsync<Rule>()).Select(item => new Rule
             {
                 Deduct = item.Object.Deduct,
                 DayNum = item.Object.DayNum,
                 Id = item.Object.Id,
             }).ToList();


            return Rule;
        }

        public async Task UpdateRule(Rule rule)
        {
            var toUpdatePlace = (await firebase
              .Child("Rule")
              .OnceAsync<Rule>()).Where(a => a.Object.Id == rule.Id).FirstOrDefault();

            await firebase
              .Child("Rule")
              .Child(toUpdatePlace.Key)
              .PutAsync(new Rule
              {
                  Id = rule.Id,
                  DayNum = rule.DayNum,
                  Deduct = rule.Deduct
              });
        }

        public async Task SendNewRule(string id, string dayNum, string deduct)
        {
            await firebase
              .Child("Rule")
              .PostAsync(new Rule()
              {
                  Id = id,
                  DayNum = dayNum,
                  Deduct = deduct

              });
        }
        public async Task DeleteRule(Rule rule)
        {
            var toDeleted = (await firebase
               .Child("Rule").OnceAsync<Flight>()).FirstOrDefault(p => p.Object.Id == rule.Id);

            await firebase.Child("Rule").Child(toDeleted.Key).DeleteAsync();

        }

    }
}