using Android.Graphics;
using Android.Widget;
using CEI.IOC.Bus;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CEI.Droid.Services
{
    public interface IImageService
    {
        Bitmap LoadImage(string url);
    }
    public class ImageService : IImageService
    {
        //TODO: Add local cache of the images so the app doesn't have to fetch them every time.
        public Bitmap LoadImage(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                var response = request.GetResponseAsync().Result;
                using (var stream = response.GetResponseStream())
                {
                    return BitmapFactory.DecodeStream(stream);
                }
            }
            catch (Exception ex)
            {
                EventBus.PublishError(ex);
            }
            return null;
        }
    }
}