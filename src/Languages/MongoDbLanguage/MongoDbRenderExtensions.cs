namespace AlbedoTeam.Sdk.FilterLanguage.Languages.MongoDbLanguage
{
    using System.Collections.Generic;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;

    public static class MongoDbRenderExtensions
    {
        public static BsonDocument RenderToBsonDocument<T>(this FilterDefinition<T> filter)
        {
            if (filter is null)
                return new BsonDocument(new Dictionary<string, object> {{"Ooops", "Deu ruim menó!"}});

            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<T>();
            return filter.Render(documentSerializer, serializerRegistry);
        }
    }
}