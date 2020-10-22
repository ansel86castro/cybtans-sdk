#nullable enable

using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cybtans.Proto.Generators.CSharp
{
    public class MessageClassInfo
    {
        readonly MessageDeclaration _msg;
        readonly SortedList<string, MessageFieldInfo> _fields;
        
        public MessageClassInfo(MessageDeclaration msg, OutputOption outputOption, ProtoFile proto)
        {
            this._msg = msg;
            Name = msg.Name.Pascal();
            Namespace = $"{proto.Option.Namespace}.{outputOption.Namespace ?? "Models"}";            
            _fields = new SortedList<string, MessageFieldInfo>();
            foreach (var item in msg.Fields.Select(x => new MessageFieldInfo(x)))
            {
                _fields.Add(item.Field.Name, item);
            }
        }

        public MessageDeclaration Message => _msg;

        public SortedList<string, MessageFieldInfo> Fields => _fields;

        public string Name { get; }

        public string Namespace { get; }

        public List<MessageFieldInfo>? GetPathBinding(string template)
        {
            if (template == null)
                return null;

            var regex = new Regex(@"{(\w+)}");
            MatchCollection matches = regex.Matches(template);
            if (matches.Any(x => x.Success))
            {
                List<MessageFieldInfo> fields = new List<MessageFieldInfo>();
                foreach (Match? match in matches)
                {
                    if (match != null && match.Success)
                    {
                        var name = match.Groups[1].Value;
                        var field = Fields[name];
                        fields.Add(field);
                    }
                }

                return fields;
            }
            return null;
        }

    }

    public class MessageFieldInfo
    {
        private readonly FieldDeclaration _field;

        public MessageFieldInfo(FieldDeclaration field)
        {
            _field = field;
            Name = field.Name.Pascal();
            Type = field.Type.GetTypeName();
            if (field.Option.Optional && field.Type.TypeDeclaration.IsValueType)
            {
                //check ist the type is nullable
                Type += "?";
            }
        }

        public FieldDeclaration Field => _field;

        public string Name { get; }

        public string Type { get; }

       
    }
}
