using System;
using System.Linq;
using System.Linq.Expressions;

namespace Cybtans.Expressions
{
    public interface IExpressionBuilder<TModel>
    {
        IQueryable<TModel> OrderBy(IQueryable<TModel> query, string expression);

        Expression<Func<TModel, bool>> Where(string expression);

        IQueryable<TModel> Query(IQueryable<TModel> query, string filter, string orderby=null, int skip=-1, int take=-1);

        IQueryable<TResponse> Query<TResponse>(IQueryable<TModel> query, string filter = null, string orderby = null, int skip = -1, int take = -1, string include = null);

        Expression<Func<TModel, TResponse>> Select<TResponse>(string include = null);

        Expression<Func<TModel, object>>[] Include(string expression);
    }
    
}