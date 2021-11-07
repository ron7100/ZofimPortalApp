using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZofimPortalApp.Views;

namespace ZofimPortalApp
{
    public partial class App : Application
    {
        public static bool IsDevEnv
        {
            get
            {
                return true; //change this before release!
            }
        }

        public App()
        {
            InitializeComponent();

            NavigationPage p = new NavigationPage(new HomePage());
            MainPage = p;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
