using Cybtans.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions.Ast
{
    public class MemberExpression:Expression
    {
        PropertyInfo pi;

        public string Name { get; set; }

        public Expression Left { get; set; }

        public MemberExpression(string name, Expression left)
        {
            Name = name;
            Left = left;
        }

        public override System.Linq.Expressions.Expression GenerateLinqExpression(IASTContext context)
        {
            var leftExp = Left.GenerateLinqExpression(context);
            var leftModelType = ((PropertyInfo)((System.Linq.Expressions.MemberExpression)leftExp).Member).PropertyType;

            NavigationPropertyAttribute navAttr = pi.GetCustomAttribute<NavigationPropertyAttribute>();
            if (navAttr != null)
            {
                var modelProperty = leftModelType.GetProperty(navAttr.NavigationProperty);
                var linqExp = System.Linq.Expressions.Expression.Property(leftExp, modelProperty);
                modelProperty = modelProperty.PropertyType.GetProperty(navAttr.Property);
                linqExp = System.Linq.Expressions.Expression.Property(linqExp, modelProperty);
                return linqExp;
            }

            var expression = System.Linq.Expressions.Expression.Property(leftExp,  pi.Name);
            return expression;
        }

        public override void CheckSemantic(IASTContext context)
        {
            Left.CheckSemantic(context);

            if (Left.NetType == null)
                throw new RecognitionException(".Net Type not recognized");          

            pi = Left.NetType.GetProperty(Name);
            if(pi == null)
            {
                throw new RecognitionException($"Property {Name} not found in {Left.NetType.Name}");
            }

            NetType = pi.PropertyType;
            type = GetExpressionType(pi.PropertyType);
        }

        public override void GenSQL(IASTContext context, StringBuilder sb, int tabOffset)
        {
            throw new NotImplementedException();
        }

        public override void GenOData(IASTContext context, StringBuilder sb, int tabOffset)
        {
            throw new NotImplementedException();
        }
    }
}
