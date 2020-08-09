using Cybtans.Serialization;

namespace Cybtans.Services
{
    internal static class IReflectorExtensor
    {
        public static TOut Map<TOut>(this IReflectorMetadataProvider obj) where TOut : IReflectorMetadataProvider, new()
        {
            var accesor = obj.GetAccesor();
            TOut clone = new TOut();
            var cloneAccesor = clone.GetAccesor();

            foreach (var code in accesor.GetPropertyCodes())
            {
                cloneAccesor.SetValue(clone, code, accesor.GetValue(obj, code));
            }

            return clone;
        }
    }
}
