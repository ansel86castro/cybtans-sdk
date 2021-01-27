using Cybtans.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Graphics.Common
{
    public interface IBindeable
    {

        /// <summary>
        /// Inverse of the BindTarget`s world pose when it is bind to this Instance. This allows to tranform properly the instance by a BindTarget
        /// </summary>
        Matrix BindAffectorPose { get; set; }

        /// <summary>
        /// Instance of an object that influences or affect the globalPose.
        /// </summary>
        IPoseable BindTarget { get; set; }

        /// <summary>
        /// This method is called when the affector has influenced his affectable instance
        /// and the affactable needs to updates its GlobalPose. For Physics simulated objects this method is called after
        /// a simulation frame is completed
        /// </summary>
        void UpdateBindPose();

    }
}
