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
        ParameterExpression _parameter;

        public FunctionCall(string name, Expression[] parameters, int col, int row)            
        {
            this.Col = col;
            this.Row = row;
            this.Name = name;
            this.Parameters = parameters ?? new Expression[0];

            if(Parameters.All(x=>x == null))
            {
                Parameters = new Expression[0];
            }
        }

        public string Name { get; private set; }

        public Expression[] Parameters { get;}

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

            if (Function.IsQueryMethod)
            {
                var genArg = Instance.NetType.GetGenericArguments();
                _parameter = System.Linq.Expressions.Expression.Parameter(genArg[0]);

                context = new QueryASTContext
                {
                    ModelType = genArg[0],
                    Parameter = _parameter
                };
            }

            foreach (var parameter in Parameters)
            {
                parameter.CheckSemantic(context);
            }

            if (!Function.IsQueryMethod && !Function.CheckParamterTypes(Parameters))
                throw new RecognitionException($"Function {Name} parameters does not match", Col, Row);
        }

        public override System.Linq.Expressions.Expression GenerateLinqExpression(IASTContext context)
        {          
            if (Function.IsQueryMethod)
            {
                var genArg = Instance.NetType.GetGenericArguments();
                var method = Function.Method.MakeGenericMethod(genArg[0]);
                if (Parameters.Length > 0)
                {

                    var lambdaExp = Parameters[0].GenerateLinqExpression(new QueryASTContext
                    {
                        ModelType = genArg[0],
                        Parameter = _parameter
                    });
                    var lambda = System.Linq.Expressions.Expression.Lambda(method.GetParameters()[1].ParameterType, lambdaExp, _parameter);

                    return System.Linq.Expressions.Expression.Call(Function.Method.DeclaringType, Function.Method.Name, new Type[] { genArg[0] },
                            new System.Linq.Expressions.Expression[]
                            {
                                Instance?.GenerateLinqExpression(context),
                                lambda
                            });
                }
                else
                {
                    return System.Linq.Expressions.Expression.Call(Function.Method.DeclaringType, Function.Method.Name, new Type[] { genArg[0] },
                                new System.Linq.Expressions.Expression[]
                                {
                                Instance?.GenerateLinqExpression(context)
                                });
                }
            }
            else
            {
                System.Linq.Expressions.Expression[] argsExp = new System.Linq.Expressions.Expression[Parameters.Length];
                for (int i = 0; i < Parameters.Length; i++)
                {
                    argsExp[i] = Parameters[i].GenerateLinqExpression(context);
                }

                System.Linq.Expressions.Expression instanceExp = Function.Target != null ?
                    System.Linq.Expressions.Expression.Constant(Function.Target)
                   : Instance?.GenerateLinqExpression(context);

                var callExp = System.Linq.Expressions.Expression.Call(instanceExp, Function.Method);
                return System.Linq.Expressions.Expression.Invoke(callExp, argsExp);
            }
           
        }

        public override void GenOData(IASTContext context, StringBuilder sb, int tabOffset)
        {
            throw new NotImplementedException();
        }

        public override void GenSQL(IASTContext context, StringBuilder sb, int tabOffset)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Name}({Parameters.Aggregate("", (x, y) => x.ToString() + ", " + y.ToString())})";
        }
    }
}
