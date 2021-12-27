using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using LikeFly.Core;
using LikeFly.Model;

namespace LikeFly.Database
{
    public class SearchService : ObservableObject
    {
        //Người dùng nhập vào tỉnh hoặc tên sân bay tới và đi
        public SearchService() { }

        private string fromPlaceText { get; set; }
        public string FromPlaceText
        {
            get { return fromPlaceText; }
            set
            {
                fromPlaceText = value;
                OnPropertyChanged("FromPlaceText");
            }
        }
        private string toPlaceText { get; set; }
        public string ToPlaceText
        {
            get { return toPlaceText; }
            set
            {
                toPlaceText = value;
                OnPropertyChanged("ToPlaceText");
            }
        }

        private string startDate { get; set; }
        public string StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        private ObservableCollection<Flight> resultList { get; set; }
        public ObservableCollection<Flight> ResultList
        {
            get { return resultList; }
            set
            {
                resultList = value;
                OnPropertyChanged("ResultList");
            }
        }

        public SearchService(string from, string to, string start)
        {
            FromPlaceText = from;
            ToPlaceText = to;
            StartDate = start;
        }
        public void RefreshDataSearch()
        {
            FromPlaceText = "";
            ToPlaceText = "";
            StartDate = "";
        }

        //Ham loc dau tieng viet
        public static string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public void GetSearchResult()
        {
            ResultList = new ObservableCollection<Flight>();
            List<Airport> temp1 = new List<Airport>();
            List<Airport> temp3 = new List<Airport>();
            List<Flight> temp2 = new List<Flight>();
            List<Airport> Fromresulttemp1 = new List<Airport>();
            List<Airport> Toresulttemp3 = new List<Airport>();
            List<Flight> FlightsHasStartDateTime = new List<Flight>();



            foreach (Airport x in DataManager.Ins.ListAirports)
            {
                temp1.Add(x);
            }
            foreach (Airport x in DataManager.Ins.ListAirports)
            {
                temp3.Add(x);
            }
            foreach (Flight x in DataManager.Ins.ListFlights)
            {
                temp2.Add(x);
            }

            List<Flight> FlightsHasFrom = new List<Flight>();
            List<Flight> FlightsHasTo = new List<Flight>();

            List<Flight> preResult = new List<Flight>();

            Fromresulttemp1 = temp1.FindAll(e => convertToUnSign(e.Name).Equals(convertToUnSign(FromPlaceText), StringComparison.CurrentCultureIgnoreCase) ||
                                            convertToUnSign(e.Province).Equals(convertToUnSign(FromPlaceText), StringComparison.CurrentCultureIgnoreCase));

            Toresulttemp3 = temp3.FindAll(e => convertToUnSign(e.Name).Equals(convertToUnSign(ToPlaceText), StringComparison.CurrentCultureIgnoreCase) ||
                                            convertToUnSign(e.Province).Equals(convertToUnSign(ToPlaceText), StringComparison.CurrentCultureIgnoreCase));

            //Danh sach chuyen bay xuat phat tu
            foreach(Flight x in temp2)
            {
                foreach(Airport y in Fromresulttemp1)
                {
                    if(x.AirportStart.Id == y.Id)
                    {
                        FlightsHasFrom.Add(x);
                    }
                }
            }

            //Danh sach chuyen bay den
            foreach (Flight x in temp2)
            {
                foreach (Airport y in Toresulttemp3)
                {
                    if (x.AirportEnd.Id == y.Id)
                    {
                        FlightsHasTo.Add(x);
                    }
                }
            }

            //Danh sach chuyen bay co ngay khoi hanh nhu nguoi dung chon
            FlightsHasStartDateTime = temp2.FindAll(e => e.StartDate == StartDate);


            //Danh sach chuyen bay co diem di va diem den theo ng dung nhap
            foreach(Flight x in FlightsHasFrom)
            {
                foreach(Flight y in FlightsHasTo)
                {
                    if(x.Id == y.Id)
                    {
                        preResult.Add(x);
                    }
                }
            }

            List<Flight> result = new List<Flight>();

            //Danh sach ket qua tim kiem
            foreach(Flight x in FlightsHasStartDateTime)
            {
                foreach(Flight y in preResult)
                {
                    if(x.Id == y.Id)
                    {
                        result.Add(x);
                    }
                }
            }
            foreach (Flight plc in result)
                if (!ResultList.Contains(plc))
                    ResultList.Add(plc);
        }
    }






 }
