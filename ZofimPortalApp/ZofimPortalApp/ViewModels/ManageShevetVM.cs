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
            
        }
        public async void SetProperties()
        {
            int permissionLevel = await proxy.GetPermissionLevelAsync(HomePage.ConnectedUser.Id);
            //1 -> admin, can see all
            //2 -> can see only from his hanhaga
            //3 -> can see only parents and cadets from his shevet
            CanSeeAllShevets = permissionLevel == 1;//only an admin can see all shevets
            if (canSeeAllShevets)
            {
                ObservableCollection<ShevetToShow> shevetsEzer = new ObservableCollection<ShevetToShow>();
                List<ShevetToShow> allShevets = await proxy.GetAllShevetsAsync();
                foreach (ShevetToShow s in allShevets)
                    shevetsEzer.Add(s);
                headerMessage = "נתוני שבטים";
            }
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
    }
}
