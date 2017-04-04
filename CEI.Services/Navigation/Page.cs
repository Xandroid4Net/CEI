using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.Services.Navigation
{
    public enum PageType
    {
        Browse = 0, Detail = 1
    }
    public interface IPage
    {
        PageType GetPageType();
        T As<T>();
    }
}
