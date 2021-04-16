namespace AlbedoTeam.Sdk.FilterLanguage.Core
{
    using System;
    using Definitions;

    public class Operator
    {
        public readonly GrammarDefinition Definition;

        /// <summary>
        ///     Applies the operator, updating the ParseState
        /// </summary>
        public readonly Action Execute;

        public readonly StringSegment SourceMap;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Operator" /> class.
        /// </summary>
        /// <param name="definition">The grammar that defined this operator</param>
        /// <param name="sourceMap">The original string and the position of all this operator are</param>
        /// <param name="execute">The action to be taken when applying this operator</param>
        public Operator(GrammarDefinition definition, StringSegment sourceMap, Action execute)
        {
            Execute = execute;
            SourceMap = sourceMap;
            Definition = definition;
        }

        public override string ToString()
        {
            return SourceMap.Value;
        }
    }
}