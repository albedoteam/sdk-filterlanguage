namespace AlbedoTeam.Sdk.FilterLanguage.Definitions
{
    using Core;
    using Exceptions;

    /// <summary>
    ///     Represents the grammar that separates items in a list
    /// </summary>
    /// <seealso cref="GrammarDefinition" />
    internal class ListDelimiterDefinition : GrammarDefinition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ListDelimiterDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the definition</param>
        /// <param name="regex">The regex to match tokens</param>
        public ListDelimiterDefinition(string name, string regex)
            : base(name, regex)
        {
        }

        /// <summary>
        ///     Applies the token to the parsing state.
        ///     Adds an error operator, a close bracket is expected to consume the error operator before being executed
        /// </summary>
        /// <param name="token">The token to apply</param>
        /// <param name="state">The state to apply the token to</param>
        public override void Apply(Token token, ParseState state)
        {
            state.Operators.Push(new Operator(
                this,
                token.SourceMap,
                () => throw new ListDelimeterNotWithinBrackets(token.SourceMap)));
        }
    }
}