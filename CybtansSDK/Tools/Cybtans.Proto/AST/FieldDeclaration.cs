using Antlr4.Runtime;
using Cybtans.Proto.Options;
using System.Collections.Generic;
using System.Linq;

namespace Cybtans.Proto.AST
{
    public class FieldDeclaration: DeclarationNode<FieldOptions>
    {        
        public FieldDeclaration(IToken start) : base(start)
        {
            
        }

        public FieldDeclaration(IToken start, TypeIdentifier typeRef, string name, int number, List<OptionsExpression> options)
            : this(start)
        {
            Type = typeRef;
            Name = name;
            Number = number;

            if (options != null)
            {
                Options = options;
            }
        }
       
        public TypeIdentifier Type { get; set; }              

        public int Number { get; set; }

        public MessageDeclaration Message { get; set; }

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            base.CheckSemantic(scope, logger);

            Type.CheckSemantic(scope, logger);
        }

        public override string ToString()
        {
            return Type.ToString() +" "+base.ToString();
        }
    }

    public class TypeIdentifier:ProtoAstNode
    {
        public TypeIdentifier() { }

        public TypeIdentifier(IdentifierExpression name)
        {
            this.Name = name;
        }

        public TypeIdentifier(string name)
        {
            this.Name = new IdentifierExpression(name);
        }

        public IdentifierExpression Name { get; set; }

        public bool IsArray { get; set; }

        public bool IsMap { get; set; }

        public TypeIdentifier[] GenericArgs { get; set; }

        public ITypeDeclaration TypeDeclaration { get; set; }

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            TypeDeclaration = scope.GetDeclaration(Name);
            if(Type == null)
            {
                logger.AddError($"Type {Name} is not defined at {Name.Line},{Name.Column}");
            }

            if(GenericArgs != null)
            {
                foreach (var genParameter in GenericArgs)
                {
                    genParameter.CheckSemantic(scope, logger);
                }
            }

        }

        public string Type
        {
            get
            {
                if (GenericArgs == null || GenericArgs.Length == 0)
                    return Name.ToString();

                var genArgs = GenericArgs
                    .Select(x => x.Type)
                    .Aggregate((a, b) => $"{a},{b}");

                return $"{Name}<{genArgs}>";
            }
        }

        public override string ToString()
        {
            return Type;
        }
    }
}
