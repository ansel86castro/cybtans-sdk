#nullable enable

using CybtansSdk.Proto.AST;
using CybtansSdk.Proto.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows.Markup;

namespace CybtansSdk.Proto.Generators.CSharp
{
    public class TypeGeneratorOption : OutputOption
    {
        public bool PartialClass { get; set; } = true;       
    }

    public class TypeGenerator : FileGenerator
    {           
        public TypeGenerator(ProtoFile proto, TypeGeneratorOption option):base(proto, option)
        {
            Namespace = $"{proto.Option.Namespace}.{option.Namespace ?? "Models"}";
        }

        public Dictionary<EnumDeclaration, EnumInfo> Enums { get; } = new Dictionary<EnumDeclaration, EnumInfo>();

        public Dictionary<ITypeDeclaration, MessageClassInfo> Messages { get; } = new Dictionary<ITypeDeclaration, MessageClassInfo>();

        public string Namespace { get; set; }

        public override void GenerateCode()
        {
            Directory.CreateDirectory(_option.OutputDirectory);

            CsFileWriter? enumWriter = null;
            bool hasEnums = _proto.Declarations.Any(x => x is EnumDeclaration);
            if (hasEnums)
            {
                var enumInfo = new EnumInfo((EnumDeclaration)_proto.Declarations.First(x => x is EnumDeclaration), _option, _proto);
                enumWriter = CreateWriter(enumInfo.Name);                
            }


            foreach (var item in _proto.Declarations)
            {
                if (item is MessageDeclaration msg)
                {                    
                    var info = new MessageClassInfo(msg, _option, _proto);

                    GenerateMessage(info);                

                    Messages.Add(msg, info);
                }

                else if (item is EnumDeclaration e && enumWriter!=null)
                {
                    var info = new EnumInfo(e, _option, _proto);
                    
                    GenerateEnum(info, enumWriter.Class);

                    Enums.Add(e, info);
                }
            }

            if (enumWriter != null)
            {
                enumWriter.Save("Enums");               
            }
        }
     
        private void GenerateEnum(EnumInfo info, CodeWriter clsWriter)
        {                     
            clsWriter.Append("public ");
            clsWriter.Append($"enum {info.Name} ").AppendLine();

            clsWriter.Append("{").AppendLine();
            clsWriter.Append('\t', 1);

            var bodyWriter = clsWriter.Block($"BODY_{info.Name}");

            foreach (var item in info.Fields.Values.OrderBy(x=>x.Field.Value))
            {                               
                bodyWriter.Append(item.Name).Append(" = ").Append(item.Field.Value.ToString()).Append(";");
                bodyWriter.AppendLine();
                bodyWriter.AppendLine();
            }
            
            clsWriter.Append("}").AppendLine();            
        }

        private void GenerateMessage(MessageClassInfo info)
        {
            var writer = CreateWriter(info.Namespace);

            var clsWriter = writer.Class;
            var usingWriter = writer.Usings;

            MessageDeclaration msg = info.Message;

        
            clsWriter.Append("public ");

            if (_option.PartialClass)
            {
                clsWriter.Append("partial ");
            }

            clsWriter.Append($"class {info.Name} ");
            
            if(msg.Option.Base != null)
            {
                clsWriter.Append($": {msg.Option.Base}");
            }
            clsWriter.AppendLine();
            clsWriter.Append("{").AppendLine();
            clsWriter.Append('\t', 1);

            var bodyWriter = clsWriter.Block("BODY");

            if(msg.Fields.Any(x=>x.Type.IsMap || x.Type.IsArray)) 
            {
                usingWriter.Append("using System.Collection.Generic;").AppendLine();
            }

            if (msg.Fields.Any(x => x.Option.Required)) 
            {
                usingWriter.Append("using System.ComponentModel.DataAnnotations;").AppendLine();
            }

            foreach (var fieldInfo in info.Fields.Values)
            {
                var field = fieldInfo.Field;
                
                if(field.Option.Required)
                {
                    bodyWriter.Append("[Required]").AppendLine();
                }

                if (field.Option.Deprecated)
                {
                    bodyWriter.Append("[Obsolete]").AppendLine();
                }
               
                bodyWriter
                    .Append("public ")                    
                    .Append(fieldInfo.Type);
                
                if (field.Option.Optional && field.Type.TypeDeclaration.Nullable)
                { 
                    //check is the type is nullable
                    bodyWriter.Append("?");
                }
                
                bodyWriter.Append($" {fieldInfo.Name} {{get; set;}}");
                
                if(field.Option.Default != null)
                {
                    bodyWriter.Append(" = ").Append(field.Option.Default.ToString()).Append(";");
                }

                bodyWriter.AppendLine();
                bodyWriter.AppendLine();
            }

            clsWriter.Append("}").AppendLine();

            writer.Save(info.Name);

        }
      

    }

   

    
}
