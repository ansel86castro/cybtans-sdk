using System;
using System.Text;

namespace Cybtans.Expressions.Ast
{
    public class LiteralExpression : Expression
    {

        String value;
        private TokenType tokenType;

        public LiteralExpression(String value, ExpressionType type)
        {
            this.value = value;
            this.type = type;
        }

        public LiteralExpression(String value, ExpressionType type, TokenType tokenType)
            : this(value, type)
        {
            this.tokenType = tokenType;
        }

        public string Value { get { return value; } }


        public override void CheckSemantic(IASTContext context)
        {
            if (type == null)
                throw new RecognitionException(String.Format("missing type for {0}", value), Col, Row);

        }

        public override void GenSQL(IASTContext context, StringBuilder sb, int tabOffset)
        {
            sb.Append('\t', tabOffset);
            if (type == ExpressionType.String)
            {
                sb.appendEscapedSQLString(value);
            }
            else if (type == ExpressionType.Null)
                sb.Append("NULL");
            else
                sb.Append(value);
        }


        public override void GenOData(IASTContext context, StringBuilder sb, int tabOffset)
        {
            if (type == ExpressionType.String)
            {
                sb.appendEscapedSQLString(value);
            }
            else if (type == ExpressionType.Null)
                sb.Append("null");
            else
                sb.Append(value);

        }


        public override System.Linq.Expressions.Expression GenerateLinqExpression(IASTContext context)
        {
            if (type == ExpressionType.String)
            {
                return System.Linq.Expressions.Expression.Constant(value, typeof(string));
            }
            else if (type == ExpressionType.Null)
            {
                return System.Linq.Expressions.Expression.Constant(null);
            }
            else if (type == ExpressionType.Bool)
            {
                return System.Linq.Expressions.Expression.Constant(bool.Parse(value));
            }
            else if (type == ExpressionType.Integer)
            {
                return System.Linq.Expressions.Expression.Constant(int.Parse(value));
            }
            else if (type == ExpressionType.Double)
            {
                return System.Linq.Expressions.Expression.Constant(double.Parse(value));
            }
            throw new InvalidOperationException();
        }

        public object GetValue()
        {
            if (type == ExpressionType.String)
            {
                return value;
            }
            else if (type == ExpressionType.Null)
            {
                return null;
            }
            else if (type == ExpressionType.Bool)
            {
                return bool.Parse(value);
            }
            else if (type == ExpressionType.Integer)
            {
               return int.Parse(value);
            }
            else if (type == ExpressionType.Double)
            {
              return double.Parse(value);
            }

            throw new InvalidOperationException();
        }

        public bool TryGetValue(Type converType, out object value)
        {
            Type nullableArg = Nullable.GetUnderlyingType(converType);
            if (nullableArg != null)
            {
                return _TryGetValue(nullableArg, out value);
            }

            return _TryGetValue(converType, out value);
        }

        private bool _TryGetValue(Type converType, out object value)
        {
            value = GetValue();
            try
            {
                if (value is IConvertible convertible)
                {
                    value = convertible.ToType(converType, null);
                }
                else
                {
                    value = Convert.ChangeType(value, converType);
                }
                return true;
            }
            catch
            {
                return false;
            }           
        }
    }
}
