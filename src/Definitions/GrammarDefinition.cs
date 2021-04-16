namespace AlbedoTeam.Sdk.FilterLanguage.Definitions
{
    using System;
    using System.Text.RegularExpressions;
    using Core;
    using Exceptions;

    /// <summary>
    ///     Represents a single piece of grammar and defines how it behaves within the system
    /// </summary>
    public class GrammarDefinition
    {
        private static readonly Regex _nameValidation = new Regex("^[a-zA-Z0-9_]+$");

        /// <summary>
        ///     Indicates whether this grammar should be ignored during tokenization
        /// </summary>
        public readonly bool Ignore;

        public readonly string Name;

        /// <summary>
        ///     Regex to match tokens
        /// </summary>
        public readonly string Regex;

        public GrammarDefinition(string name, string regex, bool ignore = false)
        {
            if (!_nameValidation.IsMatch(name))
                throw new GrammarDefinitionInvalidNameException(name);

            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
            Regex = regex ?? throw new ArgumentNullException(nameof(regex));
            Ignore = ignore;
        }

        /// <summary>
        ///     Applies the token to the parsing state
        /// </summary>
        /// <param name="token">The token to apply</param>
        /// <param name="state">The state to apply the token to</param>
        public virtual void Apply(Token token, ParseState state)
        {
        }
    }
}