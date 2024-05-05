using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSUrenRegistratie2
{
    public interface IUserInteraction
    {
        Task<bool> DisplayConfirmation(string title, string message, string accept, string cancel);
    }
}
