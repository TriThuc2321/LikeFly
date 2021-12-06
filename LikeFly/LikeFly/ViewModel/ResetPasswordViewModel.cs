﻿using LikeFly.Core;
using LikeFly.Database;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    class ResetPasswordViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public ResetPasswordViewModel() { }

        public Command EyeCommand { get; }
        public Command EyeConfirmCommand { get; }
        public Command ConfirmCommand { get; }

        public ResetPasswordViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            EyeSource = "eyeOffIcon.png";
            IsPassword = true;
            EyeCommand = new Command(eyeHandle);

            EyeSourceConfirm = "eyeOffIcon.png";
            IsPasswordConfirm = true;
            EyeConfirmCommand = new Command(eyeConfirmHandle);

            ConfirmCommand = new Command(confirmHandle);
        }
        async void confirmHandle(object obj)
        {
            if (VerifyCode == null || Password == null || ConfirmPassword == null || VerifyCode == "" || Password == "" || ConfirmPassword == "")
            {
                DependencyService.Get<IToast>().ShortToast("Please fill out your information");
            }
            else if (Password.Length < 6)
            {
                DependencyService.Get<IToast>().ShortToast("Password must be more than 6 characters");
            }
            else if (Password != ConfirmPassword)
            {
                DependencyService.Get<IToast>().ShortToast("Confirm password is incorrect");
            }
            else if (VerifyCode == DataManager.Ins.VerifyCode)
            {

                for (int i = 0; i < DataManager.Ins.users.Count; i++)
                {
                    if (DataManager.Ins.users[i].email == DataManager.Ins.CurrentUser.email)
                    {
                        DataManager.Ins.users[i].password = DataManager.Ins.UsersServices.Encode(Password);
                        DataManager.Ins.ListUsers[i].password = DataManager.Ins.UsersServices.Encode(Password);
                        await DataManager.Ins.UsersServices.UpdateUser(DataManager.Ins.users[i]);
                        break;
                    }
                }
                DependencyService.Get<IToast>().ShortToast("Reset password successfully");
                //navigation.PushAsync(new HomeView());
                await currentShell.GoToAsync($"//{nameof(LoginView)}");
            }
            else
            {
                DependencyService.Get<IToast>().ShortToast("Verify code is incorrect");
            }
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
        private string verifyCode;
        public string VerifyCode
        {
            get { return verifyCode; }
            set
            {
                verifyCode = value;
                OnPropertyChanged("VerifyCode");
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
    }
}
