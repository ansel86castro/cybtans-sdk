using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Cybtans.Entities
{
    public interface IObjectFilterDefinition<T>
    {                
    }



    public interface IFilterDefinitionBuilder<T> 
    {
        IObjectFilterDefinition<T> Expression(Expression<Func<T, bool>> predicate);

        IObjectFilterDefinition<T> And(IObjectFilterDefinition<T> a, IObjectFilterDefinition<T> b);

        IObjectFilterDefinition<T> Or(IObjectFilterDefinition<T> a, IObjectFilterDefinition<T> b);

        IObjectFilterDefinition<T> Not(IObjectFilterDefinition<T> a);

        IObjectFilterDefinition<T> Contains(Expression<Func<T, object>> prop, string value);

        IObjectFilterDefinition<T> Regex(Expression<Func<T, object>> prop, string pattern);

        IObjectFilterDefinition<T> NearSphere(Expression<Func<T, object>> prop, double x, double y, int maxDistance);

        IObjectFilterDefinition<T> Text(string text, TextSearchOptions options = null);
    }

    public class TextSearchOptions
    {
        public string Language { get; set; }
        public bool? CaseSensitive { get; set; }
        public bool? DiacriticSensitive { get; set; }
    }
}
