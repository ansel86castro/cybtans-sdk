#nullable enable

using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Markup;

namespace Cybtans.Proto.Generators.CSharp
{
    public class TypeGeneratorOption : OutputOption
    {
        public bool PartialClass { get; set; } = true;     
     
    }

    public class TypeGenerator : FileGenerator<ModelGeneratorOptions>
    {        
        Dictionary<ITypeDeclaration, MessageClassInfo> _messages = new Dictionary<ITypeDeclaration, MessageClassInfo>();

        public TypeGenerator(ProtoFile proto, ModelGeneratorOptions option):base(proto, option)
        {
            Namespace = option.Namespace ?? $"{proto.Option.Namespace ?? proto.Filename.Pascal()}.Models";            
        }

        public MessageClassInfo GetMessageInfo(ITypeDeclaration declaration)
        {            
            if(!_messages.TryGetValue(declaration, out var info))
            {
                info = new MessageClassInfo((MessageDeclaration)declaration, _option, Proto);
                _messages.Add(declaration, info);
            }
            return info;
        }        

        public string Namespace { get; set; }
        
        protected override void GenerateCode(ProtoFile proto)
        {                                  
            foreach (var item in proto.Declarations)
            {
                if (item is MessageDeclaration msg)
                {                    
                    var info = new MessageClassInfo(msg, _option, proto);

                    GenerateMessage(info);                

                    _messages.Add(msg, info);
                }               
            }            
        }
     
        private void GenerateMessage(MessageClassInfo info)
        {
            var writer = CreateWriter(info.Namespace);

            var clsWriter = writer.Class;
            var usingWriter = writer.Usings;

            MessageDeclaration msg = info.Message;

            if (msg.Option.Description != null)
            {
                clsWriter.Append("/// <summary>").AppendLine();
                clsWriter.Append("/// ").Append(msg.Option.Description).AppendLine();
                clsWriter.Append("/// </summary>").AppendLine();
                clsWriter.Append($"[Description(\"{msg.Option.Description}\")]").AppendLine();
            }

            if (msg.Option.Deprecated)
            {
                clsWriter.Append($"[Obsolete]").AppendLine();
            }

            clsWriter.Append("public ");

            if (_option.PartialClass)
            {
                clsWriter.Append("partial ");
            }

            clsWriter.Append($"class {info.Name} ");

            if (msg.Option.Base != null)
            {
                clsWriter.Append($": {msg.Option.Base}");
                if (_option.GenerateAccesor)
                {
                    usingWriter.Append("using Cybtans.Serialization;").AppendLine();
                    clsWriter.Append(", IReflectorMetadataProvider");
                }
            }
            else if (_option.GenerateAccesor)
            {
                usingWriter.Append("using Cybtans.Serialization;").AppendLine();
                clsWriter.Append(": IReflectorMetadataProvider");
            }
           
            clsWriter.AppendLine();
            clsWriter.Append("{").AppendLine();
            clsWriter.Append('\t', 1);

            var bodyWriter = clsWriter.Block("BODY");

            if(msg.Fields.Any(x=>x.Type.IsMap || x.Type.IsArray)) 
            {
                usingWriter.Append("using System.Collections.Generic;").AppendLine();
            }

            if (msg.Option.Description != null || msg.Fields.Any(x => x.Option.Description != null))
            {
                usingWriter.Append("using System.ComponentModel;").AppendLine();
            }

            if (msg.Fields.Any(x => x.Option.Required)) 
            {
                usingWriter.Append("using System.ComponentModel.DataAnnotations;").AppendLine();
            }
            

            if (_option.GenerateAccesor)
            {
                bodyWriter.Append($"private static readonly {info.Name}Accesor __accesor = new {info.Name}Accesor();")
                    .AppendLine()
                    .AppendLine();
            }

            foreach (var fieldInfo in info.Fields.Values.OrderBy(x=>x.Field.Number))
            {
                var field = fieldInfo.Field;
                
                if(field.Option.Description != null)
                {                    
                    bodyWriter.Append("/// <summary>").AppendLine();
                    bodyWriter.Append("/// ").Append(field.Option.Description).AppendLine();
                    bodyWriter.Append("/// </summary>").AppendLine();
                }

                if (field.Option.Required)
                {
                    bodyWriter.Append("[Required]").AppendLine();
                }

                if (field.Option.Deprecated)
                {
                    bodyWriter.Append("[Obsolete]").AppendLine();
                }

                if(field.Option.Description != null)
                {
                    bodyWriter.Append($"[Description(\"{field.Option.Description}\")]").AppendLine();                   
                }
               
                bodyWriter
                    .Append("public ")                    
                    .Append(fieldInfo.Type);                              
                
                bodyWriter.Append($" {fieldInfo.Name} {{get; set;}}");
                
                if(field.Option.Default != null)
                {
                    bodyWriter.Append(" = ").Append(field.Option.Default.ToString()).Append(";");
                }

                bodyWriter.AppendLine();
                bodyWriter.AppendLine();
            }

            if (_option.GenerateAccesor)
            {
                bodyWriter.Append("public IReflectorMetadata GetAccesor()\r\n{\r\n\treturn __accesor;\r\n}");           
            }

            if(info.Fields.Count == 1)
            {
                //Generate ImplicitConverter
                var field = info.Fields.First().Value;
                bodyWriter.AppendLine(2);
                bodyWriter.Append($"public static implicit operator {info.Name}({field.Type} {field.Field.Name})\r\n{{");
                bodyWriter.AppendLine();
                bodyWriter.Append('\t', 1).Append($"return new {info.Name} {{ {field.Name} = {field.Field.Name} }};");
                bodyWriter.AppendLine();
                bodyWriter.Append("}");
            }

            clsWriter.AppendLine().Append("}").AppendLine();

            if (_option.GenerateAccesor)
            {
                clsWriter.AppendLine(2);
                GenerateAccesor(info, clsWriter);
            }
          

            writer.Save(info.Name);

        }

        private void GenerateAccesor(MessageClassInfo info, CodeWriter clsWriter)
        {
            clsWriter.Append($"public sealed class {info.Name}Accesor : IReflectorMetadata").AppendLine();
            clsWriter.Append("{")
                .AppendLine().Append('\t', 1);

            var body = clsWriter.Block("ACCESOR_BODY");
            var fields = info.Fields.Values.OrderBy(x=>x.Field.Number);

            var getTypeSwtich = new StringBuilder();
            var getValueSwtich = new StringBuilder();
            var setValueSwtich = new StringBuilder();
            var getPropertyName = new StringBuilder();
            var getPropertyCode = new StringBuilder();

            foreach (var field in fields)
            {
                body.Append($"public const int {field.Name} = {field.Field.Number};").AppendLine();                

                getTypeSwtich.Append($"{field.Name} => typeof({field.Type}),\r\n");
                getValueSwtich.Append($"{field.Name} => obj.{field.Name},\r\n");
                setValueSwtich.Append($"case {field.Name}:  obj.{field.Name} = ({field.Type})value;break;\r\n");
                getPropertyName.Append($"{field.Name} => \"{field.Name}\",\r\n");
                getPropertyCode.Append($"\"{field.Name}\" => {field.Name},\r\n");
            }

            body.Append("private readonly int[] _props = new []").AppendLine();
            body.Append("{").AppendLine();

            body.Append('\t',1).Append(string.Join(",", fields.Select(x => x.Name)));
            body.AppendLine();

            body.Append("};").AppendLine(2);

            body.Append("public int[] GetPropertyCodes() => _props;")
                .AppendLine();

            body.AppendTemplate(Template, new Dictionary<string, object>
            {
                ["TYPE"] = info.Name,
                ["SWITCH"]= getTypeSwtich.ToString(),
                ["GET_VALUE"] = getValueSwtich.ToString(),
                ["SET_VALUE"] = setValueSwtich.ToString(),
                ["GET_PROPERTY_NAME"] = getPropertyName.ToString(),
                ["GET_PROPERTY_CODE"] = getPropertyCode.ToString()
            });

            clsWriter.AppendLine();
            clsWriter.Append("}")
                .AppendLine();
        }

        string Template = @"
public string GetPropertyName(int propertyCode)
{
    return propertyCode switch
    {
       @{GET_PROPERTY_NAME}
        _ => throw new InvalidOperationException(""property code not supported""),
    };
}

public int GetPropertyCode(string propertyName)
{
    return propertyName switch
    {
        @{GET_PROPERTY_CODE}
        _ => -1,
    };
}

public Type GetPropertyType(int propertyCode)
{
    return propertyCode switch
    {
        @{SWITCH}
        _ => throw new InvalidOperationException(""property code not supported""),
    };
}
       
public object GetValue(object target, int propertyCode)
{
    @{TYPE} obj = (@{TYPE})target;
    return propertyCode switch
    {
        @{GET_VALUE}
        _ => throw new InvalidOperationException(""property code not supported""),
    };
}

public void SetValue(object target, int propertyCode, object value)
{
    @{TYPE} obj = (@{TYPE})target;
    switch (propertyCode)
    {
        @{SET_VALUE}
        default: throw new InvalidOperationException(""property code not supported"");
    }
}
";

    }




}
