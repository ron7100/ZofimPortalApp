using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZofimPortalApp.ViewModels;

namespace ZofimPortalApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageUsers : ContentPage
    {
        public ManageUsers()
        {
            ManageUsersVM vm = new ManageUsersVM();
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}