using CBSUrenRegistratie2.ViewModels;
using CBSUrenRegistratie2.Services;
using Syncfusion.Maui.Picker;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;

namespace CBSUrenRegistratie2.Views
{



    public partial class TimeLoggingPage : ContentPage
    {
        public TimeLoggingPage()
        {
            InitializeComponent();
            DatabaseService databaseService = new DatabaseService();
            DialogService dialogService = new DialogService();
            this.BindingContext = new TimeLoggingViewModel(databaseService, dialogService);
        }
        private void OnDatePickerSelectionChanged(object sender, DatePickerSelectionChangedEventArgs e)
        {
            var oldDate = e.OldValue;
            var newDate = e.NewValue;
        }
        private void OnMonthSelected(object sender, DateChangedEventArgs e)
        {
            var viewModel = this.BindingContext as TimeLoggingViewModel;
            if (viewModel != null)
            {
                viewModel.PopulateMonthDays(e.NewDate);
            }
        }
        private void OnShowDatePickerClicked(object sender, EventArgs e)
        {
        }

        private void TimePicker_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TimePicker.Time))
            {
                var viewModel = this.BindingContext as TimeLoggingViewModel;
                if (viewModel != null)
                {
                    viewModel.HasUnsavedChanges = true; // Ensure there's a public setter for HasUnsavedChanges testing again and again
                }
            }
        }
    }
}