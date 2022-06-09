using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using ZofimPortalApp.Models;
using ZofimPortalApp.Services;
using ZofimPortalApp.Views;

namespace ZofimPortalApp.ViewModels
{
    class AddShevetVM : INotifyPropertyChanged
    {
        public Command AddShevetCommand => new Command(AddShevet);
        public Command BackToEditHanhagaInfoCommand => new Command(BackToEditHanhagaInfo);

        private ZofimPortalAPIProxy proxy;

        public AddShevetVM(Hanhaga h)
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            NameError = false;
            IsThereError = false;
            hanhaga = h;
            HeaderMessage = $"הוספת שבט להנהגת {h.Name}";
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        private Hanhaga hanhaga;

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

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                if(CheckForIllegalChars(Name))
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

        private bool isThereError;
        public bool IsThereError
        {
            get => isThereError;
            set
            {
                isThereError = value;
                OnPropertyChanged("IsThereError");
            }
        }
        #endregion

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

        public bool CheckForIllegalChars(string s)
        {
            if (s.Length == 0)
                return true;
            foreach(char c in s)
                if (!char.IsLetter(c))
                    return true;
            NameError = false;
            return false;
        }

        private async void AddShevet()
        {
            CheckName();
            IsThereError = false;
            if (NameError)
            {
                IsThereError = true;
                return;
            }
            Shevet s = new Shevet();
            s.Name = Name;
            s.MembersAmount = 0;
            s.HanhagaId = hanhaga.Id;
            await proxy.AddShevetAsync(s);
            ToManageHanhagas();
        }

        private async void ToManageHanhagas()
        {
            Page p = new ManageHanhaga();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void BackToEditHanhagaInfo()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
