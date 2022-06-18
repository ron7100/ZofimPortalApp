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
    class RegisterToActivitiesVM : INotifyPropertyChanged
    {
        public Command BackToHomePageCommand => new Command(BackToHomePage);
        public Command ToSignUpToActivityCommand => new Command(MakeActivitiesList);

        ZofimPortalAPIProxy proxy;

        public RegisterToActivitiesVM()
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            Selected = new ObservableCollection<BooleanEzer>();
            toSignUp = new List<ActivitiesHistory>();
            SetProperties();
        }

        public async void SetProperties()
        {
            parent = await proxy.GetParentAsync(HomePage.ConnectedUser.Id);
            cadets = await proxy.GetCadetsForParentAsync(parent.Id);
            List<ActivityToShow> allActivities = await proxy.GetAllActivitiesAsync();
            ObservableCollection<ActivityToShow> availableActivitiesEzer = new ObservableCollection<ActivityToShow>();
            ObservableCollection<CadetToShow> relevantCadetsEzer = new ObservableCollection<CadetToShow>();
            foreach (CadetToShow c in cadets)
            {
                foreach (ActivityToShow a in allActivities)
                {
                    bool isInRelevantClass = await proxy.IsInRelevantClassAsync(a.RelevantClass, c.Class, c.Role);
                    bool isRegistered = await proxy.IsRegisteredAsync(c.ID, a.ID);
                    if (a.IsOpen == "Green" && isInRelevantClass && a.Shevet == c.Shevet && a.Hanhaga == c.Hanhaga && !isRegistered)
                    {
                        availableActivitiesEzer.Add(a);
                        relevantCadetsEzer.Add(c);
                        selected.Add(new BooleanEzer(false));
                    }
                }
            }
            AvailableActivities = availableActivitiesEzer;
            RelevantCadets = relevantCadetsEzer;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region properties
        private ObservableCollection<ActivityToShow> availableActivities;
        public ObservableCollection<ActivityToShow> AvailableActivities
        {
            get => availableActivities;
            set
            {
                availableActivities = value;
                OnPropertyChanged("AvailableActivities");
            }
        }

        private ObservableCollection<CadetToShow> relevantCadets;
        public ObservableCollection<CadetToShow> RelevantCadets
        {
            get => relevantCadets;
            set
            {
                relevantCadets = value;
                OnPropertyChanged("RelevantCadets");
            }
        }

        private ObservableCollection<BooleanEzer> selected;
        public ObservableCollection<BooleanEzer> Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
            }
        }

        private int shevetId;
        public int ShevetId
        {
            get => shevetId;
            set
            {
                shevetId = value;
                OnPropertyChanged("ShevetId");
            }
        }

        private string shevet;
        public string Shevet
        {
            get => shevet;
            set
            {
                shevet = value;
                OnPropertyChanged("Shevet");
            }
        }

        private int hanhagaId;
        public int HanhagaId
        {
            get => hanhagaId;
            set
            {
                hanhagaId = value;
                OnPropertyChanged("HanhagaId");
            }
        }

        private string hanhaga;
        public string Hanhaga
        {
            get => hanhaga;
            set
            {
                hanhaga = value;
                OnPropertyChanged("Hanhaga");
            }
        }

        private Parent parent;
        private List<ActivitiesHistory> toSignUp;
        private List<CadetToShow> cadets;
        #endregion

        public void MakeActivitiesList()
        {
            List<ActivityToShow> activitiesDummy = new List<ActivityToShow>(availableActivities);
            List<CadetToShow> cadetsDummy = new List<CadetToShow>(relevantCadets);
            foreach(BooleanEzer isSelected in Selected)
            {
                if(isSelected.IsTrue)
                {
                    ActivitiesHistory activitiesHistory = new ActivitiesHistory
                    {
                        ActivityId = activitiesDummy.First().ID,
                        CadetId = cadetsDummy.First().ID
                    };
                    toSignUp.Add(activitiesHistory);
                }
                activitiesDummy.Remove(activitiesDummy.First());
                cadetsDummy.Remove(cadetsDummy.First());
            }
            ToSignUpToActivity();
        }

        public async void BackToHomePage()
        {
            Page p = new HomePage();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        public async void ToSignUpToActivity()
        {
            Page p = new SignUpToActivity(toSignUp);
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
