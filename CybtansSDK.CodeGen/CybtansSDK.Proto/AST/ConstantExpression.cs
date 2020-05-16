using Antlr4.Runtime;

namespace CybtansSdk.Proto.AST
{

    public class ConstantExp: ExpressionNode 
    {
        public ConstantExp(IToken start, object value) : base(start)
        {
            Value = value;
        }

        public object Value { get; set; }

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            
        }
        public override string ToString()
        {
            return Value?.ToString();
        }
    }

}
