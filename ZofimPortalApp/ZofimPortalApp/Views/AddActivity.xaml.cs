﻿using System;
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
    public partial class AddActivity : ContentPage
    {
        public AddActivity()
        {
            AddActivityVM vm = new AddActivityVM();
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}