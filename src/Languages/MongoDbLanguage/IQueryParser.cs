namespace AlbedoTeam.Sdk.FilterLanguage.Languages.MongoDbLanguage
{
    using Models;

    public interface IQueryParser<T> where T : class
    {
        /// <summary>
        ///     Try and Parse Query expression
        /// </summary>
        /// <param name="queryExpression">Query expression</param>
        /// <returns>Parser Document Query of type <typeparamref name="T" /></returns>
        DocumentQuery<T> TryParse(string queryExpression);
    }
}