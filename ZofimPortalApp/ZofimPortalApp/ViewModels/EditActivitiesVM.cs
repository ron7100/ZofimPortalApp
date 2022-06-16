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
    class EditActivitiesVM : INotifyPropertyChanged
    {
        public Command BackToManageActivitiesCommand => new Command(BackToManageActivities);
        public Command ToManageActivitiesCommand => new Command(ToManageActivities);
        public Command ToCadetsForActivityCommand => new Command(ToCadetsForActivity);
        public Command SaveChangesCommand => new Command(SaveChanges);

        private ZofimPortalAPIProxy proxy;
        public EditActivitiesVM(ActivityToShow a)
        {
            proxy = ZofimPortalAPIProxy.CreateProxy();
            HeaderMessage = $"עריכת מפעל: {a.Name}";
            SetPropertiesAsync(a);
        }
        public async void SetPropertiesAsync(ActivityToShow a)
        {
            EditedActivity = a;
            Name = a.Name;
            StartDate = a.StartDate.ConvertToDateTime();
            EndDate = a.EndDate.ConvertToDateTime();
            Price = a.Price.ToString();
            DiscountPercent = a.DiscountPercent.ToString();
            IsOpen = a.IsOpen == "Green";
            List<Shevet> shevetsList = await proxy.GetAllShevetsAsync();
            List<Hanhaga> hanhagasList = await proxy.GetAllHanhagasAsync();
            Shevet = shevetsList.Where(s => s.Id == a.ShevetID).FirstOrDefault();
            Hanhaga = hanhagasList.Where(h => h.Id == a.HanhagaID).FirstOrDefault();
            ShowShevet = false;
            ShowHanhaga = false;
            NameError = false;
            StartDateError = false;
            EndDateError = false;
            RelevantClassError = false;
            PriceError = false;
            DiscountPercentError = false;
            ShevetError = false;
            HanhagaError = false;
            await SetListsForPickers();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
            #region שדות
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

        private DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                CheckStartDate();
                OnPropertyChanged("StartDate");
            }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                CheckEndDate();
                OnPropertyChanged("EndDate");
            }
        }

        private string relevantClass;
        public string RelevantClass
        {
            get => relevantClass;
            set
            {
                relevantClass = value;
                CheckRelevantClass();
                OnPropertyChanged("RelevantClass");
            }
        }

        private string price;
        public string Price
        {
            get => price;
            set
            {
                price = value;
                CheckPrice();
                OnPropertyChanged("Price");
            }
        }

        private string discountPercent;
        public string DiscountPercent
        {
            get => discountPercent;
            set
            {
                discountPercent = value;
                CheckDiscountPercent();
                OnPropertyChanged("DiscountPercent");
            }
        }

        private bool isOpen;
        public bool IsOpen
        {
            get => isOpen;
            set
            {
                isOpen = value;
                OnPropertyChanged("IsOpen");
            }
        }

        private string isOpenString;
        public string IsOpenString
        {
            get => isOpenString;
            set
            {
                isOpenString = value;
                SetIsOpenBool(value);
                OnPropertyChanged("IsOpenString");
            }
        }

        private Shevet shevet;
        public Shevet Shevet
        {
            get => shevet;
            set
            {
                shevet = value;
                CheckShevet();
                OnPropertyChanged("Shevet");
            }
        }

        private Shevet pickerShevet;
        public Shevet PickerShevet
        {
            get => pickerShevet;
            set
            {
                pickerShevet = value;
                if (PickerShevet != Shevet)
                    Shevet = PickerShevet;
                OnPropertyChanged("PickerShevet");
            }
        }

        private int pickerShevetId;
        public int PickerShevetId
        {
            get => pickerShevetId;
            set
            {
                pickerShevetId = value;
                OnPropertyChanged("PickerShevetId");
            }
        }

        private bool showShevet;
        public bool ShowShevet
        {
            get => showShevet;
            set
            {
                showShevet = value;
                if (!showShevet)
                    ShevetError = showShevet;
                OnPropertyChanged("ShowShevet");
            }
        }

        private Hanhaga hanhaga;
        public Hanhaga Hanhaga
        {
            get => hanhaga;
            set
            {
                hanhaga = value;
                _ = SetShevetsAccordingToHanhaga();
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

        private bool showCadets;
        public bool ShowCadets
        {
            get => showCadets;
            set
            {
                showCadets = value;
                OnPropertyChanged("ShowCadets");
            }
        }
        #endregion

            #region שגיאות
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

        private bool startDateError;
        public bool StartDateError
        {
            get => startDateError;
            set
            {
                startDateError = value;
                OnPropertyChanged("StartDateError");
            }
        }

        private string startDateErrorMessage;
        public string StartDateErrorMessage
        {
            get => startDateErrorMessage;
            set
            {
                startDateErrorMessage = value;
                OnPropertyChanged("StartDateErrorMessage");
            }
        }

        private bool endDateError;
        public bool EndDateError
        {
            get => endDateError;
            set
            {
                endDateError = value;
                OnPropertyChanged("EndDateError");
            }
        }

        private string endDateErrorMessage;
        public string EndDateErrorMessage
        {
            get => endDateErrorMessage;
            set
            {
                endDateErrorMessage = value;
                OnPropertyChanged("EndDateErrorMessage");
            }
        }

        private bool relevantClassError;
        public bool RelevantClassError
        {
            get => relevantClassError;
            set
            {
                relevantClassError = value;
                OnPropertyChanged("RelevantClassError");
            }
        }

        private string relevantClassErrorMessage;
        public string RelevantClassErrorMessage
        {
            get => relevantClassErrorMessage;
            set
            {
                relevantClassErrorMessage = value;
                OnPropertyChanged("RelevantClassErrorMessage");
            }
        }

        private bool priceError;
        public bool PriceError
        {
            get => priceError;
            set
            {
                priceError = value;
                OnPropertyChanged("PriceError");
            }
        }

        private string priceErrorMessage;
        public string PriceErrorMessage
        {
            get => priceErrorMessage;
            set
            {
                priceErrorMessage = value;
                OnPropertyChanged("PriceErrorMessage");
            }
        }

        private bool discountPercentError;
        public bool DiscountPercentError
        {
            get => discountPercentError;
            set
            {
                discountPercentError = value;
                OnPropertyChanged("DiscountPercentError");
            }
        }

        private string discountPercentErrorMessage;
        public string DiscountPercentErrorMessage
        {
            get => discountPercentErrorMessage;
            set
            {
                discountPercentErrorMessage = value;
                OnPropertyChanged("DiscountPercentErrorMessage");
            }
        }

        private bool shevetError;
        public bool ShevetError
        {
            get => shevetError;
            set
            {
                shevetError = value;
                OnPropertyChanged("ShevetError");
            }
        }

        private string shevetErrorMessage;
        public string ShevetErrorMessage
        {
            get => shevetErrorMessage;
            set
            {
                shevetErrorMessage = value;
                OnPropertyChanged("ShevetErrorMessage");
            }
        }

        private bool hanhagaError;
        public bool HanhagaError
        {
            get => hanhagaError;
            set
            {
                hanhagaError = value;
                OnPropertyChanged("HanhagaError");
            }
        }

        private string hanhagaErrorMessage;
        public string HanhagaErrorMessage
        {
            get => hanhagaErrorMessage;
            set
            {
                hanhagaErrorMessage = value;
                OnPropertyChanged("HanhagaErrorMessage");
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

            #region רשימות פיקרים
        private List<string> relevantClassOptions;
        public List<string> RelevantClassOptions
        {
            get => relevantClassOptions;
            set
            {
                relevantClassOptions = value;
                OnPropertyChanged("RelevantClassOptions");
            }
        }

        private List<string> isOpenPicker;
        public List<string> IsOpenPicker
        {
            get => isOpenPicker;
            set
            {
                isOpenPicker = value;
                OnPropertyChanged("IsOpenPicker");
            }
        }

        private List<Shevet> shevets;
        public List<Shevet> Shevets
        {
            get => shevets;
            set
            {
                shevets = value;
                OnPropertyChanged("Shevets");
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
        #endregion

        private ActivityToShow editedActivity;
        public ActivityToShow EditedActivity
        {
            get => editedActivity;
            set
            {
                editedActivity = value;
                OnPropertyChanged("EditedActivity");
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

        private async Task SetListsForPickers()
        {
            #region relevant class options
            List<string> relevantClassOptionsEzer = new List<string>();
            relevantClassOptionsEzer.Add("כל השבט");
            relevantClassOptionsEzer.Add("צעירה");
            relevantClassOptionsEzer.Add("בוגרת");
            relevantClassOptionsEzer.Add("שכבג");
            relevantClassOptionsEzer.Add("ד");
            relevantClassOptionsEzer.Add("ה");
            relevantClassOptionsEzer.Add("ו");
            relevantClassOptionsEzer.Add("ז");
            relevantClassOptionsEzer.Add("ח");
            relevantClassOptionsEzer.Add("ט");
            relevantClassOptionsEzer.Add("י");
            relevantClassOptionsEzer.Add("יא");
            relevantClassOptionsEzer.Add("יב");
            relevantClassOptionsEzer.Add("פעילים");
            RelevantClassOptions = relevantClassOptionsEzer;
            RelevantClass = EditedActivity.RelevantClass;
            #endregion
            List<string> isOpenEzer = new List<string>();
            isOpenEzer.Add("כן");
            isOpenEzer.Add("לא");
            IsOpenPicker = isOpenEzer;
            if (IsOpen)
                IsOpenString = "כן";
            else
                IsOpenString = "לא";
            int permissionLevel = await proxy.GetPermissionLevelAsync(HomePage.ConnectedUser.Id);
            switch (permissionLevel)
            {
                case 1: Hanhaga hanhagaHolder = Hanhaga;
                    List<Hanhaga> availableHanhagas = await proxy.GetAllHanhagasAsync();
                    Hanhagas = availableHanhagas;
                    Hanhaga = hanhagaHolder;
                    int hanhagaIdInPicker = 0;
                    foreach (Hanhaga h in availableHanhagas)
                    {
                        if (h.Id != Hanhaga.Id)
                            hanhagaIdInPicker++;
                        else
                            PickerHanhagaId = hanhagaIdInPicker;
                    }
                    PickerHanhaga = Hanhaga;
                    PickerShevet = Shevet;
                    ShowShevet = true;
                    ShowHanhaga = true;
                    await SetShevetsForPicker();
                    break;
                case 2:
                    ShowShevet = true;
                    await SetShevetsAccordingToHanhaga();
                    break;
            }
        }

        private async Task SetShevetsForPicker()
        {
            Shevet shevetHolder = Shevet;
            List<Shevet> shevetsList = await proxy.GetAllShevetsAsync();
            List<Shevet> availableShevets = new List<Shevet>();
            int shevetIdInPicker = 0;
            int correctShevetIdInPicker = 231;
            foreach (Shevet s in shevetsList)
            {
                if (Hanhaga.Id == s.HanhagaId)
                {
                    availableShevets.Add(s);
                    shevetIdInPicker++;
                }
                if (s.Id == shevetHolder.Id)
                    correctShevetIdInPicker = shevetIdInPicker - 1;
            }
            Shevets = availableShevets;
            Shevet = shevetHolder;
            PickerShevetId = correctShevetIdInPicker;
            PickerHanhaga = Hanhaga;
            PickerShevet = Shevet;
        }

        private async Task SetShevetsAccordingToHanhaga()
        {
            ShowShevet = true;
            Shevet shevetHolder = Shevet;
            List<Shevet> shevetsList = await proxy.GetAllShevetsAsync();
            List<Shevet> availableShevets = new List<Shevet>();
            foreach (Shevet s in shevetsList)
            {
                if (Hanhaga.Id == s.HanhagaId)
                {
                    availableShevets.Add(s);
                }
            }
            Shevets = availableShevets;
            PickerShevetId = -1;
            string hanhagaName = "";
            hanhagaName = EditedActivity.Hanhaga;
            if (hanhagaName == Hanhaga.Name)
            {
                Shevet = shevetHolder;
                PickerShevet = Shevet;
            }
        }

        private void SetIsOpenBool(string selected)
        {
            IsOpen = selected == "כן";
        }

        #region בדיקת שדות
        private void CheckName()
        {
            NameError = false;
            if (Name == null || Name == "")
            {
                NameError = true;
                NameErrorMessage = "זהו שדה חובה";
            }
        }

        private void CheckStartDate()
        {
            StartDateError = false;
            if (DateTime.Compare(StartDate, EndDate) > 0)
            {
                StartDateError = true;
                StartDateErrorMessage = "תאריך הסיום חייב להיות אחרי תאריך ההתחלה או באותו יום";
            }
        }

        private void CheckEndDate()
        {
            EndDateError = false;
            if (DateTime.Compare(StartDate, EndDate) > 0)
            {
                EndDateError = true;
                EndDateErrorMessage = "תאריך הסיום חייב להיות אחרי תאריך ההתחלה או באותו יום";
            }
        }

        private void CheckRelevantClass()
        {
            RelevantClassError = false;
            if (RelevantClass == null || RelevantClass == "")
            {
                RelevantClassError = true;
                RelevantClassErrorMessage = "זהו שדה חובה";
            }
        }

        private void CheckPrice()
        {
            priceError = false;
            int location = 0;
            string priceHolder = Price;
            foreach (char c in priceHolder)
            {
                if (char.IsLetter(c))
                    priceHolder = priceHolder.Remove(location, 1);
                location++;
            }
            if (priceHolder != Price)
                Price = priceHolder;
            if (Price == null || Price == "")
            {
                PriceError = true;
                PriceErrorMessage = "זהו שדה חובה";
            }
        }

        private void CheckDiscountPercent()
        {
            discountPercentError = false;
            int location = 0;
            string discountPercentHolder = DiscountPercent;
            foreach (char c in discountPercentHolder)
            {
                if (char.IsLetter(c))
                    discountPercentHolder = discountPercentHolder.Remove(location, 1);
                location++;
            }
            if (discountPercentHolder != DiscountPercent)
                DiscountPercent = discountPercentHolder;
            if (DiscountPercent == null || DiscountPercent == "")
            {
                DiscountPercentError = true;
                DiscountPercentErrorMessage = "זהו שדה חובה";
            }
        }

        private void CheckShevet()
        {
            ShevetError = false;
            if (Shevet == null)
            {
                ShevetError = true;
                ShevetErrorMessage = "זהו שדה חובה";
            }
        }

        private void CheckHanhaga()
        {
            HanhagaError = false;
            if (Hanhaga == null)
            {
                HanhagaError = true;
                HanhagaErrorMessage = "זהו שדה חובה";
            }
        }
        #endregion

        private async void SaveChanges()
        {
            CheckForErrors();
            if (AreThereErrors)
                return;
            ActivityToShow a = EditedActivity;
            a.Name = Name;
            a.StartDate = new Date(StartDate);
            a.EndDate = new Date(EndDate);
            a.RelevantClass = RelevantClass;
            a.Price = int.Parse(Price);
            a.DiscountPercent = int.Parse(DiscountPercent);
            if (IsOpen)
                a.IsOpen = "Green";
            else
                a.IsOpen = "Red";
            int permissionLevel = await proxy.GetPermissionLevelAsync(HomePage.ConnectedUser.Id);
            switch(permissionLevel)
            {
                case 1: a.Shevet = Shevet.Name;
                    a.ShevetID = Shevet.Id;
                    a.Hanhaga = Hanhaga.Name;
                    a.HanhagaID = Hanhaga.Id;
                    break;
                case 2: a.Shevet = Shevet.Name;
                    a.ShevetID = Shevet.Id;
                    a.Hanhaga = await proxy.GetHanhagaAsync(HomePage.ConnectedUser.Id);
                    a.HanhagaID = await proxy.GetHanhagaIDAsync(HomePage.ConnectedUser.Id);
                    break;
                case 3: a.Shevet = await proxy.GetShevetAsync(HomePage.ConnectedUser.Id);
                    a.ShevetID = await proxy.GetShevetIDAsync(HomePage.ConnectedUser.Id);
                    a.Hanhaga = await proxy.GetHanhagaAsync(HomePage.ConnectedUser.Id);
                    a.HanhagaID = await proxy.GetHanhagaIDAsync(HomePage.ConnectedUser.Id);
                    break;
            }
            await proxy.SaveActivityChangesAsync(a);
            ToManageActivities();
        }

        private void CheckForErrors()
        {
            AreThereErrors = NameError || StartDateError || EndDateError || RelevantClassError || PriceError || DiscountPercentError || ShevetError || HanhagaError;
        }

        private async void BackToManageActivities()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }

        private async void ToManageActivities()
        {
            Page p = new Views.ManageActivities();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private async void ToCadetsForActivity()
        {
            Page p = new CadetsForActivity(EditedActivity);
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
