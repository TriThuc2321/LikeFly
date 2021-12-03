using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LikeFly.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LikeFly.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserView : ContentPage
    {
        public UserView()
        {
            InitializeComponent();
            this.BindingContext = new UserViewModel(Navigation, Shell.Current);
        }
    }
}