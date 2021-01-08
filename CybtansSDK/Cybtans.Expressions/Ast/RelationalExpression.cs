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
            type = ExpressionType.Bool;
            NetType = typeof(bool);

            Left.CheckSemantic(context);

            Right.CheckSemantic(context);
        }

    }
}
