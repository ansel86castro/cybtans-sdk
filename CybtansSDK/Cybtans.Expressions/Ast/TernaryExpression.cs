using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using LinqExpression = System.Linq.Expressions.Expression;

namespace Cybtans.Expressions.Ast
{
    public class TernaryExpression : Expression
    {
        public TernaryExpression(Expression condition, Expression onTrue, Expression onFalse)
        {
            this.Condition = condition;
            this.OnTrue = onTrue;
            this.OnFalse = onFalse;
        }

        public Expression Condition { get;  set; }

        public Expression OnTrue { get;  set; }

        public Expression OnFalse { get;  set; }

        public override void CheckSemantic(IASTContext context)
        {
            Condition.CheckSemantic(context);

            if (Condition.Type != ExpressionType.Bool)
                throw new RecognitionException($"Expression at Line {Row} Col {Col} does not return boolean value");

            OnTrue.CheckSemantic(context);

            OnFalse.CheckSemantic(context);

            type = ExpressionType.match(OnTrue.Type, OnFalse.Type);

            if (type == null)
            {
                throw new RecognitionException("Type mistmatch ", Col, Row);
            }

            NetType = OnTrue.NetType;
        }

        public override System.Linq.Expressions.Expression GenerateLinqExpression(IASTContext context)
        {
            GenerateLinqExpresions(context, OnTrue, OnFalse, out LinqExpression expLeft, out LinqExpression expRight);

            return LinqExpression.Condition(Condition.GenerateLinqExpression(context), expLeft, expRight);
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
