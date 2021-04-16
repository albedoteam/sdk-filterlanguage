namespace AlbedoTeam.Sdk.FilterLanguage.Exceptions
{
    using System;

    /// <summary>
    ///     Exception when a multiple grammar is configured with the same name
    /// </summary>
    public class GrammarDefinitionDuplicateNameException : Exception
    {
        /// <summary>
        ///     The name that was duplicated.
        /// </summary>
        public readonly string GrammarDefinitionName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GrammarDefinitionDuplicateNameException" /> class.
        /// </summary>
        /// <param name="grammarDefinitionName">Name of the duplicated grammer definition.</param>
        public GrammarDefinitionDuplicateNameException(string grammarDefinitionName) : base(
            $"Grammer definition name '{grammarDefinitionName}' has been defined multiple times")
        {
            GrammarDefinitionName = grammarDefinitionName;
        }
    }
}