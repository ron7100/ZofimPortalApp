﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using ZofimPortalApp.Services;
using ZofimPortalApp.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using ZofimPortalApp.Views;

namespace ZofimPortalApp.ViewModels
{
    class LogInVM : INotifyPropertyChanged
    {
        public Command LogInCommand { get; }
        public Command ToSignUpCommand { get; }
        public Command BackToHomePageCommand { get; }

        private ZofimPortalAPIProxy proxy;
        public LogInVM()
        {
            LogInCommand = new Command(LogIn);
            ToSignUpCommand = new Command(ToSignUp);
            BackToHomePageCommand = new Command(BackToHomePage);
            IsUserError = false;
            IsPassError = false;
            proxy = ZofimPortalAPIProxy.CreateProxy();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
            
        #region Properties
        private string uName;
        public string UName
        {
            get => uName;
            set
            {
                uName = value;
                OnPropertyChanged("UName");
            }
        }

        private string pass;
        public string Pass
        {
            get => pass;
            set
            {
                pass = value;
                OnPropertyChanged("Pass");
            }
        }

        private bool isUserError;
        public bool IsUserError
        {
            get => isUserError;
            set
            {
                isUserError = value;
                OnPropertyChanged("IsUserError");
            }
        }

        private bool isPassError;
        public bool IsPassError
        {
            get => isPassError;
            set
            {
                isPassError = value;
                OnPropertyChanged("IsPassError");
            }
        }

        #endregion

        public async void ToSignUp()
        {
            Page p = new Views.SignUp();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void LogIn()
        {
            User user = await proxy.LogInAsync(uName, pass);
            if (user == null)
                LogInFailed();
            else
                LogInSuccess(user);
        }

        private async void LogInFailed()
        {
            IsUserError = false;
            IsPassError = false;
            bool isUsernameExist = await proxy.IsUserExistAsync(UName);
            if(isUsernameExist)
            {
                IsPassError = true;
            }
            else
            {
                IsUserError = true;
            }
        }

        private async void LogInSuccess(User u)
        {
            HomePage.connectedUser = u;
            Page p = new Views.HomePage();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void BackToHomePage()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
