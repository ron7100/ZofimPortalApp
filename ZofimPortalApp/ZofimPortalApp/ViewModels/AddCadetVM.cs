using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using ZofimPortalApp.Models;
using ZofimPortalApp.Services;
using ZofimPortalApp.Views;

namespace ZofimPortalApp.ViewModels
{
    class AddCadetVM:INotifyPropertyChanged
    {
        public Command AddCadetCommand => new Command(AddCadet);
        public Command BackToEditUsersInfoCommand => new Command(BackToEditUsersInfo);

        private ZofimPortalAPIProxy proxy;

        public AddCadetVM(ParentToShow p)
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            IsFNameError = false;
            IsLNameError = false;
            IsPersonalIDError = false;
            ParentID = p.ID;
            SetRolesListAndShevet(p.Shevet);
        }

        private async void SetRolesListAndShevet(string shevetName)
        {
            List<Role> availableRoles = await proxy.GetAllRolesAsync();
            Roles = availableRoles;
            List<Shevet> shevets = await proxy.GetAllShevetsAsync();
            ShevetID = shevets.Where(s => s.Name == shevetName).FirstOrDefault().Id;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        private int? ShevetID;
        private int ParentID;

            #region Fields
        private string fName;
        public string FName
        {
            get => fName;
            set
            {
                fName = value;
                CheckFName();
                OnPropertyChanged("FName");
            }
        }

        private string lName;
        public string LName
        {
            get => lName;
            set
            {
                lName = value;
                CheckLName();
                OnPropertyChanged("LName");
            }
        }

        private string personalID;
        public string PersonalID
        {
            get => personalID;
            set
            {
                personalID = value;
                CheckID();
                OnPropertyChanged("PersonalID");
            }
        }

        private Role role;
        public Role Role
        {
            get => role;
            set
            {
                role = value;
                OnPropertyChanged("Role");
            }
        }

        private Role pickerRole;
        public Role PickerRole
        {
            get => pickerRole;
            set
            {
                pickerRole = value;
                if (PickerRole != Role)
                    Role = PickerRole;
                OnPropertyChanged("PickerRole");
            }
        }

        private int pickerRoleId;
        public int PickerRoleId
        {
            get => pickerRoleId;
            set
            {
                pickerRoleId = value;
                OnPropertyChanged("PickerRoleId");
            }
        }
        #endregion

            #region Errors
        private bool isFNameError;
        public bool IsFNameError
        {
            get => isFNameError;
            set
            {
                isFNameError = value;
                OnPropertyChanged("IsFNameError");
            }
        }

        private string fNameError;
        public string FNameError
        {
            get => fNameError;
            set
            {
                fNameError = value;
                OnPropertyChanged("FNameError");
            }
        }

        private bool isLNameError;
        public bool IsLNameError
        {
            get => isLNameError;
            set
            {
                isLNameError = value;
                OnPropertyChanged("IsLNameError");
            }
        }

        private string lNameError;
        public string LNameError
        {
            get => lNameError;
            set
            {
                lNameError = value;
                OnPropertyChanged("LNameError");
            }
        }

        private bool isPersonalIDError;
        public bool IsPersonalIDError
        {
            get => isPersonalIDError;
            set
            {
                isPersonalIDError = value;
                OnPropertyChanged("IsPersonalIDError");
            }
        }

        private string personalIDError;
        public string PersonalIDError
        {
            get => personalIDError;
            set
            {
                personalIDError = value;
                OnPropertyChanged("PersonalIDError");
            }
        }

        private bool isRoleError;
        public bool IsRoleError
        {
            get => isRoleError;
            set
            {
                isRoleError = value;
                OnPropertyChanged("IsRoleError");
            }
        }

        private bool isThereError;
        public bool IsThereError
        {
            get => isThereError;
            set
            {
                isThereError = value;
                OnPropertyChanged("IsThereError");
            }
        }
        #endregion

        private List<Role> roles;
        public List<Role> Roles
        {
            get => roles;
            set
            {
                roles = value;
                OnPropertyChanged("Roles");
            }
        }

        #endregion

        #region Check Fields
        private void CheckFName()
        {
            IsFNameError = false;
            if (FName == "")
            {
                IsFNameError = true;
                FNameError = "שדה חובה";
                return;
            }
            foreach (char c in FName)
            {
                if (!char.IsLetter(c))
                {
                    IsFNameError = true;
                    FNameError = "השם חייב לכלול אותיות בלבד";
                }
            }
        }

        private void CheckLName()
        {
            IsLNameError = false;
            if (LName == "")
            {
                IsLNameError = true;
                LNameError = "שדה חובה";
                return;
            }
            foreach (char c in LName)
            {
                if (!char.IsLetter(c))
                {
                    IsLNameError = true;
                    LNameError = "השם משפחה חייב לכלול אותיות בלבד";
                }
            }
        }

        private void CheckID()
        {
            IsPersonalIDError = false;
            if (PersonalID.Length != 9)
            {
                IsPersonalIDError = true;
                PersonalIDError = "תעודת זהות חייבת לכלול 9 ספרות";
                return;
            }
            foreach (char c in PersonalID)
            {
                if (!char.IsDigit(c))
                {
                    IsPersonalIDError = true;
                    PersonalIDError = "תעודת זהות חייבת לכלול ספרות בלבד";
                }
            }
        }

        private void CheckRole()
        {
            IsRoleError = Role == null;
        }
        #endregion

        private async void AddCadet()
        {
            if(IsFNameError || IsLNameError || IsPersonalIDError || IsRoleError)
            {
                IsThereError = true;
                return;
            }
            Cadet c = new Cadet();
            c.FName = FName;
            c.LName = LName;
            c.PersonalId = PersonalID;
            c.RoleId = Role.Id;
            c.ShevetId = (int)ShevetID;
            await proxy.AddCadetAsync(c);
            //await proxy.ConnectCadetParent()
            //לחבר חניך להורה
            ToManageUsers();
        }

        private async void ToManageUsers()
        {
            Page p = new ManageUsers();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void BackToEditUsersInfo()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
