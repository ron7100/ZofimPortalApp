using System.ComponentModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using ZofimPortalApp.Models;
using ZofimPortalApp.Services;
using ZofimPortalApp.Views;

namespace ZofimPortalApp.ViewModels
{
    class PersonalInfoVM : INotifyPropertyChanged
    {
        public Command BackToHomePageCommand => new Command(BackToHomePage);

        private ZofimPortalAPIProxy proxy;
        public PersonalInfoVM()
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            User = HomePage.ConnectedUser;
            Email = User.Email;
            FirstName = User.FirstName;
            LastName = User.LastName;
            PersonalId = User.PersonalId;
            FullName = $"{FirstName} {LastName}";
            WelcomeMessage = $"שלום, {FullName}";

        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        private User user;
        public User User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        private string firstName;
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        private string lastName;
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        private string fullName;
        public string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }

        private string personalId;
        public string PersonalId
        {
            get => personalId;
            set
            {
                personalId = value;
                OnPropertyChanged("PersonalId");
            }
        }

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

        private string emailMessage;
        public string EmailMessage
        {
            get => emailMessage;
            set
            {
                emailMessage = value;
                OnPropertyChanged("EmailMessage");
            }
        }

        private string personalIdMessage;
        public string PersonalIdMessage
        {
            get => personalIdMessage;
            set
            {
                personalIdMessage = value;
                OnPropertyChanged("PersonalIdMessage");
            }
        }
        #endregion

        private async void BackToHomePage()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
