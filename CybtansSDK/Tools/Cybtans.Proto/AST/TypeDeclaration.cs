using Antlr4.Runtime;
using Cybtans.Proto.Options;

namespace Cybtans.Proto.AST
{
    public interface ITypeDeclaration
    {
        public string Name { get; }

        public string Package { get; }

        public bool IsBuildIn { get; }

        public bool IsChecked { get; }

        public bool Nullable { get; }
    }

    //public abstract class TypeDeclaration :DeclarationNode, ITypeDeclaration
    //{
          
    //    public TypeDeclaration(string name) : base()
    //    {
    //        Name = name;
    //    }              

    //    public string Package { get; set; }

    //    public bool IsBuildIn { get; protected set; }

    //    public bool IsChecked { get; set; }
      
    //    public override string ToString()
    //    {
    //        return Name;
    //    }              
    //}


    public abstract class TypeDeclaration<TOption> : DeclarationNode<TOption>, ITypeDeclaration
         where TOption : ProtobufOption, new()
    {
        public TypeDeclaration()
        {
        }

        public TypeDeclaration(IToken start) : base(start)
        {
        }

        public TypeDeclaration(IToken start, string name) : base(start, name)
        {
         
        }

        public string Package { get; set; }

        public bool IsBuildIn => false;

        public bool IsChecked { get; private set; }

        public bool Nullable { get; set; }

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            IsChecked = true;

            base.CheckSemantic(scope, logger);
        }
    }
}
