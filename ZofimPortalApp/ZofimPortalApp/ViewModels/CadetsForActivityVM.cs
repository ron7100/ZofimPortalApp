using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZofimPortalApp.Models;

namespace ZofimPortalApp.ViewModels
{
    class CadetsForActivityVM : INotifyPropertyChanged
    {
        public CadetsForActivityVM(ActivityToShow a)
        {

        }

        public CadetsForActivityVM(CadetToShow c)
        {

        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
