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

namespace ZofimPortalApp.ViewModels
{
    class ManageHanhagaVM : INotifyPropertyChanged
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
        public ManageHanhagaVM()
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            SearchEnabled = false;
            SetProperties();
        }
        public async void SetProperties()
        {
            ObservableCollection<Hanhaga> hanhagasEzer = new ObservableCollection<Hanhaga>();
            List<Hanhaga> everyHanhagas = await proxy.GetAllHanhagasAsync();
            foreach (Hanhaga h in everyHanhagas)
                hanhagasEzer.Add(h);
            Hanhagas = hanhagasEzer;
            AllHanhagas = hanhagasEzer;
            HeaderMessage = "נתוני הנהגות";
            //search
            List<string> searchOptions = new List<string>();
            searchOptions.Add("שם");
            searchOptions.Add("אזור");
            AvailableOptionsSearchField = searchOptions;
            SearchFieldIndex = 0;
        }

        #region Properties
        private ObservableCollection<Hanhaga> hanhagas;
        public ObservableCollection<Hanhaga> Hanhagas
        {
            get => hanhagas;
            set
            {
                hanhagas = value;
                OnPropertyChanged("Hanhagas");
            }
        }

        private ObservableCollection<Hanhaga> allHanhagas;
        public ObservableCollection<Hanhaga> AllHanhagas
        {
            get => allHanhagas;
            set
            {
                allHanhagas = value;
                OnPropertyChanged("AllHanhagas");
            }
        }

        private Hanhaga selected;
        public Hanhaga Selected
        {
            get => selected;
            set
            {
                selected = value;
                if (selected != null)
                    GoToEditHanhagaInfo();
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
                    SearchFieldPlaceHolder = "אזור...";
                    break;
            }
        }

        public void Search()
        {
            Hanhagas = AllHanhagas;
            if (SearchValue != "")
            {
                switch (SearchFieldIndex)
                {
                    case 0:
                        ObservableCollection<Hanhaga> dummy = new ObservableCollection<Hanhaga>();
                        foreach (Hanhaga h in Hanhagas)
                        {
                            if (h.Name == SearchValue)
                                dummy.Add(h);
                        }
                        Hanhagas = dummy;
                        break;
                    case 1:
                        dummy = new ObservableCollection<Hanhaga>();
                        foreach (Hanhaga h in Hanhagas)
                        {
                            if (h.GeneralArea == SearchValue)
                                dummy.Add(h);
                        }
                        Hanhagas = dummy;
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

        private async void GoToEditHanhagaInfo()
        {
            Page p = new Views.EditHanhagaInfo(Selected);
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void BackToHomePage()
        {
            Page p = new Views.HomePage();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
