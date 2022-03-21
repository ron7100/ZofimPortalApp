using System.ComponentModel;
using Xamarin.Forms;
using ZofimPortalApp.Services;
using ZofimPortalApp.Models;
using System.Collections.Generic;

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

        private async void BackToManageUsers()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
