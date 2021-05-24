namespace AlbedoTeam.Sdk.FilterLanguage.Languages.ExpressionLanguage
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using Core.Utility;
    using Definitions;
    using Enumerators;
    using MongoDB.Bson;

    public class FilterExpressionLanguage
    {
        private readonly Language language;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FilterExpressionLanguage" /> class.
        /// </summary>
        public FilterExpressionLanguage()
        {
            language = new Language(GetAllDefinitions().ToArray());
        }

        protected virtual IEnumerable<GrammarDefinition> GetAllDefinitions()
        {
            var functions = FunctionDefinitions().ToList();

            var definitions = new List<GrammarDefinition>();

            definitions.AddRange(TypeDefinitions());
            definitions.AddRange(functions);
            definitions.AddRange(BracketDefinitions(functions));
            definitions.AddRange(LogicalOperatorDefinitions());
            definitions.AddRange(ArithmeticOperatorDefinitions());
            definitions.AddRange(PropertyDefinitions());
            definitions.AddRange(WhitespaceDefinitions());

            return definitions;
        }

        protected virtual IEnumerable<GrammarDefinition> TypeDefinitions()
        {
            return new[]
            {
                new OperandDefinition(
                    "GUID",
                    @"guid'[0-9A-Fa-f]{8}\-[0-9A-Fa-f]{4}\-[0-9A-Fa-f]{4}\-[0-9A-Fa-f]{4}\-[0-9A-Fa-f]{12}'",
                    x => Expression.Constant(Guid.Parse(x.Substring("guid".Length).Trim('\'')))),

                new OperandDefinition(
                    "OBJECTID",
                    @"objid'[a-f\d]{24}'",
                    x => Expression.Constant(new ObjectId(x.Substring("objid".Length).Trim('\'')))),

                new OperandDefinition(
                    "STRING",
                    @"'(?:\\.|[^'])*'",
                    x => Expression.Constant(x.Trim('\'')
                        .Replace("\\'", "'")
                        .Replace("\\r", "\r")
                        .Replace("\\f", "\f")
                        .Replace("\\n", "\n")
                        .Replace("\\\\", "\\")
                        .Replace("\\b", "\b")
                        .Replace("\\t", "\t"))),

                new OperandDefinition(
                    "BYTE",
                    @"0x[0-9A-Fa-f]{1,2}",
                    x => Expression.Constant(byte.Parse(x.Substring("0x".Length),
                        NumberStyles.HexNumber | NumberStyles.AllowHexSpecifier))),

                new OperandDefinition(
                    "NULL",
                    @"null",
                    x => Expression.Constant(null)),

                new OperandDefinition(
                    "BOOL",
                    @"true|false",
                    x => Expression.Constant(bool.Parse(x))),

                new OperandDefinition(
                    "DATETIME",
                    @"[Dd][Aa][Tt][Ee][Tt][Ii][Mm][Ee]'[^']+'",
                    x =>
                        Expression.Constant(DateTime.Parse(x.Substring("datetime".Length).Trim('\'')))),

                new OperandDefinition(
                    "DATETIMEOFFSET",
                    @"datetimeoffset'[^']+'",
                    x =>
                        Expression.Constant(DateTimeOffset.Parse(x.Substring("datetimeoffset".Length).Trim('\'')))),

                new OperandDefinition(
                    "FLOAT",
                    @"\-?\d+?\.\d*f",
                    x => Expression.Constant(float.Parse(x.TrimEnd('f')))),

                new OperandDefinition(
                    "DOUBLE",
                    @"\-?\d+\.?\d*d",
                    x => Expression.Constant(double.Parse(x.TrimEnd('d')))),

                new OperandDefinition(
                    "DECIMAL_EXPLICIT",
                    @"\-?\d+\.?\d*[m|M]",
                    x => Expression.Constant(decimal.Parse(x.TrimEnd('m', 'M')))),

                new OperandDefinition(
                    "DECIMAL",
                    @"\-?\d+\.\d+",
                    x => Expression.Constant(decimal.Parse(x))),

                new OperandDefinition(
                    "LONG",
                    @"\-?\d+L",
                    x => Expression.Constant(long.Parse(x.TrimEnd('L')))),

                new OperandDefinition(
                    "INTEGER",
                    @"\-?\d+",
                    x => Expression.Constant(int.Parse(x)))
            };
        }

        protected virtual IEnumerable<FunctionCallDefinition> FunctionDefinitions()
        {
            return new[]
            {
                new FunctionCallDefinition(
                    "FN_STARTSWITH",
                    @"startswith\(",
                    new[] {typeof(string), typeof(string)},
                    parameters => Expression.Call(
                        parameters[0],
                        StringMembers.StartsWith,
                        parameters[1])),

                new FunctionCallDefinition(
                    "FN_ENDSWITH",
                    @"endswith\(",
                    new[] {typeof(string), typeof(string)},
                    parameters => Expression.Call(
                        parameters[0],
                        StringMembers.EndsWith, parameters[1])),

                new FunctionCallDefinition(
                    "FN_SUBSTRINGOF",
                    @"substringof\(",
                    new[] {typeof(string), typeof(string)},
                    parameters => Expression.Call(
                        parameters[1],
                        StringMembers.Contains, parameters[0])),

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

        protected virtual IEnumerable<GrammarDefinition> BracketDefinitions(
            IEnumerable<FunctionCallDefinition> functionCalls)
        {
            BracketOpenDefinition openBracket;
            ListDelimiterDefinition delimeter;
            return new GrammarDefinition[]
            {
                openBracket = new BracketOpenDefinition(
                    "OPEN_BRACKET",
                    @"\("),

                delimeter = new ListDelimiterDefinition(
                    "COMMA",
                    ","),

                new BracketCloseDefinition(
                    "CLOSE_BRACKET",
                    @"\)",
                    new[] {openBracket}.Concat(functionCalls),
                    delimeter)
            };
        }

        protected virtual IEnumerable<GrammarDefinition> LogicalOperatorDefinitions()
        {
            return new GrammarDefinition[]
            {
                new BinaryOperatorDefinition(
                    "EQ",
                    @"\b(eq)\b",
                    11,
                    ConvertEnumsIfRequired(Expression.Equal)),

                new BinaryOperatorDefinition(
                    "NE",
                    @"\b(ne)\b",
                    12,
                    ConvertEnumsIfRequired(Expression.NotEqual)),

                new BinaryOperatorDefinition(
                    "GT",
                    @"\b(gt)\b",
                    13,
                    Expression.GreaterThan),

                new BinaryOperatorDefinition(
                    "GE",
                    @"\b(ge)\b",
                    14,
                    Expression.GreaterThanOrEqual),

                new BinaryOperatorDefinition(
                    "LT",
                    @"\b(lt)\b",
                    15,
                    Expression.LessThan),

                new BinaryOperatorDefinition(
                    "LE",
                    @"\b(le)\b",
                    16,
                    Expression.LessThanOrEqual),

                new BinaryOperatorDefinition(
                    "LK",
                    @"\b(lk)\b",
                    17,
                    ConvertEnumsIfRequired((left, right) =>
                    {
                        var param0Exp = Expression.Call(
                            left,
                            Type<string>.Method(x => x.ToLower()));

                        return Expression.Call(
                            param0Exp,
                            Type<string>.Method(x => x.Contains(null)),
                            right);
                    })),

                new BinaryOperatorDefinition(
                    "AND",
                    @"\b(and)\b",
                    19,
                    Expression.And),

                new BinaryOperatorDefinition(
                    "OR",
                    @"\b(or)\b",
                    20,
                    Expression.Or),

                new UnaryOperatorDefinition(
                    "NOT",
                    @"\b(not)\b",
                    21,
                    RelativePosition.Right,
                    arg =>
                    {
                        ExpressionConversions.TryBoolean(ref arg);
                        return Expression.Not(arg);
                    })
            };
        }

        protected virtual IEnumerable<GrammarDefinition> ArithmeticOperatorDefinitions()
        {
            return new[]
            {
                new BinaryOperatorDefinition(
                    "ADD",
                    @"\b(add)\b",
                    2,
                    Expression.Add),

                new BinaryOperatorDefinition(
                    "SUB",
                    @"\b(sub)\b",
                    2,
                    Expression.Subtract),

                new BinaryOperatorDefinition(
                    "MUL",
                    @"\b(mul)\b",
                    1,
                    Expression.Multiply),

                new BinaryOperatorDefinition(
                    "DIV",
                    @"\b(div)\b",
                    1,
                    Expression.Divide),

                new BinaryOperatorDefinition(
                    "MOD",
                    @"\b(mod)\b",
                    1,
                    Expression.Modulo)
            };
        }

        protected virtual IEnumerable<GrammarDefinition> PropertyDefinitions()
        {
            return new[]
            {
                //Properties
                new OperandDefinition(
                    "PROPERTY_PATH",
                    @"(?<![0-9])([A-Za-z_][A-Za-z0-9_]*/?)+",
                    (value, parameters) =>
                    {
                        var values = value.Split('/');

                        var expression = parameters[0];
                        var memberExpression = Expression.MakeMemberAccess(
                            expression,
                            TypeShim.GetProperty(expression.Type, values[0]));

                        if (memberExpression.Type == typeof(Dictionary<string, string>))
                            return Expression.Call(memberExpression,
                                Type<Dictionary<string, string>>.Method(dictionary => dictionary[values[1]]),
                                Expression.Constant(values[1]));

                        return values.Aggregate((Expression) parameters[0],
                            (exp, prop) => Expression.MakeMemberAccess(exp, TypeShim.GetProperty(exp.Type, prop)));
                    })
            };
        }

        protected virtual IEnumerable<GrammarDefinition> WhitespaceDefinitions()
        {
            return new[]
            {
                new GrammarDefinition(
                    "WHITESPACE",
                    @"\s+",
                    true)
            };
        }

        /// <summary>
        ///     Parses the specified text converting it into a predicate expression
        /// </summary>
        /// <typeparam name="T">The input type</typeparam>
        /// <param name="text">The text to parse.</param>
        /// <returns></returns>
        public Expression<Func<T, bool>> Parse<T>(string text)
        {
            var parameters = new[] {Expression.Parameter(typeof(T))};
            var body = language.Parse(text, parameters);

            ExpressionConversions.TryBoolean(ref body);

            return Expression.Lambda<Func<T, bool>>(body, parameters);
        }

        /// <summary>
        ///     Wraps the function to convert any constants to enums if required
        /// </summary>
        /// <param name="expFn">Function to wrap</param>
        /// <returns></returns>
        protected Func<Expression, Expression, Expression> ConvertEnumsIfRequired(
            Func<Expression, Expression, Expression> expFn)
        {
            return (left, right) =>
            {
                var didConvertEnum = ExpressionConversions.TryEnumNumberConvert(ref left, ref right)
                                     || ExpressionConversions.TryEnumStringConvert(ref left, ref right, true);

                return expFn(left, right);
            };
        }
    }
}