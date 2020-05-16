using Antlr4.Runtime;
using CybtansSdk.Proto.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CybtansSdk.Proto.AST
{

    public abstract class DeclarationNode : ProtoAstNode
    {      

        public DeclarationNode()
        {
        }

        public DeclarationNode(IToken start) : base(start)
        {
        }        

        public DeclarationNode(int line, int column) : base(line, column)
        {
        }

        public string Name { get; set; }

        public List<OptionsExpression> Options { get; set; } = new List<OptionsExpression>();       

        public override string ToString()
        {
            if(Options == null || Options.Count == 0)
                return Name;

            return $"{Name} :[{string.Join(",", Options)}]";
        }
    }

    public abstract class DeclarationNode<TOption> : DeclarationNode, IOptionsContainer
        where TOption : ProtobufOption, new()
    {
        public TOption Option { get; } = new TOption();

        public DeclarationNode()
        {

        }

        public DeclarationNode(IToken start) : base(start)
        {
        }

        public DeclarationNode(IToken start, string name) : base(start)
        {
            Name = name;
        }

        public DeclarationNode(int line, int column) : base(line, column)
        {
        }

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            foreach (var option in Options)
            {
                option.OptionsContainer = this;
                option.OptionType = Option?.Type ?? OptionsType.None;
                option.CheckSemantic(scope, logger);
            }
        }

        public void SetOption(IdentifierExpression path, object value, OptionsType type, IErrorReporter errorReporter)
        {
            if (Option != null)
            {
                if (type != Option.Type)
                {
                    errorReporter.AddError($"Option {type} not match {Option.Type} for {Name} at {Line},{Column}");
                    return;
                }

                Option.Set(path, value);
            }
        }
    }

}
