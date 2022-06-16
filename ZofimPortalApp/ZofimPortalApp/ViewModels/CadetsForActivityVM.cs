using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using ZofimPortalApp.Models;
using ZofimPortalApp.Services;

namespace ZofimPortalApp.ViewModels
{
    class CadetsForActivityVM : INotifyPropertyChanged
    {
        public Command EnableSearchCommand => new Command(EnableSearch);
        public Command DisableSearchCommand => new Command(DisableSearch);
        public Command BackCommand => new Command(Back);

        ZofimPortalAPIProxy proxy;
        public CadetsForActivityVM(ActivityToShow a)
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            Activity = a;
            HeaderMessage = $"חניכים שרשומים למפעל {a.Name}";
            SearchEnabled = false;
            ShowCadets = true;
            UpdateSearchFieldCadets();
            SetCadetsForActivity();
        }

        public async void SetCadetsForActivity()
        {
            List<string> searchFields = new List<string>();
            searchFields.Add("שם פרטי");
            searchFields.Add("שם משפחה");
            searchFields.Add("כיתה");
            searchFields.Add("תעודת זהות");
            searchFields.Add("תפקיד");
            AvailableOptionsSearchFieldCadets = searchFields;
            List<CadetToShow> cadetsToShow = await proxy.GetCadetsForActivityAsync(Activity.ID);
            ObservableCollection<CadetToShow> dummy = new ObservableCollection<CadetToShow>();
            foreach (CadetToShow c in cadetsToShow)
                dummy.Add(c);
            CadetsForActivity = dummy;
            AllCadetsForActivity = CadetsForActivity;
        }

        public CadetsForActivityVM(CadetToShow c)
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            Cadet = c;
            HeaderMessage = $"מפעלים עבור חניך {c.FirstName} {c.LastName}";
            SearchEnabled = false;
            ShowActivities = true;
            UpdateSearchFieldActivities();
            SetActivitiesForCadet();
        }

        public async void SetActivitiesForCadet()
        {
            List<string> searchFields = new List<string>();
            searchFields.Add("שם");
            AvailableOptionsSearchFieldActivities = searchFields;
            List<ActivityToShow> activitiesToShow = await proxy.GetActivitiesForCadetAsync(Cadet.ID);
            ObservableCollection<ActivityToShow> dummy = new ObservableCollection<ActivityToShow>();
            foreach (ActivityToShow a in activitiesToShow)
                dummy.Add(a);
            ActivitiesForCadet = dummy;
            AllActivitiesForCadet = ActivitiesForCadet;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region properties
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

            #region cadets for activity
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

        private ObservableCollection<CadetToShow> allCadetsForActivity;
        public ObservableCollection<CadetToShow> AllCadetsForActivity
        {
            get => allCadetsForActivity;
            set
            {
                allCadetsForActivity = value;
                OnPropertyChanged("AllCadetsForActivity");
            }
        }

        private ObservableCollection<CadetToShow> cadetsForActivity;
        public ObservableCollection<CadetToShow> CadetsForActivity
        {
            get => cadetsForActivity;
            set
            {
                cadetsForActivity = value;
                OnPropertyChanged("CadetsForActivity");
            }
        }

        private ActivityToShow activity;
        public ActivityToShow Activity
        {
            get => activity;
            set
            {
                activity = value;
                OnPropertyChanged("Activity"); 
            }
        }
        #endregion

            #region activities for cadet
        private bool showActivities;
        public bool ShowActivities
        {
            get => showActivities;
            set
            {
                showActivities = value;
                OnPropertyChanged("ShowActivities");
            }
        }

        private ObservableCollection<ActivityToShow> allActivitiesForCadet;
        public ObservableCollection<ActivityToShow> AllActivitiesForCadet
        {
            get => allActivitiesForCadet;
            set
            {
                allActivitiesForCadet = value;
                OnPropertyChanged("AllActivitiesForCadet");
            }
        }

        private ObservableCollection<ActivityToShow> activitiesForCadet;
        public ObservableCollection<ActivityToShow> ActivitiesForCadet
        {
            get => activitiesForCadet;
            set
            {
                activitiesForCadet = value;
                OnPropertyChanged("ActivitiesForCadet");
            }
        }

        private CadetToShow cadet;
        public CadetToShow Cadet
        {
            get => cadet;
            set
            {
                cadet = value;
                OnPropertyChanged("Cadet");
            }
        }
        #endregion

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

                #region search cadets

        private int searchFieldIndexCadets;
        public int SearchFieldIndexCadets
        {
            get => searchFieldIndexCadets;
            set
            {
                searchFieldIndexCadets = value;
                UpdateSearchFieldCadets();
                OnPropertyChanged("SearchFieldIndexCadets");
            }
        }

        private string searchValueCadets;
        public string SearchValueCadets
        {
            get => searchValueCadets;
            set
            {
                searchValueCadets = value;
                SearchCadets();
                OnPropertyChanged("SearchValueCadets");
            }
        }

        private string searchFieldPlaceHolderCadets;
        public string SearchFieldPlaceHolderCadets
        {
            get => searchFieldPlaceHolderCadets;
            set
            {
                searchFieldPlaceHolderCadets = value;
                OnPropertyChanged("SearchFieldPlaceHolderCadets");
            }
        }

        private List<string> availableOptionsSearchFieldCadets;
        public List<string> AvailableOptionsSearchFieldCadets
        {
            get => availableOptionsSearchFieldCadets;
            set
            {
                availableOptionsSearchFieldCadets = value;
                OnPropertyChanged("AvailableOptionsSearchFieldCadets");
            }
        }
        #endregion

                #region search activities

        private int searchFieldIndexActivities;
        public int SearchFieldIndexActivities
        {
            get => searchFieldIndexActivities;
            set
            {
                searchFieldIndexActivities = value;
                UpdateSearchFieldActivities();
                OnPropertyChanged("SearchFieldIndexActivities");
            }
        }

        private string searchValueActivities;
        public string SearchValueActivities
        {
            get => searchValueActivities;
            set
            {
                searchValueActivities = value;
                SearchActivities();
                OnPropertyChanged("SearchValueActivities");
            }
        }

        private string searchFieldPlaceHolderActivities;
        public string SearchFieldPlaceHolderActivities
        {
            get => searchFieldPlaceHolderActivities;
            set
            {
                searchFieldPlaceHolderActivities = value;
                OnPropertyChanged("SearchFieldPlaceHolderActivities");
            }
        }

        private List<string> availableOptionsSearchFieldActivities;
        public List<string> AvailableOptionsSearchFieldActivities
        {
            get => availableOptionsSearchFieldActivities;
            set
            {
                availableOptionsSearchFieldActivities = value;
                OnPropertyChanged("AvailableOptionsSearchFieldActivities");
            }
        }
        #endregion
        #endregion
        #endregion

        #region search
        public void UpdateSearchFieldCadets()
        {
            switch (SearchFieldIndexCadets)
            {
                case 0:
                    SearchFieldPlaceHolderCadets = "שם פרטי...";
                    break;
                case 1:
                    SearchFieldPlaceHolderCadets = "שם משפחה...";
                    break;
                case 2:
                    SearchFieldPlaceHolderCadets = "כיתה...";
                    break;
                case 3:
                    SearchFieldPlaceHolderCadets = "תעודת זהות...";
                    break;
                case 6:
                    SearchFieldPlaceHolderCadets = "תפקיד...";
                    break;
            }
        }

        public void UpdateSearchFieldActivities()
        {
            switch (SearchFieldIndexActivities)
            {
                case 0:
                    SearchFieldPlaceHolderActivities = "שם...";
                    break;
            }
        }

        public void SearchCadets()
        {
            CadetsForActivity = AllCadetsForActivity;
            if (SearchValueCadets != "")
            {
                switch (SearchFieldIndexCadets)
                {
                    case 0:
                        ObservableCollection<CadetToShow> dummy = new ObservableCollection<CadetToShow>();
                        foreach (CadetToShow c in CadetsForActivity)
                        {
                            if (c.FirstName == SearchValueCadets)
                                dummy.Add(c);
                        }
                        CadetsForActivity = dummy;
                        break;
                    case 1:
                        dummy = new ObservableCollection<CadetToShow>();
                        foreach (CadetToShow c in CadetsForActivity)
                        {
                            if (c.LastName == SearchValueCadets)
                                dummy.Add(c);
                        }
                        CadetsForActivity = dummy; 
                        break;
                    case 2:
                        dummy = new ObservableCollection<CadetToShow>();
                        foreach (CadetToShow c in CadetsForActivity)
                        {
                            if (c.Class == SearchValueCadets)
                                dummy.Add(c);
                        }
                        CadetsForActivity = dummy;
                        break;
                    case 3:
                        dummy = new ObservableCollection<CadetToShow>();
                        foreach (CadetToShow c in CadetsForActivity)
                        {
                            if (c.PersonalID == SearchValueCadets)
                                dummy.Add(c);
                        }
                        CadetsForActivity = dummy;
                        break;
                    case 4:
                        dummy = new ObservableCollection<CadetToShow>();
                        foreach (CadetToShow c in CadetsForActivity)
                        {
                            if (c.Role == SearchValueCadets)
                                dummy.Add(c);
                        }
                        CadetsForActivity = dummy;
                        break;
                }
            }
        }

        public void SearchActivities()
        {
            ActivitiesForCadet = AllActivitiesForCadet;
            if (SearchValueActivities != "")
            {
                switch (SearchFieldIndexActivities)
                {
                    case 0:
                        ObservableCollection<ActivityToShow> dummy = new ObservableCollection<ActivityToShow>();
                        foreach (ActivityToShow a in ActivitiesForCadet)
                        {
                            if (a.Name == SearchValueActivities)
                                dummy.Add(a);
                        }
                        ActivitiesForCadet = dummy;
                        break;
                }
            }
        }

        private void EnableSearch() => SearchEnabled = true;

        private void DisableSearch()
        {
            SearchEnabled = false;
            SearchField = null;
            searchValueActivities = null;
            searchValueCadets = null;
            if (ShowCadets)
                SetCadetsForActivity();
            if (ShowActivities)
                SetActivitiesForCadet();
        }
        #endregion

        public async void Back()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
