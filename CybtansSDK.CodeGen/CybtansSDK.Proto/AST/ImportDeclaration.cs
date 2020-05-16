using Antlr4.Runtime;

namespace CybtansSdk.Proto.AST
{
    public class ImportDeclaration:ProtoAstNode
    {
        public ImportDeclaration(IToken start, ImportType type, string path) : base(start)
        {
            ImportType = type;
            Name = path;
        }

        public ImportType ImportType { get; set; }      

        public string Name { get; set; }

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            
        }
    }

    public enum ImportType
    {
        Weak,Public
    }
}
