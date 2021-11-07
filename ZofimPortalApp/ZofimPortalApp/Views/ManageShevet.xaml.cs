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
    public partial class ManageShevet : ContentPage
    {
        public ManageShevet()
        {
            ManageShevetVM vm = new ManageShevetVM();
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}