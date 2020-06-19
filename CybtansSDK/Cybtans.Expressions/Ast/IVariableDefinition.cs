using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions.Ast
{
    public interface IVariableDefinition
    {
        String Name { get; }

        String DeclaringType { get; }

        String FilterColumn { get; }

        Type NetType { get; }

        ExpressionType ExpressionType { get; }

        Object GetValue(Object target);

        void SetValue(Object target, Object value);

        System.Linq.Expressions.Expression GetLinqExpression();
    }

    public class MemberVariableDefinition : IVariableDefinition
    {
        System.Linq.Expressions.MemberExpression member;
        ExpressionType expType;
        
        public MemberVariableDefinition(System.Linq.Expressions.MemberExpression member)
        {
            this.member = member;            
            var type = member.Type;
           expType = GetExpressionType(type);
        }

        public static ExpressionType GetExpressionType(Type type)
        {
            ExpressionType expType = null;
            if (type == typeof(int) || type == typeof(int?) ||
                type == typeof(short) || type == typeof(short?))
            {
                expType = ExpressionType.Integer;
            }
            else if (type == typeof(bool) || type == typeof(bool?))
            {
                expType = ExpressionType.Bool;
            }
            else if (type == typeof(double) || type == typeof(double?) ||
                      type == typeof(float) || type == typeof(float?))
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
            return expType;
        }

        public System.Linq.Expressions.MemberExpression Member
        {
            get { return member; }
        }

        #region IVariableDefinition Members

        public string Name
        {
            get { return member.Member.Name; }            
        }

        public string DeclaringType
        {
            get { return member.Member.DeclaringType.Name; }            
        }


        public string FilterColumn
        {
            get { return null; }            
        }


        public ExpressionType ExpressionType
        {
            get { return expType; }            
        }


        public object GetValue(object target)
        {
            if (member.Member is PropertyInfo)
            {
                PropertyInfo pi = (PropertyInfo)member.Member;
                pi.GetValue(target);
            }
            return null;
        }

        public void SetValue(object target, object value)
        {
            if (member.Member is PropertyInfo)
            {
                PropertyInfo pi = (PropertyInfo)member.Member;
                pi.SetValue(target, value);
            }            
        }

        #endregion

        #region IVariableDefinition Members


        public System.Linq.Expressions.Expression GetLinqExpression()
        {
            return member;
        }

        #endregion

        #region IVariableDefinition Members


        public Type NetType
        {
            get { return member.Type; }
        }

        #endregion
    }

    public class PropertyVariableDefinition : IVariableDefinition
    {
        PropertyInfo pi;
        private Ast.ExpressionType expType;
        private System.Linq.Expressions.Expression parameter;
        System.Linq.Expressions.Expression expression;

        public PropertyVariableDefinition(System.Linq.Expressions.Expression parameter, PropertyInfo pi)
        {
            this.pi = pi;
            this.parameter = parameter;
            expType = MemberVariableDefinition.GetExpressionType(pi.PropertyType);
        }

        #region IVariableDefinition Members

        public string Name
        {
            get { return pi.Name; }
        }

        public string DeclaringType
        {
            get { return pi.DeclaringType.Name; }
        }

        public string FilterColumn
        {
            get { throw new NotImplementedException(); }
        }

        public ExpressionType ExpressionType
        {
            get { return expType; }
        }

        public object GetValue(object target)
        {
           return pi.GetValue(target);
        }

        public void SetValue(object target, object value)
        {
            pi.SetValue(target, value);
        }

        #endregion

        #region IVariableDefinition Members


        public System.Linq.Expressions.Expression GetLinqExpression()
        {
            if (expression == null)
            {
                expression = System.Linq.Expressions.Expression.Property(parameter, pi);
            }
            return expression;
        }

        #endregion

        #region IVariableDefinition Members


        public Type NetType
        {
            get { return pi.PropertyType; }
        }

        #endregion
    }
}
