using System.Numerics;

namespace Cybtans.Graphics.Common
{
    public interface ITranslatable : IDeferreable
    {        
              
        Vector3 LocalPosition { get; set; }
    }
}