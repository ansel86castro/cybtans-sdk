using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Linq.Expressions;

namespace Cybtans.Entities.MongoDb
{
    public class MongoDbFilterDefinition<T> : IObjectFilterDefinition<T>
         where T : class
    {
        FilterDefinition<T> _filter;

        public MongoDbFilterDefinition(FilterDefinition<T> filter) { _filter = filter; }

        public MongoDbFilterDefinition(Expression<Func<T, bool>> predicate)
        {
            _filter = predicate;
        }

        public static implicit operator MongoDbFilterDefinition<T>(Expression<Func<T, bool>> predicate)
        {
            return new MongoDbFilterDefinition<T>(predicate);
        }

        public static implicit operator FilterDefinition<T>(MongoDbFilterDefinition<T> obj)
        {
            return obj.Filter;
        }

        public FilterDefinition<T> Filter => _filter;
    }

    public static class FilterDefinitionExtensions
    {
        public static MongoDbFilterDefinition<T> AsMongo<T>(this IObjectFilterDefinition<T> filter) where T : class => (MongoDbFilterDefinition<T>)filter;

        public static MongoDbFilterDefinition<T> AsMongo<T>(this FilterDefinition<T> filter) where T : class => new MongoDbFilterDefinition<T>(filter);
        
    }

    public class MongoDbFilterDefinitionBuilder<T> : IFilterDefinitionBuilder<T>
      where T : class
    {

        public IObjectFilterDefinition<T> And(IObjectFilterDefinition<T> a, IObjectFilterDefinition<T> b)
        {
            if (a == null) return b;
            else if (b == null) return a;

            return (a.AsMongo().Filter & b.AsMongo().Filter).AsMongo();
        }

        public IObjectFilterDefinition<T> Or(IObjectFilterDefinition<T> a, IObjectFilterDefinition<T> b)
        {
            if (a == null) return b;
            else if (b == null) return a;

            return FilterDefinitionExtensions.AsMongo(FilterDefinitionExtensions.AsMongo(a).Filter | FilterDefinitionExtensions.AsMongo(b).Filter);
        }

        public IObjectFilterDefinition<T> Contains(Expression<Func<T, object>> prop, string value)
        {
            return Builders<T>.Filter.Regex(prop, System.Text.RegularExpressions.Regex.Escape(value)).AsMongo();
        }

        public IObjectFilterDefinition<T> Expression(Expression<Func<T, bool>> predicate)
        {
            return FilterDefinitionExtensions.AsMongo<T>(predicate);
        }

        public IObjectFilterDefinition<T> NearSphere(Expression<Func<T, object>> prop, double x, double y, int maxDistance)
        {
            var geoPoint = new GeoJson2DCoordinates(x, y);

            return Builders<T>.Filter
                .NearSphere(prop, new GeoJsonPoint<GeoJson2DCoordinates>(geoPoint), maxDistance)
                .AsMongo();
        }

        public IObjectFilterDefinition<T> Not(IObjectFilterDefinition<T> a)
        {
            return FilterDefinitionExtensions.AsMongo(Builders<T>.Filter.Not(FilterDefinitionExtensions.AsMongo(a).Filter));
        }
    
        public IObjectFilterDefinition<T> Regex(Expression<Func<T, object>> prop, string pattern)
        {
            return Builders<T>.Filter.Regex(prop, pattern).AsMongo();
        }

        public IObjectFilterDefinition<T> Text(string text, TextSearchOptions options = null)
        {
            return Builders<T>.Filter.Text(text, options != null? new MongoDB.Driver.TextSearchOptions
            {
                 Language = options.Language,
                 CaseSensitive = options.CaseSensitive,
                 DiacriticSensitive = options.DiacriticSensitive
            }: null).AsMongo();
        }
    }

}
