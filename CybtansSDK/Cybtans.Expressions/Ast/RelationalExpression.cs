using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions.Ast
{
    public class RelationalExpression: BinaryExpression
    {
        public RelationalExpression(Expression left, Expression right, Operator op)
            :base(left, right, op)
        {

        }

        public override void CheckSemantic(IASTContext context)
        {
            base.CheckSemantic(context);

            if (type == ExpressionType.Null)
                throw new RecognitionException("Null type not allowed for a relational expression", Col,Row);

            type = ExpressionType.Bool;
            NetType = typeof(bool);
        }

    }
}
