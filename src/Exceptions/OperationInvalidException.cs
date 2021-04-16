namespace AlbedoTeam.Sdk.FilterLanguage.Exceptions
{
    using System;
    using Core;

    /// <summary>
    ///     Exception thrown when there is a generic problem in the processing of Expressions.
    ///     Usually caused by grammar definition settings
    /// </summary>
    internal class OperationInvalidException : ParseException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OperationInvalidException" /> class.
        /// </summary>
        /// <param name="errorSegment">The location that caused the exception</param>
        /// <param name="innerException">The inner exception</param>
        public OperationInvalidException(StringSegment errorSegment, Exception innerException)
            : base(errorSegment, $"Unable to perform operation '{errorSegment}'", innerException)
        {
        }
    }
}