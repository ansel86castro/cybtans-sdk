using Cybtans.Math;

namespace Cybtans.Graphics.Common
{
    public interface IPoseable
    {
        /// <summary>
        /// World Tranfrom of the Object
        /// </summary>
       ref Matrix GlobalPose { get; }
    }
}