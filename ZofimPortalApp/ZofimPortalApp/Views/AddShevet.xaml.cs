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
    public partial class AddShevet : ContentPage
    {
        public AddShevet(Hanhaga h)
        {
            AddShevetVM vm = new AddShevetVM(h);
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}