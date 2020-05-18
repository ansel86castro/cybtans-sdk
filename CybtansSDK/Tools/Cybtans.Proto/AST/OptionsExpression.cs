using Antlr4.Runtime;
using Cybtans.Proto.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Proto.AST
{

    public interface IOptionsContainer
    {
        void SetOption(IdentifierExpression path, object value, OptionsType type, IErrorReporter errorReporter);
    }

    public class OptionsExpression:ExpressionNode
    {             
        public OptionsExpression(IdentifierExpression id, ExpressionNode value, IToken token)
            : base(token)
        {
            Id = id;
            Value = value;
        }

        public IdentifierExpression Id { get; }

        public ExpressionNode Value { get; set; }

        public OptionsType OptionType { get; set; }

        public IOptionsContainer OptionsContainer { get; set; }

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            Value.CheckSemantic(scope, logger);         

            if(OptionsContainer != null)
            {
                OptionsContainer.SetOption(Id, Value, OptionType, logger);
            }
        }

        public override string ToString()
        {
            return $"({Id})={Value}";
        }
    }

    public enum OptionsType
    {
        None = 0,
        File,
        Message,
        Field,
        Service,
        Enum,
        Rpc
    }
}
