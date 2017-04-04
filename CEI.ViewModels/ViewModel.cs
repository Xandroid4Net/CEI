using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CEI.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        private bool _Busy = false;
        public bool Busy
        {
            get
            {
                return _Busy;
            }
            set
            {
                if (_Busy != value)
                {
                    _Busy = value;
                    RaisePropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected PropertyChangedEventHandler PropertyChangedHandler
        {
            get
            {
                return PropertyChanged;
            }
        }

        public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool CanExecuteCommand()
        {
            if (Busy) return false;
            return true;
        }
    }
}
