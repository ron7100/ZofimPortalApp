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
        public Command ToManageUsersCommand => new Command(ToManageUsers);
        public Command ToAddCadetCommand => new Command(ToAddCadet);
        public Command ToCadetsForActivityCommand => new Command(ToCadetsForActivity);
        public Command SaveChangesCommand => new Command(SaveChanges);

        private ZofimPortalAPIProxy proxy;
        public EditUsersInfoVM(object u)
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            SetProperties(u);
        }

        public const int NO_SHEVET_ID = 1000;
        public const int NO_HANHAGA_ID = 1000;
        public async void SetProperties(object u)
        {
            List<Role> rolesList = await proxy.GetAllRolesAsync();
            List<Shevet> shevetsList = await proxy.GetAllShevetsAsync();
            List<Hanhaga> hanhagasList = await proxy.GetAllHanhagasAsync();
            EditedUser = u;
            ShowClass = false;
            ShowEmail = false;
            ShowRole = false;
            ShowShevet = false;
            ShowHanhaga = false;
            ShowCadetsForActivity = false;
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
                hanhaga = hanhagasList.Where(h => h.Name == dummy.Hanhaga).FirstOrDefault();
                ShowHanhaga = true;
                shevet = shevetsList.Where(s => s.Name == dummy.Shevet && s.HanhagaId == hanhaga.Id).FirstOrDefault();
                ShowShevet = true;
                if(hanhaga!=null)
                    Hanhaga = hanhaga;
                if(shevet!=null)
                    Shevet = shevet;
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
                hanhaga = hanhagasList.Where(h => h.Name == dummy.Hanhaga).FirstOrDefault();
                ShowHanhaga = true;
                shevet = shevetsList.Where(s => s.Name == dummy.Shevet && s.HanhagaId == hanhaga.Id).FirstOrDefault();
                ShowShevet = true;
                ShowCadets = true;
                Hanhaga = hanhaga;
                Shevet = shevet;
            }
            if (u is CadetToShow)
            {
                CadetToShow dummy = (CadetToShow)u;
                HeaderMessage = "עריכת חניך";
                FirstName = dummy.FirstName;
                LastName = dummy.LastName;                
                PersonalID = dummy.PersonalID;
                Class = dummy.Class;
                ShowClass = true;
                Role = rolesList.Where(r => r.RoleName == dummy.Role).FirstOrDefault();
                ShowRole = true;
                hanhaga = hanhagasList.Where(h => h.Name == dummy.Hanhaga).FirstOrDefault();
                ShowHanhaga = true;
                shevet = shevetsList.Where(s => s.Name == dummy.Shevet && s.HanhagaId == Hanhaga.Id).FirstOrDefault();
                ShowShevet = true;
                ShowCadetsForActivity = true;
                Hanhaga = hanhaga;
                Shevet = shevet;
            }
            EmailError = false;
            FirstNameError = false;
            LastNameError = false;
            PersonalIDError = false;
            await SetListsForPickers();
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

        private bool showClass;
        public bool ShowClass
        {
            get => showClass;
            set
            {
                showClass = value;
                OnPropertyChanged("ShowClass");
            }
        }

        private string @class;
        public string Class
        {
            get => @class;
            set
            {
                @class = value;
                OnPropertyChanged("Class");
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
                if (!showShevet)
                    ShevetError = showShevet;
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
                SetShevetsAccordingToHanhaga();
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

        private bool showCadets;
        public bool ShowCadets
        {
            get => showCadets;
            set
            {
                showCadets = value;
                OnPropertyChanged("ShowCadets");
            }
        }

        private bool showCadetsforActivity;
        public bool ShowCadetsForActivity
        {
            get => showCadetsforActivity;
            set
            {
                showCadetsforActivity = value;
                OnPropertyChanged("ShowCadetsForActivity");
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
                if (!showShevet)
                    shevetError = showShevet;
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

            #region רשימות פיקרים
        private List<string> classes;
        public List<string> Classes
        {
            get => classes;
            set
            {
                classes = value;
                OnPropertyChanged("Classes");
            }
        }

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
            #region classes
            List<string> classesEzer = new List<string>();
            classesEzer.Add("ד");
            classesEzer.Add("ה");
            classesEzer.Add("ו");
            classesEzer.Add("ז");
            classesEzer.Add("ח");
            classesEzer.Add("ט");
            classesEzer.Add("י");
            classesEzer.Add("יא");
            classesEzer.Add("יב");
            string classHolder = "";
            if (Class != null)
                classHolder = Class;
            Classes = classesEzer;
            if (classHolder != "")
                Class = classHolder;
            #endregion
            List<Role> availableRoles = await proxy.GetAllRolesAsync();
            Roles = availableRoles;
            PickerRole = Role;
            int roleIdInPicker = 0;
            if (Role != null)
            {
                foreach (Role r in availableRoles)
                {
                    if (r.Id != Role.Id)
                        roleIdInPicker++;
                    else
                        PickerRoleId = roleIdInPicker;
                }
            }
            if (!(EditedUser is User))
            {
                Hanhaga hanhagaHolder = Hanhaga;
                List<Hanhaga> availableHanhagas = await proxy.GetAllHanhagasAsync();
                if (EditedUser is WorkerToShow)
                {
                    Hanhaga noHanhaga = new Hanhaga
                    {
                        Name = "אין",
                        Id = NO_HANHAGA_ID
                    };
                    availableHanhagas.Add(noHanhaga);
                    if(hanhagaHolder==null)
                        hanhagaHolder = noHanhaga;
                }
                Hanhagas = availableHanhagas;
                Hanhaga = hanhagaHolder;
                int hanhagaIdInPicker = 0;
                if (Hanhaga != null)
                {
                    foreach (Hanhaga h in availableHanhagas)
                    {
                        if (h.Id != Hanhaga.Id)
                            hanhagaIdInPicker++;
                        else
                            PickerHanhagaId = hanhagaIdInPicker;
                    }
                }
                if (hanhagaIdInPicker == 16)
                    PickerHanhagaId = hanhagaIdInPicker;
                PickerHanhaga = Hanhaga;
                PickerShevet = Shevet;
                await SetShevetsForPicker();
            }
        }

        private async Task SetShevetsForPicker()
        {
            Shevet shevetHolder = Shevet;
            List<Shevet> shevetsList = await proxy.GetAllShevetsAsync();
            List<Shevet> availableShevets = new List<Shevet>();
            int shevetIdInPicker = 0;
            int correctShevetIdInPicker = 231;
            if (Hanhaga != null)
            {
                if (Shevet == null)
                {
                    Shevet noShevet = new Shevet
                    {
                        Name = "אין",
                        Id = NO_SHEVET_ID,
                        HanhagaId = Hanhaga.Id
                    };
                    shevetsList.Add(noShevet);
                    shevetHolder = noShevet;
                    foreach (Shevet s in shevetsList)
                    {
                        if (Hanhaga.Id == s.HanhagaId)
                        {
                            availableShevets.Add(s);
                            shevetIdInPicker++;
                        }
                    }
                    correctShevetIdInPicker = shevetIdInPicker;
                }
                else
                {
                    foreach (Shevet s in shevetsList)
                    {
                        if (Hanhaga.Id == s.HanhagaId)
                        {
                            availableShevets.Add(s);
                            shevetIdInPicker++;
                        }
                        if (s.Id == shevetHolder.Id)
                            correctShevetIdInPicker = shevetIdInPicker - 1;
                    }
                }
            }
            if(Hanhaga.Id == NO_HANHAGA_ID)
            {
                ShowShevet = false;
            }
            Shevets = availableShevets;
            Shevet = shevetHolder;
            PickerShevetId = correctShevetIdInPicker;
            PickerHanhaga = Hanhaga;
            PickerShevet = Shevet;
        }

        private async void SetShevetsAccordingToHanhaga()
        {
            ShowShevet = true;
            Shevet shevetHolder = Shevet;
            List<Shevet> shevetsList = await proxy.GetAllShevetsAsync();
            List<Shevet> availableShevets = new List<Shevet>();
            if(Hanhaga!=null)
            {
                foreach (Shevet s in shevetsList)
                {
                    if (Hanhaga.Id == s.HanhagaId)
                    {
                        availableShevets.Add(s);
                    }
                }
                if (Hanhaga.Id == NO_HANHAGA_ID)
                {
                    ShowShevet = false;
                }
            }
            if(EditedUser is WorkerToShow)
            {
                Shevet noShevet = new Shevet();
                noShevet.Name = "אין";
                noShevet.Id = NO_SHEVET_ID;
                if(Hanhaga!=null)
                    noShevet.HanhagaId = Hanhaga.Id;
                availableShevets.Add(noShevet);                
            }
            Shevets = availableShevets;
            PickerShevetId = -1;
            string hanhagaName = "";
            if (!(EditedUser is User))
            {
                if (EditedUser is WorkerToShow)
                {
                    hanhagaName = ((WorkerToShow)EditedUser).Hanhaga;
                }
                else if (EditedUser is ParentToShow)
                {
                    hanhagaName = ((ParentToShow)EditedUser).Hanhaga;
                }
                else if (EditedUser is CadetToShow)
                {
                    hanhagaName = ((CadetToShow)EditedUser).Hanhaga;
                }
                if (Hanhaga != null)
                {
                    if (hanhagaName == Hanhaga.Name)
                    {
                        Shevet = shevetHolder;
                        PickerShevet = Shevet;
                    }
                }
            }
        }

        #region בדיקת שדות
        private void CheckEmail()
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            EmailError = !match.Success;
            if (EmailError)
                EmailErrorMessage = "כתובת מייל לא חוקית";
            if (Email == null || Email == "")
            {
                EmailError = true;
                EmailErrorMessage = "זהו שדה חובה";
            }
        }

        private void CheckFirstName()
        {
            FirstNameError = false;
            if (FirstName == "")
            {
                FirstNameError = true;
                FirstNameErrorMessage = "זהו שדה חובה";
                return;
            }
            int location = 0;
            string firstNameHolder = FirstName;
            foreach (char c in FirstName)
            {
                if (!char.IsLetter(c))
                    firstNameHolder = firstNameHolder.Remove(location, 1);
                location++;
            }
            if (firstNameHolder != FirstName)
                FirstName = firstNameHolder;
            if (FirstName == "")
            {
                FirstNameError = true;
                FirstNameErrorMessage = "זהו שדה חובה";
            }
        }

        private void CheckLastName()
        {
            LastNameError = false;
            if (LastName == "")
            {
                LastNameError = true;
                LastNameErrorMessage = "זהו שדה חובה";
                return;
            }
            int location = 0;
            string lastNameHolder = LastName;
            foreach (char c in LastName)
            {
                if (!char.IsLetter(c))
                    lastNameHolder = lastNameHolder.Remove(location, 1);
                location++;
            }
            if (lastNameHolder != LastName)
                LastName = lastNameHolder;
            if (LastName == "")
            {
                LastNameError = true;
                LastNameErrorMessage = "זהו שדה חובה";
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
            int location = 0;
            string personalIDHolder = PersonalID;
            foreach (char c in personalIDHolder)
            {
                if (char.IsLetter(c))
                    personalIDHolder = personalIDHolder.Remove(location, 1);
                location++;
            }
            if (personalIDHolder != PersonalID)
                PersonalID = personalIDHolder;
            if (PersonalID == null || PersonalID == "")
            {
                PersonalIDError = true;
                PersonalIDErrorMessage = "זהו שדה חובה";
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

        private async void SaveChanges()
        {
            CheckForErrors();
            if (AreThereErrors)
                return;
            if (EditedUser is User)
            {
                User u = (User)EditedUser;
                u.Email = Email;
                u.FirstName = FirstName;
                u.LastName = LastName;
                u.PersonalId = PersonalID;
                await proxy.SaveUserChangesAsync(u);
            }
            if (EditedUser is WorkerToShow)
            {
                WorkerToShow w = (WorkerToShow)EditedUser;
                w.Email = Email;
                w.FirstName = FirstName;
                w.LastName = LastName;
                w.PersonalID = PersonalID;
                w.Role = Role.RoleName;
                if(Shevet!=null)
                    w.Shevet = Shevet.Name;
                w.Hanhaga = Hanhaga.Name;
                await proxy.SaveWorkerChangesAsync(w);
            }
            if (EditedUser is ParentToShow)
            {
                ParentToShow p = (ParentToShow)EditedUser;
                p.Email = Email;
                p.FirstName = FirstName;
                p.LastName = LastName;
                p.PersonalID = PersonalID;
                p.Shevet = Shevet.Name;
                p.Hanhaga = Hanhaga.Name;
                await proxy.SaveParentChangesAsync(p);
            }
            if (EditedUser is CadetToShow)
            {
                CadetToShow c = (CadetToShow)EditedUser;
                c.FirstName = FirstName;
                c.LastName = LastName;
                c.Class = Class;
                c.PersonalID = PersonalID;
                c.Role = Role.RoleName;
                c.Shevet = Shevet.Name;
                c.Hanhaga = Hanhaga.Name;
                await proxy.SaveCadetChangesAsync(c);
            }
            ToManageUsers();
        }

        private void CheckForErrors()
        {
            AreThereErrors = EmailError || FirstNameError || LastNameError || PersonalIDError || ShevetError;
        }

        private async void BackToManageUsers()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
        
        private async void ToManageUsers()
        {
            Page p = new Views.ManageUsers();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void ToAddCadet()
        {
            Page p = new Views.AddCadet((ParentToShow)EditedUser);
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void ToCadetsForActivity()
        {
            Page p = new Views.CadetsForActivity((CadetToShow)EditedUser);
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
