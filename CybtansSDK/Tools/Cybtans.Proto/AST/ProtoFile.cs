using Cybtans.Proto.Options;
using System;
#nullable enable

using System.Collections.Generic;
using System.Text;

namespace Cybtans.Proto.AST
{
    public class ProtoFile : ProtoAstNode, IOptionsContainer
    {
        public FileOptions Option { get; set; } = new FileOptions();

        public List<ImportDeclaration> Imports { get; } = new List<ImportDeclaration>();

        public PackageDeclaration? Package { get; set; }

        public List<OptionsExpression> Options { get; set; } = new List<OptionsExpression>();

        public List<DeclarationNode> Declarations { get; set; } = new List<DeclarationNode>();

        public List<ProtoFile> ImportedFiles { get; } = new List<ProtoFile>();

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            if(Package != null)
            {
                Option.Namespace = Package.ToString();
            }

            foreach (var item in Declarations) 
            {
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

            foreach (var declaration in Declarations)
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
    }    
}
