#nullable enable

using Cybtans.Proto.AST;
using System.Linq;

namespace Cybtans.Proto.Generators.CSharp
{
    public class EnumGenerator: SingleFileGenerator<TypeGeneratorOption>
    {
        public EnumGenerator(ProtoFile proto, TypeGeneratorOption option) : base(proto, option, 
            $"{proto.Option.Namespace}.{option.Namespace ?? "Models"}")
        {            
        }

        public override void OnGenerationBegin(CsFileWriter writer)
        {
            base.OnGenerationBegin(writer);
        }

        protected override void GenerateCode(ProtoFile proto)
        {
            foreach (var item in proto.Declarations)
            {
                if (item is EnumDeclaration e)
                {
                    var info = new EnumInfo(e, _option, proto);

                    var writer = new CodeWriter();
                    GenerateEnum(info, writer);
                    AddBlock(info.Name, writer.ToString());
                }
            }
        }

        protected override void SaveFile(CsFileWriter writer)
        {
            writer.Save("Enums");
        }


        private void GenerateEnum(EnumInfo info, CodeWriter clsWriter)
        {
            clsWriter.Append("public ");
            clsWriter.Append($"enum {info.Name} ").AppendLine();

            clsWriter.Append("{").AppendLine();
            clsWriter.Append('\t', 1);

            var bodyWriter = clsWriter.Block($"BODY_{info.Name}");

            foreach (var item in info.Fields.Values.OrderBy(x => x.Field.Value))
            {
                bodyWriter.Append(item.Name).Append(" = ").Append(item.Field.Value.ToString()).Append(",");
                bodyWriter.AppendLine();
                bodyWriter.AppendLine();
            }

            clsWriter.Append("}").AppendLine();
        }

    }




}
