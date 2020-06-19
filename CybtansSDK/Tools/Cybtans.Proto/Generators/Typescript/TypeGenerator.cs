using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cybtans.Proto.Generators.Typescript
{

    public class TypeGenerator
    {
        ProtoFile _proto;
        TsOutputOption _option;
        public TypeGenerator(ProtoFile proto, TsOutputOption option)
        {
            _proto = proto;
            _option = option;
        }

        public void GenerateCode()
        {
            Directory.CreateDirectory(_option.OutputDirectory);

            var writer = CreateWriter();
            foreach (var item in _proto.ImportedFiles)
            {                
                GenerateCode(item, writer);
            }

            GenerateCode(_proto, writer);

            writer.Save("models");
        }

        private void GenerateCode(ProtoFile proto, TsFileWriter writer)
        {
            foreach (var item in proto.Declarations)
            {
                if (item is MessageDeclaration msg)
                {
                    GenerateMessage(msg, writer.Writer);
                }

                else if (item is EnumDeclaration e)
                {
                    GenerateEnum(e, writer.Writer);
                }
            }
        }

        private void GenerateEnum(EnumDeclaration e, CodeWriter writer)
        {
            writer.AppendLine(2);

            writer.Append($"export enum {e.GetTypeName()} {{").AppendLine();

            foreach (var field in e.Members)
            {
                writer.Append('\t', 1)
                    .Append($"{field.Name.Camel()} = {field.Value},")
                    .AppendLine();
            }

            writer.Append("}");

            writer.AppendLine(2);
        }

        private void GenerateMessage(MessageDeclaration msg, CodeWriter writer)
        {
            writer.AppendLine();

            writer.Append($"export interface {msg.GetTypeName()} {{").AppendLine();

            foreach (var field in msg.Fields)
            {
                writer.Append('\t', 1)
                    .Append($"{field.Name.Camel()}{IsOptional(field)} : {field.GetTypeName()};")
                    .AppendLine();
            }

            writer.Append("}");

            writer.AppendLine();
        }

        public TsFileWriter CreateWriter()
        {
            return new TsFileWriter(_option.OutputDirectory ?? Environment.CurrentDirectory);
        }

        public string IsOptional(FieldDeclaration field)
        {
            if (field.Option.Optional && field.Type.TypeDeclaration.Nullable)
            {
                //check ist the type is nullable
                return "?";
            }
            return "";
        }
    }
}
