using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeFly.Database
{
    public class InvoicesService
    {
        FirebaseClient firebase = new FirebaseClient("https://likefly-5ec61-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseClient storage = new FirebaseClient("likefly-5ec61.appspot.com");

        public List<Invoice> invoices;

        public InvoicesService() { }
        public async Task<List<Invoice>> GetAllInvoice()
        {
            return (await firebase
              .Child("Invoices")
              .OnceAsync<Invoice>()).Select(item => new Invoice
              {
                  Id = item.Object.Id,
                  Discount = item.Object.Discount,
                  DiscountMoney = item.Object.DiscountMoney,
                  Price = item.Object.Price,
                  IsPaid = item.Object.IsPaid,
                  PayingTime = item.Object.PayingTime,
                  Amount = item.Object.Amount,
                  Method = item.Object.Method,
                  Total = item.Object.Total,
                  Photo = item.Object.Photo,
                  TicketTypes = item.Object.TicketTypes
                  
              }).ToList();
        }

        public async Task AddInvoice(Invoice invoice)
        {
            await firebase
              .Child("Invoices")
              .PostAsync(new Invoice()
              {
                  Id = invoice.Id,
                  Discount = new Discount { id = invoice.Discount.id },
                  DiscountMoney = invoice.DiscountMoney,
                  Price = invoice.Price,
                  IsPaid = invoice.IsPaid,
                  PayingTime = invoice.PayingTime,
                  Amount = invoice.Amount,
                  Method = invoice.Method,
                  Total = invoice.Total,
                  Photo = invoice.Photo,
                  TicketTypes = invoice.TicketTypes
              });
        }

        public async Task DeleteInvoice(string id)
        {
            var toDelete = (await firebase
              .Child("Invoices")
              .OnceAsync<Invoice>()).Where(a => a.Object.Id == id).FirstOrDefault();
            await firebase.Child("Invoices").Child(toDelete.Key).DeleteAsync();
        }

        public async Task UpdateInvoice(Invoice invoice)
        {
            var toUpdateInvoice = (await firebase
                 .Child("Invoices")
                 .OnceAsync<Invoice>()).Where(a => a.Object.Id == invoice.Id).FirstOrDefault();

            await firebase
              .Child("Invoices")
              .Child(toUpdateInvoice.Key)
              .PutAsync(new Invoice
              {
                  Id = invoice.Id,
                  Discount = new Discount { id = invoice.Discount.id },
                  DiscountMoney = invoice.DiscountMoney,
                  Price = invoice.Price,
                  IsPaid = invoice.IsPaid,
                  PayingTime = invoice.PayingTime,
                  Amount = invoice.Amount,
                  Method = invoice.Method,
                  Total = invoice.Total,
                  Photo = invoice.Photo,
                  TicketTypes = invoice.TicketTypes
              }) ;

        }

        async public Task<string> savePhoto(Stream imgStream, string invoiceId)
        {
            var storageImage = await new FirebaseStorage("likefly-5ec61.appspot.com")
                .Child("Invoices").Child(invoiceId)
                .Child("banking.png")
                .PutAsync(imgStream);
            var imgurl = storageImage;
            return imgurl;
        }

        public string GenerateInvoiceId()
        {
            int length = 15;
            var List = DataManager.Ins.ListInvoice;
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
        public string FormatMoney(string money)
        {
            if(money != null)
            {
                try
                {
                    double value = double.Parse(money);
                    value /= 1000;
                    value *= 1000;
                    return string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", value);
                }
                catch { return ""; }                
            }
            
            return "";
        }

        public string RoundMoney(int money)
        {
            string moneyStr = money.ToString();
            string numberEnd = moneyStr.Substring(moneyStr.Length - 3, 3);
            string numberStart = moneyStr.Substring(0, moneyStr.Length - 3);

            if (int.Parse(numberEnd) >= 500)
            {

                int round = int.Parse(numberStart.ToString() + numberEnd.ToString());
                while (round % 1000 != 0)
                {
                    round++;
                }

                moneyStr = round.ToString();
            }
            else if (int.Parse(numberEnd) >= 1 && int.Parse(numberEnd) <= 499)
            {
                int round = int.Parse(numberStart.ToString() + numberEnd.ToString());
                while (round % 1000 != 0)
                {
                    round--;
                }
                moneyStr = round.ToString();
            }

            return moneyStr;
        }
    }

}
