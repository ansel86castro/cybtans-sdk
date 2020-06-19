using System.Reflection;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Cybtans.Expressions.Ast
{
    public class PropertyVariableDeclaration: VariableDeclaration
    {
        private LinqExpression expression;
        private PropertyInfo property;

        public PropertyVariableDeclaration(LinqExpression expression, PropertyInfo property) 
            :base(property.Name, property.PropertyType)
        {
            this.expression = expression;
            this.property = property;
        }

        
        protected override LinqExpression GenerateLinqExpression()
        {
            return LinqExpression.Property(expression, property);
        }
        
    }
}
