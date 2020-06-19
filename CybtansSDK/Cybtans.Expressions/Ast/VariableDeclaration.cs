using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Cybtans.Expressions.Ast
{
    public abstract class VariableDeclaration : DeclarationNode
    {      
        Type netType;
        LinqExpression expression;

        public VariableDeclaration(string name):base(name)
        {
            
        }

        public VariableDeclaration(string name, Type variableType) : base(name)
        {
            this.netType = variableType;
            Type = Expression.GetExpressionType(variableType);
        }

        public ExpressionType Type { get; set; }

        public Type NetType { get { return netType; }  set { netType = value; } }        

        public LinqExpression LinqExpression
        {
            get
            {
                if(expression == null)
                {
                    expression = GenerateLinqExpression();
                }
                return expression;
            }
        }

        protected abstract LinqExpression GenerateLinqExpression();    

        public override void CheckSemantic(IASTContext context)
        {
            throw new NotImplementedException();
        }

        public override void GenOData(IASTContext context, StringBuilder sb, int tabOffset)
        {
            throw new NotImplementedException();
        }

        public override void GenSQL(IASTContext context, StringBuilder sb, int tabOffset)
        {
            throw new NotImplementedException();
        }
       
    }
}
