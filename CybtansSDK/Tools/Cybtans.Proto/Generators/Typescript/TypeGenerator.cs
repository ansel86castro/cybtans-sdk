using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System.Net.Http.Headers;
using System.Text;

namespace Cybtans.Proto.Generators.Typescript
{

    public class TypeGenerator: BaseSingleFileGenerator
    {              
        public TypeGenerator(ProtoFile proto, TsOutputOption option)
            :base(proto, option)
        {
            option.Filename ??= "models";
        }      

        protected override void GenerateCode(ProtoFile proto)
        {          
            foreach (var item in proto.Declarations)
            {
                if (item is MessageDeclaration msg)
                {
                    AddBlock(msg.Name,GenerateMessage(msg));
                }

                else if (item is EnumDeclaration e)
                {
                    AddBlock(e.Name, GenerateEnum(e));
                }
            }
        }

        private string GenerateEnum(EnumDeclaration e)
        {
            var writer = new CodeWriter();

            writer.AppendLine(2);

            writer.Append($"export enum {e.GetTypeName()} {{").AppendLine();

            foreach (var field in e.Members)
            {
                writer.Append(' ', 2)
                    .Append($"{field.Name.Camel()} = {field.Value},")
                    .AppendLine();
            }

            writer.Append("}");

            writer.AppendLine(2);

            return writer.ToString();
        }

        private string GenerateMessage(MessageDeclaration msg)
        {
            var writer = new CodeWriter();

            writer.AppendLine();

            writer.Append($"export interface {msg.GetTypeName()} {{").AppendLine();

            foreach (var field in msg.Fields)
            {
                writer.Append(' ', 2)
                    .Append($"{field.Name.Camel()}{IsOptional(field)}: {field.GetTypeName()};")
                    .AppendLine();
            }

            writer.Append("}");

            writer.AppendLine();

            return writer.ToString();
        }

    
        public string IsOptional(FieldDeclaration field)
        {
            if (field.Option.Optional || field.Type.TypeDeclaration.Nullable)
            {
                //check ist the type is nullable
                return "?";
            }
            return "";
        }
    }
}
