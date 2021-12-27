using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class TicketTypeManagerViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;


        public TicketTypeManagerViewModel() { }
        public TicketTypeManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            InitList();
        }
        private void InitList()
        {
            ListTicketType = DataManager.Ins.ListTicketTypes;
        }
        public ICommand BackCommand => new Command<object>(async (obj) =>
        {
            //await currentShell.GoToAsync($"{nameof(NewAirportView)}");
            await navigation.PopAsync();
        });
        public ICommand BackCommand2 => new Command<object>(async (obj) =>
        {
            await navigation.PushAsync(new NewTicketTypeView());
        });


        private ObservableCollection<TicketType> listTicketType;
        public ObservableCollection<TicketType> ListTicketType
        {
            get { return listTicketType; }
            set
            {
                listTicketType = value;
                OnPropertyChanged("ListTicketType");
            }
        }
        private TicketType selectedTicketType;
        public TicketType SelectedTicketType { 
            get { return selectedTicketType; }
            set
            {
                selectedTicketType = value;
                OnPropertyChanged("SelectedTicketType");

            }
        }

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            TicketType result = obj as TicketType;
            if (result != null)
            {
                DataManager.Ins.CurrentTicketType = result;
                navigation.PushAsync(new EditTicketTypeView());
                SelectedTicketType = null;
            }
        });
    
}
}
