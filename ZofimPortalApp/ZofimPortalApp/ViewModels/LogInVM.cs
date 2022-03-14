using System.ComponentModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using ZofimPortalApp.Models;
using ZofimPortalApp.Services;
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
            proxy = ZofimPortalAPIProxy.CreateProxy();
            #region למחוק לפני הגשה
            email = "ccc@gmail.com";
            pass = "ccc";
            LogIn();
            #endregion
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
            
        #region Properties
        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                CheckEmail();
                OnPropertyChanged("Email");
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

        private bool isEmailError;
        public bool IsEmailError
        {
            get => isEmailError;
            set
            {
                isEmailError = value;
                OnPropertyChanged("IsEmailError");
            }
        }

        private string emailError;
        public string EmailError
        {
            get => emailError;
            set
            {
                emailError = value;
                OnPropertyChanged("EmailError");
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

        private void CheckEmail()
        {

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            IsEmailError = !match.Success;
            if (IsEmailError)
                EmailError = "כתובת מייל לא חוקית";
        }

        public async void ToSignUp()
        {
            Page p = new Views.SignUp();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void LogIn()
        {
            User user = await proxy.LogInAsync(email, pass);
            if (user == null)
                LogInFailed();
            else
                LogInSuccess(user);
        }

        private async void LogInFailed()
        {
            IsEmailError = false;
            IsPassError = false;
            bool isUserExist = await proxy.IsUserExistAsync(Email);
            if(isUserExist)
            {
                IsPassError = true;
            }
            else
            {
                IsEmailError = true;
                EmailError = "המייל לא קיים במערכת";
            }
        }

        private async void LogInSuccess(User u)
        {
            HomePage.ConnectedUser = u;
            Page p = new Views.HomePage();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void BackToHomePage()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
