﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using CBSUrenRegistratie2.Services;
using CBSUrenRegistratie2.Models;
using System.Windows.Input;

namespace CBSUrenRegistratie2.ViewModels
{
    public class TimeLoggingViewModel : BindableObject
    {
        private string _selectedMonth;
        private bool _hasUnsavedChanges;
        private readonly IDialogService _dialogService;
        private readonly DatabaseService _databaseService;

        public ObservableCollection<TimeLogEntry> DaysInMonth { get; set; } = new ObservableCollection<TimeLogEntry>();
        public ObservableCollection<string> Months { get; private set; }
        public ICommand AddBreakCommand { get; private set; }
        public ICommand RemoveBreakCommand { get; private set; }
        public ICommand SaveMonthCommand { get; private set; }
        public ICommand SubmitMonthCommand { get; private set; }

        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value)
                {
                    if (_hasUnsavedChanges)
                    {
                        _dialogService.ShowConfirmationDialogAsync("Unsaved Changes",
                            "You have unsaved changes. Are you sure you want to discard them?",
                            "Yes", "No")
                        .ContinueWith(task =>
                        {
                            if (task.Result)
                            {
                                UpdateMonth(value);
                            }
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                    else
                    {
                        UpdateMonth(value);
                    }
                }
            }
        }

        public TimeLoggingViewModel(DatabaseService databaseService)
        {
            //_dialogService = dialogService; i commented this out for testing purposes
            _databaseService = databaseService;
            AddBreakCommand = new Command<TimeLogEntry>(AddBreak);
            RemoveBreakCommand = new Command<BreakEntry>(RemoveBreak);
            SaveMonthCommand = new Command(SaveMonth, () => _hasUnsavedChanges);
            SubmitMonthCommand = new Command(SubmitMonth);
            InitializeMonths();
            SelectedMonth = Months.LastOrDefault();
            PopulateMonthDays(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
        }

        private void UpdateMonth(string newMonth)
        {
            _selectedMonth = newMonth;
            OnPropertyChanged(nameof(SelectedMonth));
            PopulateMonthDays(new DateTime(DateTime.Today.Year, Months.IndexOf(_selectedMonth) + 1, 1));
            _hasUnsavedChanges = false;
        }

        private void AddBreak(TimeLogEntry day)
        {
            if (day != null)
            {
                day.Breaks.Add(new BreakEntry { StartBreakTime = TimeSpan.FromHours(9), EndBreakTime = TimeSpan.FromHours(10) });
                _hasUnsavedChanges = true;
            }
        }

        private void RemoveBreak(BreakEntry breakEntry)
        {
            var day = DaysInMonth.FirstOrDefault(d => d.Breaks.Contains(breakEntry));
            if (day != null)
            {
                day.Breaks.Remove(breakEntry);
                _hasUnsavedChanges = true;
            }
        }

        private void SaveMonth()
        {
            // Implement saving logic here
            _hasUnsavedChanges = false;
        }

        private void SubmitMonth()
        {
            // Implement submit logic here test github
            _hasUnsavedChanges = true;
        }

        private void InitializeMonths()
        {
            Months = new ObservableCollection<string>();
            int currentMonth = DateTime.Today.Month;
            for (int i = 1; i <= currentMonth; i++)
            {
                Months.Add(new DateTime(DateTime.Today.Year, i, 1).ToString("MMMM"));
            }
        }

        public void PopulateMonthDays(DateTime month)
        {
            DaysInMonth.Clear();
            int days = DateTime.DaysInMonth(month.Year, month.Month);
            for (int i = 1; i <= days; i++)
            {
                DaysInMonth.Add(new TimeLogEntry(new DateTime(month.Year, month.Month, i)));
            }
        }
    }
}
