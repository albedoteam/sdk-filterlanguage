namespace AlbedoTeam.Sdk.FilterLanguage.Core
{
    using System.Linq.Expressions;

    public class Operand
    {
        public readonly Expression Expression;
        public readonly StringSegment SourceMap;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Operand" /> class.
        /// </summary>
        /// <param name="expression">The Expression that represents this operand</param>
        /// <param name="sourceMap">The original string and the position of this operand are</param>
        public Operand(Expression expression, StringSegment sourceMap)
        {
            Expression = expression;
            SourceMap = sourceMap;
        }

        public override string ToString()
        {
            return SourceMap.Value;
        }
    }
}