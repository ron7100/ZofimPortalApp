using System.ComponentModel;
using Xamarin.Forms;
using ZofimPortalApp.Services;
using ZofimPortalApp.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;

namespace ZofimPortalApp.ViewModels
{
    class EditUsersInfoVM : INotifyPropertyChanged
    {
        public Command BackToManageUsersCommand => new Command(BackToManageUsers);

        private ZofimPortalAPIProxy proxy;
        public EditUsersInfoVM(object u)
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            RealConstructorBecauseYouCantPutAsyncInConstructor(u);
        }
        
        public async void RealConstructorBecauseYouCantPutAsyncInConstructor(object u)
        {
            List<Role> rolesList = await proxy.GetAllRolesAsync();
            List<Shevet> shevetsList = await proxy.GetAllShevetsAsync();
            List<Hanhaga> hanhagasList = await proxy.GetAllHanhagasAsync();
            EditedUser = u;
            ShowEmail = false;
            ShowRole = false;
            ShowShevet = false;
            ShowHanhaga = false;
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
                Role = rolesList.Where(r => r.RoleName == dummy.Role).FirstOrDefault();
                ShowRole = true;
                Shevet = shevetsList.Where(s => s.Name == dummy.Shevet).FirstOrDefault();
                ShowShevet = true;
                Hanhaga = hanhagasList.Where(h => h.Name == dummy.Hanhaga).FirstOrDefault();
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
                Shevet = shevetsList.Where(s => s.Name == dummy.Shevet).FirstOrDefault();
                ShowShevet = true;
                Hanhaga = hanhagasList.Where(h => h.Name == dummy.Hanhaga).FirstOrDefault();
                ShowHanhaga = true;
            }
            if (u is CadetToShow)
            {
                CadetToShow dummy = (CadetToShow)u;
                HeaderMessage = "עריכת חניך";
                FirstName = dummy.FirstName;
                LastName = dummy.LastName;
                PersonalID = dummy.PersonalID;
                Role = rolesList.Where(r => r.RoleName == dummy.Role).FirstOrDefault();
                ShowRole = true;
                Shevet = shevetsList.Where(s => s.Name == dummy.Shevet).FirstOrDefault();
                ShowShevet = true;
                Hanhaga = hanhagasList.Where(h => h.Name == dummy.Hanhaga).FirstOrDefault();
                ShowHanhaga = true;
            }
            EmailError = false;
            FirstNameError = false;
            LastNameError = false;
            PersonalIDError = false;
            await SetListsForPickers();
            if (u is WorkerToShow)
            {
                Hanhaga noHanhaga = new Hanhaga();
                noHanhaga.Name = "אין";
                noHanhaga.Id = 16;
                Hanhagas.Add(noHanhaga);
                Shevet noShevet = new Shevet();
                noShevet.Name = "אין";
                noShevet.Id = 232;
                Shevets.Add(noShevet);
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

        private Shevet shevet;
        public Shevet Shevet
        {
            get => shevet;
            set
            {
                shevet = value;
                CheckShevet();
                OnPropertyChanged("Shevet");
            }
        }

        private Shevet pickerShevet;
        public Shevet PickerShevet
        {
            get => pickerShevet;
            set
            {
                pickerShevet = value;
                if (PickerShevet != Shevet)
                    Shevet = PickerShevet;
                OnPropertyChanged("PickerShevet");
            }
        }

        private int pickerShevetId;
        public int PickerShevetId
        {
            get => pickerShevetId;
            set
            {
                pickerShevetId = value;
                OnPropertyChanged("PickerShevetId");
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

        private Hanhaga hanhaga;
        public Hanhaga Hanhaga
        {
            get => hanhaga;
            set
            {
                hanhaga = value;
                //SetShevetsAccordingToHanhaga();
                OnPropertyChanged("Hanhaga");
            }
        }

        private Hanhaga pickerHanhaga;
        public Hanhaga PickerHanhaga
        {
            get => pickerHanhaga;
            set
            {
                pickerHanhaga = value;
                if (PickerHanhaga != Hanhaga)
                    Hanhaga = PickerHanhaga;
                OnPropertyChanged("PickerHanhaga");
            }
        }

        private int pickerHanhagaId;
        public int PickerHanhagaId
        {
            get => pickerHanhagaId;
            set
            {
                pickerHanhagaId = value;
                OnPropertyChanged("PickerHanhagaId");
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

        private async Task SetListsForPickers()
        {
            List<Role> availableRoles = await proxy.GetAllRolesAsync();
            Roles = availableRoles;
            PickerRole = Role;
            if (!(EditedUser is User))
            {
                List<Hanhaga> availableHanhagas = await proxy.GetAllHanhagasAsync();
                Hanhagas = availableHanhagas;
                PickerHanhaga = Hanhaga;
                PickerShevet = Shevet;
                await SetShevetsForPicker();
            }
        }

        private async Task SetShevetsForPicker()
        {
            List<Shevet> shevetsList = await proxy.GetAllShevetsAsync();
            List<Shevet> availableShevets = new List<Shevet>();
            if (Hanhaga.Name != "אין")
            {
                foreach (Shevet s in shevetsList)
                {
                    if (Hanhaga.Id == s.HanhagaId)
                    {
                        availableShevets.Add(s);
                    }
                }
            }
            else
            {
                foreach (Shevet s in shevetsList)
                {
                    availableShevets.Add(s);
                }
            }
            Shevets = availableShevets;
            PickerHanhaga = Hanhaga;
            PickerShevet = Shevet;
        }

        private async void SetShevetsAccordingToHanhaga()
        {
            List<Shevet> shevetsList = await proxy.GetAllShevetsAsync();
            List<Shevet> availableShevets = new List<Shevet>();
            int shevetIdInPicker = 0;
            if(Hanhaga.Id!=16)
            {
                foreach (Shevet s in shevetsList)
                {
                    if (Hanhaga.Id == s.HanhagaId)
                    {
                        availableShevets.Add(s);
                        shevetIdInPicker++;
                    }
                    if (s == Shevet)
                        PickerShevetId = shevetIdInPicker - 1;
                }
            }
            else
            {
                foreach(Shevet s in shevetsList)
                {
                    availableShevets.Add(s);
                }
            }
            Shevets = availableShevets;
            PickerShevet = Shevet;
            PickerHanhaga = Hanhaga;
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
