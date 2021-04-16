namespace AlbedoTeam.Sdk.FilterLanguage.Languages.MongoDbLanguage
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using MongoDB.Driver;

    public interface IMongoDbQueryRunner<T> where T : class
    {
        FilterDefinition<T> FilterDefinition { get; set; }
        SortDefinition<T> SortDefinition { get; set; }
        int Skip { get; set; }
        int Limit { get; set; }
        void Create(DocumentQuery<T> parser);
        Task<IList<T>> QueryAsync(IMongoCollection<T> mongoCollection);
    }
}