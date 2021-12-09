using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class EditTicketTypeViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command NotificaitonCommand { get; }

        public EditTicketTypeViewModel() { }
        public EditTicketTypeViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            SelectedTicketType = DataManager.Ins.CurrentTicketType;
            NewTicketTypeName = SelectedTicketType.Name;
            NewTicketTypePercent = SelectedTicketType.Percent.ToString();

        }
        private TicketType selectedTicketType;
        public TicketType SelectedTicketType
        {
            get { return selectedTicketType; }
            set
            {
                selectedTicketType = value;
                OnPropertyChanged("SelectedTicketType");

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
        public string newTicketTypeName;
        public string NewTicketTypeName
        {
            get { return newTicketTypeName; }
            set
            {
                newTicketTypeName = value;
            }
        }
        public ICommand SaveCommand => new Command<object>(async (obj) =>
        {
            if (NewTicketTypeName != "" && NewTicketTypeName != null && newTicketTypePercent != "" && newTicketTypePercent != null)
            {
                DataManager.Ins.CurrentTicketType.Name = newTicketTypeName;
                DataManager.Ins.CurrentTicketType.Percent = float.Parse(newTicketTypePercent);
                DependencyService.Get<IToast>().ShortToast("Hệ thống đang xử lý vui lòng chờ");


                SelectedTicketType = DataManager.Ins.CurrentTicketType;
                await DataManager.Ins.TicketTypeService.UpdateTicketType(DataManager.Ins.CurrentTicketType);
                UpdateList();
                DependencyService.Get<IToast>().ShortToast("Sửa hạng vé thành công");
                await navigation.PopAsync();
            }
            else if (NewTicketTypeName == "" || NewTicketTypeName == null || newTicketTypePercent == null || newTicketTypePercent == "")
            {
                DependencyService.Get<IToast>().ShortToast("Vui lòng điền đầy đủ thông tin hạng vé");
            }
        });
        public ICommand DeleteCommand => new Command<object>(async (obj) =>
        {
            DependencyService.Get<IToast>().ShortToast("Hệ thống đang xử lý vui lòng chờ");
            DataManager.Ins.CurrentTicketType.IsUsed = false;
            SelectedTicketType.IsUsed = false;
            UpdateList();
            await DataManager.Ins.TicketTypeService.UpdateTicketType(DataManager.Ins.CurrentTicketType);
            DependencyService.Get<IToast>().ShortToast("Xóa thành công hạng vé");
            await navigation.PopAsync();

        });

        private void UpdateList()
        {
            for (int i = 0; i < DataManager.Ins.ListTicketTypes.Count; i++)
            {
                if (DataManager.Ins.ListTicketTypes[i].Id == SelectedTicketType.Id)
                {
                    if (SelectedTicketType.IsUsed == false)
                    {
                        DataManager.Ins.ListTicketTypes.RemoveAt(i);
                        break;
                    }
                    DataManager.Ins.ListTicketTypes[i] = SelectedTicketType;
                }
            }
        }
    }
}
