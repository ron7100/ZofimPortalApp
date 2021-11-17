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
    public partial class LogIn : ContentPage
    {
        public LogIn()
        {
            LogInVM vm = new LogInVM();
            vm.OnHidePassword += ImageButton_Pressed;
            this.BindingContext = vm;
            InitializeComponent();
        }

        private void ImageButton_Pressed()
        {
            pass.IsPassword = !pass.IsPassword;
        }

        private void ImageButtonEvent_Pressed(object sender, EventArgs e)
        {
            ImageButton_Pressed();
        }
    }
}