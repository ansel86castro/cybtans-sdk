using Antlr4.Runtime;
using Cybtans.Proto.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Proto.AST
{
    public class ServiceDeclaration : DeclarationNode<ServiceOptions>
    {
        public ServiceDeclaration(string name, IToken start) : base(start)
        {
            Name = name;            
        }

        public List<RpcDeclaration> Rpcs { get; } = new List<RpcDeclaration>();

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            base.CheckSemantic(scope, logger);

            foreach (var item in Rpcs)
            {
                item.CheckSemantic(scope, logger);
            }
        }
    }

    public class RpcDeclaration : DeclarationNode<RpcOptions>
    {
        public RpcDeclaration(string name, IdentifierExpression request, IdentifierExpression response, IToken start) :
            base(start)
        {
            Name = name;
            Request = request;
            Response = response;            
        }

        public IdentifierExpression Request { get; set; }

        public IdentifierExpression Response { get; set; }

        public ITypeDeclaration RequestType { get; set; }

        public ITypeDeclaration ResponseType { get; set; }        

        public override void CheckSemantic(Scope scope, IErrorReporter logger)
        {
            base.CheckSemantic(scope, logger);

            Request.CheckSemantic(scope, logger);
            Response.CheckSemantic(scope, logger);

            RequestType = scope.GetDeclaration(Request);
            ResponseType = scope.GetDeclaration(Response);
        }
    }
}
