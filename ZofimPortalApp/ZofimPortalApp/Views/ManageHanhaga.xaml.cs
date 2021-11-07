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
    public partial class ManageHanhaga : ContentPage
    {
        public ManageHanhaga()
        {
            ManageHanhagaVM vm = new ManageHanhagaVM();
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}