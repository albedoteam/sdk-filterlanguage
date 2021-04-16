namespace AlbedoTeam.Sdk.FilterLanguage.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Core;
    using Core.Extensions;
    using Enumerators;
    using Exceptions;

    /// <summary>
    ///     Represents a part of the grammar that defines an operator
    /// </summary>
    /// <seealso cref="GrammarDefinition" />
    internal class OperatorDefinition : GrammarDefinition
    {
        /// <summary>
        ///     A function given zero or more expressions of operands, produces a new operand
        /// </summary>
        public readonly Func<Expression[], Expression> ExpressionBuilder;

        /// <summary>
        ///     Order relative to which this operator is to be applied. The lowest orders are applied first
        /// </summary>
        public readonly int? OrderOfPrecedence;

        /// <summary>
        ///     Positions where parameters can be found
        /// </summary>
        public readonly IReadOnlyList<RelativePosition> ParamaterPositions;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OperatorDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the definition</param>
        /// <param name="regex">The regex to match tokens</param>
        /// <param name="paramaterPositions">The relative positions where parameters can be found.</param>
        /// <param name="expressionBuilder">A function given zero or more expressions of operands, produces a new operand</param>
        /// <exception cref="System.ArgumentNullException">
        ///     paramaterPositions
        ///     or
        ///     expressionBuilder
        /// </exception>
        public OperatorDefinition(string name,
            string regex,
            IEnumerable<RelativePosition> paramaterPositions,
            Func<Expression[], Expression> expressionBuilder)
            : this(name, regex, null, paramaterPositions, expressionBuilder)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OperatorDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the definition</param>
        /// <param name="regex">The regex to match tokens</param>
        /// <param name="orderOfPrecedence">The order in which that operator is to be applied. The lowest orders are applied first</param>
        /// <param name="paramaterPositions">The relative positions where the parameters can be found</param>
        /// <param name="expressionBuilder">A function given zero or more expressions of operands, produces a new operand</param>
        /// <exception cref="System.ArgumentNullException">
        ///     paramaterPositions
        ///     or
        ///     expressionBuilder
        /// </exception>
        public OperatorDefinition(string name,
            string regex,
            int? orderOfPrecedence,
            IEnumerable<RelativePosition> paramaterPositions,
            Func<Expression[], Expression> expressionBuilder)
            : base(name, regex)
        {
            if (paramaterPositions == null)
                throw new ArgumentNullException(nameof(paramaterPositions));

            ParamaterPositions = paramaterPositions.ToList();
            ExpressionBuilder = expressionBuilder ?? throw new ArgumentNullException(nameof(expressionBuilder));
            OrderOfPrecedence = orderOfPrecedence;
        }

        /// <summary>
        ///     Applies the token to the parsing state. Adds an operator to the state, when executed, the operator will
        ///     check that there are enough operands and that they are in the correct position.
        ///     It will then run the expressionBuilder putting the result in the state.
        /// </summary>
        /// <param name="token">The token to apply</param>
        /// <param name="state">The state to apply the token to</param>
        public override void Apply(Token token, ParseState state)
        {
            // Apply previous operators if they have a high precedence and share an operand
            var anyLeftOperators = ParamaterPositions.Any(x => x == RelativePosition.Left);
            while (state.Operators.Count > 0 && OrderOfPrecedence != null && anyLeftOperators)
            {
                var prevOperator = state.Operators.Peek().Definition as OperatorDefinition;
                var prevOperatorPrecedence = prevOperator?.OrderOfPrecedence;
                if (prevOperatorPrecedence <= OrderOfPrecedence &&
                    prevOperator.ParamaterPositions.Any(x => x == RelativePosition.Right))
                    state.Operators.Pop().Execute();
                else
                    break;
            }

            state.Operators.Push(new Operator(this, token.SourceMap, () =>
            {
                //Pop all our right arguments, and check there is the correct number and they are all to the right
                var rightArgs = new Stack<Operand>(state.Operands
                    .PopWhile(x => x.SourceMap.IsRightOf(token.SourceMap)));

                var expectedRightArgs = ParamaterPositions.Count(x => x == RelativePosition.Right);
                if (expectedRightArgs > 0 && rightArgs.Count > expectedRightArgs)
                {
                    var spanWhereOperatorExpected = StringSegment.Encompass(rightArgs
                        .Reverse()
                        .Take(rightArgs.Count - expectedRightArgs)
                        .Select(x => x.SourceMap));

                    throw new OperandUnexpectedException(token.SourceMap, spanWhereOperatorExpected);
                }

                if (rightArgs.Count < expectedRightArgs)
                    throw new OperandExpectedException(token.SourceMap,
                        new StringSegment(token.SourceMap.SourceString, token.SourceMap.End, 0));

                //Pop all our left arguments, and check they are not to the left of the next operator
                var nextOperatorEndIndex = state.Operators.Count == 0 ? 0 : state.Operators.Peek().SourceMap.End;

                var expectedLeftArgs = ParamaterPositions.Count(x => x == RelativePosition.Left);
                var leftArgs = new Stack<Operand>(state.Operands
                    .PopWhile((x, i) => i < expectedLeftArgs && x.SourceMap.IsRightOf(nextOperatorEndIndex)));

                if (leftArgs.Count < expectedLeftArgs)
                    throw new OperandExpectedException(token.SourceMap,
                        new StringSegment(token.SourceMap.SourceString, token.SourceMap.Start, 0));

                //Map the operators into the correct argument positions
                var args = ParamaterPositions
                    .Select(paramPos => paramPos == RelativePosition.Right
                        ? rightArgs.Pop()
                        : leftArgs.Pop())
                    .ToList();

                //our new source map will encompass this operator and all its operands
                var sourceMapSpan = StringSegment.Encompass(new[] {token.SourceMap}
                    .Concat(args.Select(x => x.SourceMap)));

                Expression expression;
                try
                {
                    expression = ExpressionBuilder(args.Select(x => x.Expression).ToArray());
                }
                catch (Exception ex)
                {
                    throw new OperationInvalidException(sourceMapSpan, ex);
                }

                state.Operands.Push(new Operand(expression, sourceMapSpan));
            }));
        }
    }
}