using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using ZofimPortalApp.Services;
using ZofimPortalApp.Models;
using Xamarin.Essentials;
using System.Linq;
using System.Collections.ObjectModel;
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
            HeaderMessage = "ניהול משתמשים";
            proxy = ZofimPortalAPIProxy.CreateProxy();
            SearchEnabled = false;
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
            //צריך להוסיף חיפוש בעמוד הזה לפי שלוש עמודות (להוסיף פיקר עם העמודות ולהוריד את מה שכבר נבחר), לחפש ברשימה עם foreach
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
                Search();
                OnPropertyChanged("FirstField");
            }
        }

        private string firstFieldPlaceHolder;
        public string FirstFieldPlaceHolder
        {
            get => firstFieldPlaceHolder;
            set
            {
                firstFieldPlaceHolder = value;
                Search();
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

        private string secondField;
        public string SecondField
        {
            get => secondField;
            set
            {
                secondField = value;
                OnPropertyChanged("SecondField");
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
                OnPropertyChanged("SecondFieldValue");
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
            if(IsUserSelected)
            {
                List<string> availableFields = new List<string>();
                //availableFields.Add()
                //AvailableOptionsFirstField
            }
        }

        private void Search()
        {
            //להוסיף כאן את העריכה של הרשימות to show
            //בודקים לפי המשתנה הראשון, אחרי זה לפי השני ממה שנשאר
        }

        private void EnableSearch() => SearchEnabled = true;

        private void DisableSearch() => SearchEnabled = false;

        private async void GoToEditUsers()
        {
            Page p = new Views.EditUsersInfo(Selected);
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void BackToHomePage()
        {
            Page p = new Views.HomePage();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
