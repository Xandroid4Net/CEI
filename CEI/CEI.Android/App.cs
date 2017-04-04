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
using CEI.PortableUI;
using CEI.Services.Navigation;
using CEI.Droid.Services;
using CEI.IOC;

namespace CEI.Droid
{
    [Application(HardwareAccelerated = true)]
    public class App : Application
    {
        public App(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            new UIApplication().Initialize(() =>
            {
                Locator.Register<INavigationService>(new NavigationService());
                //Locator.Register<IUIDispatcher>(new UIDispatcher());
            });

        }
    }
}