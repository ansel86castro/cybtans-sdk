using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions.Ast
{
    public class FunctionCall : Expression
    {
        
        public FunctionCall(string name, Expression[] parameters, int col, int row)            
        {
            this.Col = col;
            this.Row = row;
            this.Name = name;
            this.Parameters = parameters;
        }

        public string Name { get; private set; }

        public Expression[] Parameters { get; private set; }

        public Expression Instance { get; set; }

        public FunctionDeclaration Function { get; set; }

        public override void CheckSemantic(IASTContext context)
        {
            if (Instance != null)
            {
                Instance.CheckSemantic(context);
                Function = context.GetFunctionDeclaration(Instance.NetType, Name);
            }
            else
            {
                Function = context.GetFunctionDeclaration(Name);
            }

            if (Function == null)
                throw new RecognitionException($"Function {Name} not found in context", Col, Row);

            Type = Function.ReturnType;
            NetType = Function.ReturnNetType;

            foreach (var parameter in Parameters)
            {
                parameter.CheckSemantic(context);
            }

            if (!Function.CheckParamterTypes(Parameters))
                throw new RecognitionException($"Function {Name} parameters does not match", Col, Row);
            
        }

        public override System.Linq.Expressions.Expression GenerateLinqExpression(IASTContext context)
        {
            System.Linq.Expressions.Expression instanceExp = Function.Target !=null ?
                 System.Linq.Expressions.Expression.Constant(Function.Target)
                : Instance?.GenerateLinqExpression(context);

            var callExp = System.Linq.Expressions.Expression.Call(instanceExp, Function.Method);

            System.Linq.Expressions.Expression[] argsExp = new System.Linq.Expressions.Expression[Parameters.Length];

            for (int i = 0; i < Parameters.Length; i++)
            {
                argsExp[i] = Parameters[i].GenerateLinqExpression(context);
            }

            return System.Linq.Expressions.Expression.Invoke(callExp, argsExp);
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
