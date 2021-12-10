using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    class NewFlightViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command NotificaitonCommand { get; }

        public NewFlightViewModel() { }
        public NewFlightViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
        }
        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });
        public ICommand SaveCommand => new Command<object>(async (obj) =>
        {
            ObservableCollection<IntermediaryAirport> intermediary = new ObservableCollection<IntermediaryAirport>();
            Airport a = new Airport("A01", "Nội Bài", "Hà Nội", "11", true);
            intermediary.Add(new IntermediaryAirport(a, "2h30" ));
            intermediary.Add(new IntermediaryAirport(a, "2h30" ));

            ObservableCollection<DetailTicketType> ticket = new ObservableCollection<DetailTicketType>();
            ticket.Add(new DetailTicketType( new TicketType("TT01", "Phổ thông", 1, true), 100,100));
            ticket.Add(new DetailTicketType( new TicketType("TT02", "Phổ thông đặc biệt", (float)1.2, true), 100, 100));
            ticket.Add(new DetailTicketType( new TicketType("TT03", "Thương gia", (float)1.3, true), 100, 100));
            ticket.Add(new DetailTicketType( new TicketType("TT04", "Hạng nhất", (float)1.5, true), 100, 100));



            Flight temp = new Flight("01", "HN-TSN", "5h", "8:30","30/12/2021","01","Hà Nội - TP Hồ Chí Minh",100,false,2000000,a,a, intermediary, ticket);
            await DataManager.Ins.FlightService.AddFlight(temp);
        });

    }
}
