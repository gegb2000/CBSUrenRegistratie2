using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CBSUrenRegistratie2.Models
{

    public class BreakEntry : INotifyPropertyChanged
    {
        private int _breakId;
        private TimeSpan _startBreakTime;
        private TimeSpan _endBreakTime;

        public int BreakId
        {
            get => _breakId;
            set => SetProperty(ref _breakId, value);
        }

        public TimeSpan StartBreakTime
        {
            get => _startBreakTime;
            set => SetProperty(ref _startBreakTime, value, onChanged: () => OnPropertyChanged(nameof(StartBreakTime)));
        }

        public TimeSpan EndBreakTime
        {
            get => _endBreakTime;
            set => SetProperty(ref _endBreakTime, value, onChanged: () => OnPropertyChanged(nameof(EndBreakTime)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, Action onChanged = null, [CallerMemberName] String propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }

}
