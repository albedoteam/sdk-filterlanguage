namespace AlbedoTeam.Sdk.FilterLanguage.Exceptions
{
    using Core;

    /// <summary>
    ///     Exception when a function does not have the correct number of operands
    /// </summary>
    internal class FunctionArgumentCountException : ParseException
    {
        public readonly int ActualOperandCount;
        public readonly StringSegment BracketStringSegment;
        public readonly int ExpectedOperandCount;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FunctionArgumentCountException" /> class.
        /// </summary>
        /// <param name="bracketStringSegment">The location of the function arguments</param>
        /// <param name="expectedOperandCount">The Expected number of operands</param>
        /// <param name="actualOperandCount">The actual number of operands</param>
        public FunctionArgumentCountException(
            StringSegment bracketStringSegment,
            int expectedOperandCount,
            int actualOperandCount)
            : base(
                bracketStringSegment,
                $"Bracket '{bracketStringSegment.Value}' contains {actualOperandCount} " +
                $"operand{(actualOperandCount > 1 ? "s" : "")} but was expecting {expectedOperandCount} " +
                $"operand{(expectedOperandCount > 1 ? "s" : "")}")
        {
            BracketStringSegment = bracketStringSegment;
            ExpectedOperandCount = expectedOperandCount;
            ActualOperandCount = actualOperandCount;
        }
    }
}