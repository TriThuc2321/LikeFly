using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class EditAirportViewModel :ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command OpenLibrary { get; }

        public EditAirportViewModel() { }
        public EditAirportViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            InitProvinceList();
            SelectedAirport = DataManager.Ins.CurrentAirport;
            SelectedProvince = SelectedAirport.Province;
            NewAirportName = SelectedAirport.Name;
            Img = SelectedAirport.ImgSource;
            OpenLibrary = new Command(addHandleAsync);


        }
        #region Add
        private ImageSource img;
        public ImageSource Img
        {
            get { return img; }
            set
            {
                img = value;
                OnPropertyChanged("Img");
            }
        }
        public Stream imgTemp;
        private async void addHandleAsync(object obj)
        {
            await CrossMedia.Current.Initialize();
            var imgData = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());

            if (imgData != null)
            {
                Img = ImageSource.FromStream(imgData.GetStream);
                imgTemp = imgData.GetStream();
            }

        }
        public ICommand SaveCommand => new Command<object>(async (obj) =>
        {
            if (NewAirportName != "" && NewAirportName != null && selectedProvince != null)
            {
                DataManager.Ins.CurrentAirport.Name = NewAirportName;
                DataManager.Ins.CurrentAirport.Province = SelectedProvince;
                string url = SelectedAirport.ImgSource;
                DependencyService.Get<IToast>().ShortToast("Hệ thống đang xử lý vui lòng chờ");

                if (imgTemp != null)
                {
                 url =  await DataManager.Ins.AirportServices.saveImage(imgTemp, SelectedAirport.Id);
                DataManager.Ins.CurrentAirport.ImgSource = url;
                }
                SelectedAirport = DataManager.Ins.CurrentAirport;
                await DataManager.Ins.AirportServices.UpdateAirport(DataManager.Ins.CurrentAirport);
                UpdateList();
                DependencyService.Get<IToast>().ShortToast("Sửa thành công sân bay");
                await navigation.PopAsync();
            }
            else if (NewAirportName == "" || NewAirportName == null || selectedProvince == null)
            {
                DependencyService.Get<IToast>().ShortToast("Vui lòng điền đầy đủ thông tin sân bay");
            }
        }); 
        public ICommand DeleteCommand => new Command<object>(async (obj) =>
        {
            DependencyService.Get<IToast>().ShortToast("Hệ thống đang xử lý vui lòng chờ");
            DataManager.Ins.CurrentAirport.Enable = false;
            SelectedAirport.Enable = false;
            UpdateList();
            await DataManager.Ins.AirportServices.UpdateAirport(DataManager.Ins.CurrentAirport);
            DependencyService.Get<IToast>().ShortToast("Xóa thành công sân bay");
            await navigation.PopAsync();
          
        });

        private void UpdateList()
        {
            for (int i = 0; i < DataManager.Ins.ListAirports.Count; i++)
            {
                if (DataManager.Ins.ListAirports[i].Id == SelectedAirport.Id)
                {
                    if (SelectedAirport.Enable == false)
                    {
                        DataManager.Ins.ListAirports.RemoveAt(i);
                        break;
                    }
                    DataManager.Ins.ListAirports[i] = SelectedAirport;
                }
            }
        }
        #endregion
        #region Init Province List
        private void InitProvinceList()
        {
            ProvinceList = new List<string>();
            ProvinceList.Add("An Giang");
            ProvinceList.Add("Bà rịa – Vũng tàu");
            ProvinceList.Add("Bắc Giang");
            ProvinceList.Add("Bắc Kạn");
            ProvinceList.Add("Bạc Liêu");
            ProvinceList.Add("Bắc Ninh");
            ProvinceList.Add("Bến Tre");
            ProvinceList.Add("Bình Định");
            ProvinceList.Add("Bình Dương");
            ProvinceList.Add("Bình Phước");
            ProvinceList.Add("Bình Thuận");
            ProvinceList.Add("Cà Mau");
            ProvinceList.Add("Cần Thơ");
            ProvinceList.Add("Cao Bằng ");
            ProvinceList.Add("Đà Nẵng");
            ProvinceList.Add("Đắk Lắk");
            ProvinceList.Add("Đắk Nông");
            ProvinceList.Add("Điện Biên");
            ProvinceList.Add("Đồng Nai");
            ProvinceList.Add("Đồng Tháp");
            ProvinceList.Add("Gia Lai");
            ProvinceList.Add("Hà Giang");
            ProvinceList.Add("Hà Nam");
            ProvinceList.Add("Hà Nội");
            ProvinceList.Add("Hà Tĩnh");
            ProvinceList.Add("Hải Dương");
            ProvinceList.Add("Hải Phòng");
            ProvinceList.Add("Hậu Giang");
            ProvinceList.Add("Hòa Bình");
            ProvinceList.Add("Hưng Yên");
            ProvinceList.Add("Khánh Hòa");
            ProvinceList.Add("Kiên Giang");
            ProvinceList.Add("Kon Tum");
            ProvinceList.Add("Lai Châu");
            ProvinceList.Add("Lâm Đồng");
            ProvinceList.Add("Lạng Sơn");
            ProvinceList.Add("Lào Cai");
            ProvinceList.Add("Long An");
            ProvinceList.Add("Nam Định");
            ProvinceList.Add("Nghệ An");
            ProvinceList.Add("Ninh Bình");
            ProvinceList.Add("Ninh Thuận");
            ProvinceList.Add("Phú Thọ");
            ProvinceList.Add("Phú Yên");
            ProvinceList.Add("Quảng Bình");
            ProvinceList.Add("Quảng Nam");
            ProvinceList.Add("Quảng Ngãi");
            ProvinceList.Add("Quảng Ninh");
            ProvinceList.Add("Quảng Trị");
            ProvinceList.Add("Sóc Trăng");
            ProvinceList.Add("Sơn La");
            ProvinceList.Add("Tây Ninh");
            ProvinceList.Add("Thái Bình");
            ProvinceList.Add("Thái Nguyên");
            ProvinceList.Add("Thanh Hóa");
            ProvinceList.Add("Thừa Thiên Huế");
            ProvinceList.Add("Tiền Giang");
            ProvinceList.Add("Thành phố Hồ Chí Minh");
            ProvinceList.Add("Trà Vinh");
            ProvinceList.Add("Tuyên Quang");
            ProvinceList.Add("Vĩnh Long");
            ProvinceList.Add("Vĩnh Phúc");
            ProvinceList.Add("Yên Bái");
        }
        public List<string> provinceList;
        public List<string> ProvinceList
        {
            get { return provinceList; }
            set
            {
                provinceList = value;
            }
        }
        #endregion
        private Airport selectedAirport;
        public Airport SelectedAirport
        {
            get { return selectedAirport; }
            set
            {
                selectedAirport = value;
                OnPropertyChanged("SelectedAirport");

            }
        }
        public string selectedProvince;
        public string SelectedProvince
        {
            get { return selectedProvince; }
            set
            {
                selectedProvince = value;
            }
        }
        public string newAirportName;
        public string NewAirportName
        {
            get { return newAirportName; }
            set
            {
                newAirportName = value;
            }
        }
    }
}
