using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSUrenRegistratie2.Services;
using CBSUrenRegistratie2.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Syncfusion.Maui.NavigationDrawer;


namespace CBSUrenRegistratie2.ViewModels
{
    public class AdminDashboardViewModel : ObservableObject
    {
        public ICommand NavigateCommand { get; }
        public ICommand OpenDrawerCommand { get; }
        public AdminDashboardViewModel()
        {
            NavigateCommand = new Command<string>(NavigateToPage);
            OpenDrawerCommand = new Command(OnOpenDrawer);
        }
        private async void NavigateToPage(string pageName)
        {
            await Shell.Current.GoToAsync($"//{pageName}", true);
        }
        private void OnOpenDrawer()
        {
            // This command will do nothing by itself, it's here to be bound to a button that toggles the navigation drawer in the code-behind.
            // We raise an event or handle this in the code-behind.
        }
    }
}
