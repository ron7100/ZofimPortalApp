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
    class HomePageVM : INotifyPropertyChanged
    {
        public Command ToLogInCommand { get; }
        public Command ToSignUpCommand { get; }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region properties
        private string welcomeMessage;
        public string WelcomeMessage
        {
            get => welcomeMessage;
            set
            {
                welcomeMessage = value;
                OnPropertyChanged("WelcomeMessage");
            }
        }

        private bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }

        private bool notConnected;
        public bool NotConnected
        {
            get => notConnected;
            set
            {
                notConnected = value;
                OnPropertyChanged("NotConnected");
            }
        }
        #endregion

        public HomePageVM(User u)
        {
            ToLogInCommand = new Command(ToLogIn);
            ToSignUpCommand = new Command(ToSignUp);
            if(u!=null)
            {
                WelcomeMessage = "מחובר כעת: " + u.Username;
                SignedIn();
            }
        }
        public void SignedIn()
        {
            IsConnected = true;
            NotConnected = false;
        }
        public async void ToLogIn()
        {
            Page p = new Views.LogIn();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
        public async void ToSignUp()
        {
            Page p = new Views.SignUp();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
