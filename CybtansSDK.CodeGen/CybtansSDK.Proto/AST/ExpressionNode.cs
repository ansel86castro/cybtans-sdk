using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace CybtansSdk.Proto.AST
{
    public abstract class ExpressionNode : ProtoAstNode
    {
        public ExpressionNode()
        {
        }

        public ExpressionNode(IToken start) : base(start)
        {
        }

        public ExpressionNode(int line, int column) : base(line, column)
        {
        }      
    }

}
