﻿using System;
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
using ZofimPortalApp.Views;
using System.Text.RegularExpressions;

namespace ZofimPortalApp.ViewModels
{
    class EditPersonalInfoVM : INotifyPropertyChanged
    {
        public Command BackToPersonalInfoCommand => new Command(BackToPersonalInfo);
        public Command SaveChangesCommand => new Command(SaveChanges);

        private ZofimPortalAPIProxy proxy;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public EditPersonalInfoVM()
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            User = HomePage.ConnectedUser;
            Email = User.Email;
            FirstName = User.FirstName;
            LastName = User.LastName;
            PersonalId = User.PersonalId;
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

        #region fields
        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                CheckEmail();
                OnPropertyChanged("Email");
            }
        }

        private string firstName;
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                CheckFirstName();
                OnPropertyChanged("FirstName");
            }
        }

        private string lastName;
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                CheckLastName();
                OnPropertyChanged("LastName");
            }
        }

        private string personalId;
        public string PersonalId
        {
            get => personalId;
            set
            {
                personalId = value;
                CheckPersonalId();
                OnPropertyChanged("PersonalId");
            }
        }
        #endregion

        #region errors
        private bool emailError;
        public bool EmailError
        {
            get => emailError;
            set
            {
                emailError = value;
                OnPropertyChanged("EmailError");
            }
        }

        private string emailErrorMessage;
        public string EmailErrorMessage
        {
            get => emailErrorMessage;
            set
            {
                emailErrorMessage = value;
                OnPropertyChanged("EmailErrorMessage");
            }
        }

        private bool firstNameError;
        public bool FirstNameError
        {
            get => firstNameError;
            set
            {
                firstNameError = value;
                OnPropertyChanged("FirstNameError");
            }
        }

        private string firstNameErrorMessage;
        public string FirstNameErrorMessage
        {
            get => firstNameErrorMessage;
            set
            {
                firstNameErrorMessage = value;
                OnPropertyChanged("FirstNameErrorMessage");
            }
        }

        private bool lastNameError;
        public bool LastNameError
        {
            get => lastNameError;
            set
            {
                lastNameError = value;
                OnPropertyChanged("LastNameError");
            }
        }

        private string lastNameErrorMessage;
        public string LastNameErrorMessage
        {
            get => lastNameErrorMessage;
            set
            {
                lastNameErrorMessage = value;
                OnPropertyChanged("LastNameErrorMessage");
            }
        }

        private bool personalIdError;
        public bool PersonalIdError
        {
            get => personalIdError;
            set
            {
                personalIdError = value;
                OnPropertyChanged("PersonalIdError");
            }
        }

        private string personalIdErrorMessage;
        public string PersonalIdErrorMessage
        {
            get => personalIdErrorMessage;
            set
            {
                personalIdErrorMessage = value;
                OnPropertyChanged("PersonalIdErrorMessage");
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
        #endregion

        #region check fields
        private async void CheckEmail()
        {
            EmailError = false;
            if (Email == User.Email)
                return;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(Email);
            EmailError = !match.Success;
            if (EmailError)
            {
                EmailErrorMessage = "כתובת מייל לא חוקית";
                return;
            }
            if (await proxy.IsUserExistAsync(Email))
            {
                EmailError = true;
                EmailErrorMessage = "כתובת מייל כבר קיימת במערכת";
            }
        }

        private void CheckFirstName()
        {
            FirstNameError = false;
            if (FirstName == "")
            {
                FirstNameError = true;
                FirstNameErrorMessage = "שדה חובה";
                return;
            }
            foreach (char c in FirstName)
            {
                if (!char.IsLetter(c))
                {
                    FirstNameError = true;
                    FirstNameErrorMessage = "השם חייב לכלול אותיות בלבד";
                }
            }
        }

        private void CheckLastName()
        {
            LastNameError = false;
            if (LastName == "")
            {
                LastNameError = true;
                LastNameErrorMessage = "שדה חובה";
                return;
            }
            foreach (char c in LastName)
            {
                if (!char.IsLetter(c))
                {
                    LastNameError = true;
                    LastNameErrorMessage = "השם משפחה חייב לכלול אותיות בלבד";
                }
            }
        }

        private async void CheckPersonalId()
        {
            PersonalIdError = false;
            if (PersonalId == User.PersonalId)
                return;
            if (PersonalId.Length != 9)
            {
                PersonalIdError = true;
                PersonalIdErrorMessage = "תעודת זהות חייבת לכלול 9 ספרות";
                return;
            }
            foreach (char c in PersonalId)
            {
                if (!char.IsDigit(c))
                {
                    PersonalIdError = true;
                    PersonalIdErrorMessage = "תעודת זהות חייבת לכלול ספרות בלבד";
                    return;
                }
            }
            if (await proxy.IsIdExistAsync(PersonalId))
            {
                PersonalIdError = true;
                PersonalIdErrorMessage = "תעודת זהות כבר קיימת במערכת";
            }
        }
        #endregion

        private async void SaveChanges()
        {
            CheckForErrors();
            if (AreThereErrors)
                return;
            User.Email = Email;
            User.FirstName = FirstName;
            User.LastName = LastName;
            User.PersonalId = PersonalId;
            await proxy.SaveUserChangesAsync(User);
            BackToPersonalInfo();
        }

        private void CheckForErrors()
        {
            AreThereErrors = EmailError || FirstNameError || LastNameError || PersonalIdError;
        }

        private async void BackToPersonalInfo()
        {
            Page p = new PersonalInfo();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
