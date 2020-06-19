using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Cybtans.Proto.AST
{
    public class PackageDeclaration:ProtoAstNode, IEquatable<PackageDeclaration>
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

        public bool Equals([AllowNull] PackageDeclaration other)
        {
            if (other == null) return false;
            return other.Name == Name;
        }

        public override bool Equals(object obj)
        {            
            return Equals(obj as PackageDeclaration);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return Id.ToString();
        }      
    }
}
