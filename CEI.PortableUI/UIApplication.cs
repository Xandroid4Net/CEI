using CEI.IOC;
using CEI.ViewModels;
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
            Locator.Register<BrowseViewModel>(false);
        }

        public void Initialize(Action createDependentServices)
        {
            createDependentServices.Invoke();
        }
    }
}
