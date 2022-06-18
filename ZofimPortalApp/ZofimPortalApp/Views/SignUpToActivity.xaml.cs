using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZofimPortalApp.Models;
using ZofimPortalApp.ViewModels;

namespace ZofimPortalApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpToActivity : ContentPage
    {
        public SignUpToActivity(List<ActivitiesHistory> toSignUp)
        {
            SignUpToActivityVM vm = new SignUpToActivityVM(toSignUp);
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}