using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.Services
{
    public interface IUIDispatcher
    {
        void RunOnUiThread(Action action);
    }
}
