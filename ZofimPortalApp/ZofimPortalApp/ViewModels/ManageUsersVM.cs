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
        }

        public async void SetLists()
        {
            UsersList = await proxy.GetAllUsersAsync();
            WorkersList = await proxy.GetAllWorkersAsync();
            ParentsList = await proxy.GetAllParentsAsync();
            //CadetsList = new ObservableCollection<Cadet>(await proxy.GetAllCadetsAsync());
        }

        #region Properties
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

        private ObservableCollection<Cadet> cadetsList;
        public ObservableCollection<Cadet> CadetsList
        {
            get => cadetsList;
            set
            {
                cadetsList = value;
                OnPropertyChanged("CadetsList");
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
