using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LinqExpression = System.Linq.Expressions.Expression;

namespace Cybtans.Expressions.Ast
{
    public abstract class BinaryExpression : Expression
    {
       
        Expression left;
        Expression right;
        Operator op;

        public Expression Left
        {
            get { return left; }
            set { left = value; }
        }

        public Expression Right
        {
            get
            {
                return right;
            }
            set
            {
                right = value;
            }
        }

        public Operator Operator
        {
            get { return op; }
            set { op = value; }
        }

        public BinaryExpression()
        {

        }
      
        public BinaryExpression(Expression left, Expression right,
                Operator op)
        {
            this.left = left;
            this.right = right;
            this.op = op;
        }


        public override void CheckSemantic(IASTContext context)
        {
            left.CheckSemantic(context);

            right.CheckSemantic(context);

            type = ExpressionType.match(left.Type, right.Type);

            if (type == null)
            {
                throw new RecognitionException("Type mistmatch ", Col, Row);
            }

            NetType = left.NetType;
        }

        public override void GenSQL(IASTContext context, StringBuilder sb, int tabOffset)
        {
            sb.Append('\t', tabOffset);

            if (left is BinaryExpression)
            {
                sb.Append('(');
                left.GenSQL(context, sb, 0);
                sb.Append(')');
            }
            else
            {
                left.GenSQL(context, sb, 0);
            }

            sb.Append(' ');

            sb.Append(getSQLOperator(op));

            sb.Append(' ');

            if (right is BinaryExpression)
            {
                sb.Append('(');
                right.GenSQL(context, sb, 0);
                sb.Append(')');
            }
            else
            {
                right.GenSQL(context, sb, 0);
            }

        }

        public override void GenOData(IASTContext context, StringBuilder sb, int tabOffset)
        {
            if (left is BinaryExpression)
            {
                sb.Append('(');
                left.GenOData(context, sb, 0);
                sb.Append(')');
            }
            else
            {
                left.GenOData(context, sb, 0);
            }

            sb.Append(' ');

            sb.Append(getODataOperator(op));

            sb.Append(' ');

            if (right is BinaryExpression)
            {
                sb.Append('(');
                right.GenOData(context, sb, 0);
                sb.Append(')');
            }
            else
            {
                right.GenOData(context, sb, 0);
            }


        }

        public static String getSQLOperator(Operator op)
        {
            switch (op)
            {
                case Operator.OP_ADD: return "+";
                case Operator.OP_SUB: return "-";
                case Operator.OP_MUL: return "*";
                case Operator.OP_DIV: return "/";
                case Operator.OP_EQUAL: return "=";
                case Operator.OP_LESS: return "<";
                case Operator.OP_GREATHER: return ">";
                case Operator.OP_GREATHER_EQ: return ">=";
                case Operator.OP_LESS_EQ: return "<=";
                case Operator.OP_AND: return "AND";
                case Operator.OP_OR: return "OR";
                case Operator.OP_LIKE: return "LIKE";
                case Operator.OP_DISTINT: return "!=";
                default: return null;
            }
        }

        public static String getODataOperator(Operator op)
        {
            switch (op)
            {
                case Operator.OP_ADD: return "plus";
                case Operator.OP_SUB: return "sub";
                case Operator.OP_MUL: return "mul";
                case Operator.OP_DIV: return "div";
                case Operator.OP_EQUAL: return "eq";
                case Operator.OP_LESS: return "lt";
                case Operator.OP_GREATHER: return "gt";
                case Operator.OP_GREATHER_EQ: return "ge";
                case Operator.OP_LESS_EQ: return "le";
                case Operator.OP_AND: return "and";
                case Operator.OP_OR: return "or";
                case Operator.OP_LIKE: return "like";
                case Operator.OP_DISTINT: return "ne";
                default: return null;
            }
        }

        public override System.Linq.Expressions.Expression GenerateLinqExpression(IASTContext context)
        {
          
            GenerateLinqExpresions(context, left, right , out LinqExpression expLeft, out LinqExpression expRight);

            if (IsNullable(expLeft.Type) && !IsNullable(expRight.Type))
            {
                expRight = LinqExpression.MakeUnary(System.Linq.Expressions.ExpressionType.Convert, expRight, expLeft.Type);
            }
            else if (IsNullable(expRight.Type) && !IsNullable(expLeft.Type))
            {
                expLeft = LinqExpression.MakeUnary(System.Linq.Expressions.ExpressionType.Convert, expLeft, expRight.Type);
            }
            else if (expLeft.Type != expRight.Type)
            {
                expRight = LinqExpression.MakeUnary(System.Linq.Expressions.ExpressionType.Convert, expRight, expLeft.Type);
            }


            switch (op)
            {
                case Operator.OP_ADD:
                    return LinqExpression.Add(expLeft, expRight);
                case Operator.OP_SUB:
                    return LinqExpression.Subtract(expLeft, expRight);
                case Operator.OP_DIV:
                    return LinqExpression.Divide(expLeft, expRight);
                case Operator.OP_MUL:
                    return LinqExpression.Multiply(expLeft, expRight);
                case Operator.OP_AND:
                    return LinqExpression.AndAlso(expLeft, expRight);
                case Operator.OP_OR:
                    return LinqExpression.OrElse(expLeft, expRight);
                case Operator.OP_EQUAL:
                    return LinqExpression.Equal(expLeft, expRight);
                case Operator.OP_DISTINT:
                    return LinqExpression.NotEqual(expLeft, expRight);
                case Operator.OP_LIKE:
                    return CreateLike(expLeft, expRight);
                case Operator.OP_LESS:
                    return LinqExpression.LessThan(expLeft, expRight);
                case Operator.OP_GREATHER:
                    return LinqExpression.GreaterThan(expLeft, expRight);
                case Operator.OP_GREATHER_EQ:
                    return LinqExpression.GreaterThanOrEqual(expLeft, expRight);
                case Operator.OP_LESS_EQ:
                    return LinqExpression.LessThanOrEqual(expLeft, expRight);
                default:
                    throw new InvalidOperationException("operator not supported");
            }
        }

       
        private bool IsNullable(System.Type type)
        {
            return type == typeof(int?) ||
                type == typeof(short?) ||
                type == typeof(byte?) ||
                type == typeof(long?) ||
                type == typeof(float?) ||
                type == typeof(double?) ||
                type == typeof(bool?) ||
                type == typeof(DateTime?) ||
                type == typeof(Guid?) ;
        }

        public LinqExpression CreateLike(LinqExpression expleft, LinqExpression expright)
        {
            LiteralExpression lit = (LiteralExpression)right;                        
            var value = lit.Value;
            
            if(string.IsNullOrWhiteSpace(value))
            {
                return LinqExpression.Equal(expleft, expright);
            }

            MethodInfo method = null;
            string compareValue = value;
            if(value[0] == '%' && value[value.Length-1]=='%')
            {
                method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
                compareValue = value.Substring(1, value.Length-2);
            }
            else if(value[0] == '%')
            {                
                method = typeof(String).GetMethod("EndsWith", new Type[]{typeof(String)});
                compareValue = value.Substring(1, value.Length-1);
            }
            else if(value[value.Length-1] == '%')
            {                
                method = typeof(String).GetMethod("StartsWith", new Type[] { typeof(String) });
                compareValue = value.Substring(0, value.Length-1);                              
            }

            if(method !=null)
            {
                return LinqExpression.Call(expleft, method, LinqExpression.Constant(compareValue, typeof(String)));
            }

            return LinqExpression.Equal(expleft, expright);
        }

        public override string ToString()
        {
            return $"{Left} {op} {Right}";
        }
    }
}
