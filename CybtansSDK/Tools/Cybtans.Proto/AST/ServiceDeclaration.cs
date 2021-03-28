using Antlr4.Runtime;
using Cybtans.Proto.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

            if(Rpcs.ToLookup(x=>x.Name).Any(x => x.Count() > 1))
            {
                logger.AddError($"Duplicated fields numbers in {Name} ({ string.Join(",", Rpcs.ToLookup(x => x.Name).Where(x => x.Count() > 1).Select(x => string.Join(",", x) + "=" + x.Key)) }) , {Line},{Column}");
            }

            foreach (var item in Rpcs)
            {
                item.CheckSemantic(scope, logger);
            }
        }

        public void Merge(ServiceDeclaration srv)
        {
            var lookup = Rpcs.ToDictionary(x => x.Name);            
            foreach (var rpc in srv.Rpcs)
            {
                if(!lookup.TryGetValue(rpc.Name, out var target))               
                {
                    Rpcs.Add(rpc.Clone());
                }                
            }

            foreach (var option in srv.Options)
            {
                if (!Options.Any(x => x.Id == option.Id))
                {
                    Options.Add(option);
                }
            }
        }
    }

    public class RpcDeclaration : DeclarationNode<RpcOptions>
    {
        public RpcDeclaration(string name, IdentifierExpression request, IdentifierExpression response)
        {
            Name = name;
            Request = request;
            Response = response;
        }

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

            if(RequestType == null)
            {
                logger.AddError($"Type {Request} is not defined at {Line},{Column}");
            }

            if(ResponseType == null)
            {
                logger.AddError($"Type {Response} is not defined at {Line},{Column}");
            }
        }

        public RpcDeclaration Clone()
        {
            return new RpcDeclaration(Name, Request, Response) 
            {
                Line = Line, 
                Column = Column,
                Options = Options
            };
        }
    }
}
