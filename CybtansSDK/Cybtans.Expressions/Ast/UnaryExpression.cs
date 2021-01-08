using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions.Ast
{
    public class UnaryExpression : Expression
    {
        Expression expression;
        Operator op;

        public UnaryExpression(Expression expression, Operator op)
        {
            this.expression = expression;
            this.op = op;
        }

        public override void CheckSemantic(IASTContext context)
        {
            expression.CheckSemantic(context);
            type = expression.Type;
        }



        public override void GenSQL(IASTContext context, StringBuilder sb, int tabOffset)
        {
            sb.Append('\t', tabOffset);

            sb.Append(BinaryExpression.getSQLOperator(op));
            expression.GenSQL(context, sb, 0);
        }


        public override void GenOData(IASTContext context, StringBuilder sb, int tabOffset)
        {
            sb.Append(BinaryExpression.getODataOperator(op));
            expression.GenOData(context, sb, 0);

        }


        public override System.Linq.Expressions.Expression GenerateLinqExpression(IASTContext context)
        {
            var linqExp = expression.GenerateLinqExpression(context);
            return System.Linq.Expressions.Expression.Negate(linqExp);
        }

        public override string ToString()
        {
            return $"{op} {expression}";
        }
    }
}
