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
using ZofimPortalApp.Views;
using System.Text.RegularExpressions;

namespace ZofimPortalApp.ViewModels
{
    class SignUpVM : INotifyPropertyChanged
    {
        public Command TogglePasswordCommand { get; }
        public Command ToggleCheckPasswordCommand { get; }
        public Command SignUpCommand { get; }
        public Command ToLogInCommand { get; }
        public Command BackToHomePageCommand { get; }


        private ZofimPortalAPIProxy proxy;
        public SignUpVM()
        {
            TogglePasswordCommand = new Command(TogglePassword);
            ToggleCheckPasswordCommand = new Command(ToggleCheckPassword);
            SignUpCommand = new Command(SignUp);
            ToLogInCommand = new Command(ToLogIn);
            BackToHomePageCommand = new Command(BackToHomePage);
            IsPassword = true;
            IsCheckPassword = true;
            IsEmailError = false;
            IsFNameError = false;
            IsLNameError = false;
            IsPersonalIDError = false;
            IsPassError = false;
            IsCheckPassError = false;
            proxy = ZofimPortalAPIProxy.CreateProxy();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
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

        private string fName;
        public string FName
        {
            get => fName;
            set
            {
                fName = value;
                CheckFName();
                OnPropertyChanged("FName");
            }
        }

        private string lName;
        public string LName
        {
            get => lName;
            set
            {
                lName = value;
                CheckLName();
                OnPropertyChanged("LName");
            }
        }

        private string personalID;
        public string PersonalID
        {
            get => personalID;
            set
            {
                personalID = value;
                CheckID();
                OnPropertyChanged("PersonalID");
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

        private int userType;
        public int UserType
        {
            get => userType;
            set
            {
                userType = value;
                OnPropertyChanged("UserType");
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

        private bool isEmailError;
        public bool IsEmailError
        {
            get => isEmailError;
            set
            {
                isEmailError = value;
                OnPropertyChanged("IsEmailError");
            }
        }

        private string emailError;
        public string EmailError
        {
            get => emailError;
            set
            {
                emailError = value;
                OnPropertyChanged("EmailError");
            }
        }

        private bool isFNameError;
        public bool IsFNameError
        {
            get => isFNameError;
            set
            {
                isFNameError = value;
                OnPropertyChanged("IsFNameError");
            }
        }

        private string fNameError;
        public string FNameError
        {
            get => fNameError;
            set
            {
                fNameError = value;
                OnPropertyChanged("FNameError");
            }
        }

        private bool isLNameError;
        public bool IsLNameError
        {
            get => isLNameError;
            set
            {
                isLNameError = value;
                OnPropertyChanged("IsLNameError");
            }
        }

        private string lNameError;
        public string LNameError
        {
            get => lNameError;
            set
            {
                lNameError = value;
                OnPropertyChanged("LNameError");
            }
        }

        private bool isPersonalIDError;
        public bool IsPersonalIDError
        {
            get => isPersonalIDError;
            set
            {
                isPersonalIDError = value;
                OnPropertyChanged("IsPersonalIDError");
            }
        }

        private string personalIDError;
        public string PersonalIDError
        {
            get => personalIDError;
            set
            {
                personalIDError = value;
                OnPropertyChanged("PersonalIDError");
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

        private string generalError;
        public string GeneralError
        {
            get => generalError;
            set
            {
                generalError = value;
                OnPropertyChanged("GeneralError");
            }
        }
        #endregion

        public async void ToLogIn()
        {
            Page p = new Views.LogIn();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        public void AreThereErrors()
        {
            IsThereError = false;
            if (Email == null || FName == null || LName == null || PersonalID == null || Pass == null || CheckPass == null)
            {
                IsThereError = true;
                GeneralError = "יש שדה ריק";
            }
            else if (IsEmailError || IsFNameError || IsLNameError || IsPersonalIDError || IsPassError || IsCheckPassError)
            {
                IsThereError = true;
                GeneralError = "ישנן בעיות בטופס";
            }
        }

        private async void SignUp()
        {
            AreThereErrors();
            if (proxy.IsUserExistAsync(email).Result)
            {
                SignUpFailed();
                return;
            }
            //create user object here and send the data as a user to the proxy
            User user = new User();
            user.Email = email;
            user.FirstName = fName;
            user.LastName = lName;
            user.PersonalId = personalID;
            user.Password = pass;
            user.Id = proxy.GetLastUserIDAsync().Result + 1;
            Object userToReturn = await proxy.SignUpAsync(user);
            if (userToReturn == null)
                SignUpFailed();
            else
                SignUpSuccess(user);
        }

        private void SignUpFailed()
        {
            //email already exists
            IsEmailError = true;
            EmailError = "המייל כבר קיים במערכת";
        }

        #region check fields
        private void CheckEmail()
        {

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            IsEmailError = !match.Success;
            if (IsEmailError)
                EmailError = "כתובת מייל לא חוקית";
        }
        
        private void CheckFName()
        {
            IsFNameError = false;
            if (FName == "")
            {
                IsFNameError = true;
                FNameError = "שדה חובה";
                return;
            }
            foreach (char c in FName)
            {
                if (!char.IsLetter(c))
                {
                    IsFNameError = true;
                    FNameError = "השם חייב לכלול אותיות בלבד";
                }
            }
        }

        private void CheckLName()
        {
            IsLNameError = false;
            if (LName == "")
            {
                IsLNameError = true;
                LNameError = "שדה חובה";
                return;
            }
            foreach (char c in LName)
            {
                if (!char.IsLetter(c))
                {
                    IsLNameError = true;
                    LNameError = "השם משפחה חייב לכלול אותיות בלבד";
                }
            }
        }

        private void CheckID()
        {
            IsPersonalIDError = false;
            if(PersonalID.Length!=9)
            {
                IsPersonalIDError = true;
                PersonalIDError = "תעודת זהות חייבת לכלול 9 ספרות";
                return;
            }
            foreach (char c in PersonalID)
            {
                if (!char.IsDigit(c))
                {
                    IsPersonalIDError = true;
                    PersonalIDError = "תעודת זהות חייבת לכלול ספרות בלבד";
                }
            }
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
            if(CheckPass!=null)
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

        private async void SignUpSuccess(User u)
        {
            HomePage.connectedUser = u;
            await App.Current.MainPage.Navigation.PopAsync();
        }

        private void TogglePassword()
        {
            IsPassword=!IsPassword;
        }

        private void ToggleCheckPassword()
        {
            IsCheckPassword = !IsCheckPassword;
        }

        private async void BackToHomePage()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
