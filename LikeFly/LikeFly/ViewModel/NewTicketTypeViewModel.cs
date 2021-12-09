using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
   public class NewTicketTypeViewModel :ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command NotificaitonCommand { get; }

        public NewTicketTypeViewModel() { }
        public NewTicketTypeViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
        }
        public ICommand AddCommand => new Command<object>(async (obj) =>
        {
            if (NewTicketTypeName != "" && NewTicketTypeName != "")
            {
                DependencyService.Get<IToast>().ShortToast("Hệ thống đang xử lý vui lòng chờ");
                string TicketTypeId = GenerateId(10);
               
                TicketType newTicketType = new TicketType(TicketTypeId, NewTicketTypeName,float.Parse(NewTicketTypePercent),true);
                await DataManager.Ins.TicketTypeService.AddTicket(newTicketType);
                DataManager.Ins.ListTicketTypes.Add(newTicketType);
                DependencyService.Get<IToast>().ShortToast("Thêm thành công hạng vé");
                await navigation.PopAsync();
            }
            else if (NewTicketTypeName == "" || selectedProvince == null)
            {
                DependencyService.Get<IToast>().ShortToast("Vui lòng điền đầy đủ thông tin");

            }


        });
        public string GenerateId(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
        public List<string> provinceList;
        public List<string> ProvinceList
        {
            get { return provinceList; }
            set
            {
                provinceList = value;
            }
        }
        public string selectedProvince;
        public string SelectedProvince
        {
            get { return selectedProvince; }
            set
            {
                selectedProvince = value;
            }
        }
        public string newTicketTypeName;
        public string NewTicketTypeName
        {
            get { return newTicketTypeName; }
            set
            {
                newTicketTypeName = value;
            }
        }
        public string newTicketTypePercent;
        public string NewTicketTypePercent
        {
            get { return newTicketTypePercent; }
            set
            {
                newTicketTypePercent = value;
            }
        }
    }
}
