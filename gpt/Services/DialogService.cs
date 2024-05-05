using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSUrenRegistratie2.Services
{
    public class DialogService : IDialogService
    {
        public async Task<bool> ShowConfirmationDialogAsync(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
