namespace AlbedoTeam.Sdk.FilterLanguage.Languages.MongoDbLanguage
{
    using System;
    using Models;

    public class QueryParser<T> : IQueryParser<T> where T : class
    {
        public DocumentQuery<T> TryParse(string queryExpression)
        {
            var model = new DocumentQuery<T>();
            if (string.IsNullOrEmpty(queryExpression)) throw new ArgumentNullException(nameof(queryExpression));

            model.Filter.TryParseFilter(queryExpression);
            return model;
        }
    }
}