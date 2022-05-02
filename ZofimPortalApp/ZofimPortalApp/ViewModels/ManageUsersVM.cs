using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using ZofimPortalApp.Models;
using ZofimPortalApp.Services;
using ZofimPortalApp.Views;

namespace ZofimPortalApp.ViewModels
{
    class ManageUsersVM : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ICommand BackToHomePageCommand => new Command(BackToHomePage);
        public ICommand EnableSearchCommand => new Command(EnableSearch);
        public ICommand DisableSearchCommand => new Command(DisableSearch);

        private ZofimPortalAPIProxy proxy;
        public ManageUsersVM()
        {
            Selected = null;
            HeaderMessage = "ניהול משתמשים";
            proxy = ZofimPortalAPIProxy.CreateProxy();
            SearchEnabled = false;
            IsSecondFieldEnabled();
            SetLists();
        }

        public async void SetLists()
        {
            List<string> toShow = new List<string>();
            int permissionLevel = await proxy.GetPermissionLevelAsync(HomePage.ConnectedUser.Id);
            switch (permissionLevel)
            {
                case 0:
                    break;
                case 1:
                    toShow.Add("משתמשים");
                    toShow.Add("עובדים");
                    toShow.Add("הורים");
                    toShow.Add("חניכים");
                    break;
                case 2:
                    toShow.Add("משתמשים");
                    toShow.Add("עובדים");
                    toShow.Add("הורים");
                    toShow.Add("חניכים");
                    AvailableHanhaga = await proxy.GetHanhagaAsync(HomePage.ConnectedUser.Id);
                    break;
                case 3:
                    toShow.Add("הורים");
                    toShow.Add("חניכים");
                    AvailableShevet = await proxy.GetShevetAsync(HomePage.ConnectedUser.Id);
                    UpdateSelection();
                    break;
            }
            AvailableToShow = toShow;

            SelectedType = 0;
            SelectedTypeItem = AvailableToShow[0];

            UsersList = await proxy.GetAllUsersAsync();
            WorkersList = await proxy.GetAllWorkersAsync();
            ParentsList = await proxy.GetAllParentsAsync();
            CadetsList = await proxy.GetAllCadetsAsync();
            if(AvailableHanhaga!=null)
            {
                SetRelevantUsersList();
                SetRelevantWorkersList();
                SetRelevantParentsList("Hanhaga");
                SetRelevantCadetsList("Hanhaga");
            }
            if(AvailableShevet!=null)
            {
                SetRelevantParentsList("Shevet");
                SetRelevantCadetsList("Shevet");
            }
        }

        #region Properties
            #region Lists
        private ObservableCollection<User> usersListToShow;
        public ObservableCollection<User> UsersListToShow
        {
            get => usersListToShow;
            set
            {
                usersListToShow = value;
                OnPropertyChanged("UsersListToShow");
            }
        }

        private ObservableCollection<WorkerToShow> workersListToShow;
        public ObservableCollection<WorkerToShow> WorkersListToShow
        {
            get => workersListToShow;
            set
            {
                workersListToShow = value;
                OnPropertyChanged("WorkersListToShow");
            }
        }

        private ObservableCollection<ParentToShow> parentsListToShow;
        public ObservableCollection<ParentToShow> ParentsListToShow
        {
            get => parentsListToShow;
            set
            {
                parentsListToShow = value;
                OnPropertyChanged("ParentsListToShow");
            }
        }

        private ObservableCollection<CadetToShow> cadetsListToShow;
        public ObservableCollection<CadetToShow> CadetsListToShow
        {
            get => cadetsListToShow;
            set
            {
                cadetsListToShow = value;
                OnPropertyChanged("CadetsListToShow");
            }
        }


        private ObservableCollection<User> usersList;
        public ObservableCollection<User> UsersList
        {
            get => usersList;
            set
            {
                usersList = value;
                UsersListToShow = value;
                OnPropertyChanged("UsersList");
            }
        }

        private ObservableCollection<WorkerToShow> workersList;
        public ObservableCollection<WorkerToShow> WorkersList
        {
            get => workersList;
            set
            {
                workersList = value;
                WorkersListToShow = value;
                OnPropertyChanged("WorkersList");
            }
        }

        private ObservableCollection<ParentToShow> parentsList;
        public ObservableCollection<ParentToShow> ParentsList
        {
            get => parentsList;
            set
            {
                parentsList = value;
                ParentsListToShow = value;
                OnPropertyChanged("ParentsList");
            }
        }

        private ObservableCollection<CadetToShow> cadetsList;
        public ObservableCollection<CadetToShow> CadetsList
        {
            get => cadetsList;
            set
            {
                cadetsList = value;
                CadetsListToShow = value;
                OnPropertyChanged("CadetsList");
            }
        }
        #endregion

            #region Is selected
        private bool isUserSelected;
        public bool IsUserSelected
        {
            get => isUserSelected;
            set
            {
                isUserSelected = value;
                OnPropertyChanged("IsUserSelected");
            }
        }

        private bool isWorkerSelected;
        public bool IsWorkerSelected
        {
            get => isWorkerSelected;
            set
            {
                isWorkerSelected = value;
                OnPropertyChanged("IsWorkerSelected");
            }
        }

        private bool isParentSelected;
        public bool IsParentSelected
        {
            get => isParentSelected;
            set
            {
                isParentSelected = value;
                OnPropertyChanged("IsParentSelected");
            }
        }

        private bool isCadetSelected;
        public bool IsCadetSelected
        {
            get => isCadetSelected;
            set
            {
                isCadetSelected = value;
                OnPropertyChanged("IsCadetSelected");
            }
        }

        private int selectedType;
        public int SelectedType
        {
            get => selectedType;
            set
            {
                selectedType = value;
                UpdateSelection();
                OnPropertyChanged("SelectedType");
            }
        }

        private string selectedTypeItem;
        public string SelectedTypeItem
        {
            get => selectedTypeItem;
            set
            {
                selectedTypeItem = value;
                UpdateSelection();
                OnPropertyChanged("SelectedTypeItem");
            }
        }
        #endregion

            #region Availables
        private List<string> availableToShow;
        public List<string> AvailableToShow
        {
            get => availableToShow;
            set
            {
                availableToShow = value;
                OnPropertyChanged("AvailableToShow");
            }
        }

        private string availableHanhaga;
        public string AvailableHanhaga
        {
            get => availableHanhaga;
            set
            {
                availableHanhaga = value;
                OnPropertyChanged("AvailableHanhaga");
            }
        }

        private string availableShevet;
        public string AvailableShevet
        {
            get => availableShevet;
            set
            {
                availableShevet = value;
                OnPropertyChanged("AvailableShevet");
            }
        }
        #endregion

            #region Search
        private string firstField;
        public string FirstField
        {
            get => firstField;
            set
            {
                firstField = value;
                IsSecondFieldEnabled();
                Search();
                //if(!CheckedFirstField)
                //    ChangedFirstFieldSelection();
                OnPropertyChanged("FirstField");
            }
        }

        private int firstFieldIndex;
        public int FirstFieldIndex
        {
            get => firstFieldIndex;
            set
            {
                firstFieldIndex = value;
                OnPropertyChanged("FirstFieldValue");
            }
        }

        private string firstFieldPlaceHolder;
        public string FirstFieldPlaceHolder
        {
            get => firstFieldPlaceHolder;
            set
            {
                firstFieldPlaceHolder = value;
                OnPropertyChanged("FirstFieldPlaceHolder");
            }
        }

        private string firstFieldValue;
        public string FirstFieldValue
        {
            get => firstFieldValue;
            set
            {
                firstFieldValue = value;
                IsSecondFieldEnabled();
                Search();
                OnPropertyChanged("FirstFieldValue");
            }
        }

        private List<string> availableOptionsFirstField;
        public List<string> AvailableOptionsFirstField
        {
            get => availableOptionsFirstField;
            set
            {
                availableOptionsFirstField = value;
                OnPropertyChanged("AvailableOptionsFirstField");
            }
        }
        public bool CheckedFirstField { get; set; }

        private string secondField;
        public string SecondField
        {
            get => secondField;
            set
            {
                secondField = value;
                Search();
                //if(!CheckedSecondField)
                //    ChangedSecondFieldSelection();
                OnPropertyChanged("SecondField");
            }
        }

        private int secondFieldIndex;
        public int SecondFieldIndex
        {
            get => secondFieldIndex;
            set
            {
                secondFieldIndex = value;
                OnPropertyChanged("SecondFieldValue");
            }
        }

        private string secondFieldPlaceHolder;
        public string SecondFieldPlaceHolder
        {
            get => secondFieldPlaceHolder;
            set
            {
                secondFieldPlaceHolder = value;
                OnPropertyChanged("SecondFieldPlaceHolder");
            }
        }

        private string secondFieldValue;
        public string SecondFieldValue
        {
            get => secondFieldValue;
            set
            {
                secondFieldValue = value;
                Search();
                OnPropertyChanged("SecondFieldValue");
            }
        }

        private bool secondFieldEnabled;
        public bool SecondFieldEnabled
        {
            get => secondFieldEnabled;
            set
            {
                if (secondFieldEnabled != value)
                {
                    secondFieldEnabled = value;
                    OnPropertyChanged("SecondFieldEnabled");
                }
            }
        }

        private List<string> availableOptionsSecondField;
        public List<string> AvailableOptionsSecondField
        {
            get => availableOptionsSecondField;
            set
            {
                availableOptionsSecondField = value;
                OnPropertyChanged("AvailableOptionsSecondField");
            }
        }
       
        public bool CheckedSecondField { get; set; }

        private bool searchEnabled;
        public bool SearchEnabled
        {
            get => searchEnabled;
            set
            {
                searchEnabled = value;
                SearchDisabled = !searchEnabled;
                OnPropertyChanged("SearchEnabled");
            }
        }

        private bool searchDisabled;
        public bool SearchDisabled
        {
            get => searchDisabled;
            set
            {
                searchDisabled = value;
                OnPropertyChanged("SearchDisabled");
            }
        }
        #endregion

        private object selected;
        public object Selected
        {
            get => selected;
            set
            {
                selected = value;
                if(selected!=null)
                    GoToEditUsers();
                OnPropertyChanged("Selected");
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

        #region set relevant lists
        public async void SetRelevantUsersList()
        {
            ObservableCollection<User> relevantUsers = new ObservableCollection<User>();
            foreach(User u in UsersList)
            {
                if (await proxy.GetHanhagaAsync(u.Id) == AvailableHanhaga)
                    relevantUsers.Add(u);
            }
            UsersList = relevantUsers;
        }

        public void SetRelevantWorkersList()
        {
            ObservableCollection<WorkerToShow> relevantWorkers = new ObservableCollection<WorkerToShow>();
            foreach (WorkerToShow w in WorkersList)
            {
                if (w.Hanhaga == AvailableHanhaga)
                    relevantWorkers.Add(w);
            }
            WorkersList = relevantWorkers;
        }

        public async void SetRelevantParentsList(string option)
        {
            ObservableCollection<ParentToShow> relevantParents = new ObservableCollection<ParentToShow>();
            foreach(ParentToShow p in ParentsList)
            {
                int Id = UsersList.Where(u => u.PersonalId == p.PersonalID).FirstOrDefault().Id;
                if (option == "Hanhaga")
                {
                    if (await proxy.GetHanhagaAsync(Id) == AvailableHanhaga)
                        relevantParents.Add(p);
                }
                else
                {
                    if (await proxy.GetShevetAsync(Id) == AvailableShevet)
                        relevantParents.Add(p);
                }
            }
            ParentsList = relevantParents;
        }

        public void SetRelevantCadetsList(string option)
        {
            ObservableCollection<CadetToShow> relevantCadets = new ObservableCollection<CadetToShow>();
            foreach (CadetToShow c in CadetsList)
            {
                if (option == "Hanhaga")
                {
                    if (c.Hanhaga == AvailableHanhaga)
                        relevantCadets.Add(c);
                }
                else
                {
                    if (c.Shevet == AvailableShevet)
                        relevantCadets.Add(c);
                }
            }
            CadetsList = relevantCadets;
        }

        #endregion

        private void UpdateSelection()
        {
            IsUserSelected = false;
            IsWorkerSelected = false;
            IsParentSelected = false;
            IsCadetSelected = false;
            if (AvailableShevet == null)
            {
                switch (selectedType)
                {
                    case 0:
                        IsUserSelected = true;
                        break;
                    case 1:
                        IsWorkerSelected = true;
                        break;
                    case 2:
                        IsParentSelected = true;
                        break;
                    case 3:
                        IsCadetSelected = true;
                        break;
                }
            }
            else
            {
                switch (selectedType)
                {
                    case 0:
                        IsParentSelected = true;
                        break;
                    case 1:
                        IsCadetSelected = true;
                        break;
                }
            }
            AvailableOptionsFirstField = GetAvailableFieldsOptions();
            AvailableOptionsSecondField = GetAvailableFieldsOptions();
        }

        private List<string> GetAvailableFieldsOptions()
        {
            List<string> availableFields = new List<string>();
            if (IsUserSelected)
            {
                availableFields.Add("כתובת מייל");
                availableFields.Add("שם פרטי");
                availableFields.Add("שם משפחה");
                availableFields.Add("תעודת זהות");
            }
            else if (IsWorkerSelected)
            {
                availableFields.Add("כתובת מייל");
                availableFields.Add("שם פרטי");
                availableFields.Add("שם משפחה");
                availableFields.Add("תעודת זהות");
                availableFields.Add("תפקיד");
                availableFields.Add("שבט");
                availableFields.Add("הנהגה");
            }
            else if (IsParentSelected)
            {
                availableFields.Add("כתובת מייל");
                availableFields.Add("שם פרטי");
                availableFields.Add("שם משפחה");
                availableFields.Add("תעודת זהות");
                availableFields.Add("שבט");
                availableFields.Add("הנהגה");
            }
            else if (IsCadetSelected)
            {
                availableFields.Add("שם פרטי");
                availableFields.Add("שם משפחה");
                availableFields.Add("תעודת זהות");
                availableFields.Add("שבט");
                availableFields.Add("הנהגה");
                availableFields.Add("תפקיד");
            }
            return availableFields;
        }

        //להוריד/לבצע התאמות עם החיפוש
        private void ChangedFirstFieldSelection()
        {
            
            List<string> availableOptionsHolder = GetAvailableFieldsOptions();
            string chosenOptionHolder = SecondField;
            secondField = null;
            List<string> availableFields = new List<string>();
            foreach (string s in availableOptionsHolder)
            {
                if (s != FirstField)
                    availableFields.Add(s);
            }
            CheckedSecondField = true;
            CheckedFirstField = false;
            AvailableOptionsSecondField = availableFields;
            secondField = chosenOptionHolder;
        }

        private void ChangedSecondFieldSelection()
        {
            List<string> availableOptionsHolder = GetAvailableFieldsOptions();
            string chosenOptionHolder = FirstField;
            firstField = null;
            List<string> availableFields = new List<string>();
            foreach (string s in availableOptionsHolder)
            {
                if (s != SecondField)
                    availableFields.Add(s);
            }
            CheckedFirstField = true;
            CheckedSecondField = false;
            AvailableOptionsFirstField = availableFields;
            firstField = chosenOptionHolder;
        }

        private void IsSecondFieldEnabled()
        {
            SecondFieldEnabled = FirstField != null && FirstFieldValue != null && FirstFieldValue != "" && SearchEnabled;
        }

        private void Search()
        {
            if (SelectedTypeItem == "משתמשים")
            {
                UsersListToShow = UsersList;
                if (FirstFieldValue != null && FirstFieldValue != "")
                {
                    switch (FirstFieldIndex)
                    {
                        case 0:
                            ObservableCollection<User> dummy = new ObservableCollection<User>();
                            foreach (User u in UsersList)
                            {
                                if (u.Email == FirstFieldValue)
                                    dummy.Add(u);
                            }
                            UsersListToShow = dummy;
                            break;
                        case 1:
                            dummy = new ObservableCollection<User>();
                            foreach (User u in UsersList)
                            {
                                if (u.FirstName == FirstFieldValue)
                                    dummy.Add(u);
                            }
                            UsersListToShow = dummy;
                            break;
                        case 2:
                            dummy = new ObservableCollection<User>();
                            foreach (User u in UsersList)
                            {
                                if (u.LastName == FirstFieldValue)
                                    dummy.Add(u);
                            }
                            UsersListToShow = dummy;
                            break;
                        case 3:
                            dummy = new ObservableCollection<User>();
                            foreach (User u in UsersList)
                            {
                                if (u.PersonalId == FirstFieldValue)
                                    dummy.Add(u);
                            }
                            UsersListToShow = dummy;
                            break;
                    }
                    if(SecondFieldValue != null && SecondFieldValue != "")
                    {
                        switch (SecondFieldIndex)
                        {
                            case 0:
                                ObservableCollection<User> dummy = new ObservableCollection<User>();
                                foreach (User u in UsersListToShow)
                                {
                                    if (u.Email == SecondFieldValue)
                                        dummy.Add(u);
                                }
                                UsersListToShow = dummy;
                                break;
                            case 1:
                                dummy = new ObservableCollection<User>();
                                foreach (User u in UsersListToShow)
                                {
                                    if (u.FirstName == SecondFieldValue)
                                        dummy.Add(u);
                                }
                                UsersListToShow = dummy;
                                break;
                            case 2:
                                dummy = new ObservableCollection<User>();
                                foreach (User u in UsersListToShow)
                                {
                                    if (u.LastName == SecondFieldValue)
                                        dummy.Add(u);
                                }
                                UsersListToShow = dummy;
                                break;
                            case 3:
                                dummy = new ObservableCollection<User>();
                                foreach (User u in UsersListToShow)
                                {
                                    if (u.PersonalId == SecondFieldValue)
                                        dummy.Add(u);
                                }
                                UsersListToShow = dummy;
                                break;
                        }
                    }
                }
            }
            if (SelectedTypeItem == "עובדים")
            {
                WorkersListToShow = WorkersList;
                if (FirstFieldValue != null && FirstFieldValue != "")
                {
                    switch (FirstFieldIndex)
                    {
                        case 0:
                            ObservableCollection<WorkerToShow> dummy = new ObservableCollection<WorkerToShow>();
                            foreach (WorkerToShow w in WorkersList)
                            {
                                if (w.Email == FirstFieldValue)
                                    dummy.Add(w);
                            }
                            WorkersListToShow = dummy;
                            break;
                        case 1:
                            dummy = new ObservableCollection<WorkerToShow>();
                            foreach (WorkerToShow w in WorkersList)
                            {
                                if (w.FirstName == FirstFieldValue)
                                    dummy.Add(w);
                            }
                            WorkersListToShow = dummy;
                            break;
                        case 2:
                            dummy = new ObservableCollection<WorkerToShow>();
                            foreach (WorkerToShow w in WorkersList)
                            {
                                if (w.LastName == FirstFieldValue)
                                    dummy.Add(w);
                            }
                            WorkersListToShow = dummy;
                            break;
                        case 3:
                            dummy = new ObservableCollection<WorkerToShow>();
                            foreach (WorkerToShow w in WorkersList)
                            {
                                if (w.PersonalID == FirstFieldValue)
                                    dummy.Add(w);
                            }
                            WorkersListToShow = dummy;
                            break;
                        case 4:
                            dummy = new ObservableCollection<WorkerToShow>();
                            foreach (WorkerToShow w in WorkersList)
                            {
                                if (w.Role == FirstFieldValue)
                                    dummy.Add(w);
                            }
                            WorkersListToShow = dummy;
                            break;
                        case 5:
                            dummy = new ObservableCollection<WorkerToShow>();
                            foreach (WorkerToShow w in WorkersList)
                            {
                                if(w.Shevet!=null)
                                    if (w.Shevet == FirstFieldValue)
                                        dummy.Add(w);
                            }
                            WorkersListToShow = dummy;
                            break;
                        case 6:
                            dummy = new ObservableCollection<WorkerToShow>();
                            foreach (WorkerToShow w in WorkersList)
                            {
                                if (w.Hanhaga != null)
                                    if (w.Hanhaga == FirstFieldValue)
                                        dummy.Add(w);
                            }
                            WorkersListToShow = dummy;
                            break;
                    }
                    if (SecondFieldValue != null && SecondFieldValue != "")
                    {
                        switch (SecondFieldIndex)
                        {
                            case 0:
                                ObservableCollection<WorkerToShow> dummy = new ObservableCollection<WorkerToShow>();
                                foreach (WorkerToShow w in WorkersListToShow)
                                {
                                    if (w.Email == SecondFieldValue)
                                        dummy.Add(w);
                                }
                                WorkersListToShow = dummy;
                                break;
                            case 1:
                                dummy = new ObservableCollection<WorkerToShow>();
                                foreach (WorkerToShow w in WorkersListToShow)
                                {
                                    if (w.FirstName == SecondFieldValue)
                                        dummy.Add(w);
                                }
                                WorkersListToShow = dummy;
                                break;
                            case 2:
                                dummy = new ObservableCollection<WorkerToShow>();
                                foreach (WorkerToShow w in WorkersListToShow)
                                {
                                    if (w.LastName == SecondFieldValue)
                                        dummy.Add(w);
                                }
                                WorkersListToShow = dummy;
                                break;
                            case 3:
                                dummy = new ObservableCollection<WorkerToShow>();
                                foreach (WorkerToShow w in WorkersListToShow)
                                {
                                    if (w.PersonalID == SecondFieldValue)
                                        dummy.Add(w);
                                }
                                WorkersListToShow = dummy;
                                break;
                            case 4:
                                dummy = new ObservableCollection<WorkerToShow>();
                                foreach (WorkerToShow w in WorkersListToShow)
                                {
                                    if (w.Role == SecondFieldValue)
                                        dummy.Add(w);
                                }
                                WorkersListToShow = dummy;
                                break;
                            case 5:
                                dummy = new ObservableCollection<WorkerToShow>();
                                foreach (WorkerToShow w in WorkersListToShow)
                                {
                                    if (w.Shevet != null)
                                        if (w.Shevet == SecondFieldValue)
                                            dummy.Add(w);
                                }
                                WorkersListToShow = dummy;
                                break;
                            case 6:
                                dummy = new ObservableCollection<WorkerToShow>();
                                foreach (WorkerToShow w in WorkersListToShow)
                                {
                                    if (w.Hanhaga != null)
                                        if (w.Hanhaga == SecondFieldValue)
                                            dummy.Add(w);
                                }
                                WorkersListToShow = dummy;
                                break;
                        }
                    }
                }
            }
            if (SelectedTypeItem == "הורים")
            {
                ParentsListToShow = ParentsList;
                if (FirstFieldValue != null && FirstFieldValue != "")
                {
                    switch (FirstFieldIndex)
                    {
                        case 0:
                            ObservableCollection<ParentToShow> dummy = new ObservableCollection<ParentToShow>();
                            foreach (ParentToShow p in ParentsList)
                            {
                                if (p.Email == FirstFieldValue)
                                    dummy.Add(p);
                            }
                            ParentsListToShow = dummy;
                            break;
                        case 1:
                            dummy = new ObservableCollection<ParentToShow>();
                            foreach (ParentToShow p in ParentsList)
                            {
                                if (p.FirstName == FirstFieldValue)
                                    dummy.Add(p);
                            }
                            ParentsListToShow = dummy;
                            break;
                        case 2:
                            dummy = new ObservableCollection<ParentToShow>();
                            foreach (ParentToShow p in ParentsList)
                            {
                                if (p.LastName == FirstFieldValue)
                                    dummy.Add(p);
                            }
                            ParentsListToShow = dummy;
                            break;
                        case 3:
                            dummy = new ObservableCollection<ParentToShow>();
                            foreach (ParentToShow p in ParentsList)
                            {
                                if (p.PersonalID == FirstFieldValue)
                                    dummy.Add(p);
                            }
                            ParentsListToShow = dummy;
                            break;
                        case 4:
                            dummy = new ObservableCollection<ParentToShow>();
                            foreach (ParentToShow p in ParentsList)
                            {
                                if (p.Shevet == FirstFieldValue)
                                    dummy.Add(p);
                            }
                            ParentsListToShow = dummy;
                            break;
                        case 5:
                            dummy = new ObservableCollection<ParentToShow>();
                            foreach (ParentToShow p in ParentsList)
                            {
                                if (p.Hanhaga == FirstFieldValue)
                                    dummy.Add(p);
                            }
                            ParentsListToShow = dummy;
                            break;
                    }
                    if (SecondFieldValue != null && SecondFieldValue != "")
                    {
                        switch (SecondFieldIndex)
                        {
                            case 0:
                                ObservableCollection<ParentToShow> dummy = new ObservableCollection<ParentToShow>();
                                foreach (ParentToShow p in ParentsListToShow)
                                {
                                    if (p.Email == SecondFieldValue)
                                        dummy.Add(p);
                                }
                                ParentsListToShow = dummy;
                                break;
                            case 1:
                                dummy = new ObservableCollection<ParentToShow>();
                                foreach (ParentToShow p in ParentsListToShow)
                                {
                                    if (p.FirstName == SecondFieldValue)
                                        dummy.Add(p);
                                }
                                ParentsListToShow = dummy;
                                break;
                            case 2:
                                dummy = new ObservableCollection<ParentToShow>();
                                foreach (ParentToShow p in ParentsListToShow)
                                {
                                    if (p.LastName == SecondFieldValue)
                                        dummy.Add(p);
                                }
                                ParentsListToShow = dummy;
                                break;
                            case 3:
                                dummy = new ObservableCollection<ParentToShow>();
                                foreach (ParentToShow p in ParentsListToShow)
                                {
                                    if (p.PersonalID == SecondFieldValue)
                                        dummy.Add(p);
                                }
                                ParentsListToShow = dummy;
                                break;
                            case 4:
                                dummy = new ObservableCollection<ParentToShow>();
                                foreach (ParentToShow p in ParentsListToShow)
                                {
                                    if (p.Shevet == SecondFieldValue)
                                        dummy.Add(p);
                                }
                                ParentsListToShow = dummy;
                                break;
                        }
                    }
                }
            }
            if (SelectedTypeItem == "חניכים")
            {
                CadetsListToShow = CadetsList;
                if (FirstFieldValue != null && FirstFieldValue != "")
                {
                    switch (FirstFieldIndex)
                    {
                        case 0:
                            ObservableCollection<CadetToShow> dummy = new ObservableCollection<CadetToShow>();
                            foreach (CadetToShow c in CadetsList)
                            {
                                if (c.FirstName == FirstFieldValue)
                                    dummy.Add(c);
                            }
                            CadetsListToShow = dummy;
                            break;
                        case 1:
                            dummy = new ObservableCollection<CadetToShow>();
                            foreach (CadetToShow c in CadetsList)
                            {
                                if (c.LastName == FirstFieldValue)
                                    dummy.Add(c);
                            }
                            CadetsListToShow = dummy;
                            break;
                        case 2:
                            dummy = new ObservableCollection<CadetToShow>();
                            foreach (CadetToShow c in CadetsList)
                            {
                                if (c.PersonalID == FirstFieldValue)
                                    dummy.Add(c);
                            }
                            CadetsListToShow = dummy;
                            break;
                        case 3:
                            dummy = new ObservableCollection<CadetToShow>();
                            foreach (CadetToShow c in CadetsList)
                            {
                                if (c.Shevet == FirstFieldValue)
                                    dummy.Add(c);
                            }
                            CadetsListToShow = dummy;
                            break;
                        case 4:
                            dummy = new ObservableCollection<CadetToShow>();
                            foreach (CadetToShow c in CadetsList)
                            {
                                if (c.Hanhaga == FirstFieldValue)
                                    dummy.Add(c);
                            }
                            CadetsListToShow = dummy;
                            break;
                        case 5:
                            dummy = new ObservableCollection<CadetToShow>();
                            foreach (CadetToShow c in CadetsList)
                            {
                                if (c.Role == FirstFieldValue)
                                    dummy.Add(c);
                            }
                            CadetsListToShow = dummy;
                            break;
                    }
                    if (SecondFieldValue != null && SecondFieldValue != "")
                    {
                        switch (SecondFieldIndex)
                        {
                            case 0:
                                ObservableCollection<CadetToShow> dummy = new ObservableCollection<CadetToShow>();
                                foreach (CadetToShow c in CadetsListToShow)
                                {
                                    if (c.FirstName == SecondFieldValue)
                                        dummy.Add(c);
                                }
                                CadetsListToShow = dummy;
                                break;
                            case 1:
                                dummy = new ObservableCollection<CadetToShow>();
                                foreach (CadetToShow c in CadetsListToShow)
                                {
                                    if (c.LastName == SecondFieldValue)
                                        dummy.Add(c);
                                }
                                CadetsListToShow = dummy;
                                break;
                            case 2:
                                dummy = new ObservableCollection<CadetToShow>();
                                foreach (CadetToShow c in CadetsListToShow)
                                {
                                    if (c.PersonalID == SecondFieldValue)
                                        dummy.Add(c);
                                }
                                CadetsListToShow = dummy;
                                break;
                            case 3:
                                dummy = new ObservableCollection<CadetToShow>();
                                foreach (CadetToShow c in CadetsList)
                                {
                                    if (c.Shevet == SecondFieldValue)
                                        dummy.Add(c);
                                }
                                CadetsListToShow = dummy;
                                break;
                            case 4:
                                dummy = new ObservableCollection<CadetToShow>();
                                foreach (CadetToShow c in CadetsListToShow)
                                {
                                    if (c.Hanhaga == SecondFieldValue)
                                        dummy.Add(c);
                                }
                                CadetsListToShow = dummy;
                                break;
                            case 5:
                                dummy = new ObservableCollection<CadetToShow>();
                                foreach (CadetToShow c in CadetsListToShow)
                                {
                                    if (c.Role == SecondFieldValue)
                                        dummy.Add(c);
                                }
                                CadetsListToShow = dummy;
                                break;
                        }
                    }
                }
            }
        }

        private void EnableSearch() => SearchEnabled = true;

        private void DisableSearch()
        {
            SearchEnabled = false;
            UsersListToShow = UsersList;
            WorkersListToShow = WorkersList;
            ParentsListToShow = ParentsList;
            CadetsListToShow = CadetsList;
            FirstField = null;
            FirstFieldValue = null;
            SecondField = null;
            SecondFieldValue = null;
        }

        private async void GoToEditUsers()
        {
            Page p = new Views.EditUsersInfo(Selected);
            //Selected = null;
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void BackToHomePage()
        {
            Page p = new Views.HomePage();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}