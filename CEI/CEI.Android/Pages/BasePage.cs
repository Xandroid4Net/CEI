using Android.App;
using CEI.Services.Navigation;

namespace CEI.Droid.Pages
{
    public abstract class BasePage : Fragment, IPage
    {
        protected ProgressDialog progress;

        protected void InitProgressDialog()
        {
            progress = new ProgressDialog(Activity);
        }

        public abstract PageType GetPageType();

        public T As<T>()
        {
            return (T)((object)this);
        }
    }
}