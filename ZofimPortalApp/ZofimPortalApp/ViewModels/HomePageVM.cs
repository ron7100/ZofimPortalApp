using System.ComponentModel;
using Xamarin.Forms;
using ZofimPortalApp.Models;
using ZofimPortalApp.Services;
using ZofimPortalApp.Views;

namespace ZofimPortalApp.ViewModels
{
    class HomePageVM : INotifyPropertyChanged
    {
        public Command ToLogInCommand => new Command(ToLogIn);
        public Command ToSignUpCommand => new Command(ToSignUp);
        public Command ToManageUsersCommand => new Command(ToManageUsers);
        public Command ToPersonalInfoCommand => new Command(ToPersonalInfo);
        public Command SignOutCommand => new Command(SignOut);

        private ZofimPortalAPIProxy proxy;

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
            proxy = ZofimPortalAPIProxy.CreateProxy();
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
        public async void SignedIn()
        {
            IsConnected = true;
            NotConnected = false;
            IsAdmin = true;
            int permissionLevel = await proxy.GetPermissionLevelAsync(HomePage.ConnectedUser.Id);
            if (permissionLevel == 0)
                IsAdmin = false;
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
        public async void ToPersonalInfo()
        {
            Page p = new Views.PersonalInfo();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
