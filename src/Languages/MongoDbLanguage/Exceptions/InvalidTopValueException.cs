namespace AlbedoTeam.Sdk.FilterLanguage.Languages.MongoDbLanguage.Exceptions
{
    using System;

    public class InvalidTopValueException : Exception
    {
        public InvalidTopValueException(string message) : base(message)
        {
        }
    }
}