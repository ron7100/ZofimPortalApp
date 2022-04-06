using System.ComponentModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using ZofimPortalApp.Models;
using ZofimPortalApp.Views;

namespace ZofimPortalApp.ViewModels
{
    class PersonalInfoVM : INotifyPropertyChanged
    {
        public Command BackToHomePageCommand => new Command(BackToHomePage);
        public Command SaveChangesCommand => new Command(SaveChanges);

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public PersonalInfoVM()
        {
            User u = HomePage.ConnectedUser;
            Email = u.Email;
            FirstName = u.FirstName;
            LastName = u.LastName;
            PersonalId = u.PersonalId;
        }
        #region Properties
            #region fields
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

        private string firstName;
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                CheckFirstName();
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
                CheckLastName();
                OnPropertyChanged("LastName");
            }
        }

        private string personalId;
        public string PersonalId
        {
            get => personalId;
            set
            {
                personalId = value;
                CheckPersonalId();
                OnPropertyChanged("PersonalId");
            }
        }
        #endregion

            #region errors
        private bool emailError;
        public bool EmailError
        {
            get => emailError;
            set
            {
                emailError = value;
                OnPropertyChanged("EmailError");
            }
        }

        private string emailErrorMessage;
        public string EmailErrorMessage
        {
            get => emailErrorMessage;
            set
            {
                emailErrorMessage = value;
                OnPropertyChanged("EmailErrorMessage");
            }
        }

        private bool firstNameError;
        public bool FirstNameError
        {
            get => firstNameError;
            set
            {
                firstNameError = value;
                OnPropertyChanged("FirstNameError");
            }
        }

        private string firstNameErrorMessage;
        public string FirstNameErrorMessage
        {
            get => firstNameErrorMessage;
            set
            {
                firstNameErrorMessage = value;
                OnPropertyChanged("FirstNameErrorMessage");
            }
        }

        private bool lastNameError;
        public bool LastNameError
        {
            get => lastNameError;
            set
            {
                lastNameError = value;
                OnPropertyChanged("LastNameError");
            }
        }

        private string lastNameErrorMessage;
        public string LastNameErrorMessage
        {
            get => lastNameErrorMessage;
            set
            {
                lastNameErrorMessage = value;
                OnPropertyChanged("LastNameErrorMessage");
            }
        }

        private bool personalIdError;
        public bool PersonalIdError
        {
            get => personalIdError;
            set
            {
                personalIdError = value;
                OnPropertyChanged("PersonalIdError");
            }
        }

        private string personalIdErrorMessage;
        public string PersonalIdErrorMessage
        {
            get => personalIdErrorMessage;
            set
            {
                personalIdErrorMessage = value;
                OnPropertyChanged("PersonalIdErrorMessage");
            }
        }

        private bool areThereErrors;
        public bool AreThereErrors
        {
            get => areThereErrors;
            set
            {
                areThereErrors = value;
                OnPropertyChanged("AreThereErrors");
            }
        }
        #endregion
        #endregion

        #region check fields
        private void CheckEmail()
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            EmailError = !match.Success;
            if (EmailError)
                EmailErrorMessage = "כתובת מייל לא חוקית";
        }

        private void CheckFirstName()
        {
           FirstNameError = false;
            if (FirstName == "")
            {
                FirstNameError = true;
                FirstNameErrorMessage = "שדה חובה";
                return;
            }
            foreach (char c in FirstName)
            {
                if (!char.IsLetter(c))
                {
                    FirstNameError = true;
                    FirstNameErrorMessage = "השם חייב לכלול אותיות בלבד";
                }
            }
        }

        private void CheckLastName()
        {
            LastNameError = false;
            if (LastName == "")
            {
                LastNameError = true;
                LastNameErrorMessage = "שדה חובה";
                return;
            }
            foreach (char c in LastName)
            {
                if (!char.IsLetter(c))
                {
                    LastNameError = true;
                    LastNameErrorMessage = "השם משפחה חייב לכלול אותיות בלבד";
                }
            }
        }

        private void CheckPersonalId()
        {
            PersonalIdError = false;
            if (PersonalId.Length != 9)
            {
                PersonalIdError = true;
                PersonalIdErrorMessage = "תעודת זהות חייבת לכלול 9 ספרות";
                return;
            }
            foreach (char c in PersonalId)
            {
                if (!char.IsDigit(c))
                {
                    PersonalIdError = true;
                    PersonalIdErrorMessage = "תעודת זהות חייבת לכלול ספרות בלבד";
                }
            }
        }
        #endregion

        private void SaveChanges()
        {
            CheckForErrors();
            if (AreThereErrors)
                return;
            //לשמור שינויים
        }

        private void CheckForErrors()
        {
            AreThereErrors = EmailError || FirstNameError || LastNameError || PersonalIdError;
        }

        private async void BackToHomePage() => await App.Current.MainPage.Navigation.PopAsync();
    }
}
