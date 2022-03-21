using System.ComponentModel;
using Xamarin.Forms;
using ZofimPortalApp.Services;
using ZofimPortalApp.Models;

namespace ZofimPortalApp.ViewModels
{
    class EditUsersInfoVM : INotifyPropertyChanged
    {
        public Command BackToManageUsersCommand => new Command(BackToManageUsers);

        private ZofimPortalAPIProxy proxy;
        public EditUsersInfoVM(object u)
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            EditedUser = u;
            if (u is User)
                HeaderMessage = "עריכת משתמש";
            if (u is WorkerToShow)
                HeaderMessage = "עריכת עובד";
            if (u is ParentToShow)
                HeaderMessage = "עריכת הורה";
            if (u is CadetToShow)
                HeaderMessage = "עריכת חניך";
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
        #endregion

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
