using Antlr4.Runtime;
using System.Collections.Generic;

namespace CybtansSdk.Proto.AST
{
    public abstract class ProtoAstNode
    {
        public int Line { get; set; }

        public int Column { get; set; }

        public ProtoAstNode(IToken start)
        {
            Line = start.Line;
            Column = start.Column;           
        }

        public ProtoAstNode(int line, int column)
        {
            this.Line = line;
            this.Column = column;
        }

        public ProtoAstNode()
        {

        }

        public abstract void CheckSemantic(Scope scope, IErrorReporter logger);
    }

    public enum ProtoNodeType
    {
        Import,
        Package,

    }
   
}
