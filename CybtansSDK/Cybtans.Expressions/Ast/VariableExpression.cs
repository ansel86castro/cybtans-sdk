using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions.Ast
{
    public class VariableExpression : Expression
    {
        String name;
        VariableDeclaration declaration;

        public VariableExpression(String name)
        {
            this.name = name;
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }            

        public override void CheckSemantic(IASTContext context)
        {
            declaration = context.GetVariableDeclaration(name);
            if(declaration == null)
            {
                throw new RecognitionException($"Variable {name} not found in context");
            }
            Type = declaration.Type;
            NetType = declaration.NetType;
        }

        public override void GenSQL(IASTContext context, StringBuilder sb, int tabOffset)
        {
            //sb.Append('\t', tabOffset);

            //String column = modelProperty.Name;
            //sb.Append(column);

        }

        public override void GenOData(IASTContext context, StringBuilder sb, int tabOffset)
        {
            //sb.Append(modelProperty.Name);
        }


        public override System.Linq.Expressions.Expression GenerateLinqExpression(IASTContext context)
        {

            return declaration.LinqExpression;

            //responseProperty = context.ResponseType.GetProperty(name);
            //if (responseProperty == null)
            //    throw new RecognitionException("Property not found " + name + " in " + context.ResponseType.Name);

            //if (context.ResponseType == context.ModelType)
            //{
            //    modelProperty = responseProperty;
            //    linqExpression = System.Linq.Expressions.Expression.Property(context.Parameter, modelProperty);
            //}
            //else
            //{
            //    NavigationPropertyAttribute navAttr = responseProperty.GetCustomAttribute<NavigationPropertyAttribute>();
            //    if (navAttr != null)
            //    {
            //        modelProperty = context.ModelType.GetProperty(navAttr.NavigationProperty);
            //        linqExpression = System.Linq.Expressions.Expression.Property(context.Parameter, modelProperty);
            //        modelProperty = modelProperty.PropertyType.GetProperty(navAttr.Property);
            //        linqExpression = System.Linq.Expressions.Expression.Property(linqExpression, modelProperty);
            //    }
            //    else
            //    {
            //        modelProperty = context.ModelType.GetProperty(name);
            //        linqExpression = System.Linq.Expressions.Expression.Property(context.Parameter, modelProperty);
            //    }
            //}

            //type = Expression.GetExpressionType(responseProperty.PropertyType);
            //NetType = responseProperty.PropertyType;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
