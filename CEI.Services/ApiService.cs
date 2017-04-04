﻿using CEI.IOC.Bus;
using CEI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CEI.Services
{
    public interface IApiService
    {
        Configuration Config { get; }
        Task<bool> Initilaize(string key = "ab41356b33d100ec61e6c098ecc92140", string apiUrl = "http://api.themoviedb.org/3/");
        Task<NowPlayingResponse> GetNowPlaying(int page, string region = null, string language = "en-US", string sortBy = "popularity.des");
        Task<MovieResponse> GetPopular(int page, string region = "US", string language = "en-US", string sortBy = "popularity.des");
        Task<MovieResponse> GetTopRated(int page, string region = "US", string language = "en-US", string sortBy = "popularity.des");
        Task<MovieResponse> GetSimilar(int page, int movieId, string language = "en-US");
        Task<VideoResponse> GetVideos(int movieId, string language = "en-US");

    }
    public class ApiService : IApiService
    {
        private string apiKey;
        private string baseUrl;
        public Configuration Config { get; private set; }
        const string NowPlayingEndpoint = "http://api.themoviedb.org/3/movie/now_playing?api_key=ab41356b33d100ec61e6c098ecc92140&sort_by=popularity.des";

        private string GetMoviesEndpoint(string endpoint, int page, string region, string language, string sortBy)
        {
            return string.Format("{0}movie/{1}?api_key={2}&region={3}&language={4}&sort_by={5}&page={6}", baseUrl, endpoint, apiKey, region, language, sortBy, page);
        }

        private string GetMovieEndpoint(string endpoint, int id, string language, int page)
        {
            return string.Format("{0}movie/{1}/{2}?api_key={3}&language={4}&page={5}", baseUrl, id, endpoint, apiKey, language, page);
        }

        private string GetVideoEndpoint(int id, string language)
        {
            return string.Format("{0}movie/{1}/videos?api_key={2}&language={3}", baseUrl, id, apiKey, language);
        }

        private async Task<string> GetMoviesJson(string endPoint, int page, string region, string language, string sortBy)
        {
            var path = GetMoviesEndpoint(endPoint, page, region, language, sortBy);
            var json = await Get(path);
            return json;
        }

        private async Task<string> GetMovieJson(string endPoint, int page, string language, int id)
        {
            var path = GetMovieEndpoint(endPoint, id, language, page);
            var json = await Get(path);
            return json;
        }

        private async Task<string> GetVideoJson(int id, string language)
        {
            var path = GetVideoEndpoint(id, language);
            var json = await Get(path);
            return json;
        }

        public async Task<NowPlayingResponse> GetNowPlaying(int page, string region = "US", string language = "en-US", string sortBy = "popularity.des")
        {
            try
            {
                var json = await GetMoviesJson("now_playing", page, region, language, sortBy);
                return JsonConvert.DeserializeObject<NowPlayingResponse>(json);
            }
            catch (Exception ex)
            {
                EventBus.PublishError(ex);
            }
            return null;
        }
        /// <summary>
        /// Returns unassociated movie results.
        /// </summary>
        private async Task<MovieResponse> GetMoviesResults(string endPoint, int page, string region, string language, string sortBy)
        {
            var json = await GetMoviesJson(endPoint, page, region, language, sortBy);
            return JsonConvert.DeserializeObject<MovieResponse>(json);
        }

        /// <summary>
        /// Returns results associated with the movie id
        /// </summary>
        private async Task<MovieResponse> GetMovieResults(string endPoint, int page, int id, string language)
        {
            var json = await GetMovieJson(endPoint, id, language, page);
            return JsonConvert.DeserializeObject<MovieResponse>(json);
        }

        private async Task<MovieResponse> GetSimilarResults(int page, int id, string language)
        {
            var json = await GetMovieJson("similar", page, language, id);
            return JsonConvert.DeserializeObject<MovieResponse>(json);
        }

        private async Task<VideoResponse> GetVideoResults(int id, string language)
        {
            var json = await GetVideoJson(id, language);
            return JsonConvert.DeserializeObject<VideoResponse>(json);
        }

        public async Task<MovieResponse> GetPopular(int page, string region = "US", string language = "en-US", string sortBy = "popularity.des")
        {
            try
            {
                return await GetMoviesResults("popular", page, region, language, sortBy);
            }
            catch (Exception ex)
            {
                EventBus.PublishError(ex);
            }
            return null;
        }

        public async Task<MovieResponse> GetTopRated(int page, string region = "US", string language = "en-US", string sortBy = "popularity.des")
        {
            try
            {
                return await GetMoviesResults("top_rated", page, region, language, sortBy);
            }
            catch (Exception ex)
            {
                EventBus.PublishError(ex);
            }
            return null;
        }

        public async Task<MovieResponse> GetSimilar(int page, int movieId, string language = "en-US")
        {
            try
            {
                return await GetSimilarResults(page, movieId, language);
            }
            catch (Exception ex)
            {
                EventBus.PublishError(ex);
            }
            return null;
        }

        public async Task<VideoResponse> GetVideos(int movieId, string language = "en-US")
        {
            try
            {
                return await GetVideoResults(movieId, language);
            }
            catch (Exception ex)
            {
                EventBus.PublishError(ex);
            }
            return null;
        }

        private async Task<string> Get(string path)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
            request.ContentType = "application/json";
            request.Method = "GET";

            return await CompleteRequest(request);
        }

        private async Task<string> Post(string path, string jsonData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
            request.ContentType = "application/json";
            request.Method = "POST";

            var requestStream = await request.GetRequestStreamAsync();
            using (StreamWriter sw = new StreamWriter(requestStream))
            {
                await sw.WriteAsync(jsonData);
                await sw.FlushAsync();
            }

            return await CompleteRequest(request);

        }

        private async Task<string> CompleteRequest(HttpWebRequest request)
        {
            var response = await request.GetResponseAsync();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                return await sr.ReadToEndAsync();
            }
        }

        public async Task<bool> Initilaize(string key = "ab41356b33d100ec61e6c098ecc92140", string apiUrl = "http://api.themoviedb.org/3/")
        {
            apiKey = key;
            baseUrl = apiUrl;
            try
            {
                var url = string.Format("{0}configuration?api_key={1}", baseUrl, key);
                var json = await Get(url);
                Config = JsonConvert.DeserializeObject<Configuration>(json);
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
