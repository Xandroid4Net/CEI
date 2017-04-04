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
        Task<bool> LoadImage(string url, ImageView iv);
    }
    public class ImageService : IImageService
    {
        //TODO: A simple optimization would be a local cache of the images so the app doesn't have to fetch them every time.
        public async Task<bool> LoadImage(string url, ImageView iv)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                var response = await request.GetResponseAsync();
                using (var stream = response.GetResponseStream())
                {
                    using (Bitmap b = BitmapFactory.DecodeStream(stream))
                    {
                        iv.SetImageBitmap(b);
                        b.Dispose();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                EventBus.PublishError(ex);
            }
            return false;
        }
    }
}