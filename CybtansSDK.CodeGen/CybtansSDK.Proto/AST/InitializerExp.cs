using Antlr4.Runtime;
using CybtansSdk.Proto.Options;
using System;
using System.Collections.Generic;

namespace CybtansSdk.Proto.AST
{
    public class InitializerExp: ExpressionNode
    {
        public InitializerExp(IToken start) : base(start)
        {
        }

        public List<MemberInitializerExp> Expressions { get; } = new List<MemberInitializerExp>();

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            foreach (var item in Expressions)
            {
                item.CheckSemantic(scope, logger);
            }
        }

        public void Initialize(ProtobufOption instance)
        {
            foreach (var item in Expressions)
            {
                instance.Set(item.Name, item.Value);
            }
        }
    }
}
