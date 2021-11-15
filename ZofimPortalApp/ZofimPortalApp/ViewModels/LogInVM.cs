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
        public LogInVM()
        {
            HidePassword = true;
            this.ShowPasswordCommand = new Command(ShowPassword);
            this.LogInCommand = new Command(LogIn);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

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

        private void ShowPassword()
        {
            if (OnHidePassword != null)
                OnHidePassword();
        }

        
        private void LogIn()
        {
            ZofimPortalAPIProxy proxy = ZofimPortalAPIProxy.CreateProxy();
            Task<Object> user = proxy.LogInAsync(this.uName, this.pass);
            if (user == null)
                LogInFailed();
            else
                LogInSuccess((Object)user);
        }

        private void LogInFailed()
        {

        }

        private void LogInSuccess(Object u)
        {
            HomePage.connectedUser = u;
            App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
