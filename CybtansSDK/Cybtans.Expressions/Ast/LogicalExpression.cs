using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions.Ast
{
    public class LogicalExpression:BinaryExpression
    {
        public LogicalExpression(Expression left, Expression right, Operator op)
            :base(left, right, op)
        {
        }
    }
}
