using Cybtans.Graphics.Lights;
using Cybtans.Graphics.Models;
using Cybtans.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Graphics.Components
{
    public class LightComponent : IFrameComponent
    {
        private readonly Boundings bounding = new Boundings();
        private Light _light;
        private Vector3 _localPosition;
        private Vector3 _localDirection;
        private Vector3 _direction;
        private Vector3 _position;
        private Frame _frame;

        public LightComponent() { }

        public LightComponent(Light light)
        {
            this._light = light;
        }

        public Vector3 LocalPosition { get => _localPosition; set => _localPosition = value; }

        public Vector3 LocalDirection { get => _localDirection; set => _localDirection = value; }

        public Vector3 Direction { get => _direction; set => _direction = value; }

        public Vector3 Position { get => _position; set => _position = value; }

        public Frame Frame { get => _frame; set => _frame = value; }

        public Boundings Bounding => bounding;

        public Light Light
        {
            get => _light;
            set
            {
                _light = value;
                Bounding.Sphere = new Sphere(_localPosition, value.Range);
            }
        }

        public void Dispose()
        {

        }

        public void OnPoseUpdated()
        {
            _direction = Vector3.TransformNormal(_localDirection, _frame.GlobalPose);
            _position = Vector3.Transform(_localPosition, _frame.GlobalPose);
        }

        public FrameComponentDto ToDto()
        {
            return new FrameComponentDto
            {
                Light = new FrameLightDto
                {
                    Light = Light.Id,
                    LocalDirection = LocalDirection.ToList(),
                    LocalPosition = LocalPosition.ToList()
                }
            };
        }
    }
}
