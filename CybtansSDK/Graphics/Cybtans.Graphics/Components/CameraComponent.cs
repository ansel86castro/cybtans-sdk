using Cybtans.Graphics.Models;
using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Cybtans.Graphics.Components
{
    public class CameraComponent : IFrameComponent
    {
        public CameraComponent(Camera camera)
        {
            Camera = camera;
        }

        public Camera Camera { get; set; }
        public Frame? Frame { get; set; }
        public Boundings? Bounding { get; }

        public void Dispose()
        {
            
        }

        public void OnPoseUpdated()
        {
            if (Frame != null)
            {
                Camera.Transform(Frame.GlobalPose);
            }
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
