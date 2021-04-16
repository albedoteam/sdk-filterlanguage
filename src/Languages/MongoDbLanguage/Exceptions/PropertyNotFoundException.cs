namespace AlbedoTeam.Sdk.FilterLanguage.Languages.MongoDbLanguage.Exceptions
{
    using System;

    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException(string message) : base(message)
        {
        }
    }
}