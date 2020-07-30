using Cybtans.Proto.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable enable

namespace Cybtans.Proto
{

    public class Scope 
    {
        Scope? _parent;
        Dictionary<string, Scope> _packages = new Dictionary<string, Scope>();
        Dictionary<string, ITypeDeclaration> _declarations;

        public Scope() : this(null, null) { }

        public Scope(Scope? parent, IEnumerable<ITypeDeclaration>? stdDeclarations)
        {
            this._parent = parent;
            this._declarations = stdDeclarations == null ? new Dictionary<string, ITypeDeclaration>() : stdDeclarations.ToDictionary(x => x.Name);
        }

        public IReadOnlyCollection<ITypeDeclaration> Declarations => _declarations.Values;
        
        public void AddDeclaration(ITypeDeclaration type)
        {            
            this._declarations[type.Name] = type;
        }

        public ITypeDeclaration? GetDeclaration(string name)
        {
            if (!_declarations.TryGetValue(name, out var type) && _parent != null)
            {
                return _parent.GetDeclaration(name);
            }

            return type;
        }

        public ITypeDeclaration? GetDeclaration(IdentifierExpression path)
        {            
            if(path.Left == null)
            {
                return GetDeclaration(path.Id);
            }

            Scope? scope = FindScope(path.Left) ?? _parent?.FindScope(path.Left);
            return scope?.GetDeclaration(path.Id);
        }     

        public Scope? FindScope(string name)
        {
            if (_packages.TryGetValue(name, out var scope))
            {
                return scope;
            }

            if (_declarations.TryGetValue(name, out var type) && type is MessageDeclaration message)
            {
                return message.GetScope(this);
            }

            return null;
        }
      
        public Scope AddPackage(IdentifierExpression path, Scope? package= null)
        { 
            if(path.Left == null)
            {
                return AddPackage(path.Id, package);
            }

            var parent = AddPackage(path.Left);
            package = parent.AddPackage(path.Id, package);
            package._parent = parent;
            return package;
        }

        private Scope AddPackage(string package, Scope? scope)
        {
            if (_packages.TryGetValue(package, out var packageScope))
            {
                if (scope != null)
                {
                    packageScope.Merge(scope);
                }
                return packageScope;
            }

            if (scope == null)
            {
                scope = new Scope();
            }
            
            _packages.Add(package, scope);
            return scope;
        }

        private Scope? FindScope(IdentifierExpression path)
        {
            if (path.Left == null)
            {
                return FindScope(path.Id);
            }

            var scope = FindScope(path.Left);
            return scope?.FindScope(path.Id);
        }

        public void Merge(Scope scope)
        {
            foreach (var item in scope._declarations)
            {
                if (!this._declarations.TryAdd(item.Key, item.Value))
                {                   
                    throw new InvalidOperationException($"There is already a declaration with the same name {item.Key}");
                }
            }
            foreach (var item in scope._packages)
            {
                if (!this._packages.TryAdd(item.Key, item.Value))
                {
                    this._packages[item.Key].Merge(item.Value);
                }
            }
        }

        public Scope CreateScope()
        {
            return new Scope(this, null);
        }

        public static Scope CreateRootScope()
        {
            return new Scope(null,new ITypeDeclaration[]
            {
                PrimitiveType.Double,
                PrimitiveType.Float,
                PrimitiveType.Int8 ,
                PrimitiveType.Int16,
                PrimitiveType.Int32 ,
                PrimitiveType.Int64 ,
                PrimitiveType.UInt16 ,
                PrimitiveType.UInt32 ,
                PrimitiveType.UInt64 ,
                PrimitiveType.Bool ,
                PrimitiveType.String,
                PrimitiveType.Bytes,
                PrimitiveType.Datetime,
                PrimitiveType.Map,
                PrimitiveType.Object,
                PrimitiveType.Void,
                PrimitiveType.Guid,
                PrimitiveType.Decimal
            });
        }
    }
}

#nullable disable