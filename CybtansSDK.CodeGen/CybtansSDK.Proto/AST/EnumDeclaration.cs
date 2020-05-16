using Antlr4.Runtime;
using CybtansSdk.Proto.Options;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace CybtansSdk.Proto.AST
{
    public class EnumDeclaration:TypeDeclaration<EnumOptions>
    {
        public EnumDeclaration(string name, IToken start) : base(start, name)
        {
            Nullable = true;
        }      

        public List<EnumMemberDeclaration> Members { get; } = new List<EnumMemberDeclaration>();

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {           
            base.CheckSemantic(scope, logger);

            foreach (var member in Members)
            {
                member.CheckSemantic(scope, logger);
            }
        }
    }

    public class EnumMemberDeclaration : DeclarationNode<FieldOptions>
    {
        public EnumMemberDeclaration(string name, IToken start) : base(start)
        {
            Name = name;
        }

        public int Value { get; set; }        
    }
}
