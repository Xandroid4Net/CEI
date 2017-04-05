using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CEI.Services.Navigation;
using CEI.Droid.Pages;

namespace CEI.Droid.Services
{
    public class NavigationService : AbstractNavigationService
    {
        public override void Exit()
        {
            throw new NotImplementedException();
        }

        public override IPage GetPage(PageType type)
        {
            switch (type)
            {
                default:
                case PageType.Browse:
                    return new BrowsePage();
                case PageType.Detail:
                    return new DetailPage();
            }
        }
    }
}