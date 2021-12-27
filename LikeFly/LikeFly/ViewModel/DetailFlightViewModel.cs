using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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
