using Cybtans.Proto.Options;
using System;
#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cybtans.Proto.AST
{
    public class ProtoFile : ProtoAstNode, IOptionsContainer
    {
        public static HashSet<string> WhellKnowImports = new HashSet<string>
        {
            "cybtans.proto",
            "google/protobuf/empty.proto",
            "google/protobuf/timestamp.proto",
            "google/protobuf/duration.proto",
            "google/protobuf/wrappers.proto"
        };

        public static bool IsWhellKnowImport(string filename)
        {
            return WhellKnowImports.Contains(filename);
        }

        public string? Filename { get; set; }

        public FileOptions Option { get; set; } = new FileOptions();

        public List<ImportDeclaration> Imports { get; } = new List<ImportDeclaration>();

        public PackageDeclaration? Package { get; set; }

        public List<OptionsExpression> Options { get; set; } = new List<OptionsExpression>();

        public List<DeclarationNode> Declarations { get; set; } = new List<DeclarationNode>();

        public List<ProtoFile> ImportedFiles { get; } = new List<ProtoFile>();

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            if(Package != null && Option.Namespace == null)
            {
                Option.Namespace = Package.ToString();
            }         

            foreach (var item in Declarations) 
            {
                if (item.ProtoDeclaration == null)
                {
                    item.ProtoDeclaration = this;
                }

                if(item is ITypeDeclaration type)
                {
                    scope.AddDeclaration(type);
                }
            }

            foreach (var option in Options)
            {
                option.OptionType = OptionsType.File;
                option.OptionsContainer = this;
                option.CheckSemantic(scope, logger);
            }

            foreach (var declaration in Declarations.ToList())
            {
                declaration.CheckSemantic(scope, logger);
            }
        }

        public void SetOption(IdentifierExpression path, object value, OptionsType type, IErrorReporter errorReporter)
        {
            if (type != OptionsType.File)
            {
                errorReporter.AddError($"Option {type} not match {Option.Type}");
                return;
            }

            Option.Set(path, value);
        }

        public void Merge(ProtoFile importFileNode)
        {
            var lookup = Declarations.ToDictionary(x => x.Name);
            foreach (var decl in importFileNode.Declarations)
            {
                if(decl is MessageDeclaration msg)
                {
                    if(lookup.TryGetValue(msg.Name , out var target))
                    {
                        ((MessageDeclaration)target).Merge(msg);
                    }
                }
                else if(decl is ServiceDeclaration srv)
                {
                    if (lookup.TryGetValue(srv.Name, out var target))
                    {
                        ((ServiceDeclaration)target).Merge(srv);
                    }
                }
            }
        }

        public bool HaveMessages => Declarations.Any(x => x is MessageDeclaration || x is EnumDeclaration) || ImportedFiles.Any(x => x.HaveMessages);

        public bool HaveEnums => Declarations.Any(x => x is EnumDeclaration) || ImportedFiles.Any(x => x.HaveEnums);

        public bool HaveServices => Declarations.Any(x => x is ServiceDeclaration) || ImportedFiles.Any(x => x.HaveServices);

    }    
}
