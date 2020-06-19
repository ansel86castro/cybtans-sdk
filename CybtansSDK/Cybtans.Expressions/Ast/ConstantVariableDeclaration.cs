using System;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Cybtans.Expressions.Ast
{
    public class ConstantVariableDeclaration : VariableDeclaration
    {
        Object value;
        Func<Object> resolver;

        public ConstantVariableDeclaration(string name, Object value, Type type):base(name, type)
        {
            this.value = value;            
        }

        public ConstantVariableDeclaration(string name, Func<Object> resolver, Type type) : base(name, type)
        {
            this.resolver = resolver;
        }

     
        public object Value
        {
            get
            {
                return value ?? (value = resolver());
            }
        }

        protected override LinqExpression GenerateLinqExpression()
        {
            return LinqExpression.Constant(Value, NetType);
        }
    }
}
