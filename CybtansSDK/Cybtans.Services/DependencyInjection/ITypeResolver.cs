using System;

namespace Cybtans.Services.DependencyInjection
{
    public interface ITypeResolver
    {
        Type ResolveImplementer(Type contract, out LifeType? lifeType);
    }


}
