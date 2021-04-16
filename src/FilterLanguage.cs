namespace AlbedoTeam.Sdk.FilterLanguage
{
    using System;
    using System.Linq.Expressions;
    using Languages.ExpressionLanguage;
    using Languages.MongoDbLanguage;
    using MongoDB.Driver;

    public static class FilterLanguage
    {
        public static FilterDefinition<T> ParseToFilterDefinition<T>(string filter) where T : class
        {
            var parser = new QueryParser<T>();
            var documentQuery = parser.TryParse(filter);
            var runner = new MongoDBQueryRunner<T>();
            runner.Create(documentQuery);

            return runner.FilterDefinition;
        }

        public static Expression<Func<T, bool>> ParseToExpression<T>(string filter) where T : class
        {
            var language = new FilterExpressionLanguage();
            return language.Parse<T>(filter);
        }
    }
}