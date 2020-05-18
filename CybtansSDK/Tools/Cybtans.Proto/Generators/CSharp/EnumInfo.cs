#nullable enable

using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Cybtans.Proto.Generators.CSharp
{
    public class EnumInfo
    {        
        public EnumInfo(EnumDeclaration @enum, OutputOption outputOption, ProtoFile proto)
        {
            Enum = @enum;
            Name = @enum.Name.Pascal();
            Fields = @enum.Members.ToDictionary(x => x.Name, x => new EnumMemberInfo(x));
            Namespace = $"{proto.Option.Namespace}.{outputOption.Namespace ?? "Models"}";
        }

        public EnumDeclaration Enum { get; }

        public string Name { get; }

        public string Namespace { get; }

        public Dictionary<string, EnumMemberInfo> Fields { get; }
    }

    public class EnumMemberInfo
    {
        public EnumMemberInfo(EnumMemberDeclaration member)
        {
            Field = member;
            Name = member.Name.Pascal();          
        }

        public EnumMemberDeclaration Field { get; }

        public string Name { get; }
   
    }
}
