using System;
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
    class LogInVM
    {
        public Command ShowPasswordCommand { get; }

        public LogInVM()
        {
            hidePassword = true;
            ShowPasswordCommand = new Command(ShowPassword);
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
            get { return hidePassword; }
            set
            {
                hidePassword = value;
                OnPropertyChanged("HidePassword");
            }
        }

        private string uName;
        public string UName
        {
            get { return uName; }
            set
            {
                uName = value;
                OnPropertyChanged("UName");
            }
        }

        private string pass;
        public string Pass
        {
            get { return pass; }
            set
            {
                pass = value;
                OnPropertyChanged("Pass");
            }
        }

        public void ShowPassword()
        {
            HidePassword = !HidePassword;
        }

        public ICommand LogInCommand => new Command(LogIn);
        public void LogIn()
        {

        }
    }
}
