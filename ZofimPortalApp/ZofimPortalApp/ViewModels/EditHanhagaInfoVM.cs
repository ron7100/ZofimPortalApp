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
    class EditHanhagaInfoVM : INotifyPropertyChanged
    {
        public Command ToManageHanhagasCommand => new Command(ToManageHanhaga);
        public Command ToAddShevetCommand => new Command(ToAddShevet);
        public Command SaveChangesCommand => new Command(SaveChanges);

        private ZofimPortalAPIProxy proxy;

        public EditHanhagaInfoVM(Hanhaga hanhaga)
        {
            Hanhaga = hanhaga;
            proxy = ZofimPortalAPIProxy.CreateProxy();
            Name = hanhaga.Name;
            GeneralArea = hanhaga.GeneralArea;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        private Hanhaga Hanhaga;

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

        private string generalArea;
        public string GeneralArea
        {
            get => generalArea;
            set
            {
                generalArea = value;
                CheckGeneralArea();
                OnPropertyChanged("GeneralArea");
            }
        }

        private bool generalAreaError;
        public bool GeneralAreaError
        {
            get => generalAreaError;
            set
            {
                generalAreaError = value;
                OnPropertyChanged("GeneralAreaError");
            }
        }

        private string generalAreaErrorMessage;
        public string GeneralAreaErrorMessage
        {
            get => generalAreaErrorMessage;
            set
            {
                generalAreaErrorMessage = value;
                OnPropertyChanged("GeneralAreaErrorMessage");
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
            Hanhaga hanhagaToSave = Hanhaga;
            hanhagaToSave.Name = Name;
            hanhagaToSave.GeneralArea = GeneralArea;
            hanhagaToSave.ShevetNumber = Hanhaga.ShevetNumber;
            hanhagaToSave.Shevets = await proxy.GetShevetsObjectsForHanhagaAsync(hanhagaToSave.Name);
            await proxy.SaveHanhagaChangesAsync(hanhagaToSave);
            ToManageHanhaga();
        }

        public void CheckName()
        {
            NameError = false;
            if (Name == "")
            {
                NameError = true;
                NameErrorMessage = "זהו שדה חובה";
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
            if(nameHolder!=Name)
                Name = nameHolder;
            if (Name == "")
            {
                NameError = true;
                NameErrorMessage = "זהו שדה חובה";
            }
        }

        public void CheckGeneralArea()
        {
            GeneralAreaError = false;
            if (GeneralArea == "")
            {
                GeneralAreaError = true;
                GeneralAreaErrorMessage = "זהו שדה חובה";
                return;
            }
            int location = 0;
            string generalAreaHolder = GeneralArea;
            foreach (char c in Name)
            {
                if (!char.IsLetter(c))
                    generalAreaHolder = generalAreaHolder.Remove(location, 1);
                location++;
            }
            if(generalAreaHolder != GeneralArea)
                GeneralArea = generalAreaHolder;
            if (GeneralArea == "")
            {
                GeneralAreaError = true;
                GeneralAreaErrorMessage = "זהו שדה חובה";
            }
        }

        public bool CheckForErrors()
        {
            return NameError || GeneralAreaError;
        }

        private async void ToManageHanhaga()
        {
            Page p = new Views.ManageHanhaga();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void ToAddShevet()
        {
            Page p = new Views.AddShevet(Hanhaga);
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
