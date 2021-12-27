using LikeFly.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeFly.Model
{
    public class IntermediaryAirport: ObservableObject
    {
        public IntermediaryAirport() { }

        public IntermediaryAirport(Airport airport, string stopTime)
        {
            Airport = airport;
            StopTime = stopTime;
        }
        /*private string airportId;
        public string AirportId
        {
            get { return airportId; }
            set
            {
                airportId = value;
                OnPropertyChanged("AirportId");
            }
        }*/

        private Airport airport;
        public Airport Airport
        {
            get { return airport; }
            set
            {
                airport = value;
                OnPropertyChanged("Airport");
            }
        }
        private string stopTime;
        public string StopTime
        {
            get { return stopTime; }
            set
            {
                stopTime = value;
                OnPropertyChanged("StopTime");
            }
        }

    }
}
