using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Proto.AST
{
    public class PackageDeclaration:ProtoAstNode
    {
        public PackageDeclaration(IToken start, IdentifierExpression id):base(start)
        {
            Id = id;
            Name = id.Id;
        }

        public IdentifierExpression Id { get; }

        public string Name { get; set; }

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
