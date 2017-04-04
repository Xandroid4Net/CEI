using CEI.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.PortableUI
{
    public class UIApplication
    {
        public UIApplication()
        {
        }

        public void Initialize(Action createDependentServices)
        {
            createDependentServices.Invoke();
        }
    }
}
