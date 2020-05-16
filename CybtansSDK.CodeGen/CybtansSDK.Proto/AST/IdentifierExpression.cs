using Antlr4.Runtime;
using System;
using System.Text;

namespace CybtansSdk.Proto.AST
{
    public class IdentifierExpression:ExpressionNode
    {
      
        public IdentifierExpression(string id, IdentifierExpression left, IToken start)
            :base(start)
        {
            Id = id;
            Left = left;
        }

        public IdentifierExpression(string id)
        {
            Id = id;
        }

        public IdentifierExpression Left { get; }

        public string Id { get; }

        public override string ToString()
        {
            return Left != null ? $"{Left}.{Id}" : Id;
        }

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
           
        }
    }
}
