using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using ZofimPortalApp.Services;
using ZofimPortalApp.Models;
using ZofimPortalApp.Views;
using Xamarin.Essentials;
using System.Linq;

namespace ZofimPortalApp.ViewModels
{
    class HomePageVM : INotifyPropertyChanged
    {
        public Command ToLogInCommand { get; }
        public Command ToSignUpCommand { get; }
        public Command ToManageUsersCommand { get; }
        public Command SignOutCommand { get; }

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

        private bool isAdmin;
        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }
        #endregion

        public HomePageVM(User u)
        {
            ToLogInCommand = new Command(ToLogIn);
            ToSignUpCommand = new Command(ToSignUp);
            SignOutCommand = new Command(SignOut);
            ToManageUsersCommand = new Command(ToManageUsers);
            if (u != null)
            {
                WelcomeMessage = "מחובר כעת: " + u.FirstName + " " + u.LastName;
                SignedIn();
            }
            else
            {
                NotConnected = true;
                IsConnected = false;
                IsAdmin = false;
            }
        }
        public void SignedIn()
        {
            IsConnected = true;
            NotConnected = false;
            Worker worker = HomePage.ConnectedUser.Workers.Find(w => w.Role == "admin");
            IsAdmin = worker != null;
        }

        public async void SignOut()
        {
            HomePage.ConnectedUser = null;
            Page p = new Views.HomePage();
            await App.Current.MainPage.Navigation.PushAsync(p);
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
        public async void ToManageUsers()
        {
            Page p = new Views.ManageUsers();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
