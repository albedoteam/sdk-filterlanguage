namespace AlbedoTeam.Sdk.FilterLanguage.Core.Utility
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    internal class ExpressionEvaluator : ExpressionVisitor
    {
        private ExpressionEvaluator()
        {
        }

        public bool CanEvaluateLocally { get; private set; } = true;

        /// <summary>
        ///     Try to evaluate the expression locally to produce a result
        /// </summary>
        /// <typeparam name="T">the result type</typeparam>
        /// <param name="exp">The expression to evaluate locally</param>
        /// <param name="result">The output of the expression</param>
        /// <returns><c>true</c> if expression was evaluated; otherwise, <c>false</c>.</returns>
        public static bool TryEvaluate<T>(Expression exp, out T result)
        {
            if (exp.Type != typeof(T) && typeof(T).GetTypeInfo().IsAssignableFrom(exp.Type.GetTypeInfo()))
            {
                result = default;
                return false;
            }

            //if its a constant we can avoid compiling a lambda and get the value directly
            if (exp is ConstantExpression constExp)
            {
                result = (T) constExp.Value;
                return true;
            }

            //check to see if we can evaluate
            if (!CanEvaluate(exp))
            {
                result = default;
                return false;
            }

            try
            {
                var lambdaExp = Expression.Lambda<Func<T>>(exp);
                var lambda = lambdaExp.Compile();
                result = lambda();
                return true;
            }
            catch (Exception)
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        ///     Determines whether an expression can be evaluated locally
        /// </summary>
        /// <param name="exp">The expression to evaluate locally.</param>
        /// <returns>
        ///     <c>true</c> if this instance can be evaluated locallay; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanEvaluate(Expression exp)
        {
            var evaluator = new ExpressionEvaluator();
            evaluator.Visit(exp);
            return evaluator.CanEvaluateLocally;
        }

        public override Expression Visit(Expression node)
        {
            //if its using a parameter it means we can not evaulate the locally
            CanEvaluateLocally &= node.NodeType != ExpressionType.Parameter;
            return !CanEvaluateLocally ? node : base.Visit(node);
        }
    }
}