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
    public partial class EditShevetInfo : ContentPage
    {
        public EditShevetInfo(ShevetToShow shevet)
        {
            EditShevetInfoVM vm = new EditShevetInfoVM(shevet);
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}