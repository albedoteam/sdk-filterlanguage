namespace AlbedoTeam.Sdk.FilterLanguage.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Core;
    using Core.Utility;
    using Exceptions;

    /// <summary>
    ///     Represents the grammar for a function call
    /// </summary>
    /// <seealso cref="BracketOpenDefinition" />
    public class FunctionCallDefinition : BracketOpenDefinition
    {
        public readonly IReadOnlyList<Type> ArgumentTypes;
        public readonly Func<Expression[], Expression> ExpressionBuilder;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FunctionCallDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the definition</param>
        /// <param name="regex">The regex to match tokens</param>
        /// <param name="argumentTypes">The argument types that the function accepts</param>
        /// <param name="expressionBuilder">The function given the expressions of single operand, produces a new operand</param>
        public FunctionCallDefinition(
            string name,
            string regex,
            IEnumerable<Type> argumentTypes,
            Func<Expression[], Expression> expressionBuilder)
            : base(name, regex)
        {
            ArgumentTypes = argumentTypes?.ToList();
            ExpressionBuilder = expressionBuilder;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FunctionCallDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the definition</param>
        /// <param name="regex">The regex to match tokens</param>
        /// <param name="expressionBuilder">The function given the expressions of single operand, produces a new operand</param>
        public FunctionCallDefinition(string name, string regex, Func<Expression[], Expression> expressionBuilder)
            : this(name, regex, null, expressionBuilder)
        {
        }

        /// <summary>
        ///     Applies the bracket operands. Executes expressionBuilder with all operands enclosed in square brackets
        /// </summary>
        /// <param name="bracketOpen">The operator that opened the bracket</param>
        /// <param name="bracketOperands">The list of operands within the brackets</param>
        /// <param name="bracketClose">The operator that closed the bracket</param>
        /// <param name="state">The current parse state</param>
        /// <exception cref="FunctionArgumentCountException">When the number of operands does not match the number of arguments</exception>
        /// <exception cref="FunctionArgumentTypeException">When argument Type does not match the type of the expression</exception>
        /// <exception cref="OperationInvalidException">When an error occured while executing the expressionBuilder</exception>
        public override void ApplyBracketOperands(Operator bracketOpen, Stack<Operand> bracketOperands,
            Operator bracketClose, ParseState state)
        {
            var operandSource = StringSegment.Encompass(bracketOperands.Select(x => x.SourceMap));
            var functionArguments = bracketOperands.Select(x => x.Expression);

            // if we have been given specific argument types validate them
            if (ArgumentTypes != null)
            {
                var expectedArgumentCount = ArgumentTypes.Count;
                if (expectedArgumentCount != bracketOperands.Count)
                    throw new FunctionArgumentCountException(
                        operandSource,
                        expectedArgumentCount,
                        bracketOperands.Count);

                functionArguments = bracketOperands.Zip(ArgumentTypes, (o, t) =>
                {
                    try
                    {
                        return ExpressionConversions.Convert(o.Expression, t);
                    }
                    catch (InvalidOperationException)
                    {
                        // if we can't convert to the argument type, then something is wrong with the argument,
                        // so let's throw it out
                        throw new FunctionArgumentTypeException(o.SourceMap, t, o.Expression.Type);
                    }
                });
            }

            var functionSourceMap = StringSegment.Encompass(bracketOpen.SourceMap, operandSource);
            var functionArgumentsArray = functionArguments.ToArray();

            Expression output;
            try
            {
                output = ExpressionBuilder(functionArgumentsArray);
            }
            catch (Exception ex)
            {
                throw new OperationInvalidException(functionSourceMap, ex);
            }

            if (output != null)
                state.Operands.Push(new Operand(output, functionSourceMap));
        }
    }
}