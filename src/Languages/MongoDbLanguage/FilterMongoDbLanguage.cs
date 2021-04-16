namespace AlbedoTeam.Sdk.FilterLanguage.Languages.MongoDbLanguage
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Core.Utility;
    using Definitions;
    using ExpressionLanguage;

    public class FilterMongoDbLanguage : FilterExpressionLanguage
    {
        /// <summary>
        ///     Returns the definitions for functions used within the language.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<FunctionCallDefinition> FunctionDefinitions()
        {
            return new[]
            {
                new FunctionCallDefinition(
                    "FN_STARTSWITH",
                    @"startswith\(",
                    new[] {typeof(string), typeof(string)},
                    parameters =>
                    {
                        var param0Exp = Expression.Call(
                            parameters[0],
                            Type<string>.Method(x => x.ToLower()));

                        return Expression.Call(
                            param0Exp,
                            StringMembers.StartsWith,
                            parameters[1]);
                    }),

                new FunctionCallDefinition(
                    "FN_ENDSWITH",
                    @"endswith\(",
                    new[] {typeof(string), typeof(string)},
                    parameters =>
                    {
                        var param0Exp = Expression.Call(
                            parameters[0],
                            Type<string>.Method(x => x.ToLower()));

                        return Expression.Call(
                            param0Exp,
                            StringMembers.EndsWith,
                            parameters[1]);
                    }),

                new FunctionCallDefinition(
                    "FN_SUBSTRINGOF",
                    @"substringof\(",
                    new[] {typeof(string), typeof(string)},
                    parameters => Expression.Call(
                        parameters[1],
                        StringMembers.Contains,
                        parameters[0])),

                new FunctionCallDefinition(
                    "FN_CONTAINS",
                    @"contains\(",
                    new[] {typeof(string), typeof(string)},
                    parameters =>
                    {
                        var param0Exp = Expression.Call(
                            parameters[0],
                            Type<string>.Method(x => x.ToLower()));

                        return Expression.Call(
                            param0Exp,
                            Type<string>.Method(x => x.Contains(null)), parameters[1]);
                    }),

                new FunctionCallDefinition(
                    "FN_TOLOWER",
                    @"tolower\(",
                    new[] {typeof(string)},
                    parameters => Expression.Call(
                        parameters[0],
                        StringMembers.ToLower)),

                new FunctionCallDefinition(
                    "FN_TOUPPER",
                    @"toupper\(",
                    new[] {typeof(string)},
                    parameters => Expression.Call(
                        parameters[0],
                        StringMembers.ToUpper)),

                new FunctionCallDefinition(
                    "FN_DAY",
                    @"day\(",
                    new[] {typeof(DateTime)},
                    parameters => Expression.MakeMemberAccess(
                        parameters[0],
                        DateTimeMembers.Day)),

                new FunctionCallDefinition(
                    "FN_HOUR",
                    @"hour\(",
                    new[] {typeof(DateTime)},
                    parameters => Expression.MakeMemberAccess(
                        parameters[0],
                        DateTimeMembers.Hour)),

                new FunctionCallDefinition(
                    "FN_MINUTE",
                    @"minute\(",
                    new[] {typeof(DateTime)},
                    parameters => Expression.MakeMemberAccess(
                        parameters[0],
                        DateTimeMembers.Minute)),

                new FunctionCallDefinition(
                    "FN_MONTH",
                    @"month\(",
                    new[] {typeof(DateTime)},
                    parameters => Expression.MakeMemberAccess(
                        parameters[0],
                        DateTimeMembers.Month)),

                new FunctionCallDefinition(
                    "FN_YEAR",
                    @"year\(",
                    new[] {typeof(DateTime)},
                    parameters => Expression.MakeMemberAccess(
                        parameters[0],
                        DateTimeMembers.Year)),

                new FunctionCallDefinition(
                    "FN_SECOND",
                    @"second\(",
                    new[] {typeof(DateTime)},
                    parameters => Expression.MakeMemberAccess(
                        parameters[0],
                        DateTimeMembers.Second))
            };
        }
    }
}