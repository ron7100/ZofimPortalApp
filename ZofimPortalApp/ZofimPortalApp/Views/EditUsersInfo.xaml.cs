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
    public partial class EditUsersInfo : ContentPage
    {
        public EditUsersInfo(object u)
        {
            EditUsersInfoVM vm = new EditUsersInfoVM(u);
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}