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
    public partial class EditPersonalInfo : ContentPage
    {
        public EditPersonalInfo()
        {
            EditPersonalInfoVM vm = new EditPersonalInfoVM();
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}