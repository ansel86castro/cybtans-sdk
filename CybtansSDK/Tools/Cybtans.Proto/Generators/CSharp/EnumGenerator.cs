#nullable enable

using Cybtans.Proto.AST;
using System.Linq;

namespace Cybtans.Proto.Generators.CSharp
{
    public class EnumGenerator: SingleFileGenerator<TypeGeneratorOption>
    {
        public EnumGenerator(ProtoFile proto, TypeGeneratorOption option) : base(proto, option,
            option.Namespace ?? $"{proto.Option.Namespace}.Models")
        {            
        }

        public override void OnGenerationBegin(CsFileWriter writer)
        {
            writer.Usings.Append("using System.ComponentModel;").AppendLine();
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
            if(info.Enum.Option.Description != null)
            {
                clsWriter.Append("/// <summary>").AppendLine();
                clsWriter.Append("/// ").Append(info.Enum.Option.Description).AppendLine();
                clsWriter.Append("/// </summary>").AppendLine();
                clsWriter.Append($"[Description(\"{info.Enum.Option.Description}\")]").AppendLine();
            }

            if (info.Enum.Option.Deprecated)
            {
                clsWriter.Append($"[Obsolete]").AppendLine();
            }

            clsWriter.Append("public ");
            clsWriter.Append($"enum {info.Name} ").AppendLine();

            clsWriter.Append("{").AppendLine();
            clsWriter.Append('\t', 1);

            var bodyWriter = clsWriter.Block($"BODY_{info.Name}");

            foreach (var item in info.Fields.Values.OrderBy(x => x.Field.Value))
            {
                if (item.Field.Option.Description != null)
                {
                    bodyWriter.Append("/// <summary>").AppendLine();
                    bodyWriter.Append("/// ").Append(item.Field.Option.Description).AppendLine();
                    bodyWriter.Append("/// </summary>").AppendLine();
                    bodyWriter.Append($"[Description(\"{item.Field.Option.Description}\")]").AppendLine();
                }

                if (item.Field.Option.Deprecated)
                {
                    bodyWriter.Append($"[Obsolete]").AppendLine();
                }

                bodyWriter.Append(item.Name).Append(" = ").Append(item.Field.Value.ToString()).Append(",");
                bodyWriter.AppendLine();
                bodyWriter.AppendLine();
            }

            clsWriter.Append("}").AppendLine();
        }

    }




}
