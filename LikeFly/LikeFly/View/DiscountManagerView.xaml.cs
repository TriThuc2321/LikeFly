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
    public partial class DiscountManagerView : ContentPage
    {
        public DiscountManagerView()
        {
            InitializeComponent();
            this.BindingContext = new DiscountManagerViewModel(Navigation, Shell.Current);
        }
    }
}