using System;
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
    class LogInVM
    {
        public Command ShowPasswordCommand { get; }
        public Command LogInCommand { get; }

        public event Action OnHidePassword;

        private ZofimPortalAPIProxy proxy;
        public LogInVM()
        {
            HidePassword = true;
            this.ShowPasswordCommand = new Command(ShowPassword);
            this.LogInCommand = new Command(LogIn);
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
        private bool hidePassword;
        public bool HidePassword
        {
            get => hidePassword;
            set
            {
                hidePassword = value;
                OnPropertyChanged("HidePassword");
            }
        }

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

        private void ShowPassword()
        {
            if (OnHidePassword != null)
                OnHidePassword();
        }

        private void LogIn()
        {
            Task<object> user = proxy.LogInAsync(this.uName, this.pass);
            if (user.Result == null)
                LogInFailed();
            else
                LogInSuccess((object)user);
        }

        private void LogInFailed()
        {
            Task<bool> isUsernameExist = proxy.IsUserExistAsync(this.UName);
            if(isUsernameExist.Result)
            {
                this.IsPassError = true;
            }
            else
            {
                this.IsUserError = true;
            }
        }

        private void LogInSuccess(Object u)
        {
            HomePage.connectedUser = u;
            App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
