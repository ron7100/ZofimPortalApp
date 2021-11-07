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
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        
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

        public ICommand ShowPasswordCommand => new Command(ShowPassword);
        public void ShowPassword()
        {
            
        }

        public ICommand LogInCommand => new Command(LogIn);
        public void LogIn()
        {

        }
    }
}
