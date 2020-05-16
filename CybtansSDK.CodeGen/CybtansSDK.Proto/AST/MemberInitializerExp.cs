using Antlr4.Runtime;
using System.Net.Sockets;

namespace CybtansSdk.Proto.AST
{
    public class MemberInitializerExp:ExpressionNode
    {
        public MemberInitializerExp(IToken start, string name, ConstantExp value) 
            : base(start)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public ConstantExp Value { get; set; }

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            Value.CheckSemantic(scope, logger);
        }
    }
}
