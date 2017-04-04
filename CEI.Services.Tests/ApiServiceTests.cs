using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace CEI.Services.Tests
{
    //Quick test before moving on to UI. Some assumptions may be bad.
    [TestClass]
    public class ApiServiceTests
    {
        IApiService testSubject = new ApiService();

        [TestMethod]
        public void InitTest()
        {
            Task.Run(async () =>
            {
                await testSubject.Initilaize();
                Assert.IsTrue(testSubject.Config != null);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void GetNowPlayingTest()
        {
            Task.Run(async () =>
            {
                await testSubject.Initilaize();
                var results = await testSubject.GetNowPlaying(1);
                Assert.IsTrue(results != null);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void GetPopularTest()
        {
            Task.Run(async () =>
            {
                await testSubject.Initilaize();
                var results = await testSubject.GetPopular(1);
                Assert.IsTrue(results != null && results.Page == 1);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void GetTopRatedTest()
        {
            Task.Run(async () =>
            {
                await testSubject.Initilaize();
                var results = await testSubject.GetTopRated(1);
                Assert.IsTrue(results != null && results.Page == 1);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void GetSimilarTest()
        {
            Task.Run(async () =>
            {
                await testSubject.Initilaize();
                var results = await testSubject.GetSimilar(1, 293660);
                Assert.IsTrue(results != null && results.Page == 1);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void GetVideoTest()
        {
            Task.Run(async () =>
            {
                await testSubject.Initilaize();
                var results = await testSubject.GetVideos(293660);
                Assert.IsTrue(results != null && results.Id == 293660);
            }).GetAwaiter().GetResult();

        }

    }
}
