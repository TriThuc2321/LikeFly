﻿using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    class LoginViewModel : ObservableObject
    {
        INavigation navigation;
        Shell curentShell;
        public LoginViewModel() { }

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public Command ForgotPasswordCommand { get; }
        public Command RememberCommand { get; }
        public Command EyeCommand { get; }
        public LoginViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.curentShell = curentShell;
            bool flag = DataManager.Ins.LoadData;

            RememberLogin();

            LoginCommand = new Command(loginHandleAsync);
            RegisterCommand = new Command(registerHandle);
            ForgotPasswordCommand = new Command(forgotHandle);

            EyeSource = "eyeOffIcon.png";
            IsPassword = true;
            EyeCommand = new Command(eyeHandle);

            if (Preferences.Get("remeber_key", "") == "true")
            {
                Account = Preferences.Get("email_key", "");
                Password = Preferences.Get("password_key", "");
                RememberAccount = true;
            }

        }

        private void RememberLogin()
        {
            string email = Preferences.Get("email_key", "");
            if (Preferences.Get("remeber_login_key", "") == "true")
            {
                for (int i = 0; i < DataManager.Ins.ListUsers.Count; i++)
                {
                    if (DataManager.Ins.ListUsers[i].email == email)
                    {
                        DataManager.Ins.CurrentUser = DataManager.Ins.ListUsers[i];
                        curentShell.GoToAsync($"//{nameof(HomeView)}");
                    }
                }
            }
        }

        void eyeHandle(object obj)
        {
            IsPassword = !IsPassword;
            if (!IsPassword)
            {
                EyeSource = "eyeIcon.png";
            }
            else
            {
                EyeSource = "eyeOffIcon.png";
            }
        }
        async void registerHandle(object obj)
        {
            //navigation.PushAsync(new RegisterView());
            await curentShell.GoToAsync($"{nameof(RegisterView)}");
        }
    
        async void loginHandleAsync(object obj)
        {
            int i = 0;
            if (Account == null || Account == "" || !DataManager.Ins.UsersServices.checkEmail(Account) || Password == null || Password == "")
            {
                DependencyService.Get<IToast>().ShortToast("Email invalid");
                return;
            }
            for (i = 0; i < DataManager.Ins.ListUsers.Count(); i++)
            {
                if (DataManager.Ins.ListUsers[i].email == Account)
                {
                    if (DataManager.Ins.ListUsers[i].password == DataManager.Ins.UsersServices.Encode(Password))
                    {
                        DataManager.Ins.CurrentUser = DataManager.Ins.ListUsers[i];
                        DependencyService.Get<IToast>().ShortToast("Login successfully");

                        if (RememberAccount)
                        {
                            Preferences.Set("email_key", Account);
                            Preferences.Set("password_key", Password);
                            Preferences.Set("remeber_key", "true");
                            Preferences.Set("remeber_login_key", "true");
                        }
                        else
                        {
                            Preferences.Set("remeber_key", "false");
                        }

                        await curentShell.GoToAsync($"//{nameof(HomeView)}");
                        //navigation.PushAsync(new MainPage());
                        break;
                    }
                    else
                    {
                        DependencyService.Get<IToast>().ShortToast("Password is incorrect");
                        break;
                    }

                }
            }
            if (i == (DataManager.Ins.ListUsers.Count()))
            {
                DependencyService.Get<IToast>().ShortToast("Email is not registered");
            }



        }
        async void forgotHandle(object obj)
        {
            if (Account == null || Account == "" || !DataManager.Ins.UsersServices.checkEmail(Account))
            {
                DependencyService.Get<IToast>().ShortToast("Enter your email to continue");
            }
            else if (!DataManager.Ins.UsersServices.ExistEmail(Account, DataManager.Ins.users))
            {
                DependencyService.Get<IToast>().ShortToast("Email is not registed");
            }
            else
            {
                Random rand = new Random();
                string randomCode = (rand.Next(999999)).ToString();
                DataManager.Ins.VerifyCode = randomCode;
                DataManager.Ins.CurrentUser = new User()
                {
                    email = Account,
                    password = "",
                    name = "",
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
                //navigation.PushAsync(new ResetPassword());
                await curentShell.GoToAsync($"{nameof(ResetPasswordView)}");
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
        private bool rememberAccount;
        public bool RememberAccount
        {
            get { return rememberAccount; }
            set
            {
                rememberAccount = value;
                OnPropertyChanged("RememberAccount");
            }
        }

    }
}
