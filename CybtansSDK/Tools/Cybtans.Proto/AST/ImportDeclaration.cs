using Antlr4.Runtime;

namespace Cybtans.Proto.AST
{
    public class ImportDeclaration:ProtoAstNode
    {
        public ImportDeclaration(IToken start, ImportType type, string path) : base(start)
        {
            ImportType = type;
            if (path.StartsWith('"'))
            {
                path = path.Substring(1, path.Length - 2);
            }
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
