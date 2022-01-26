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
using ZofimPortalApp.Views;

namespace ZofimPortalApp.ViewModels
{
    class SignUpVM : INotifyPropertyChanged
    {
        public Command TogglePasswordCommand { get; }
        public Command ToggleCheckPasswordCommand { get; }
        public Command SignUpCommand { get; }
        public Command ToLogInCommand { get; }
        public Command BackToHomePageCommand { get; }


        private ZofimPortalAPIProxy proxy;
        public SignUpVM()
        {
            TogglePasswordCommand = new Command(TogglePassword);
            ToggleCheckPasswordCommand = new Command(ToggleCheckPassword);
            SignUpCommand = new Command(SignUp);
            ToLogInCommand = new Command(ToLogIn);
            BackToHomePageCommand = new Command(BackToHomePage);
            IsPassword = true;
            IsCheckPassword = true;
            IsUserError = false;
            IsPassError = false;
            IsCheckPassError = false;
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

        private string checkPass;
        public string CheckPass
        {
            get => checkPass;
            set
            {
                checkPass = value;
                OnPropertyChanged("CheckPass");
            }
        }

        private int userType;
        public int UserType
        {
            get => userType;
            set
            {
                userType = value;
                OnPropertyChanged("UserType");
            }
        }

        private bool isPassword;
        public bool IsPassword
        {
            get => isPassword;
            set
            {
                isPassword = value;
                OnPropertyChanged("IsPassword");
            }
        }

        private bool isCheckPassword;
        public bool IsCheckPassword
        {
            get => isCheckPassword;
            set
            {
                isCheckPassword = value;
                OnPropertyChanged("IsCheckPassword");
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

        private bool isCheckPassError;
        public bool IsCheckPassError
        {
            get => isCheckPassError;
            set
            {
                isCheckPassError = value;
                OnPropertyChanged("IsCheckPassError");
            }
        }
        #endregion

        public async void ToLogIn()
        {
            Page p = new Views.LogIn();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void SignUp()
        {//create user object here and send the data as a user to the proxy
            User user = new User();
            Object userToReturn = await proxy.SignUpAsync(user);
            if (userToReturn == null)
                SignUpFailed();
            else
                SignUpSuccess(user);
        }

        private void SignUpFailed()
        {//username already exists
            
        }

        private void SignUpSuccess(User u)
        {
            HomePage.connectedUser = u;
            App.Current.MainPage.Navigation.PopAsync();
        }

        private void TogglePassword()
        {
            IsPassword=!IsPassword;
        }

        private void ToggleCheckPassword()
        {
            IsCheckPassword = !IsCheckPassword;
        }

        private async void BackToHomePage()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
