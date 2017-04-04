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
using CEI.ViewModels;
using CEI.IOC;

namespace CEI.Droid.Pages
{
    public class BrowsePage : BasePage
    {
        private BrowseViewModel viewModel;
        public override PageType GetPageType()
        {
            return PageType.Browse;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.browsePage, container, false);
            viewModel = Locator.GetNewInstance<BrowseViewModel>();
            return view;
        }
    }
}