﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using ZofimPortalApp.Services;
using ZofimPortalApp.Models;
using Xamarin.Essentials;
using System.Linq;

namespace ZofimPortalApp.ViewModels
{
    class SignUpVM
    {
        public Command ShowPasswordCommand { get; }
        public Command SignUpCommand { get; }
        public Command ToLogInCommand { get; }

        public event Action OnHidePassword;

        private ZofimPortalAPIProxy proxy;
        public SignUpVM()
        {
            this.ShowPasswordCommand = new Command(ShowPassword);
            this.SignUpCommand = new Command(SignUp);
            this.IsUserError = false;
            this.IsPassError = false;
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

        private int id;
        public int ID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("ID");
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

        public async void ToLogIn()
        {
            Page p = new Views.LogIn();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void SignUp()
        {
            User user = new User();
            Object userToReturn = await proxy.SignUpAsync(user);
            if (userToReturn == null)
                SignUpFailed();
            else
                SignUpSuccess(user);
        }

        private void SignUpFailed()
        {
            Task<bool> isUsernameExist = proxy.IsUserExistAsync(this.UName);
            if (isUsernameExist.Result)
            {
                this.IsUserErrorExist = true;
            }
            else
            {
                if(this.pass.Length<8)
            }
        }

        private void SignUpSuccess(Object u)
        {
            HomePage.connectedUser = u;
            App.Current.MainPage.Navigation.PopAsync();
        }

        private void ShowPassword()
        {
            if (OnHidePassword != null)
                OnHidePassword();
        }


    }
}