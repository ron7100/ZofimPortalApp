using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZofimPortalApp.ViewModels;
using ZofimPortalApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZofimPortalApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditHanhagaInfo : ContentPage
    {
        public EditHanhagaInfo(Hanhaga hanhaga)
        {
            EditHanhagaInfoVM vm = new EditHanhagaInfoVM(hanhaga);
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}