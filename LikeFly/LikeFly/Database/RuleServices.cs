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
    public class RuleServices
    {
        FirebaseClient firebase = new FirebaseClient("https://likefly-5ec61-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseStorage storage = new FirebaseStorage("likefly-5ec61.appspot.com");
        public List<Rule> RuleList { get; set; }

        public RuleServices()
        {
        }
        public async Task<List<Rule>> GetRule()
        {
            RuleList = (await firebase
             .Child("Rule")
             .OnceAsync<Rule>()).Select(item => new Rule
             {
                 Deduct = item.Object.Deduct,
                 DayNum = item.Object.DayNum,
                 Id = item.Object.Id,
             }).ToList();

           for (int i = 0; i < RuleList.Count - 1; i++)
            {
                for (int j = i + 1; j < RuleList.Count; j ++)
                {
                    if (int.Parse(RuleList[i].DayNum.Trim()) > int.Parse(RuleList[j].DayNum.Trim()))
                    {
                        var temp = RuleList[j];
                        RuleList[j] = RuleList[i];
                        RuleList[i] = temp;
                    }
                }
            }

            return RuleList;
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