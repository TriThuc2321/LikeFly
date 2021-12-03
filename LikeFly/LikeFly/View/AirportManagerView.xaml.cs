﻿using LikeFly.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LikeFly.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AirportManagerView : ContentPage
    {
        public AirportManagerView()
        {
            InitializeComponent();
            this.BindingContext = new AirportManagerViewModel(Navigation, Shell.Current);

        }
    }
}