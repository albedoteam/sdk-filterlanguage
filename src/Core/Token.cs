namespace AlbedoTeam.Sdk.FilterLanguage.Core
{
    using Definitions;

    /// <summary>
    ///     An individual part of the complete entry
    /// </summary>
    public class Token
    {
        public readonly GrammarDefinition Definition;
        public readonly StringSegment SourceMap;
        public readonly string Value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Token" /> class.
        /// </summary>
        /// <param name="definition">The type of token and how it is defined</param>
        /// <param name="value">The value stored in the token</param>
        /// <param name="sourceMap">The original string and the position from which this token was extracted</param>
        public Token(GrammarDefinition definition, string value, StringSegment sourceMap)
        {
            Definition = definition;
            Value = value;
            SourceMap = sourceMap;
        }
    }
}