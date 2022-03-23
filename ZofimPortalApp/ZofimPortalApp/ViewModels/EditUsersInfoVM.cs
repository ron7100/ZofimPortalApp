using System.ComponentModel;
using Xamarin.Forms;
using ZofimPortalApp.Services;
using ZofimPortalApp.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ZofimPortalApp.ViewModels
{
    class EditUsersInfoVM : INotifyPropertyChanged
    {
        public Command BackToManageUsersCommand => new Command(BackToManageUsers);

        private ZofimPortalAPIProxy proxy;
        public EditUsersInfoVM(object u)
        {
            ShowEmail = false;
            ShowRole = false;
            ShowShevet = false;
            ShowHanhaga = false;
            proxy = ZofimPortalAPIProxy.CreateProxy();
            EditedUser = u;
            if (u is User)
            {
                User dummy = (User)u;
                HeaderMessage = "עריכת משתמש";
                Email = dummy.Email;
                ShowEmail = true;
                FirstName = dummy.FirstName;
                LastName = dummy.LastName;
                PersonalID = dummy.PersonalId;
            }
            if (u is WorkerToShow)
            {
                WorkerToShow dummy = (WorkerToShow)u;
                HeaderMessage = "עריכת עובד";
                Email = dummy.Email;
                ShowEmail = true;
                FirstName = dummy.FirstName;
                LastName = dummy.LastName;
                PersonalID = dummy.PersonalID;
                Role = dummy.Role;
                ShowRole = true;
                Shevet = dummy.Shevet;
                ShowShevet = true;
                Hanhaga = dummy.Hanhaga;
                ShowHanhaga = true;
            }
            if (u is ParentToShow)
            {
                ParentToShow dummy = (ParentToShow)u;
                HeaderMessage = "עריכת הורה";
                Email = dummy.Email;
                ShowEmail = true;
                FirstName = dummy.FirstName;
                LastName = dummy.LastName;
                PersonalID = dummy.PersonalID;
                Shevet = dummy.Shevet;
                ShowShevet = true;
            }
            if (u is CadetToShow)
            {
                CadetToShow dummy = (CadetToShow)u;
                HeaderMessage = "עריכת חניך";
                FirstName = dummy.FirstName;
                LastName = dummy.LastName;
                PersonalID = dummy.PersonalID;
                Role = dummy.Role;
                ShowRole = true;
                Shevet = dummy.Shevet;
                ShowShevet = true;
                Hanhaga = dummy.Hanhaga;
                ShowHanhaga = true;
            }
            EmailError = false;
            FirstNameError = false;
            LastNameError = false;
            PersonalIDError = false;
            
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
            #region שדות
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

        private bool showEmail;
        public bool ShowEmail
        {
            get => showEmail;
            set
            {
                showEmail = value;
                OnPropertyChanged("ShowEmail");
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

        private string personalID;
        public string PersonalID
        {
            get => personalID;
            set
            {
                personalID = value;
                OnPropertyChanged("PersonalID");
            }
        }

        private string role;
        public string Role
        {
            get => role;
            set
            {
                role = value;
                OnPropertyChanged("Role");
            }
        }

        private bool showRole;
        public bool ShowRole
        {
            get => showRole;
            set
            {
                showRole = value;
                OnPropertyChanged("ShowRole");
            }
        }

        private string shevet;
        public string Shevet
        {
            get => shevet;
            set
            {
                shevet = value;
                OnPropertyChanged("Shevet");
            }
        }

        private bool showShevet;
        public bool ShowShevet
        {
            get => showShevet;
            set
            {
                showShevet = value;
                OnPropertyChanged("ShowShevet");
            }
        }

        private string hanhaga;
        public string Hanhaga
        {
            get => hanhaga;
            set
            {
                shevet = value;
                OnPropertyChanged("Hanhaga");
            }
        }

        private bool showHanhaga;
        public bool ShowHanhaga
        {
            get => showHanhaga;
            set
            {
                showHanhaga = value;
                OnPropertyChanged("ShowHanhaga");
            }
        }
        #endregion

            #region שגיאות
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

        private bool personalIDError;
        public bool PersonalIDError
        {
            get => personalIDError;
            set
            {
                personalIDError = value;
                OnPropertyChanged("PersonalIDError");
            }
        }

        private string personalIDErrorMessage;
        public string PersonalIDErrorMessage
        {
            get => personalIDErrorMessage;
            set
            {
                personalIDErrorMessage = value;
                OnPropertyChanged("PersonalIDErrorMessage");
            }
        }
        #endregion
        private List<string> shevets;
        public List<string> Shevets
        {
            get => shevets;
            set
            {
                shevets = value;
                OnPropertyChanged("Shevets");
            }
        }

        private List<string> hanhagas;
        public List<string> Hanhagas
        {
            get => hanhagas;
            set
            {
                hanhagas = value;
                OnPropertyChanged("Hanhagas");
            }
        }

        private object editedUser;
        public object EditedUser
        {
            get => editedUser;
            set
            {
                editedUser = value;
                OnPropertyChanged("EditedUser");
            }
        }

        private string headerMessage;
        public string HeaderMessage
        {
            get => headerMessage;
            set
            {
                headerMessage = value;
                OnPropertyChanged("HeaderMessage");
            }
        }
        #endregion

        #region בדיקת שדות
        private void CheckEmail()
        {

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            EmailError = !match.Success;
            if (EmailError)
                EmailErrorMessage = "כתובת מייל לא חוקית";
        }

        private void CheckFName()
        {
            FirstNameError = false;
            if (FirstName == "")
            {
                FirstNameError = true;
                FirstNameError = "שדה חובה";
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

        private void CheckLName()
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

        private void CheckID()
        {
            PersonalIDError = false;
            if (PersonalID.Length != 9)
            {
                PersonalIDError = true;
                PersonalIDErrorMessage = "תעודת זהות חייבת לכלול 9 ספרות";
                return;
            }
            foreach (char c in PersonalID)
            {
                if (!char.IsDigit(c))
                {
                    PersonalIDError = true;
                    PersonalIDErrorMessage = "תעודת זהות חייבת לכלול ספרות בלבד";
                }
            }
        }
        #endregion

        private async void BackToManageUsers()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
