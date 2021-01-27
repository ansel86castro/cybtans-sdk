using Cybtans.Graphics.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Graphics.Components
{
    class CameraComponent : IFrameComponent
    {
        public Camera Camera { get; set; }
        public Frame Frame { get; set; }
        public Boundings Bounding { get; }

        public void Dispose()
        {
            
        }

        public void OnPoseUpdated()
        {
            Camera.Transform(Frame.GlobalPose);
        }

        public FrameComponentDto ToDto()
        {
            return new FrameComponentDto
            {
                 Camera = Camera.Id
            };
        }
    }
}
