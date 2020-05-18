using Antlr4.Runtime;

namespace Cybtans.Proto.AST
{
    public class OptionIdentifierExpression : IdentifierExpression
    {
        public OptionIdentifierExpression(string id, IdentifierExpression left, IToken start) : base(id, left, start)
        {
        }

        public bool IsExtension { get; set; }

        public string ReservedWord { get; set; }
    }
}
