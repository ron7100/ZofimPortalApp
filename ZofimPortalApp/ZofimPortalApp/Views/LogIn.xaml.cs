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
            vm.OnHidePassword += pass_TextChanged;
            this.BindingContext = vm;
            InitializeComponent();
        }

        private void pass_TextChanged()
        {
            pass.IsPassword = !pass.IsPassword;
        }
    }
}