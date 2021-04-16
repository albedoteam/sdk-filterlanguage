namespace AlbedoTeam.Sdk.FilterLanguage.Languages.MongoDbLanguage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Enumerators;
    using Models;
    using MongoDB.Driver;

    public class MongoDBQueryRunner<T> : IMongoDbQueryRunner<T> where T : class
    {
        public FilterDefinition<T> FilterDefinition { get; set; }
        public SortDefinition<T> SortDefinition { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }

        public async Task<IList<T>> QueryAsync(IMongoCollection<T> mongoCollection)
        {
            if (mongoCollection == null) throw new ArgumentNullException(nameof(mongoCollection));

            FilterDefinition ??= Builders<T>.Filter.Empty;

            return await mongoCollection
                .Find(FilterDefinition)
                .Sort(SortDefinition)
                .Skip(Skip)
                .Limit(Limit)
                .ToListAsync();
        }

        /// <summary>
        ///     Creates MongoDB friendly filter, sort , skip and limt options from <paramref name="documentQuery" />
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="documentQuery">Document Query generated from query expression</param>
        public void Create(DocumentQuery<T> documentQuery)
        {
            if (documentQuery == default(DocumentQuery<T>)) throw new ArgumentNullException(nameof(documentQuery));

            if (documentQuery.Filter.FilterExpression != default(Expression<Func<T, bool>>))
                FilterDefinition = new ExpressionFilterDefinition<T>(documentQuery.Filter.FilterExpression);

            var sortBuilder = Builders<T>.Sort;
            var sortDefinitions = new List<SortDefinition<T>>();

            documentQuery
                .OrderBy
                .OrderByNodes
                .OrderBy(o => o.Sequence)
                .ToList()
                .ForEach(of =>
                {
                    switch (of.Direction)
                    {
                        case OrderByDirectionType.Ascending:
                            sortDefinitions.Add(sortBuilder.Ascending(of.PropertyName));
                            break;
                        case OrderByDirectionType.Descending:
                            sortDefinitions.Add(sortBuilder.Descending(of.PropertyName));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });

            SortDefinition = sortBuilder.Combine(sortDefinitions);
            Skip = documentQuery.Skip;
            Limit = documentQuery.Top;
        }
    }
}