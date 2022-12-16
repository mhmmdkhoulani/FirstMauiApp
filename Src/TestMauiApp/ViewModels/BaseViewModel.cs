using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestMauiApp.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {

        public BaseViewModel()
        {
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        public bool IsNotBusy => !IsBusy;
        //public bool IsBusy
        //{
        //    get => IsBusy;
        //    set
        //    {
        //        if (isBusy == value)
        //            return;
        //        isBusy = value;
        //        OnPropertyChanged();
        //        OnPropertyChanged(nameof(IsNotBusy));
        //    }
        //}

        //public string Title
        //{
        //    get => title;
        //    set
        //    {
        //        if (title == value)
        //            return;
        //        title = value;
        //        OnPropertyChanged();
        //    }
        //}

       

        //public event PropertyChangedEventHandler PropertyChanged;

        //public void OnPropertyChanged([CallerMemberName] string name = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //}
    }
}
