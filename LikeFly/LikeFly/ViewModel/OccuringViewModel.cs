using LikeFly.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class OccuringViewModel: ObservableObject
    {
        Shell shell;
        INavigation navigation;

        public OccuringViewModel() { }

        public OccuringViewModel(INavigation navigation, Shell shell)
        {
            this.shell = shell;
            this.navigation = navigation;
        }
    }
}
