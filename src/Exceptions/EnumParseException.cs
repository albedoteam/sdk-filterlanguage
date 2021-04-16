namespace AlbedoTeam.Sdk.FilterLanguage.Exceptions
{
    using System;

    /// <summary>
    ///     Exception when a string cannot be parsed as an enumeration
    /// </summary>
    public class EnumParseException : Exception
    {
        public readonly Type EnumType;
        public readonly string StringValue;

        public EnumParseException(string stringValue, Type enumType, Exception ex)
            : base($"'{stringValue}' is not a valid value for enum type '{enumType}'", ex)
        {
            StringValue = stringValue;
            EnumType = enumType;
        }
    }
}