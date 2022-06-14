using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using ZofimPortalApp.Services;
using ZofimPortalApp.Models;
using ZofimPortalApp.Views;
using Xamarin.Essentials;
using System.Linq;
using System.Collections.ObjectModel;

namespace ZofimPortalApp.ViewModels
{
    class ManageShevetVM : INotifyPropertyChanged
    {
        public Command BackToHomePageCommand => new Command(BackToHomePage);
        public ICommand EnableSearchCommand => new Command(EnableSearch);
        public ICommand DisableSearchCommand => new Command(DisableSearch);

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private ZofimPortalAPIProxy proxy;
        public ManageShevetVM()
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            SearchEnabled = false;
            SetProperties();
        }
        public async void SetProperties()
        {   
            //fill shevets list
            int permissionLevel = await proxy.GetPermissionLevelAsync(HomePage.ConnectedUser.Id);
            //1 -> admin, can see all
            //2 -> can see only from his hanhaga
            CanSeeAllShevets = permissionLevel == 1;//only an admin can see all shevets
            if (canSeeAllShevets)
            {
                ObservableCollection<ShevetToShow> shevetsEzer = new ObservableCollection<ShevetToShow>();
                List<ShevetToShow> allShevets = await proxy.GetAllShevetsToShowAsync();
                foreach (ShevetToShow s in allShevets)
                    shevetsEzer.Add(s);
                Shevets = shevetsEzer;
                AllShevets = shevetsEzer;
                HeaderMessage = "נתוני שבטים";
            }
            else
            {
                ObservableCollection<ShevetToShow> shevetsEzer = new ObservableCollection<ShevetToShow>();
                string hanhaga = await proxy.GetHanhagaAsync(HomePage.ConnectedUser.Id);
                List<ShevetToShow> shevetsInHanhaga = await proxy.GetShevetsForHanhagaAsync(hanhaga);
                foreach (ShevetToShow s in shevetsInHanhaga)
                    shevetsEzer.Add(s);
                Shevets = shevetsEzer;
                ShevetsForHanhaga = shevetsEzer;
                HeaderMessage = $"נתוני שבטים להנהגת {hanhaga}";
            }
            //search
            List<string> searchOptions = new List<string>();
            searchOptions.Add("שם");
            if (CanSeeAllShevets)
                searchOptions.Add("הנהגה");
            AvailableOptionsSearchField = searchOptions;
            SearchFieldIndex = 0;
        }

        #region Properties
        private bool canSeeAllShevets;
        public bool CanSeeAllShevets
        {
            get => canSeeAllShevets;
            set
            {
                canSeeAllShevets = value;
                cantSeeAllShevets = !value;
                OnPropertyChanged("CanSeeAllShevets");
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

        private ObservableCollection<ShevetToShow> allShevets;
        public ObservableCollection<ShevetToShow> AllShevets
        {
            get => allShevets;
            set
            {
                allShevets = value;
                OnPropertyChanged("AllShevets");
            }
        }

        private ObservableCollection<ShevetToShow> shevetsForHanhaga;
        public ObservableCollection<ShevetToShow> ShevetsForHanhaga
        {
            get => shevetsForHanhaga;
            set
            {
                shevetsForHanhaga = value;
                OnPropertyChanged("ShevetsForHanhaga");
            }
        }
        
        private ObservableCollection<ShevetToShow> shevets;
        public ObservableCollection<ShevetToShow> Shevets
        {
            get => shevets;
            set
            {
                shevets = value;
                OnPropertyChanged("Shevets");
            }
        }

        private ShevetToShow selected;
        public ShevetToShow Selected
        {
            get => selected;
            set
            {
                selected = value;
                if (selected != null)
                    GoToEditShevetInfo();
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
        #endregion

        #endregion

        public void UpdateSearchField()
        {
            switch (SearchFieldIndex)
            {
                case 0:
                    SearchFieldPlaceHolder = "שם...";
                    break;
                case 1:
                    SearchFieldPlaceHolder = "הנהגה...";
                    break;
            }
        }

        public void Search()
        {
            if (CanSeeAllShevets)
                Shevets = AllShevets;
            else
                Shevets = ShevetsForHanhaga;
            if (SearchValue != "")
            {
                switch (SearchFieldIndex)
                {
                    case 0:
                        ObservableCollection<ShevetToShow> dummy = new ObservableCollection<ShevetToShow>();
                        foreach (ShevetToShow s in Shevets)
                        {
                            if (s.Name == SearchValue)
                                dummy.Add(s);
                        }
                        Shevets = dummy;
                        break;
                    case 1:
                        dummy = new ObservableCollection<ShevetToShow>();
                        foreach (ShevetToShow s in Shevets)
                        {
                            if (s.Hanhaga == SearchValue)
                                dummy.Add(s);
                        }
                        Shevets = dummy;
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

        private async void GoToEditShevetInfo()
        {
            Page p = new Views.EditShevetInfo(Selected);
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void BackToHomePage()
        {
            Page p = new Views.HomePage();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
