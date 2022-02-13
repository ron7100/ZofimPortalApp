using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZofimPortalApp.ViewModels;
using ZofimPortalApp.Models;

namespace ZofimPortalApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public static User ConnectedUser;
        public HomePage()
        {
            HomePageVM vm = new HomePageVM(ConnectedUser);
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}