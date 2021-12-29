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
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            SignUpVM vm = new SignUpVM();
            vm.OnHidePassword += ImageButton_Pressed;
            vm.OnHidePassword += ImageButton_Pressed2;
            this.BindingContext = vm;
            InitializeComponent();
        }

        private void ImageButton_Pressed()
        {
            pass.IsPassword = !pass.IsPassword;
        }

        private void ImageButton_Pressed2()
        {
            checkPass.IsPassword = !checkPass.IsPassword;
        }

        private void ImageButtonEvent_Pressed(object sender, EventArgs e)
        {
            ImageButton_Pressed();
        }

        private void ImageButtonEvent_Pressed2(object sender, EventArgs e)
        {
            ImageButton_Pressed2();
        }
    }
}