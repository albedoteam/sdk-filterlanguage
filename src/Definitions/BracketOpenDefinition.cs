namespace AlbedoTeam.Sdk.FilterLanguage.Definitions
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Exceptions;

    /// <summary>
    ///     Represents an opening bracket
    /// </summary>
    /// <seealso cref="GrammarDefinition" />
    public class BracketOpenDefinition : GrammarDefinition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BracketOpenDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the definition</param>
        /// <param name="regex">The regex to match tokens</param>
        public BracketOpenDefinition(string name, string regex)
            : base(name, regex)
        {
        }

        /// <summary>
        ///     Applies an error operator to the state.
        ///     A corresponding closing bracket is expected to remove the error operator before being executed
        /// </summary>
        /// <param name="token">The token to apply.</param>
        /// <param name="state">The state to apply the token to.</param>
        public override void Apply(Token token, ParseState state)
        {
            state.Operators.Push(new Operator(
                this,
                token.SourceMap,
                () => throw new BracketUnmatchedException(token.SourceMap)));
        }

        /// <summary>
        ///     Applies the bracket operands. Adds the evaluated operand inside the bracket to the state
        /// </summary>
        /// <param name="bracketOpen">The operator that opened the bracket</param>
        /// <param name="bracketOperands">The list of operands within the brackets</param>
        /// <param name="bracketClose">The operator that closed the bracket</param>
        /// <param name="state">The current parse state</param>
        /// <exception cref="OperandExpectedException">When brackets are empty</exception>
        /// <exception cref="OperandUnexpectedException">When there is more than one element in the brackets</exception>
        public virtual void ApplyBracketOperands(Operator bracketOpen, Stack<Operand> bracketOperands,
            Operator bracketClose, ParseState state)
        {
            if (bracketOperands.Count == 0)
            {
                var insideBrackets = StringSegment.Between(bracketOpen.SourceMap, bracketClose.SourceMap);
                throw new OperandExpectedException(insideBrackets);
            }

            if (bracketOperands.Count > 1)
            {
                var operandSpan = StringSegment.Encompass(bracketOperands.Skip(1).Select(x => x.SourceMap));
                throw new OperandUnexpectedException(operandSpan);
            }

            var bracketOperand = bracketOperands.Pop();

            var sourceMap = StringSegment.Encompass(
                bracketOpen.SourceMap,
                bracketOperand.SourceMap,
                bracketClose.SourceMap);

            state.Operands.Push(new Operand(bracketOperand.Expression, sourceMap));
        }
    }
}