using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AutoKadr.mvvm.viewmodel
{
        public class BaseVM : INotifyPropertyChanged
        {
            protected void Signal([CallerMemberName]string prop = null) 
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));  
            }
            public event PropertyChangedEventHandler? PropertyChanged;
        }
}