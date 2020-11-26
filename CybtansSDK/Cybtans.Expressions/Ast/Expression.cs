using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LinqExpression = System.Linq.Expressions.Expression;

namespace Cybtans.Expressions.Ast
{
    public abstract class Expression : ASTNode
    {
       public static readonly string[] DateFormats = new string[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd" };

        protected ExpressionType type;

        public ExpressionType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public Type NetType { get; set; }

        public abstract System.Linq.Expressions.Expression GenerateLinqExpression(IASTContext context);

        public static ExpressionType GetExpressionType(Type type)
        {
            ExpressionType expType = ExpressionType.Custom;
            if (type == typeof(int) || type == typeof(int?) ||
                type == typeof(short) || type == typeof(short?) ||
                type == typeof(long) || type == typeof(long?) ||
                type == typeof(byte) || type == typeof(byte?) ||
                type == typeof(uint) || type == typeof(uint?) ||
                type == typeof(ulong) || type == typeof(ulong?))
            {
                expType = ExpressionType.Integer;
            }
            else if (type == typeof(bool) || type == typeof(bool?))
            {
                expType = ExpressionType.Bool;
            }
            else if (type == typeof(double) || type == typeof(double?) ||
                      type == typeof(float) || type == typeof(float?) ||
                      type == typeof(decimal) || type == typeof(decimal?))
            {
                expType = ExpressionType.Double;
            }
            else if (type == typeof(string))
            {
                expType = ExpressionType.String;
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                expType = ExpressionType.String;
            }
            else if (type == typeof(Guid) || type == typeof(Guid?))
            {
                expType = ExpressionType.String;
            }
            return expType;
        }

        public static void GenerateLinqExpresions(IASTContext context, Expression left, Expression right, out LinqExpression expLeft, out LinqExpression expRight)
        {
            expLeft = left.GenerateLinqExpression(context);

            if ((expLeft.Type == typeof(DateTime) || expLeft.Type == typeof(DateTime?)) ||
               ( expLeft.Type == typeof(Guid) || expLeft.Type == typeof(Guid?)))
            {
                if (right.Type == ExpressionType.String && right is LiteralExpression)
                {
                    var literal = (LiteralExpression)right;

                    if (expLeft.Type == typeof(Guid) || expLeft.Type == typeof(Guid?))
                    {
                        if (!Guid.TryParse(literal.Value, out var guid))
                        {
                            throw new RecognitionException($"Invalid guid format '{literal.Value}'", literal.Col, literal.Row);
                        }
                        expRight = LinqExpression.Constant(guid);
                    }
                    else
                    {
                        if (!DateTime.TryParse(literal.Value, out var date))//!DateTime.TryParseExact(literal.Value, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                        {
                            throw new RecognitionException($"Invalid date format '{literal.Value}'", literal.Col, literal.Row);
                        }
                        expRight = LinqExpression.Constant(date);
                    }
                }
                else
                {
                    expRight = right.GenerateLinqExpression(context);
                }
            }
            else
            {
                expRight = right.GenerateLinqExpression(context);
                if ((expRight.Type == typeof(DateTime) || expLeft.Type == typeof(DateTime?)) ||
                    (expRight.Type == typeof(Guid) || expLeft.Type == typeof(Guid?)))
                {
                    if (left.Type == ExpressionType.String && left is LiteralExpression)
                    {
                        var literal = (LiteralExpression)left;
                        if (expRight.Type == typeof(Guid) || expLeft.Type == typeof(Guid?))
                        {
                            if (!Guid.TryParse(literal.Value, out var guid))
                            {
                                throw new RecognitionException($"Invalid guid format '{literal.Value}'", literal.Col, literal.Row);
                            }
                            expLeft = LinqExpression.Constant(guid);
                        }
                        else
                        {
                            if (!DateTime.TryParse(literal.Value, out var date)) //!DateTime.TryParseExact(literal.Value, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                            {
                                throw new RecognitionException($"Invalid date format '{literal.Value}'", literal.Col, literal.Row);
                            }
                            expLeft = LinqExpression.Constant(date);
                        }
                    }
                }
            }
        }
      
    }
}
