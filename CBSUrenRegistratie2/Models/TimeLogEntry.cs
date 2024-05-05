using System;
using System.Collections.ObjectModel;

namespace CBSUrenRegistratie2.Models
{
    public class TimeLogEntry
    {
        public event Action OnChange; // Event to notify the ViewModel of changes

        public int HoursId { get; set; }
        public int WorkerId { get; set; }
        public int ProjectId { get; set; }
        public DateTime DateWorked { get; set; }

        private TimeSpan _startTime;
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnChange?.Invoke(); // Trigger the event when value changes
                }
            }
        }

        private TimeSpan _endTime;
        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    OnChange?.Invoke(); // Trigger the event when value changes
                }
            }
        }

        public ObservableCollection<BreakEntry> Breaks { get; set; } = new ObservableCollection<BreakEntry>();

        public TimeLogEntry(DateTime date)
        {
            DateWorked = date;
            StartTime = new TimeSpan(8, 0, 0); // Default start time
            EndTime = new TimeSpan(17, 0, 0); // Default end time
            Breaks = new ObservableCollection<BreakEntry>();
        }
    }
}
