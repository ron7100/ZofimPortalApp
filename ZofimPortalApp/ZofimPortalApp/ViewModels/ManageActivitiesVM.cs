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
using ZofimPortalApp.Views;
using System.Collections.ObjectModel;

namespace ZofimPortalApp.ViewModels
{
    class ManageActivitiesVM : INotifyPropertyChanged
    {
        public Command BackToHomePageCommand => new Command(BackToHomePage);
        public Command EnableSearchCommand => new Command(EnableSearch);
        public Command DisableSearchCommand => new Command(DisableSearch);
        public Command ToggleOpenOnlyCommand => new Command(ToggleOpenOnly);
        public Command ToAddActivityCommand => new Command(ToAddActivity);

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private ZofimPortalAPIProxy proxy;
        public ManageActivitiesVM()
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            SearchEnabled = false;
            ToggleText = "הצג מפעלים פתוחים בלבד";
            SetProperties();
        }
        public async void SetProperties()
        {
            //fill shevets list
            int permissionLevel = await proxy.GetPermissionLevelAsync(HomePage.ConnectedUser.Id);
            //1 -> admin, can see all
            //2 -> can see only from his hanhaga
            //3 -> can see only from his shevet
            CanSeeAllShevets = permissionLevel == 1;//only an admin can see all shevets
            CanSeeAllShevetsInHanhaga = permissionLevel == 2;
            CantSeeAllShevets = permissionLevel == 3;
            if (CanSeeAllShevets)
            {
                ObservableCollection<ActivityToShow> ActivitiesEzer = new ObservableCollection<ActivityToShow>();
                List<ActivityToShow> allActivities = await proxy.GetAllActivitiesAsync();
                foreach (ActivityToShow a in allActivities)
                    ActivitiesEzer.Add(a);
                Activities = ActivitiesEzer;
                AllActivities = ActivitiesEzer;
                HeaderMessage = "נתוני מפעלים";
            }
            else if(CanSeeAllShevetsInHanhaga)
            { 
                ObservableCollection<ActivityToShow> activitiesEzer = new ObservableCollection<ActivityToShow>();
                string hanhaga = await proxy.GetHanhagaAsync(HomePage.ConnectedUser.Id);
                List<ActivityToShow> activitiesInHanhaga = await proxy.GetActivitiesForHanhagaAsync(hanhaga);
                foreach (ActivityToShow a in activitiesInHanhaga)
                    activitiesEzer.Add(a);
                Activities = activitiesEzer;
                ActivitiesForHanhaga = activitiesEzer;
                HeaderMessage = $"נתוני מפעלים להנהגת {hanhaga}";
            }
            else
            {
                ObservableCollection<ActivityToShow> activitiesEzer = new ObservableCollection<ActivityToShow>();
                string shevet = await proxy.GetShevetAsync(HomePage.ConnectedUser.Id);
                string hanhaga = await proxy.GetHanhagaAsync(HomePage.ConnectedUser.Id);
                List<ActivityToShow> activitiesInShevet = await proxy.GetActivitiesForShevetAsync(shevet, hanhaga);
                foreach (ActivityToShow a in activitiesInShevet)
                    activitiesEzer.Add(a);
                Activities = activitiesEzer;
                ActivitiesForShevet = activitiesEzer;
                HeaderMessage = $"נתוני מפעלים לשבט {shevet}";
            }
            //search
            List<string> searchOptions = new List<string>();
            searchOptions.Add("שם");
            if (CanSeeAllShevets)
            {
                searchOptions.Add("שבט");
                searchOptions.Add("הנהגה");
            }
            else if (CanSeeAllShevetsInHanhaga)
                searchOptions.Add("שבט");
            AvailableOptionsSearchField = searchOptions;
            SearchFieldIndex = 0;
        }

        #region Properties

            #region permissions
        private bool canSeeAllShevets;
        public bool CanSeeAllShevets
        {
            get => canSeeAllShevets;
            set
            {
                canSeeAllShevets = value;
                OnPropertyChanged("CanSeeAllShevets");
            }
        }

        private bool canSeeAllShevetsInHanhaga;
        public bool CanSeeAllShevetsInHanhaga
        {
            get => canSeeAllShevetsInHanhaga;
            set
            {
                canSeeAllShevetsInHanhaga = value;
                OnPropertyChanged("CanSeeAllShevetsInHanhaga");
            }
        }

        private bool cantSeeAllShevets;
        public bool CantSeeAllShevets
        {
            get => cantSeeAllShevets;
            set
            {
                cantSeeAllShevets = value;
                OnPropertyChanged("CantSeeAllShevets");
            }
        }
        #endregion

            #region Lists
        private ObservableCollection<ActivityToShow> allActivities;
        public ObservableCollection<ActivityToShow> AllActivities
        {
            get => allActivities;
            set
            {
                allActivities = value;
                ChangeAmount();
                OnPropertyChanged("AllActivities");
            }
        }

        private ObservableCollection<ActivityToShow> activitiesForHanhaga;
        public ObservableCollection<ActivityToShow> ActivitiesForHanhaga
        {
            get => activitiesForHanhaga;
            set
            {
                activitiesForHanhaga = value;
                ChangeAmount();
                OnPropertyChanged("ActivitiesForHanhaga");
            }
        }

        private ObservableCollection<ActivityToShow> activitiesForShevet;
        public ObservableCollection<ActivityToShow> ActivitiesForShevet
        {
            get => activitiesForShevet;
            set 
            { 
                activitiesForShevet = value;
                ChangeAmount();
                OnPropertyChanged("ActivitiesForShevet");
            }
        }

        private ObservableCollection<ActivityToShow> activities;
        public ObservableCollection<ActivityToShow> Activities
        {
            get => activities;
            set
            {
                activities = value;
                OnPropertyChanged("Activities");
            }
        }
            #endregion

        private ActivityToShow selected;
        public ActivityToShow Selected
        {
            get => selected;
            set
            {
                selected = value;
                if (selected != null)
                    GoToEditActivities();
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

        private string listAmount;
        public string ListAmount
        {
            get => listAmount;
            set
            {
                listAmount = value;
                OnPropertyChanged("ListAmount");
            }
        }

            #region Search
        private bool searchEnabled;
        public bool SearchEnabled
        {
            get => searchEnabled;
            set
            {
                searchEnabled = value;
                SearchDisabled = !value;
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

        private string searchField;
        public string SearchField
        {
            get => searchField;
            set
            {
                searchField = value;
                OnPropertyChanged("SearchField");
            }
        }

        private int searchFieldIndex;
        public int SearchFieldIndex
        {
            get => searchFieldIndex;
            set
            {
                searchFieldIndex = value;
                UpdateSearchField();
                OnPropertyChanged("SearchFieldIndex");
            }
        }

        private string searchValue;
        public string SearchValue
        {
            get => searchValue;
            set
            {
                searchValue = value;
                Search();
                OnPropertyChanged("SearchValue");
            }
        }

        private string searchFieldPlaceHolder;
        public string SearchFieldPlaceHolder
        {
            get => searchFieldPlaceHolder;
            set
            {
                searchFieldPlaceHolder = value;
                OnPropertyChanged("SearchFieldPlaceHolder");
            }
        }

        private List<string> availableOptionsSearchField;
        public List<string> AvailableOptionsSearchField
        {
            get => availableOptionsSearchField;
            set
            {
                availableOptionsSearchField = value;
                OnPropertyChanged("AvailableOptionsSearchField");
            }
        }

        private bool showsOpenOnly;
        public bool ShowsOpenOnly
        {
            get => showsOpenOnly;
            set
            {
                showsOpenOnly = value;
                OnPropertyChanged("ShowsOpenOnly");
            }
        }

        private string toggleText;
        public string ToggleText
        {
            get => toggleText;
            set
            {
                toggleText = value;
                OnPropertyChanged("ToggleText");
            }
        }
        #endregion

        #endregion

        private void ChangeAmount()
        {
            if (CanSeeAllShevets && Activities != null)
                ListAmount = $"מספר מפעלים: {Activities.Count}";
            if (CantSeeAllShevets && ActivitiesForShevet != null)
                ListAmount = $"מספר מפעלים: {ActivitiesForShevet.Count}";
            if (CanSeeAllShevetsInHanhaga && ActivitiesForHanhaga != null)
                ListAmount = $"מספר מפעלים: {ActivitiesForHanhaga.Count}";
        }

        public void UpdateSearchField()
        {
            switch (SearchFieldIndex)
            {
                case 0:
                    SearchFieldPlaceHolder = "שם...";
                    break;
                case 1:
                    SearchFieldPlaceHolder = "שבט...";
                    break;
                case 2:
                    SearchFieldPlaceHolder = "הנהגה...";
                    break;
            }
        }

        public void Search()
        {
            if (CanSeeAllShevets)
                Activities = AllActivities;
            else if (CanSeeAllShevetsInHanhaga)
                Activities = ActivitiesForHanhaga;
            else
                Activities = ActivitiesForShevet;
            if (SearchValue != "")
            {
                switch (SearchFieldIndex)
                {
                    case 0:
                        ObservableCollection<ActivityToShow> dummy = new ObservableCollection<ActivityToShow>();
                        foreach (ActivityToShow a in Activities)
                        {
                            string partialString = a.Name.Substring(0, SearchValue.Length);
                            if (partialString == SearchValue)
                                dummy.Add(a);
                        }
                        Activities = dummy;
                        break;
                    case 1:
                        dummy = new ObservableCollection<ActivityToShow>();
                        foreach (ActivityToShow a in Activities)
                        {
                            string partialString = a.Shevet.Substring(0, SearchValue.Length);
                            if (partialString == SearchValue)
                                dummy.Add(a);
                        }
                        Activities = dummy;
                        break;
                    case 2:
                        dummy = new ObservableCollection<ActivityToShow>();
                        foreach (ActivityToShow a in Activities)
                        {
                            string partialString = a.Hanhaga.Substring(0, SearchValue.Length);
                            if (partialString == SearchValue)
                                dummy.Add(a);
                        }
                        Activities = dummy;
                        break;
                }
            }
        }

        private void EnableSearch() => SearchEnabled = true;

        private void DisableSearch()
        {
            SearchEnabled = false;
            SearchField = null;
            SearchValue = null;
            SetProperties();
        }

        private void ToggleOpenOnly()
        {
            if(!ShowsOpenOnly)
            {
                ObservableCollection<ActivityToShow> dummy = new ObservableCollection<ActivityToShow>();
                foreach (ActivityToShow a in Activities)
                {
                    if (a.IsOpen == "Green")
                        dummy.Add(a);
                }
                Activities = dummy;
                ShowsOpenOnly = !ShowsOpenOnly;
                ToggleText = "הצג את כל המפעלים";
            }
            else
            {
                ShowsOpenOnly = !ShowsOpenOnly;
                ToggleText = "הצג מפעלים פתוחים בלבד";
                SetProperties();
            }
        }

        private async void GoToEditActivities()
        {
            Page p = new Views.EditActivities(Selected);
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void BackToHomePage()
        {
            Page p = new Views.HomePage();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void ToAddActivity()
        {
            Page p = new Views.AddActivity();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
