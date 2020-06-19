using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions.Ast
{
    public class FunctionDeclaration:DeclarationNode
    {
        public FunctionDeclaration(string name, MethodInfo method) : base(name)
        {
            this.Method = method;
            this.ReturnType = Expression.GetExpressionType(method.ReturnType);
            Parameters = method.GetParameters();
        }

        public FunctionDeclaration(MethodInfo method):this(method.Name, method)
        {
          
        }

        public ExpressionType ReturnType { get; set; }

        public MethodInfo Method { get; set; }

        public Type DeclaringType { get { return Method?.DeclaringType; } }

        public Type ReturnNetType => Method.ReturnType;

        public ParameterInfo[] Parameters { get; private set; }

        public object Target { get; set; }

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

        public bool CheckParamterTypes(Expression[] expressions)
        {
            if (expressions.Length != Parameters.Length)
                return false;
            
            
            for (int i = 0; i < expressions.Length; i++)
            {
                var p = Parameters[i  + (Target != null ? 1 : 0)];
                
                var exp = expressions[i];

                if (!p.ParameterType.IsAssignableFrom(exp.NetType))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
