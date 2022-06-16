using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZofimPortalApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZofimPortalApp.Models;

namespace ZofimPortalApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadetsForActivity : ContentPage
    {
        public CadetsForActivity(ActivityToShow a)
        {
            CadetsForActivityVM vm = new CadetsForActivityVM(a);
            this.BindingContext = vm;
            InitializeComponent();
        }

        public CadetsForActivity(CadetToShow c)
        {
            CadetsForActivityVM vm = new CadetsForActivityVM(c);
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}