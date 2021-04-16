namespace AlbedoTeam.Sdk.FilterLanguage.Languages.MongoDbLanguage.Exceptions
{
    using System;

    public class InvalidSkipValueException : Exception
    {
        public InvalidSkipValueException(string message) : base(message)
        {
        }
    }
}