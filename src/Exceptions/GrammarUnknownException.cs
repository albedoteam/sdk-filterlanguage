namespace AlbedoTeam.Sdk.FilterLanguage.Exceptions
{
    using Core;

    /// <summary>
    ///     Exception when an unknown grammar is found
    /// </summary>
    internal class GrammarUnknownException : ParseException
    {
        public readonly StringSegment UnexpectedGrammarStringSegment;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GrammarUnknownException" /> class.
        /// </summary>
        /// <param name="unexpectedGrammarStringSegment">The location of the unknown grammer.</param>
        public GrammarUnknownException(StringSegment unexpectedGrammarStringSegment)
            : base(unexpectedGrammarStringSegment, $"Unexpected token '{unexpectedGrammarStringSegment.Value}' found")
        {
            UnexpectedGrammarStringSegment = unexpectedGrammarStringSegment;
        }
    }
}