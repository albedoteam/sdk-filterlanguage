namespace AlbedoTeam.Sdk.FilterLanguage.Exceptions
{
    using Core;

    /// <summary>
    ///     Exception when an operand was expected but not found
    /// </summary>
    internal class OperandExpectedException : ParseException
    {
        public readonly StringSegment ExpectedOperandStringSegment;
        public readonly StringSegment OperatorStringSegment;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OperandExpectedException" /> class.
        /// </summary>
        /// <param name="expectedOperandStringSegment">The location where the operand was expected to be.</param>
        public OperandExpectedException(StringSegment expectedOperandStringSegment)
            : base(expectedOperandStringSegment, "Expected operands to be found")
        {
            OperatorStringSegment = null;
            ExpectedOperandStringSegment = expectedOperandStringSegment;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OperandExpectedException" /> class.
        /// </summary>
        /// <param name="operatorStringSegment">The operator that was expecting the operand.</param>
        /// <param name="expectedOperandStringSegment">The location where the operand was expected to be.</param>
        public OperandExpectedException(StringSegment operatorStringSegment, StringSegment expectedOperandStringSegment)
            : base(expectedOperandStringSegment, $"Expected operands to be found for '{operatorStringSegment.Value}'")
        {
            OperatorStringSegment = operatorStringSegment;
            ExpectedOperandStringSegment = expectedOperandStringSegment;
        }
    }
}