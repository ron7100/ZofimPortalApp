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
    class EditShevetInfoVM : INotifyPropertyChanged
    {
        public Command ToManageShevetsCommand => new Command(ToManageShevet);
        public Command SaveChangesCommand => new Command(SaveChanges);

        private ZofimPortalAPIProxy proxy;

        public EditShevetInfoVM(ShevetToShow shevet)
        {
            Shevet = shevet;
            proxy = ZofimPortalAPIProxy.CreateProxy();
            SetProperties();
        }

        public async void SetProperties()
        {
            int permissionLevel = await proxy.GetPermissionLevelAsync(HomePage.ConnectedUser.Id);
            Name = Shevet.Name;
            MembersAmount = Shevet.MembersAmount.ToString();
            if (permissionLevel == 1)//admin, can edit all info
            {
                ShowHanhaga = true;
                List<Hanhaga> HanhagasHolder = await proxy.GetAllHanhagasAsync();
                Hanhaga PickerHanhagaHolder = new Hanhaga();
                foreach (Hanhaga h in HanhagasHolder)
                {
                    if (h.Name == Shevet.Hanhaga)
                        PickerHanhagaHolder = h;
                }
                Hanhagas = HanhagasHolder;
                PickerHanhaga = PickerHanhagaHolder;
            }
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
                CheckName();
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
            get => nameErrorMessage;
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
                CheckMembersAmount();
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

        private List<Hanhaga> hanhagas;
        public List<Hanhaga> Hanhagas
        {
            get => hanhagas;
            set
            {
                hanhagas = value;
                OnPropertyChanged("Hanhagas");
            }
        }

        private bool areThereErrors;
        public bool AreThereErrors
        {
            get => areThereErrors;
            set
            {
                areThereErrors = value;
                OnPropertyChanged("AreThereErrors");
            }
        }
        #endregion

        public async void SaveChanges()
        {
            CheckForErrors();
            if (AreThereErrors)
                return;
            ShevetToShow shevetToSave = Shevet;
            shevetToSave.Name = Name;
            shevetToSave.MembersAmount = int.Parse(MembersAmount);
            shevetToSave.Hanhaga = Hanhaga.Name;
            await proxy.SaveShevetChangesAsync(shevetToSave);
            ToManageShevet();
        }

        public void CheckName()
        {
            NameError = false;
            if (Name == "")
            {
                NameError = true;
                NameErrorMessage = "שדה חובה";
                return;
            }
            int location = 0;
            string nameHolder = Name;
            foreach (char c in Name)
            {
                if (!char.IsLetter(c))
                    nameHolder = nameHolder.Remove(location, 1);
                location++;
            }
            Name = nameHolder;
        }

        public void CheckMembersAmount()
        {
            MembersAmountError = false;
            if(MembersAmount == "")
            {
                MembersAmountError = true;
                MembersAmountErrorMessage = "שדה חובה";
                return;
            }
            int location = 0;
            string membersAmountHolder = MembersAmount;
            foreach (char c in membersAmountHolder)
            {
                if (char.IsLetter(c))
                    membersAmountHolder = membersAmountHolder.Remove(location, 1);
                location++;
            }
            membersAmount = membersAmountHolder;
        }

        public bool CheckForErrors()
        {
            return NameError || MembersAmountError;
        }

        private async void ToManageShevet()
        {
            Page p = new Views.ManageShevet();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
