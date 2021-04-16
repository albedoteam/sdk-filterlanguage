namespace AlbedoTeam.Sdk.FilterLanguage.Languages
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Core;
    using Definitions;

    internal class Language
    {
        /// <summary>
        ///     Parser to convert tokens into an expression.
        /// </summary>
        public readonly Parser Parser;

        /// <summary>
        ///     Tokenizer to generate tokens.
        /// </summary>
        public readonly Tokenizer Tokenizer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Language" /> class.
        /// </summary>
        /// <param name="grammerDefintions">The configuration for this language.</param>
        public Language(params GrammarDefinition[] grammerDefintions)
        {
            Tokenizer = new Tokenizer(grammerDefintions);
            Parser = new Parser();
        }

        /// <summary>
        ///     Configuration used to create the language
        /// </summary>
        public IReadOnlyList<GrammarDefinition> TokenDefinitions => Tokenizer.GrammarDefinitions;

        /// <summary>
        ///     Converts a string into an expression.
        /// </summary>
        /// <param name="text">The input string.</param>
        /// <param name="parameters">The parameters that are accessible by the expression.</param>
        /// <returns>
        ///     expression that represents the input string.
        /// </returns>
        public Expression Parse(string text, params ParameterExpression[] parameters)
        {
            var tokenStream = Tokenizer.Tokenize(text);
            var expression = Parser.Parse(tokenStream, parameters);
            return expression;
        }

        /// <summary>
        ///     Converts a string into a function expression.
        /// </summary>
        /// <typeparam name="TOut">The output of the function.</typeparam>
        /// <param name="text">The input string.</param>
        /// <returns>
        ///     expression that represents the input string.
        /// </returns>
        public Expression<Func<TOut>> Parse<TOut>(string text)
        {
            var body = Parse(text);
            return Expression.Lambda<Func<TOut>>(body);
        }

        /// <summary>
        ///     Converts a string into a function expression.
        /// </summary>
        /// <typeparam name="TIn">The input type of the function.</typeparam>
        /// <typeparam name="TOut">The output type of the function.</typeparam>
        /// <param name="text">The input string.</param>
        /// <returns>
        ///     expression that represents the input string.
        /// </returns>
        public Expression<Func<TIn, TOut>> Parse<TIn, TOut>(string text)
        {
            var parameters = new[] {Expression.Parameter(typeof(TIn))};
            var body = Parse(text, parameters);

            return Expression.Lambda<Func<TIn, TOut>>(body, parameters);
        }
    }
}