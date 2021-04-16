namespace AlbedoTeam.Sdk.FilterLanguage.Languages.MongoDbLanguage.Models
{
    using System;
    using System.Linq.Expressions;
    using Enumerators;

    /// <summary>
    ///     Represents single order by field node
    /// </summary>
    public class OrderByNode<T>
    {
        public int Sequence { get; set; }
        public string PropertyName { get; set; }
        public OrderByDirectionType Direction { get; set; }
        public Expression<Func<T, object>> Expression { get; set; }
    }
}