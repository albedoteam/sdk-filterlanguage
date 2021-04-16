namespace AlbedoTeam.Sdk.FilterLanguage.Exceptions
{
    using System;
    using Core;

    internal class FunctionArgumentTypeException : ParseException
    {
        public readonly Type ActualType;
        public readonly StringSegment ArgumentStringSegment;
        public readonly Type ExpectedType;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FunctionArgumentTypeException" /> class.
        /// </summary>
        /// <param name="argumentStringSegment">The location of the argument.</param>
        /// <param name="expectedType">The expected type of the argument.</param>
        /// <param name="actualType">The actual type of the argument.</param>
        public FunctionArgumentTypeException(
            StringSegment argumentStringSegment,
            Type expectedType,
            Type actualType)
            : base(
                argumentStringSegment,
                $"Argument '{argumentStringSegment.Value}' type expected {expectedType} but was {actualType}")
        {
            ArgumentStringSegment = argumentStringSegment;
            ExpectedType = expectedType;
            ActualType = actualType;
        }
    }
}