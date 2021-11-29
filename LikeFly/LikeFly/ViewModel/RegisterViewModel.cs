﻿using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    class RegisterViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command EyeCommand { get; }
        public Command EyeConfirmCommand { get; }
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public RegisterViewModel() { }
        public RegisterViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            EyeSource = "eyeOffIcon.png";
            IsPassword = true;
            EyeCommand = new Command(eyeHandle);

            EyeSourceConfirm = "eyeOffIcon.png";
            IsPasswordConfirm = true;
            EyeConfirmCommand = new Command(eyeConfirmHandle);

            LoginCommand = new Command(loginHandleAsync);
            RegisterCommand = new Command(registerHandleAsync);
        }

        async void registerHandleAsync(object obj)
        {
            if (Account == null || Password == null || ConfirmPassword == null || Name == null || Account == "" || Password == "" || ConfirmPassword == "" || Name == "")
            {
                DependencyService.Get<IToast>().ShortToast("Please fill out your information");
            }
            else if (!DataManager.Ins.UsersServices.checkEmail(Account))
            {
                DependencyService.Get<IToast>().ShortToast("Email invalid");
            }
            else if (Password.Length < 6)
            {
                DependencyService.Get<IToast>().ShortToast("Password must be more than 6 characters");
            }
            else if (Password != ConfirmPassword)
            {
                DependencyService.Get<IToast>().ShortToast("Confirm password is incorrect");
            }
            else if (DataManager.Ins.UsersServices.ExistEmail(Account, DataManager.Ins.users))
            {
                DependencyService.Get<IToast>().ShortToast("Email is existed");
            }
            else
            {
                Random rand = new Random();
                string randomCode = (rand.Next(999999)).ToString();
                DataManager.Ins.VerifyCode = randomCode;

                DataManager.Ins.CurrentUser = new User()
                {
                    email = Account,
                    password = DataManager.Ins.UsersServices.Encode(Password),
                    name = Name,
                    contact = "",
                    birthday = "",
                    cmnd = "",
                    profilePic = "defaultUser.png",
                    address = "",
                    score = 0,
                    rank = 3
                };

                await SendEmail("VERIFY CODE", "Thank you for using LikeFly, this is your verify code: " + randomCode, Account);
                DependencyService.Get<IToast>().ShortToast("Verify code has been sent to your email");
                DataManager.Ins.users.Add(DataManager.Ins.CurrentUser);
                DataManager.Ins.ListUser.Add(DataManager.Ins.CurrentUser);
                //navigation.PushAsync(new ConfirmEmailView());
                await currentShell.GoToAsync($"{nameof(ConfirmEmailView)}");
            }

        }

        public async Task SendEmail(string subject, string body, string recipient)
        {
            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("flanerapplication@gmail.com");
                mail.To.Add(recipient);
                mail.Subject = subject;
                mail.Body = body;

                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("flanerapplication@gmail.com", "flanerflaner");

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {

            }
        }
        async void loginHandleAsync(object obj)
        {
            //await navigation.PopAsync();
            await currentShell.GoToAsync("..");
        }


        void eyeHandle(object obj)
        {
            IsPassword = !IsPassword;
            EyeSource = !IsPassword ? "eyeIcon.png" : "eyeOffIcon.png";
        }
        void eyeConfirmHandle(object obj)
        {
            IsPasswordConfirm = !IsPasswordConfirm;
            EyeSourceConfirm = !IsPasswordConfirm ? "eyeIcon.png" : "eyeOffIcon.png";
        }


        private string eyeSource;
        public string EyeSource
        {
            get { return eyeSource; }
            set
            {
                eyeSource = value;
                OnPropertyChanged("EyeSource");
            }
        }
        private bool isPassword;
        public bool IsPassword
        {
            get { return isPassword; }
            set
            {
                isPassword = value;
                OnPropertyChanged("IsPassword");
            }
        }
        private string eyeSourceConfirm;
        public string EyeSourceConfirm
        {
            get { return eyeSourceConfirm; }
            set
            {
                eyeSourceConfirm = value;
                OnPropertyChanged("EyeSourceConfirm");
            }
        }
        private bool isPasswordConfirm;
        public bool IsPasswordConfirm
        {
            get { return isPasswordConfirm; }
            set
            {
                isPasswordConfirm = value;
                OnPropertyChanged("IsPasswordConfirm");
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        private string account;
        public string Account
        {
            get { return account; }
            set
            {
                account = value;
                OnPropertyChanged("Account");
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
            }
        }
    }
}
