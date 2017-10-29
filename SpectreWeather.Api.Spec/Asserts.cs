namespace SpectreWeather.Api.Spec
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PublicModel;

    internal static class Asserts
    {
        public static void MultipleForecastsReturned(ICurrentConditions[] expectedCurrentConditionses, ICurrentConditions[] currentConditions)
        {
            CollectionAssert.AreEquivalent(expectedCurrentConditionses, currentConditions);            
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