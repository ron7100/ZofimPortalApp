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

        private ZofimPortalAPIProxy proxy;
        public ManageUsersVM()
        {
            HeaderMessage = "עריכת משתמשים";
            proxy = ZofimPortalAPIProxy.CreateProxy();
            SetLists();
            SelectedType = 0;
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
                    break;
            }
            AvailableToShow = toShow;

            UsersList = await proxy.GetAllUsersAsync();
            WorkersList = await proxy.GetAllWorkersAsync();
            ParentsList = await proxy.GetAllParentsAsync();
            CadetsList = await proxy.GetAllCadetsAsync();
            //להוסיף כאן שזה מוציא רק את מה שרלוונטי למשתמש (לא בעיה, עם פעולות בשרת שמוציאות הכל לפי שבט/הנהגה)
            //צריך להוסיף חיפוש בעמוד הזה לפי שלוש עמודות (להוסיף פיקר עם העמודות ולהוריד את מה שכבר נבחר, לחפש ברשימה עם foreach
        }

        #region Properties
            #region Lists
        private ObservableCollection<User> usersList;
        public ObservableCollection<User> UsersList
        {
            get => usersList;
            set
            {
                usersList = value;
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

        public void UpdateSelection()
        {
            IsUserSelected = false;
            IsWorkerSelected = false;
            IsParentSelected = false;
            IsCadetSelected = false;
            switch(selectedType)
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

        private async void BackToHomePage()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
