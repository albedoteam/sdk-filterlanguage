namespace AlbedoTeam.Sdk.FilterLanguage.Exceptions
{
    using System;
    using Core;

    /// <summary>
    ///     A base class for all managed exceptions when parsing a string, a catch all can be used within a try / catch
    /// </summary>
    internal abstract class ParseException : Exception
    {
        /// <summary>
        ///     The location that caused the exception
        /// </summary>
        public readonly StringSegment ErrorSegment;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParseException" /> class.
        /// </summary>
        /// <param name="errorSegment">The location that caused the exception</param>
        /// <param name="message">A message describing the exception</param>
        public ParseException(StringSegment errorSegment, string message)
            : base(message)
        {
            ErrorSegment = errorSegment;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParseException" /> class.
        /// </summary>
        /// <param name="errorSegment">The location that caused the exception</param>
        /// <param name="message">A message describing the exception</param>
        /// <param name="innerException">The exception that caused this exception</param>
        public ParseException(StringSegment errorSegment, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorSegment = errorSegment;
        }
    }
}