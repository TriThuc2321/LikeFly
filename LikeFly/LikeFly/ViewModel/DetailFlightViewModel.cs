using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    class DetailFlightViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command NotificaitonCommand { get; }
        public Command MenuCommand { get; }

        public DetailFlightViewModel() { }
        public DetailFlightViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            Flight = DataManager.Ins.CurrentFlight;

            CalTime();
        }

        private void CalTime()
        {
            try
            {
                string[] date = Flight.StartDate.Split('/');
                string[] time = Flight.StartTime.Split(':');
                string[] durationTemp = Flight.Duration.Split('h');

                DateTime start = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]), int.Parse(time[0]), int.Parse(time[1]), 0);

                TimeSpan duration;
                if (durationTemp[1] == "")
                {
                    duration = new TimeSpan(int.Parse(durationTemp[0]), 0, 0);
                }
                else
                {
                    duration = new TimeSpan(int.Parse(durationTemp[0]), int.Parse(durationTemp[1]), 0);
                }
                CultureInfo viVn = new CultureInfo("vi-VN");
                EndDate = start.Add(duration).ToString("d", viVn);
                EndTime = start.Add(duration).GetDateTimeFormats('t')[2];

            }
            catch { }
        }
        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });
        public ICommand BookCommand => new Command<object>((obj) =>
        {
            if(IsOccured(DataManager.Ins.CurrentFlight))
            {
                DependencyService.Get<IToast>().ShortToast("Chuyến bay đã đi, không thể đặt");
                return;
            }
            navigation.PushAsync(new BookFlightView());
        });

        public bool IsOccured(Flight flight)
        {         

            string[] flightStartDate = flight.StartDate.Split('/');
            string[] flightStartTime = flight.StartTime.Split(':');
            DateTime timeStart = new DateTime(
                int.Parse(flightStartDate[2]),
                int.Parse(flightStartDate[1]),
                int.Parse(flightStartDate[0]),
                int.Parse(flightStartTime[0]),
                int.Parse(flightStartTime[1]),
                0
                );

            DateTime currentTime = DateTime.Now.AddDays(0);
            TimeSpan interval = timeStart.Subtract(currentTime);

            /// string maxDuration = int.Parse(duration[0]) > int.Parse(duration[1]) ? duration[0] : duration[1];           

            // Thoi gian bat dau flight den current time
            double count = interval.TotalSeconds;
            if (count > 0)
            {
                return false;
            }           
            return true;
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

        private string endDate;
        public string EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        private string endTime;
        public string EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }
    }
}
