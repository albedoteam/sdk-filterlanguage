namespace AlbedoTeam.Sdk.FilterLanguage.Exceptions
{
    using System;

    /// <summary>
    ///     Exception when a grammar definition is configured with an invalid name
    /// </summary>
    public class GrammarDefinitionInvalidNameException : Exception
    {
        /// <summary>
        ///     The invalid name
        /// </summary>
        public readonly string GrammarDefinitionName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GrammarDefinitionInvalidNameException" /> class.
        /// </summary>
        /// <param name="grammarDefinitionName">Invalid grammar definition name</param>
        public GrammarDefinitionInvalidNameException(string grammarDefinitionName) :
            base($"Invalid grammer definition name '{grammarDefinitionName}' name may only contain [a-zA-Z0-9_]")
        {
            GrammarDefinitionName = grammarDefinitionName;
        }
    }
}