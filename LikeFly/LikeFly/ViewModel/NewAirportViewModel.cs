using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class NewAirportViewModel :ObservableObject
    {
        INavigation navigation;

        public Command BackCommand { get; }
        public Command AddImageCommand { get; }

        public NewAirportViewModel() { }
        public NewAirportViewModel(INavigation _navigation)
        {
            this.navigation = _navigation;
            SelectedAirport = DataManager.Ins.CurrentAirport;
            InitProvinceList();
            BackCommand = new Command(() =>  navigation.PopAsync());
            AddImageCommand = new Command(addHandleAsync);

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
        public ICommand AddCommand => new Command<object>(async (obj) =>
        {
            if (imgTemp != null && NewAirportName != "" && selectedProvince != null)
            {
                DependencyService.Get<IToast>().ShortToast("Hệ thống đang xử lý vui lòng chờ");
                string AirPortId = GenerateId(10);
                string url = await DataManager.Ins.AirportServices.saveImage(imgTemp, AirPortId, 0);
                Airport newAirport = new Airport(AirPortId, NewAirportName, selectedProvince, url);
                await DataManager.Ins.AirportServices.AddAirport(newAirport);
                DataManager.Ins.ListAirports.Add(newAirport);
                DependencyService.Get<IToast>().ShortToast("Thêm thành công sân bay");
                await navigation.PopAsync();
            }
            else if (imgTemp == null)
            {
                DependencyService.Get<IToast>().ShortToast("Vui lòng thêm hình ảnh cho sân bay");
            }
            else if (NewAirportName == "" || selectedProvince == null)
            {
                DependencyService.Get<IToast>().ShortToast("Vui lòng điền đầy đủ thông tin sân bay");

            }


        });
        #endregion
        public string GenerateId(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
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

    }
}
