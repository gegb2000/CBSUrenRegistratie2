using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSUrenRegistratie2.Services
{
    public interface IDialogService
    {
        Task<bool> ShowConfirmationDialogAsync(string title, string message, string accept, string cancel);
    }
}
