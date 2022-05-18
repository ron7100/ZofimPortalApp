using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using ZofimPortalApp.Models;
using ZofimPortalApp.Services;
using ZofimPortalApp.Views;

namespace ZofimPortalApp.ViewModels
{
    class EditShevetInfoVM
    {
        private ZofimPortalAPIProxy proxy;

        public EditShevetInfoVM(ShevetToShow shevet)
        {
            Shevet = shevet;
            proxy = ZofimPortalAPIProxy.CreateProxy();
            SetProperties();
        }

        public void SetProperties()
        {
            Name = Shevet.Name;
            MembersAmount = Shevet.MembersAmount.ToString();
            string hanhagaName = Shevet.Hanhaga;
            //HanhagaPicker = 
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        private ShevetToShow Shevet;

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private bool nameError;
        public bool NameError
        {
            get => nameError;
            set
            {
                nameError = value;
                OnPropertyChanged("NameError");
            }
        }

        private string nameErrorMessage;
        public string NameErrorMessage
        {
            get => NameErrorMessage;
            set
            {
                nameErrorMessage = value;
                OnPropertyChanged("NameErrorMessage");
            }
        }

        private string membersAmount;
        public string MembersAmount
        {
            get => membersAmount;
            set
            {
                membersAmount = value;
                OnPropertyChanged("MembersAmount");
            }
        }

        private bool membersAmountError;
        public bool MembersAmountError
        {
            get => membersAmountError;
            set
            {
                membersAmountError = value;
                OnPropertyChanged("MembersAmountError");
            }
        }

        private string membersAmountErrorMessage;
        public string MembersAmountErrorMessage
        {
            get => membersAmountErrorMessage;
            set
            {
                membersAmountErrorMessage = value;
                OnPropertyChanged("MembersAmountErrorMessage");
            }
        }

        private Hanhaga hanhaga;
        public Hanhaga Hanhaga
        {
            get => hanhaga;
            set
            {
                hanhaga = value;
                OnPropertyChanged("Hanhaga");
            }
        }

        private Hanhaga pickerHanhaga;
        public Hanhaga PickerHanhaga
        {
            get => pickerHanhaga;
            set
            {
                pickerHanhaga = value;
                if (PickerHanhaga != Hanhaga)
                    Hanhaga = PickerHanhaga;
                OnPropertyChanged("PickerHanhaga");
            }
        }

        private int pickerHanhagaId;
        public int PickerHanhagaId
        {
            get => pickerHanhagaId;
            set
            {
                pickerHanhagaId = value;
                OnPropertyChanged("PickerHanhagaId");
            }
        }

        private bool showHanhaga;
        public bool ShowHanhaga
        {
            get => showHanhaga;
            set
            {
                showHanhaga = value;
                OnPropertyChanged("ShowHanhaga");
            }
        }

        private List<string> hanhagas;
        public List<string> Hanhagas
        {
            get => hanhagas;
            set
            {
                hanhagas = value;
                OnPropertyChanged("Hanhagas");
            }
        }
        #endregion
    }
}
