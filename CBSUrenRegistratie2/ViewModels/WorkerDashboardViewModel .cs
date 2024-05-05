using CBSUrenRegistratie2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSUrenRegistratie2.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CBSUrenRegistratie2.ViewModels
{
    public class WorkerDashboardViewModel
    {
        public ICommand NavigateCommand { get; private set; }

        public WorkerDashboardViewModel()
        {
            NavigateCommand = new Command<string>(NavigateToPage);
        }

        private async void NavigateToPage(string pageName)
        {
            await Shell.Current.GoToAsync($"//{pageName}", true);
        }
    }

}
