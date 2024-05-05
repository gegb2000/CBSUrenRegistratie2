using CBSUrenRegistratie2.ViewModels;
using CBSUrenRegistratie2.Services;
using Syncfusion.Maui.Picker;

namespace CBSUrenRegistratie2.Views
{



    public partial class TimeLoggingPage : ContentPage
    {
        public TimeLoggingPage()
        {
            InitializeComponent();
            DatabaseService databaseService = new DatabaseService();
            this.BindingContext = new TimeLoggingViewModel(databaseService);
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
    }
}