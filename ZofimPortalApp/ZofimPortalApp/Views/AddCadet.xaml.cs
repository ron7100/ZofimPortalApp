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
    public partial class AddCadet : ContentPage
    {
        public AddCadet(ParentToShow p)
        {
            AddCadetVM vm = new AddCadetVM(p);
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}