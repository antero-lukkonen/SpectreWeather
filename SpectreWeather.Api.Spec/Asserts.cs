namespace SpectreWeather.Api.Spec
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PublicModel;

    internal static class Asserts
    {
        public static void AllForecastsReturned(ICurrentConditions[] expectedCurrentConditions, ICurrentConditions[] currentConditions)
        {
            Assert.IsTrue(expectedCurrentConditions.All(expectedCurrentConditions.Contains)); 
        }

        public static void Exception<T>(Action action, Action<T> assert) where T : Exception
        {
            try
            {
                action();
            }
            catch (T e)
            {
                assert(e);
                return;
            }
            Assert.Fail($"Exception of type {typeof(T)} expected.");
        }
    }
}