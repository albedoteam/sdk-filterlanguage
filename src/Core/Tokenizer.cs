namespace AlbedoTeam.Sdk.FilterLanguage.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Definitions;
    using Exceptions;

    /// <summary>
    ///     Converts a string into a stream of tokens
    /// </summary>
    internal class Tokenizer
    {
        /// <summary>
        ///     Configuration of the tokens
        /// </summary>
        public readonly IReadOnlyList<GrammarDefinition> GrammarDefinitions;

        /// <summary>
        ///     Regex to identify tokens
        /// </summary>
        protected readonly Regex TokenRegex;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Tokenizer" /> class.
        /// </summary>
        /// <param name="grammarDefinitions">The configuration for this language</param>
        /// <exception cref="GrammarDefinitionDuplicateNameException">Thrown when two definitions have the same name</exception>
        public Tokenizer(params GrammarDefinition[] grammarDefinitions)
        {
            var duplicateKey = grammarDefinitions.GroupBy(x => x.Name).FirstOrDefault(g => g.Count() > 1)?.Key;
            if (duplicateKey != null)
                throw new GrammarDefinitionDuplicateNameException(duplicateKey);

            GrammarDefinitions = grammarDefinitions.ToList();

            var pattern = string.Join("|", GrammarDefinitions.Select(x => $"(?<{x.Name}>{x.Regex})"));
            TokenRegex = new Regex(pattern);
        }

        /// <summary>
        ///     Convert text into a stream of tokens
        /// </summary>
        /// <param name="text">text to tokenize</param>
        /// <returns>stream of tokens.</returns>
        public IEnumerable<Token> Tokenize(string text)
        {
            var matches = TokenRegex.Matches(text).OfType<Match>();

            var expectedIndex = 0;
            foreach (var match in matches)
            {
                if (match.Index > expectedIndex)
                    throw new GrammarUnknownException(new StringSegment(text, expectedIndex,
                        match.Index - expectedIndex));
                expectedIndex = match.Index + match.Length;

                var matchedTokenDefinition = GrammarDefinitions.FirstOrDefault(x => match.Groups[x.Name].Success);
                if (matchedTokenDefinition != null && matchedTokenDefinition.Ignore)
                    continue;

                yield return new Token(
                    matchedTokenDefinition,
                    match.Value,
                    new StringSegment(text, match.Index, match.Length));
            }
        }
    }
}