using Antlr4.Runtime;
using Cybtans.Proto.Options;
using System;
using System.Linq;

namespace Cybtans.Proto.AST
{
    public interface ITypeDeclaration
    {
        public string Name { get; }

        public string Package { get; }

        public bool IsBuildIn { get; }

        public bool IsChecked { get; }

        public bool IsValueType { get; }        

        public bool HasStreams()
        {
            if (this == PrimitiveType.Stream)
                return true;

            var msg = this as MessageDeclaration;
            if (msg == null)
                return false;

            foreach (var field in msg.Fields)
            {
                if (field.FieldType == PrimitiveType.Stream)
                    return true;
            }

            if (msg.Fields.Any(x => x.FieldType is MessageDeclaration && x.FieldType.HasStreams()))
                throw new InvalidOperationException($"Streams are only allowed a the root message in  {msg.Name}");

            return false;
        }
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

        public bool IsValueType { get; set; }

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            IsChecked = true;

            base.CheckSemantic(scope, logger);
        }
    }
}
