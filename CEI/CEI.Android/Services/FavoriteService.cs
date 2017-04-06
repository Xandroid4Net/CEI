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
using CEI.Models;
using CEI.Services;
using Android.Preferences;

namespace CEI.Droid.Services
{
    public class FavoriteService : AbstractFavoriteService
    {
        private readonly Application context;
        const string PREF_KEY = "Favorites";
        public FavoriteService(Application app)
        {
            context = app;
        }

        public override void AddToFavorites(Item item)
        {
            var favorites = GetFavorites();
            if (favorites.Contains(item.Id)) return;

            favorites.Add(item.Id);
            UpdateFavorites(favorites);
            //base.AddToFavorites(item);
        }

        private void UpdateFavorites(List<int> favorites)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(PREF_KEY, ListOfFavoritesToString(favorites));
            editor.Apply();
        }

        public override List<int> GetFavorites()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            var json = prefs.GetString(PREF_KEY, "");
            return JsonToListOfFavorites(json);
        }

        public override bool IsFavorite(Item item)
        {
            return GetFavorites().Contains(item.Id);
        }

        public override void RemoveFromFavorites(Item item)
        {
            var favorites = GetFavorites();
            if (!favorites.Contains(item.Id)) return;

            favorites.Remove(item.Id);
            UpdateFavorites(favorites);
            //base.RemoveFromFavorites(item);
        }
    }
}