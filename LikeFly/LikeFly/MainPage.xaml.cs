using LikeFly.Database;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LikeFly
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(ResetPasswordView), typeof(ResetPasswordView));
            Routing.RegisterRoute(nameof(ConfirmEmailView), typeof(ConfirmEmailView));
            Routing.RegisterRoute(nameof(UserView), typeof(UserView));
            Routing.RegisterRoute(nameof(ManagerView), typeof(ManagerView));
            Routing.RegisterRoute(nameof(FavoriteView), typeof(FavoriteView));
            Routing.RegisterRoute(nameof(NotificationView), typeof(NotificationView));
            this.BindingContext = DataManager.Ins;
        }
    }
}
