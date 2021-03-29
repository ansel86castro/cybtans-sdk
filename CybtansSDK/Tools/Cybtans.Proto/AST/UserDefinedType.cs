using Antlr4.Runtime;
using Cybtans.Proto.Options;

namespace Cybtans.Proto.AST
{

    public interface IUserDefinedType : ITypeDeclaration
    {
        MessageDeclaration DeclaringMessage { get; }

        string SourceName { get; }
    }

    public abstract class UserDefinedType<TOption> : TypeDeclaration<TOption> ,IUserDefinedType
          where TOption : ProtobufOption, new()
    {
        public UserDefinedType()
        {
        }

        public UserDefinedType(IToken start) : base(start)
        {
        }

        public UserDefinedType(IToken start, string name) : base(start, name)
        {
        }

        public string SourceName { get; set; }

        public MessageDeclaration DeclaringMessage { get; set; }

    }


}
