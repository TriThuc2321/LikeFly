using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    
    class NewPilotViewModel: ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;
        public NewPilotViewModel() { }
        public NewPilotViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;

            CurrUser = new User();
            init();
        }
        void init()
        {
            ListTypes = new ObservableCollection<string>();

            ListTypes.Add("Phi công");
            ListTypes.Add("Quản lí");

            SelectedType = "Phi công";
        }
        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });
        public ICommand AddCommand => new Command<object>(async (obj) =>
        {
            if (checkInfo())
            {
                await updateDatabaseAsync();
                await navigation.PopAsync();
            }
        });

        private async Task updateDatabaseAsync()
        {
            CurrUser.password = DataManager.Ins.UsersServices.Encode(CurrUser.password);
            CurrUser.profilePic = "defaultUser.png";
            CurrUser.address = "";
            CurrUser.birthday = "";
            CurrUser.score = 0;
            CurrUser.rank = 2;
            CurrUser.isEnable = true;

            if(SelectedType == "Phi công")
            {
                CurrUser.rank = 2;
            }
            else
            {
                CurrUser.rank = 1;
            }

            await DataManager.Ins.UsersServices.addUser(CurrUser);
            DataManager.Ins.users.Add(CurrUser);
            DataManager.Ins.ListUsers.Add(CurrUser);

            DependencyService.Get<IToast>().ShortToast("Thêm nhân viên thành công");
        }

        private bool checkInfo()
        {
            if (string.IsNullOrWhiteSpace(CurrUser.name) || string.IsNullOrWhiteSpace(CurrUser.contact) || string.IsNullOrWhiteSpace(CurrUser.cmnd) || string.IsNullOrWhiteSpace(CurrUser.email) || string.IsNullOrWhiteSpace(CurrUser.password))
            {
                DependencyService.Get<IToast>().ShortToast("Hãy điền đầy đủ thông tin");
                return false;
            }

            if (!DataManager.Ins.UsersServices.checkEmail(CurrUser.email))
            {
                DependencyService.Get<IToast>().ShortToast("Email không đúng định dạng");
                return false;
            }

            foreach (User user in DataManager.Ins.ListUsers)
            {
                if (user.email == CurrUser.email)
                {
                    DependencyService.Get<IToast>().ShortToast("Tài khoản đã tồn tại");
                    return false;
                }
            }

            if (CurrUser.password.Length < 6)
            {
                DependencyService.Get<IToast>().ShortToast("Mật khẩu dài hơn 5 kí tự");
                return false;
            }

            if (ContactValidation(CurrUser.contact) == false)
            {
                DependencyService.Get<IToast>().ShortToast("Số điện thoại cần có 10 chữ số và bắt đầu bằng 0");
                return false;
            }

            if (IDCardValidation(CurrUser.cmnd) == false)
            {
                DependencyService.Get<IToast>().ShortToast("CMND/CCCD cần có 9 chữ số hoặc 12 chữ số");
                return false;
            }
            return true;
        }
        
        private bool ContactValidation(string contact)
        {
            if (contact.Length == 10 && contact[0].ToString() == "0" && contact.All(char.IsDigit))
            {
                return true;
            }
            return false;
        }
        private bool IDCardValidation(string vcmnd)
        {
            bool ex;
            if (!vcmnd.All(char.IsDigit))
            {
                ex = false;
            }
            else
            {
                if (vcmnd.Length < 9)
                {
                    ex = false;
                }
                else if (vcmnd.Length > 9)
                {
                    if (vcmnd.Length < 12)
                    {
                        ex = false;
                    }
                    else if (vcmnd.Length > 12)
                    {
                        ex = false;
                    }
                    else
                    {
                        ex = true;
                    }
                }
                else
                {
                    ex = true;
                }
            }
            return ex;
        }
        private User currUser;
        public User CurrUser
        {
            get { return currUser; }
            set
            {
                currUser = value;
                OnPropertyChanged("CurrUser");
            }
        }
        public ObservableCollection<string> listTypes;
        public ObservableCollection<string> ListTypes
        {
            get { return listTypes; }
            set
            {
                listTypes = value;
                OnPropertyChanged("ListTypes");
            }
        }
        public string selectedType;
        public string SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                OnPropertyChanged("SelectedType");
            }
        }
    }        
}
