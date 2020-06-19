using Cybtans.Expressions.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Expression = System.Linq.Expressions.Expression;
using MemberExpression = System.Linq.Expressions.MemberExpression;

namespace Cybtans.Expressions
{
    public class ExpressionBuilder<T> : ASTContext, IExpressionBuilder<T>
    {
        protected class ParamExpresionVisitor : ExpressionVisitor
        {
            public Expression ReplaceParameter;

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node.Type == ReplaceParameter.Type && node != ReplaceParameter)
                    return ReplaceParameter;
                return node;
            }
        }

        protected ParamExpresionVisitor paramExpresionVisitor = new ParamExpresionVisitor();

        public ExpressionBuilder(string parameter = "x", MemberExpression referenceExpression = null)
        {
            ResponseType = typeof(T);

            if (referenceExpression != null)
            {
                ModelType = ((PropertyInfo)referenceExpression.Member).PropertyType;
                Parameter = referenceExpression;
                paramExpresionVisitor.ReplaceParameter = Parameter;
            }
            else
            {
                ModelType = typeof(T);
                Parameter = Expression.Parameter(ModelType, parameter);
                paramExpresionVisitor.ReplaceParameter = Parameter;
            }


        }

        public ExpressionBuilder(Expression<Func<T, object>> expression)
            : this("x", expression.Body as MemberExpression)
        {

        }

        public Expression<Func<T, bool>> Where(string expression)
        {
            QueryParser parser = new QueryParser();
            var astExp = parser.Parse(expression);

            astExp.CheckSemantic(this);
            Expression exp = astExp.GenerateLinqExpression(this);

            Expression current = Parameter;
            while (current != null && !(current is ParameterExpression))
            {
                current = ((MemberExpression)current).Expression;
            }

            return Expression.Lambda<Func<T, bool>>(exp, (ParameterExpression)current);
        }

        public IQueryable<T> OrderBy(IQueryable<T> query, string expression)
        {
            var method = typeof(Queryable).GetMember("OrderBy")[0] as MethodInfo;
            var methodDesc = typeof(Queryable).GetMember("OrderByDescending")[0] as MethodInfo;


            var thenBy = typeof(Queryable).GetMember("ThenBy")[0] as MethodInfo;
            var thenByDesc = typeof(Queryable).GetMember("ThenByDescending")[0] as MethodInfo;

            Expression current = Parameter;
            while (current != null && !(current is ParameterExpression))
            {
                current = ((MemberExpression)current).Expression;
            }

            var parts = expression.Split(',');
            IQueryable<T> ordered = query;

            var index = 0;

            QueryParser parser = new QueryParser();

            foreach (var part in parts)
            {
                var value = part.Trim();
                string property;
                bool descend = false;
                if (value.Contains(' '))
                {
                    var split = value.Split(' ');
                    descend = split[1].Trim().ToLowerInvariant() == "desc";
                    property = split[0].Trim();
                }
                else
                {
                    property = value;
                }


                var astExp = parser.Parse(property);

                astExp.CheckSemantic(this);
                Expression exp = astExp.GenerateLinqExpression(this);

                MethodInfo orderByMethod;
                if (index == 0)
                {
                    orderByMethod = descend ? methodDesc : method;
                }
                else
                {
                    orderByMethod = descend ? thenByDesc : thenBy;
                }
                index++;

                var lambda = Expression.Lambda(exp, (ParameterExpression)current);
                ordered = ordered.Provider.CreateQuery<T>(Expression.Call(null, orderByMethod.MakeGenericMethod(new Type[] { typeof(T), exp.Type }), new Expression[] { ordered.Expression, Expression.Quote(lambda) }));

            }
            return ordered;
        }

        public IQueryable<T> Query(IQueryable<T> query, string filter, string orderby, int skip, int take)
        {
            if (filter != null)
                query = query.Where(Where(filter));

            if (orderby != null)
                query = OrderBy(query, orderby);

            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (take > 0)
                query = query.Take(take);

            return query;
        }

        public IQueryable<TResponse> Query<TResponse>(IQueryable<T> query, string filter = null, string orderby = null, int skip = -1, int take = -1, string include = null)
        {
            if (filter != null)
                query = query.Where(Where(filter));

            if (orderby != null)
                query = OrderBy(query, orderby);

            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (take > 0)
                query = query.Take(take);

            return query.Select(Select<TResponse>(include));
        }

        public Expression<Func<T, TResponse>> Select<TResponse>(string include = null)
        {
            List<MemberBinding> bindings = new List<MemberBinding>();
            foreach (var p in ResponseType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanWrite))
            {
                NavigationPropertyAttribute navAttr = p.GetCustomAttribute<NavigationPropertyAttribute>();
                if (navAttr != null)
                {
                    bindings.Add(Expression.Bind(p, Expression.Property(Expression.Property(Parameter, navAttr.NavigationProperty), navAttr.Property)));
                }
                else if (IncludeTree.IsMappeable(p))
                {
                    bindings.Add(Expression.Bind(p, Expression.Property(Parameter, p.Name)));
                }
            }

            if (!string.IsNullOrWhiteSpace(include))
            {
                var incTrees = IncludeTree.Build(include, ResponseType, ModelType, Parameter);
                foreach (var item in incTrees)
                {
                    bindings.Add(Expression.Bind(item.ResponseProperty, item.CreateInitExpression()));
                }
            }

            MemberInitExpression initExpression = Expression.MemberInit(Expression.New(typeof(TResponse)), bindings);

            Expression current = Parameter;
            while (current != null && !(current is ParameterExpression))
            {
                current = ((MemberExpression)current).Expression;
            }

            return Expression.Lambda<Func<T, TResponse>>(initExpression, (ParameterExpression)current);

        }

        public Expression<Func<T, object>>[] Include(string expression)
        {
            var parts = expression.Split(',');

            Expression<Func<T, object>>[] includes = new Expression<Func<T, object>>[parts.Length];

            QueryParser parser = new QueryParser();

            Expression current = Parameter;
            while (current != null && !(current is ParameterExpression))
            {
                current = ((MemberExpression)current).Expression;
            }

            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var value = part.Trim();
                var astExp = parser.Parse(value);

                astExp.CheckSemantic(this);
                Expression exp = astExp.GenerateLinqExpression(this);

                includes[i] = Expression.Lambda<Func<T, object>>(exp, (ParameterExpression)current);
            }

            return includes;
        }

        class IncludeTree
        {
            internal static bool IsMappeable(PropertyInfo p)
            {
                return p.GetCustomAttribute<NonMappeableAttribute>() == null && (
                    p.PropertyType.IsValueType ||
                    p.PropertyType.IsEnum ||
                    p.PropertyType == typeof(byte[]) ||
                    p.PropertyType == typeof(string));
            }


            public string PropertyName;

            public PropertyInfo ResponseProperty;

            public MemberExpression ModelProperty;

            public Dictionary<string, IncludeTree> Includes = new Dictionary<string, IncludeTree>();

            public static IncludeTree[] Build(string include, Type resposeType, Type modelType, Expression parameter)
            {
                string[][] includesArray = include.Split(',').Select(x => x.Split('.')).ToArray();

                Dictionary<string, IncludeTree> dic = new Dictionary<string, IncludeTree>();

                for (int i = 0; i < includesArray.Length; i++)
                {
                    IncludeTree incTree = null;
                    for (int j = 0; j < includesArray[i].Length; j++)
                    {
                        var propertyName = includesArray[i][j];

                        if (j == 0)
                        {
                            if (!dic.TryGetValue(propertyName, out incTree))
                            {
                                incTree = new IncludeTree
                                {
                                    PropertyName = propertyName,
                                    ResponseProperty = resposeType.GetProperty(propertyName),
                                    ModelProperty = Expression.Property(parameter, modelType.GetProperty(propertyName))
                                };
                                dic.Add(propertyName, incTree);
                            }
                        }
                        else
                        {
                            IncludeTree child;
                            if (!incTree.Includes.TryGetValue(propertyName, out child))
                            {
                                child = new IncludeTree
                                {
                                    PropertyName = propertyName,
                                    ResponseProperty = incTree.ResponseProperty.PropertyType.GetProperty(propertyName),
                                    ModelProperty = Expression.Property(incTree.ModelProperty, ((PropertyInfo)incTree.ModelProperty.Member).PropertyType.GetProperty(propertyName))
                                };
                                incTree.Includes.Add(propertyName, child);
                            }

                            incTree = child;
                        }

                    }
                }

                return dic.Values.ToArray();
            }

            public Expression CreateInitExpression()
            {
                List<MemberBinding> bindings = new List<MemberBinding>();

                var modelType = ((PropertyInfo)ModelProperty.Member).PropertyType;
                var responseType = ResponseProperty.PropertyType;
                var props = responseType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanWrite);

                foreach (var p in props)
                {
                    IncludeTree include;
                    Expression bind = null;

                    if (!Includes.TryGetValue(p.Name, out include))
                    {
                        NavigationPropertyAttribute navAttr = p.GetCustomAttribute<NavigationPropertyAttribute>();

                        if (navAttr != null)
                        {
                            var navModelProp = Expression.Property(ModelProperty, modelType.GetProperty(navAttr.NavigationProperty));
                            var navProp = Expression.Property(navModelProp, ((PropertyInfo)navModelProp.Member).PropertyType.GetProperty(navAttr.Property));
                            bind = navProp;

                        }
                        else if (IsMappeable(p))
                        {
                            bind = Expression.Property(ModelProperty, modelType.GetProperty(p.Name));
                        }

                    }
                    else
                    {
                        bind = include.CreateInitExpression();
                    }

                    if (bind != null)
                    {
                        bindings.Add(Expression.Bind(p, bind));
                    }

                }

                NewExpression newResponse = Expression.New(responseType);
                MemberInitExpression initExpression = Expression.MemberInit(newResponse, bindings);

                return initExpression;
            }
        }

    }

    public class ExpressionBuilder<T, TResponse> : ExpressionBuilder<T>
    {

        public ExpressionBuilder(string parameter = "x", MemberExpression referenceExpression = null)
            : base(parameter, referenceExpression)
        {
            ResponseType = typeof(TResponse);
        }

        public ExpressionBuilder(Expression<Func<T, object>> expression)
            : base(expression)
        {
            ResponseType = typeof(TResponse);
        }

    }

    public static class ExpressionBuilder
    {
        class ParamExpresionVisitor : ExpressionVisitor
        {
            public Expression ReplaceParameter;

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node.Type == ReplaceParameter.Type && node != ReplaceParameter)
                    return ReplaceParameter;
                return node;
            }
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> querable, string expression)
        {
            if (expression == null)
                return querable;

            ExpressionBuilder<T> builder = new ExpressionBuilder<T>();
            return querable.Where(builder.Where(expression));
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> querable, string expression)
        {
            if (expression == null)
                return querable;

            ExpressionBuilder<T> builder = new ExpressionBuilder<T>();
            return builder.OrderBy(querable, expression);
        }

        public static IQueryable<TOut> Select<Tin, TOut>(this IQueryable<Tin> querable, Expression<Func<Tin, object>> expression = null, string include = null)
        {
            ExpressionBuilder<Tin, TOut> builder = new ExpressionBuilder<Tin, TOut>(expression);
            return querable.Select(builder.Select<TOut>(include));

        }

        public static IQueryable<T> Where<T, TOut>(this IQueryable<T> querable, IExpressionBuilder<T> builder, string expression)
        {
            return querable.Where(builder.Where(expression));
        }

        public static IQueryable<T> OrderBy<T, TOut>(this IQueryable<T> querable, IExpressionBuilder<T> builder, string expression)
        {
            return builder.OrderBy(querable, expression);
        }

        public static IQueryable<TOut> Select<T, TOut>(this IQueryable<T> querable, IExpressionBuilder<T> builder, Expression<Func<T, object>> expression = null, string include = null)
        {
            return querable.Select(builder.Select<TOut>(include));
        }

        public static IQueryable<TOut> Query<T, TOut>(this IQueryable<T> querable, IExpressionBuilder<T> builder, string filter = null, string orderby = null, int skip = -1, int take = -1, string include = null)
        {
            return builder.Query<TOut>(querable, filter, orderby, skip, take, include);
        }

        public static IQueryable<T> Query<T>(this IQueryable<T> querable, IExpressionBuilder<T> builder, string filter = null, string orderby = null, int skip = -1, int take = -1)
        {
            return builder.Query(querable, filter, orderby, skip, take);
        }

        public static IQueryable<T> Query<T>(this IQueryable<T> querable, string filter = null, string orderby = null, int skip = -1, int take = -1)
        {
            ExpressionBuilder<T> builder = new ExpressionBuilder<T>();
            return builder.Query(querable, filter, orderby, skip, take);
        }


        public static IQueryable<T> Where<T>(this IQueryable<T> querable, IExpressionBuilder<T> builder, string expression)
        {
            if (expression == null)
                return querable;
            return querable.Where(builder.Where(expression));
        }

        public static Expression<Func<T, bool>> Combine<T>(Expression<Func<T, bool>> exp1, Expression<Func<T, bool>> exp2, bool or)
        {
            if (exp1 == null && exp2 == null) return null;
            else if (exp1 == null) return exp2;
            else if (exp2 == null) return exp1;

            var body1 = exp1.Body;
            var body2 = exp2.Body;

            var parameter = exp1.Parameters[0];
            Expression exp = or ? Expression.OrElse(exp1, exp2) : Expression.AndAlso(exp1, exp2);

            ParamExpresionVisitor visitor = new ParamExpresionVisitor { ReplaceParameter = parameter };
            exp = visitor.Visit(exp);

            return Expression.Lambda<Func<T, bool>>(exp, parameter);
        }

        public static Expression<Func<T1, bool>> Combine<T1, T2>(Expression<Func<T1, T2>> targetExp, Expression<Func<T2, bool>> expression)
        {
            ParamExpresionVisitor visitor = new ParamExpresionVisitor { ReplaceParameter = targetExp.Body };
            var exp = visitor.Visit(expression.Body);

            Expression current = targetExp.Body;
            while (current != null && !(current is ParameterExpression))
            {
                current = ((MemberExpression)current).Expression;
            }

            return Expression.Lambda<Func<T1, bool>>(exp, (ParameterExpression)current);
        }


        public static Expression<Func<T1, object>> Combine<T1, T2>(Expression<Func<T1, T2>> targetExp, Expression<Func<T2, object>> expression)
        {
            ParamExpresionVisitor visitor = new ParamExpresionVisitor { ReplaceParameter = targetExp.Body };
            var exp = visitor.Visit(expression.Body);

            Expression current = targetExp.Body;
            while (current != null && !(current is ParameterExpression))
            {
                current = ((MemberExpression)current).Expression;
            }

            return Expression.Lambda<Func<T1, object>>(exp, (ParameterExpression)current);
        }
    }
}
