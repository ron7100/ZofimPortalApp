using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZofimPortalApp.Models;
using ZofimPortalApp.Services;
using ZofimPortalApp.Views;

namespace ZofimPortalApp.ViewModels
{
    class SignUpToActivityVM : INotifyPropertyChanged
    {
        public Command BackToRegisterToActivitiesCommand => new Command(BackToRegisterToActivities);

        public Command SignUpToActivitiesCommand => new Command(SignUpToActivities);

        ZofimPortalAPIProxy proxy;

        public SignUpToActivityVM(List<ActivitiesHistory> toSignUp)
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            ToSignUp = toSignUp;
            SetProperties();
        }

        public async void SetProperties()
        {
            int totalPrice = 0;
            List<ActivityToShow> activities = await proxy.GetAllActivitiesAsync();
            foreach (ActivitiesHistory activitiesHistory in ToSignUp)
            {
                ActivityToShow activity = activities.Where(a => a.ID == activitiesHistory.ActivityId).FirstOrDefault();
                totalPrice += activity.Price * (100 - (int)activity.DiscountPercent) / 100;
            }
            Price = totalPrice;
            List<int> availableMonthsHolder = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                availableMonthsHolder.Add(i);
            }
            AvailableMonths = availableMonthsHolder;
            List<int> availableYearsHolder = new List<int>();
            for (int i = 0; i < 15; i++)
            {
                availableYearsHolder.Add(i + DateTime.Now.Year);
            }
            AvailableYears = availableYearsHolder;
            List<int> availableNumberOfPaymentsEzer = new List<int>();
            availableNumberOfPaymentsEzer.Add(1);
            for (int i = 2; i < Price/200; i++)
            {
                availableNumberOfPaymentsEzer.Add(i);
            }
            AvailableNumberOfPayments = availableNumberOfPaymentsEzer;
            PaymentsNumber = 1;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region properties
        private List<ActivitiesHistory> ToSignUp;

            #region fields
        private string cardNumber;
        public string CardNumber
        {
            get => cardNumber;
            set
            {
                cardNumber = value;
                CheckCardNumber();
                OnPropertyChanged("CardNumber");
            }
        }

        private int expirationMonth;
        public int ExpirationMonth
        {
            get => expirationMonth;
            set
            {
                expirationMonth = value;
                CheckExpirationDate();
                OnPropertyChanged("ExpirationMonth");
            }
        }

        private List<int> availableMonths;
        public List<int> AvailableMonths
        {
            get => availableMonths;
            set
            {
                availableMonths = value;
                OnPropertyChanged("AvailableMonths");
            }
        }

        private int expirationYear;
        public int ExpirationYear
        {
            get => expirationYear;
            set
            {
                expirationYear = value;
                CheckExpirationDate();
                OnPropertyChanged("ExpirationYear");
            }
        }

        private List<int> availableYears;
        public List<int> AvailableYears
        {
            get => availableYears;
            set
            {
                availableYears = value;
                OnPropertyChanged("AvailableYears");
            }
        }

        private string cvv;
        public string Cvv
        {
            get => cvv;
            set
            {
                cvv = value;
                CheckCvv();
                OnPropertyChanged("Cvv");
            }
        }

        private string personalID;
        public string PersonalID
        {
            get => personalID;
            set
            {
                personalID = value;
                CheckPersonalID();
                OnPropertyChanged("PersonalID");
            }
        }

        private int price;
        public int Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        private int paymentsNumber;
        public int PaymentsNumber
        {
            get => paymentsNumber;
            set
            {
                paymentsNumber = value;
                CalculatePayments();
                OnPropertyChanged("PaymentsNumber");
            }
        }

        private List<int> availableNumberOfPayments;
        public List<int> AvailableNumberOfPayments
        {
            get => availableNumberOfPayments;
            set
            {
                availableNumberOfPayments = value;
                OnPropertyChanged("AvailableNumberOfPayments");
            }
        }

        private int firstPaymentAmount;
        public int FirstPaymentAmount
        {
            get => firstPaymentAmount;
            set
            {
                firstPaymentAmount = value;
                OnPropertyChanged("FirstPaymentAmount");
            }
        }

        private int otherPaymentsAmount;
        public int OtherPaymentsAmount
        {
            get => otherPaymentsAmount;
            set
            {
                otherPaymentsAmount = value;
                OnPropertyChanged("OtherPaymentsAmount");
            }
        }
        #endregion

            #region errors
        private bool cardNumberError;
        public bool CardNumberError
        {
            get => cardNumberError;
            set
            {
                cardNumberError = value;
                OnPropertyChanged("CardNumberError");
            }
        }

        private string cardNumberErrorMessage;
        public string CardNumberErrorMessage
        {
            get => cardNumberErrorMessage;
            set
            {
                cardNumberErrorMessage = value;
                OnPropertyChanged("CardNumberErrorMessage");
            }
        }

        private bool expirationDateError;
        public bool ExpirationDateError
        {
            get => expirationDateError;
            set
            {
                expirationDateError = value;
                OnPropertyChanged("ExpirationDateError");
            }
        }

        private string expirationDateErrorMessage;
        public string ExpirationDateErrorMessage
        {
            get => expirationDateErrorMessage;
            set
            {
                expirationDateErrorMessage = value;
                OnPropertyChanged("ExpirationDateErrorMessage");
            }
        }

        private bool cvvError;
        public bool CvvError
        {
            get => cvvError;
            set
            {
                cvvError = value;
                OnPropertyChanged("CvvError");
            }
        }

        private string cvvErrorMessage;
        public string CvvErrorMessage
        {
            get => cvvErrorMessage;
            set
            {
                cvvErrorMessage = value;
                OnPropertyChanged("CvvErrorMessage");
            }
        }

        private bool personalIDError;
        public bool PersonalIDError
        {
            get => personalIDError;
            set
            {
                personalIDError = value;
                OnPropertyChanged("PersonalIDError");
            }
        }

        private string personalIDErrorMessage;
        public string PersonalIDErrorMessage
        {
            get => personalIDErrorMessage;
            set
            {
                personalIDErrorMessage = value;
                OnPropertyChanged("PersonalIDErrorMessage");
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

        public void CalculatePayments()
        {
            if (PaymentsNumber == 0)
                return;
            if(PaymentsNumber == 1)
            {
                OtherPaymentsAmount = 0;
                FirstPaymentAmount = Price;
                return;
            }
            double exactDivident = Price / PaymentsNumber;
            OtherPaymentsAmount = (int)exactDivident;
            FirstPaymentAmount = Price - OtherPaymentsAmount * (PaymentsNumber - 1);
        }

        #region check fields
        public void CheckCardNumber()
        {
            CardNumberError = false;
            if (CardNumber == "")
            {
                CardNumberError = true;
                CardNumberErrorMessage = "זהו שדה חובה";
                return;
            }
            int location = 0;
            string cardNumberHolder = CardNumber;
            foreach (char c in cardNumberHolder)
            {
                if (char.IsLetter(c))
                    cardNumberHolder = cardNumberHolder.Remove(location, 1);
                location++;
            }
            if (cardNumberHolder != CardNumber)
                CardNumber = cardNumberHolder;
            if (CardNumber == "")
            {
                CardNumberError = true;
                CardNumberErrorMessage = "זהו שדה חובה";
            }
        }

        public void CheckExpirationDate()
        {
            ExpirationDateError = false;
            if (ExpirationYear == 0 && ExpirationMonth == 0)
            {
                ExpirationDateError = true;
                ExpirationDateErrorMessage = "זהו שדה חובה";
                return;
            }
            else if (ExpirationYear == 0)
                return;
            else if (ExpirationMonth == 0)
                return;
            DateTime expirationDate = new DateTime(ExpirationYear, ExpirationMonth, 1);
            if(DateTime.Compare(DateTime.Now, expirationDate)>0)
            {
                ExpirationDateError = true;
                ExpirationDateErrorMessage = "הכרטיס פג תוקף";
            }
        }

        public void CheckCvv()
        {
            CvvError = false;
            if (Cvv == "")
            {
                CvvError = true;
                CvvErrorMessage = "זהו שדה חובה";
                return;
            }
            int location = 0;
            string cvvHolder = Cvv;
            foreach (char c in cvvHolder)
            {
                if (char.IsLetter(c))
                    cvvHolder = cvvHolder.Remove(location, 1);
                location++;
            }
            if (cvvHolder != Cvv)
                Cvv = cvvHolder;
            if (Cvv == "")
            {
                CvvError = true;
                CvvErrorMessage = "זהו שדה חובה";
            }
        }

        public void CheckPersonalID()
        {
            PersonalIDError = false;
            if (PersonalID == "")
            {
                PersonalIDError = true;
                PersonalIDErrorMessage = "זהו שדה חובה";
                return;
            }
            int location = 0;
            string personalIDHolder = PersonalID;
            foreach (char c in personalIDHolder)
            {
                if (char.IsLetter(c))
                    personalIDHolder = personalIDHolder.Remove(location, 1);
                location++;
            }
            if (personalIDHolder != PersonalID)
                PersonalID = personalIDHolder;
            if (PersonalID == "")
            {
                PersonalIDError = true;
                PersonalIDErrorMessage = "זהו שדה חובה";
            }
        }

        public bool CheckForErrors()
        {
            CheckCardNumber();
            CheckExpirationDate();
            CheckCvv();
            CheckPersonalID();
            if(CardNumber.Length!=16)
            {
                CardNumberError = true;
                CardNumberErrorMessage = "אנא הכנס מספר כרטיס תקין בעל 16 תווים";
                return true;
            }
            if (Cvv.Length < 3 || Cvv.Length > 4)
            {
                CvvError = true;
                CvvErrorMessage = "אנא הכנס מספר CVV בעל 3-4 תווים";
                return true;
            }
            if (PersonalID.Length != 9)
            {
                PersonalIDError = true;
                PersonalIDErrorMessage = "אנא הכנס מספר תעודת זהות תקין בעל 9 תווים";
                return true;
            }
            return CardNumberError || ExpirationDateError || CvvError || PersonalIDError;
        }
        #endregion
        
        public async void SignUpToActivities()
        {
            CheckForErrors();
            if (AreThereErrors)
                return;
            await proxy.SignUpToActivitiesAsync(ToSignUp);
            await App.Current.MainPage.DisplayAlert("תשלום בוצע בהצלחה!", "לחץ המשך כדי לחזור", "המשך");
            BackToRegisterToActivities();
        }

        public async void BackToRegisterToActivities()
        {
            Page p = new RegisterToActivities();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
