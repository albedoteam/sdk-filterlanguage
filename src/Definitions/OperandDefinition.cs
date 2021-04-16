namespace AlbedoTeam.Sdk.FilterLanguage.Definitions
{
    using System;
    using System.Linq.Expressions;
    using Core;
    using Exceptions;

    /// <summary>
    ///     Represents a part of the grammar that defines an operand
    /// </summary>
    /// <seealso cref="GrammarDefinition" />
    internal class OperandDefinition : GrammarDefinition
    {
        /// <summary>
        ///     A function to generate the operator expression
        /// </summary>
        public readonly Func<string, ParameterExpression[], Expression> ExpressionBuilder;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OperandDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the definition</param>
        /// <param name="regex">The regex to match tokens</param>
        /// <param name="expressionBuilder">The function to generate the operator expression</param>
        /// <exception cref="System.ArgumentNullException">expressionBuilder</exception>
        public OperandDefinition(string name, string regex, Func<string, Expression> expressionBuilder)
            : this(name, regex, (v, a) => expressionBuilder(v))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OperandDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the definition</param>
        /// <param name="regex">The regex to match tokens</param>
        /// <param name="expressionBuilder">The function to generate the operator expression</param>
        /// <exception cref="System.ArgumentNullException">expressionBuilder</exception>
        public OperandDefinition(string name, string regex,
            Func<string, ParameterExpression[], Expression> expressionBuilder)
            : base(name, regex)
        {
            ExpressionBuilder = expressionBuilder ?? throw new ArgumentNullException(nameof(expressionBuilder));
        }

        /// <summary>
        ///     Applies the token to the parsing state and adds the result of the expression builder to the state
        /// </summary>
        /// <param name="token">The token to apply</param>
        /// <param name="state">The state to apply the token to</param>
        /// <exception cref="OperationInvalidException">When an error is detected during expressionBuilder execution</exception>
        public override void Apply(Token token, ParseState state)
        {
            Expression expression;
            try
            {
                expression = ExpressionBuilder(token.Value, state.Parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw new OperationInvalidException(token.SourceMap, ex);
            }

            state.Operands.Push(new Operand(expression, token.SourceMap));
        }
    }
}