using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    class EditFlightViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command NotificaitonCommand { get; }

        public EditFlightViewModel() { }
        public EditFlightViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            CantEdit = DataManager.Ins.CurrentFlight.IsOccured;
            
            Init();
        }
        void Init()
        {
            Flight = DataManager.Ins.CurrentFlight;
            string[] duration = Flight.Duration.Split('h');
            DurationHour = int.Parse(duration[0]);

            if (duration[1] == "")
            {
                DurationMinute = 0;
            }
            else
            {
                DurationMinute = int.Parse(duration[1]);
            }

            string[] date = Flight.StartDate.Split('/');
            StartDatePicker = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]));

            string[] time = Flight.StartTime.Split(':');
            StartTimePicker = new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), 0);

            AirportsStart = new ObservableCollection<Airport>();
            foreach (Airport airport in DataManager.Ins.ListAirports)
            {
                if (airport.Enable) AirportsStart.Add(airport);
            }

            AirportsEnd = new ObservableCollection<Airport>();
            foreach (Airport airport in DataManager.Ins.ListAirports)
            {
                if (airport.Enable && airport != DataManager.Ins.CurrentFlight.AirportStart) AirportsEnd.Add(airport);
            }

            CurrentPilots = Flight.ListPilots;
            Pilots = new ObservableCollection<User>();
            foreach (User pilot in DataManager.Ins.ListUsers)
            {
                if (pilot.isEnable && pilot.rank == 1 && !ExistPilot(pilot)) Pilots.Add(pilot);
            }
            SelectedPilot = Flight.ListPilots[0];

            DetailTicketTypes = new ObservableCollection<DetailTicketType>();
            foreach (TicketType ticket in DataManager.Ins.ListTicketTypes)
            {
                if (ticket.IsUsed)
                {
                    int total = ExistTicketType(ticket);
                    DetailTicketTypes.Add(new DetailTicketType(ticket, total, total));
                }
            }
        }
        int ExistTicketType(TicketType ticket)
        {
            foreach (DetailTicketType a in Flight.TicketTypes)
            {
                if (ticket.Id == a.TicketType.Id)
                {
                    return a.Total;
                }
            }

            return 0;
        }
        bool ExistPilot(User pilot)
        {
            foreach (User a in DataManager.Ins.CurrentFlight.ListPilots)
            {
                if (pilot.email == a.email) return true;
            }

            return false;
        }
        
        bool checkInfo()
        {
            if (DataManager.Ins.CurrentFlight.IsOccured)
            {
                DependencyService.Get<IToast>().ShortToast("Không thể sửa chuyến bay đã đi");
                return false;
            }
            if (DataManager.Ins.CurrentFlight.Name == null || DataManager.Ins.CurrentFlight.Name == "")
            {
                DependencyService.Get<IToast>().ShortToast("Tên chuyến bay không được để trống");
                return false;
            }
            if (DataManager.Ins.CurrentFlight.Description == null || DataManager.Ins.CurrentFlight.Description == "")
            {
                DependencyService.Get<IToast>().ShortToast("Mô tả không được để trống");
                return false;
            }
            if (DurationHour == 0 && DurationMinute == 0)
            {
                DependencyService.Get<IToast>().ShortToast("Thời gian bay không được để trống");
                return false;
            }
            if (DataManager.Ins.CurrentFlight.Price == 0)
            {
                DependencyService.Get<IToast>().ShortToast("Giá chuyến bay không được để trống");
                return false;
            }
            if (CurrentPilots.Count == 0)
            {
                DependencyService.Get<IToast>().ShortToast("Vui lòng chọn phi công cho chuyến bay");
                return false;
            }           

            int count = 0;
            foreach (DetailTicketType detail in DetailTicketTypes)
            {
                count += detail.Total;
            }
            if (count == 0)
            {
                DependencyService.Get<IToast>().ShortToast("Vui lòng thêm loại hạng vé cho chuyến bay");
                return false;
            }

            return true;
        }
        async Task UpdateFlightAsync()
        {
            if (DurationMinute == 0)
            {
                DataManager.Ins.CurrentFlight.Duration = DurationHour + "h";
            }
            else
            {
                DataManager.Ins.CurrentFlight.Duration = DurationHour + "h" + DurationMinute;
            }

            CultureInfo viVn = new CultureInfo("vi-VN");
            //EndDate = start.Add(duration).ToString("d", viVn);
            //EndTime = start.Add(duration).GetDateTimeFormats('t')[2];
            DataManager.Ins.CurrentFlight.StartDate = StartDatePicker.ToString("d", viVn);
            DataManager.Ins.CurrentFlight.StartTime = StartTimePicker.ToString();
            DataManager.Ins.CurrentFlight.StartTime = DataManager.Ins.CurrentFlight.StartTime.Substring(0, DataManager.Ins.CurrentFlight.StartTime.Length - 3); ;

            DataManager.Ins.CurrentFlight.ListPilots = CurrentPilots;

            DataManager.Ins.CurrentFlight.TicketTypes = new ObservableCollection<DetailTicketType>();
            foreach (DetailTicketType detail in DetailTicketTypes)
            {
                if (detail.Total != 0)
                {
                    detail.Remain = detail.Total;
                    DataManager.Ins.CurrentFlight.TicketTypes.Add(detail);
                }
            }

            await DataManager.Ins.FlightService.UpdateFlight(DataManager.Ins.CurrentFlight);
        }
        public ICommand SaveCommand => new Command<object>(async (obj) =>
        {
            if (checkInfo())
            {
                try
                {
                    await UpdateFlightAsync();
                    DependencyService.Get<IToast>().ShortToast("Cập nhật chuyến bay thành công");
                }
                catch
                {
                    DependencyService.Get<IToast>().ShortToast("Thêm chuyến bay thất bại");
                }
                await navigation.PopAsync();
            }
        });
        public ICommand AddPilotCommand => new Command<object>((obj) =>
        {
            if (SelectedPilot != null)
            {
                if (CurrentPilots == null) CurrentPilots = new ObservableCollection<User>();

                CurrentPilots.Add(SelectedPilot);
                Pilots.Remove(SelectedPilot);
            }
        });
        public ICommand DeletePitlotCommand => new Command<object>((obj) =>
        {
            User result = obj as User;

            if (result != null)
            {
                Pilots.Add(result);
                CurrentPilots.Remove(result);
            }
        });
        public ICommand DeleteIntermediaryCommand => new Command<object>((obj) =>
        {
            IntermediaryAirport result = obj as IntermediaryAirport;

            if (result != null)
            {
                Flight.IntermediaryAirportList.Remove(result);
            }
        });

        public ICommand NewIntermediaryCommand => new Command<object>((obj) =>
        {
            navigation.PushAsync(new NewIntermediaryAirportView());
        });
        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });
        public string GenerateId(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
        public ICommand TempCommand => new Command<object>(async (obj) =>
        {
            ObservableCollection<IntermediaryAirport> intermediary = new ObservableCollection<IntermediaryAirport>();
            Airport a = new Airport("A01", "Nội Bài", "Hà Nội", "11", true);
            intermediary.Add(new IntermediaryAirport(a, "2h30"));
            intermediary.Add(new IntermediaryAirport(a, "2h30"));

            ObservableCollection<DetailTicketType> ticket = new ObservableCollection<DetailTicketType>();
            ticket.Add(new DetailTicketType(new TicketType("TT01", "Phổ thông", 1, true), 100, 100));
            ticket.Add(new DetailTicketType(new TicketType("TT02", "Phổ thông đặc biệt", (float)1.2, true), 100, 100));
            ticket.Add(new DetailTicketType(new TicketType("TT03", "Thương gia", (float)1.3, true), 100, 100));
            ticket.Add(new DetailTicketType(new TicketType("TT04", "Hạng nhất", (float)1.5, true), 100, 100));

            ObservableCollection<User> pilots = new ObservableCollection<User>();
            pilots.Add(new User("pl01", "e10adc3949ba59abbe56e057f20f883e", "Trần Phi Công", "032111223", "21/02/1990", "234564333", "defaultUser.png", "Sóc Trăng", 0, 1, true));

            Flight temp = new Flight("01", "HN-TSN", "5h", "8:30", "30/12/2021", "Hà Nội - TP Hồ Chí Minh", false, 2000000, a, a, intermediary, ticket, pilots);
            await DataManager.Ins.FlightService.AddFlight(temp);
        });
        private bool cantEdit;
        public bool CantEdit
        {
            get { return cantEdit; }
            set
            {
                cantEdit = value;
                OnPropertyChanged("CantEdit");
            }
        }
        private Flight flight;
        public Flight Flight
        {
            get { return flight; }
            set
            {
                flight = value;
                OnPropertyChanged("Flight");
            }
        }

        private int durationHour;
        public int DurationHour
        {
            get { return durationHour; }
            set
            {
                durationHour = value;
                OnPropertyChanged("DurationHour");
            }
        }
        private int durationMinute;
        public int DurationMinute
        {
            get { return durationMinute; }
            set
            {
                durationMinute = value;
                OnPropertyChanged("DurationMinute");
            }
        }

        private DateTime startDatePicker;
        public DateTime StartDatePicker
        {
            get { return startDatePicker; }
            set
            {
                startDatePicker = value;
                OnPropertyChanged("StartDatePikcer");
            }
        }
        private TimeSpan startTimePicker;
        public TimeSpan StartTimePicker
        {
            get { return startTimePicker; }
            set
            {
                startTimePicker = value;
                OnPropertyChanged("StartTimePicker");

            }
        }
        private ObservableCollection<Airport> airportsStart;
        public ObservableCollection<Airport> AirportsStart
        {
            get { return airportsStart; }
            set
            {
                airportsStart = value;
                OnPropertyChanged("AirportsStart");
            }
        }
        private ObservableCollection<Airport> airportsEnd;
        public ObservableCollection<Airport> AirportsEnd
        {
            get { return airportsEnd; }
            set
            {
                airportsEnd = value;
                OnPropertyChanged("AirportsEnd");
            }
        }
       
        private ObservableCollection<User> currentPilots;
        public ObservableCollection<User> CurrentPilots
        {
            get { return currentPilots; }
            set
            {
                currentPilots = value;
                OnPropertyChanged("CurrentPilots");
            }
        }

        private ObservableCollection<User> pilots;
        public ObservableCollection<User> Pilots
        {
            get { return pilots; }
            set
            {
                pilots = value;
                OnPropertyChanged("Pilots");
            }
        }

        private User selectedPilot;
        public User SelectedPilot
        {
            get { return selectedPilot; }
            set
            {
                selectedPilot = value;
                OnPropertyChanged("SelectedPilot");
            }
        }
        private ObservableCollection<DetailTicketType> detailTicketTypes;
        public ObservableCollection<DetailTicketType> DetailTicketTypes
        {
            get { return detailTicketTypes; }
            set
            {
                detailTicketTypes = value;
                OnPropertyChanged("DetailTicketTypes");
            }
        }
    }

}
