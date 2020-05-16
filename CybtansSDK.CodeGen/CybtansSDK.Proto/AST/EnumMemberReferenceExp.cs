using Antlr4.Runtime;

namespace CybtansSdk.Proto.AST
{
    public class EnumMemberReferenceExp : ConstantExp
    {
        public EnumMemberReferenceExp(IToken start, IdentifierExpression identifier) 
            : base(start, null)
        {
            Name = identifier;            
        }

        public IdentifierExpression Name { get; }

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            Name.CheckSemantic(scope, logger);

            var enumDecl = scope.GetDeclaration(Name.Left) as EnumDeclaration;
            if (enumDecl == null)
            {
                logger.AddError($"Missing Declaration {Name.Left}");
            }

            var field = enumDecl.Members.Find(x => x.Name == Name.Id);

            Value = field.Value;
        }
    }

}
