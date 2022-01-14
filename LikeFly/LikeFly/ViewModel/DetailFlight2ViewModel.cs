using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class DetailFlight2ViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command NotificaitonCommand { get; }
        public Command MenuCommand { get; }

        public DetailFlight2ViewModel() { }
        public DetailFlight2ViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            Flight = DataManager.Ins.CurrentFlight;
            Percent = "0";
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
                TimeSpan check = DateTime.Now - start;
                if (check.TotalSeconds > 0 && check.TotalSeconds < duration.TotalSeconds)
                    StartCountDownTimer();
                else if (check.TotalSeconds > 0 && check.TotalSeconds >= duration.TotalSeconds)
                {
                    Percent = "1";
                    RemainingTime = "Đã đến nơi";
                }
                else if (check.TotalSeconds < 0)
                {
                    Percent = "0";
                    RemainingTime = "Chưa khởi hành";
                }
            }
            catch { }
        }
        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            if (timer != null && timer.Enabled)
                timer.Stop();
            navigation.PopAsync();
        });
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

        private string percent;
        public string Percent
        {
            get { return percent; }
            set
            {
                percent = value;
                OnPropertyChanged("Percent");
            }
        }
        private string remainingTime;
        public string RemainingTime
        {
            get { return remainingTime; }
            set
            {
                remainingTime = value;
                OnPropertyChanged("RemainingTime");
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

        public void StartCountDownTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 10000;
            timer.Elapsed += t_Tick;
            string[] date = Flight.StartDate.Split('/');
            string[] time = Flight.StartTime.Split(':');
            string[] durationTemp = Flight.Duration.Split('h');
            TimeSpan duration;
            if (durationTemp[1] == "")
            {
                duration = new TimeSpan(int.Parse(durationTemp[0]), 0, 0);
            }
            else
            {
                duration = new TimeSpan(int.Parse(durationTemp[0]), int.Parse(durationTemp[1]), 0);
            }
            DateTime start = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]), int.Parse(time[0]), int.Parse(time[1]), 0);
            TimeSpan ts = DateTime.Now - start;
            RemainingTime = ts.ToString("h'h:'m'p'");
            if (((ts.TotalSeconds / duration.TotalSeconds)) < 1)
                Percent = ((ts.TotalSeconds / duration.TotalSeconds)).ToString();
            else if (((ts.TotalSeconds / duration.TotalSeconds)) >= 1)
                Percent = "Đã đến nơi";
            timer.Start();
        }
        string cTimer;
        System.Timers.Timer timer;
        void t_Tick(object sender, EventArgs e)
        {
            string[] date = Flight.StartDate.Split('/');
            string[] time = Flight.StartTime.Split(':');
            string[] durationTemp = Flight.Duration.Split('h');
            TimeSpan duration;
            if (durationTemp[1] == "")
            {
                duration = new TimeSpan(int.Parse(durationTemp[0]), 0, 0);
            }
            else
            {
                duration = new TimeSpan(int.Parse(durationTemp[0]), int.Parse(durationTemp[1]), 0);
            }
            DateTime start = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]), int.Parse(time[0]), int.Parse(time[1]), 0);
            TimeSpan ts = DateTime.Now - start;
            cTimer = ts.ToString("h'h:'m'p'");
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Code to run on the main thread

                if (((ts.TotalSeconds / duration.TotalSeconds)) < 1)
                {
                    Percent = ((ts.TotalSeconds / duration.TotalSeconds)).ToString();
                    RemainingTime = cTimer;
                }
                else if (((ts.TotalSeconds / duration.TotalSeconds)) >= 1)
                    RemainingTime = "Đã đến nơi";
            });
        }
    }
}
