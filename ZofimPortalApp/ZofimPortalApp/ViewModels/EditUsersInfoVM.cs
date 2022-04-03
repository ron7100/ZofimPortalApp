﻿using System.ComponentModel;
using Xamarin.Forms;
using ZofimPortalApp.Services;
using ZofimPortalApp.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

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
                Hanhaga = dummy.Hanhaga;
                ShowHanhaga = true;
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
            Hanhagas = new List<Hanhaga>();
            Shevets = new List<Shevet>();
            Roles = new List<Role>();
            if (u is WorkerToShow)
            {
                Hanhaga noHanhaga = new Hanhaga();
                noHanhaga.Name = "אין";
                Hanhagas.Add(noHanhaga);
                Shevet noShevet = new Shevet();
                noShevet.Name = "אין";
                Shevets.Add(noShevet);
            }
            SetListsForPickers();
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
                CheckEmail();
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
                CheckFName();
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
                CheckLName();
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
                CheckID();
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
                CheckShevet();
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
                hanhaga = value;
                SetShevetsAccordingToHanhaga();
                OnPropertyChanged("Hanhaga");
            }
        }

        private int hanhagaId;
        private string hanhagaHolder;

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

        private bool shevetError;
        public bool ShevetError
        {
            get => shevetError;
            set
            {
                shevetError = value;
                OnPropertyChanged("ShevetError");
            }
        }

        private string shevetErrorMessage;
        public string ShevetErrorMessage
        {
            get => shevetErrorMessage;
            set
            {
                shevetErrorMessage = value;
                OnPropertyChanged("ShevetErrorMessage");
            }
        }
        #endregion

            #region רשימות פיקרים
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

        private List<Shevet> shevets;
        public List<Shevet> Shevets
        {
            get => shevets;
            set
            {
                shevets = value;
                OnPropertyChanged("Shevets");
            }
        }

        private List<Hanhaga> hanhagas;
        public List<Hanhaga> Hanhagas
        {
            get => hanhagas;
            set
            {
                hanhagas = value;
                OnPropertyChanged("Hanhagas");
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

        private async void SetListsForPickers()
        {
            hanhagaHolder = Hanhaga;
            string shevetHolder = Shevet;
            string roleHolder = Role;
            List<Role> rolesList = await proxy.GetAllRolesAsync();
            List<string> availableRoles = new List<string>();
            foreach (Role r in rolesList)
            {
                availableRoles.Add(r.RoleName);
            }
            Roles = availableRoles;
            Role = roleHolder;
            if (!(EditedUser is User))
            {
                List<Hanhaga> hanhagasList = await proxy.GetAllHanhagasAsync();
                List<string> availableHanhagas = new List<string>();
                foreach (Hanhaga h in hanhagasList)
                {
                    availableHanhagas.Add(h.Name);
                }
                Hanhagas = availableHanhagas;
                Hanhaga hanhagaObject = hanhagasList.Where(h => h.Name == hanhagaHolder).FirstOrDefault();
                hanhagaId = hanhagaObject.Id;
                Hanhaga = hanhagaHolder;
                Shevet = shevetHolder;
                SetShevetsForPicker();
            }
        }

        private async void SetShevetsForPicker()
        {
            hanhagaHolder = Hanhaga;
            string shevetHolder = Shevet;
            List<Hanhaga> hanhagasList = await proxy.GetAllHanhagasAsync();
            List<Shevet> shevetsList = await proxy.GetAllShevetsAsync();
            List<string> availableShevets = new List<string>();
            if (hanhagaHolder != "אין")
            {
                Hanhaga hanhagaObject = hanhagasList.Where(h => h.Name == hanhagaHolder).FirstOrDefault();
                foreach (Shevet s in shevetsList)
                {
                    if (hanhagaObject.Id == s.HanhagaId)
                    {
                        availableShevets.Add(s.Name);
                    }
                }
                hanhagaId = hanhagaObject.Id;
            }
            else
            {
                foreach (Shevet s in shevetsList)
                {
                    availableShevets.Add(s.Name);
                }
                hanhagaId = 16;
            }
            Shevets = availableShevets;
            Hanhaga = hanhagaHolder;
            Shevet = shevetHolder;
        }

        private async void SetShevetsAccordingToHanhaga()
        {
            string shevetHolder = Shevet;
            List<Hanhaga> hanhagasList = await proxy.GetAllHanhagasAsync();
            Hanhaga hanhagaObject = hanhagasList.Where(h => h.Name == hanhagaHolder).FirstOrDefault();
            hanhagaId = hanhagaObject.Id;
            List<Shevet> shevetsList = await proxy.GetAllShevetsAsync();
            List<string> availableShevets = new List<string>();
            if(hanhagaId!=16)
            {
                foreach (Shevet s in shevetsList)
                {
                    if (hanhagaId == s.HanhagaId)
                    {
                        availableShevets.Add(s.Name);
                    }
                }
            }
            else
            {
                foreach(Shevet s in shevetsList)
                {
                    availableShevets.Add(s.Name);
                }
            }
            Shevets = availableShevets;
            Shevet = shevetHolder;
        }

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

        private void CheckShevet()
        {
            ShevetError = false;
            if(Shevet==null)
            {
                ShevetError = true;
                ShevetErrorMessage = "זהו שדה חובה";
            }
        }
        #endregion

        private async void BackToManageUsers()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
