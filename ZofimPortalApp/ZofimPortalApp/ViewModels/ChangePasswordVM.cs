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
    class ChangePasswordVM:INotifyPropertyChanged
    {
        public Command BackToPersonalInfoCommand => new Command(BackToPersonalInfo);
        public Command ChangePasswordCommand => new Command(ChangePassword);
        public Command ToggleCurrentPasswordCommand => new Command(ToggleCurrentPassword);
        public Command TogglePasswordCommand => new Command(TogglePassword);
        public Command ToggleCheckPasswordCommand => new Command(ToggleCheckPassword);

        private ZofimPortalAPIProxy proxy;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ChangePasswordVM()
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            User = HomePage.ConnectedUser;
            IsCurrentPassword = true;
            IsPassword = true;
            IsCheckPassword = true;
        }

        #region Properties
        private User user;
        public User User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        private string currentPass;
        public string CurrentPass
        {
            get => currentPass;
            set
            {
                currentPass = value;
                CheckCurrentPass();
                OnPropertyChanged("CurrentPass");
            }
        }

        private bool isCurrentPassError;
        public bool IsCurrentPassError
        {
            get => isCurrentPassError;
            set
            {
                isCurrentPassError = value;
                OnPropertyChanged("IsCurrentPassError");
            }
        }

        private string pass;
        public string Pass
        {
            get => pass;
            set
            {
                pass = value;
                CheckPassword();
                OnPropertyChanged("Pass");
            }
        }

        private string checkPass;
        public string CheckPass
        {
            get => checkPass;
            set
            {
                checkPass = value;
                CheckCheckPassword();
                OnPropertyChanged("CheckPass");
            }
        }

        private bool isCurrentPassword;
        public bool IsCurrentPassword
        {
            get => isCurrentPassword;
            set
            {
                isCurrentPassword = value;
                OnPropertyChanged("IsCurrentPassword");
            }
        }

        private bool isPassword;
        public bool IsPassword
        {
            get => isPassword;
            set
            {
                isPassword = value;
                OnPropertyChanged("IsPassword");
            }
        }

        private bool isCheckPassword;
        public bool IsCheckPassword
        {
            get => isCheckPassword;
            set
            {
                isCheckPassword = value;
                OnPropertyChanged("IsCheckPassword");
            }
        }

        private bool isPassError;
        public bool IsPassError
        {
            get => isPassError;
            set
            {
                isPassError = value;
                OnPropertyChanged("IsPassError");
            }
        }

        private string passError;
        public string PassError
        {
            get => passError;
            set
            {
                passError = value;
                OnPropertyChanged("PassError");
            }
        }

        private bool isCheckPassError;
        public bool IsCheckPassError
        {
            get => isCheckPassError;
            set
            {
                isCheckPassError = value;
                OnPropertyChanged("IsCheckPassError");
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

        #region Check fields
        private void CheckCurrentPass()
        {
            IsCurrentPassError = currentPass != User.Password;
        }

        private void CheckPassword()
        {
            IsPassError = false;
            bool passLengthError = Pass.Length < 6;
            bool passCharsError = false;

            bool isThereLetter = false;
            bool isThereNumber = false;
            bool noSpecialSigns = true;
            foreach (char c in Pass)
            {
                if (char.IsDigit(c))
                    isThereNumber = true;
                else if (char.IsLetter(c))
                    isThereLetter = true;
                else if (c >= 'א' && c <= 'ת')
                    isThereLetter = true;
                else
                    noSpecialSigns = false;
            }
            if (!(noSpecialSigns && isThereLetter && isThereNumber))
            {
                passCharsError = true;
            }
            if (passCharsError && !passLengthError)
            {
                PassError = "הסיסמה חייבת לכלול גם אותיות בעברית/אנגלית וגם מספרים, בלי תווים אחרים";
                IsPassError = true;
            }
            else if (passLengthError)
            {
                PassError = "הסיסמה חייבת להיות לפחות שישה תווים, ולכלול גם אותיות בעברית/אנגלית וגם מספרים, בלי תווים אחרים";
                IsPassError = true;
            }
            else
                IsPassError = false;
            if (CheckPass != null)
                CheckCheckPassword();
        }

        private void CheckCheckPassword()
        {
            IsCheckPassError = false;
            if (pass != checkPass)
                IsCheckPassError = true;
            else
                IsCheckPassError = false;
        }
        #endregion

        private void ToggleCurrentPassword() => IsCurrentPassword = !IsCurrentPassword;

        private void TogglePassword() => IsPassword = !IsPassword;

        private void ToggleCheckPassword() => IsCheckPassError = !IsCheckPassword;

        private async void ChangePassword()
        {
            CheckCurrentPass();
            CheckPassword();
            CheckCheckPassword();
            if (IsPassError || IsCheckPassError || IsCurrentPassError)
            {
                AreThereErrors = true;
                return;
            }
            User.Password = pass;
            await proxy.SaveUserChangesAsync(User);
            BackToPersonalInfo();
        }

        public async void BackToPersonalInfo()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
