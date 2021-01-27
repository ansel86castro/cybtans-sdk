using Cybtans.Graphics.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cybtans.Graphics.Components
{
    public interface IFrameComponent: IDisposable
    {
        public Frame Frame { get; set; }

        public Boundings Bounding { get; }

        void OnPoseUpdated();
        FrameComponentDto ToDto();
    }
}
