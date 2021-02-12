using Cybtans.Graphics.Models;
using Cybtans.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Graphics.Components
{
    public enum ProjectionType { Perspective, Orthographic }

    public class Camera
    {         
        private float _zn;
        private float _zf;
        private float _fov = Numerics.PIover3;
        private float _aspectRatio = 4 / 3;
        private float _width = 512;
        private float _height = 512;
        private ProjectionType _projectionType = ProjectionType.Perspective;
        private Matrix _local = Matrix.Identity;
        private Matrix _view = Matrix.Identity;
        private Matrix _proj;            

        public Camera(string name = null, float zn = 1f, float zf = 1000f)
        {
            this._zn = zn;
            this._zf = zf;
            Name = name;

        }

        public Camera(string name, Vector3 position, Vector3 right, Vector3 up, Vector3 front, float zn = 1f, float zf = 1000f)
            : this(name, zn, zf)
        {            
            _view = Matrix.View(right, up, front, position);
            _UpdateProjection();

        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }        
        public float NearPlane { get => _zn; set => _zn = value; }    
        public float FarPlane { get => _zf; set => _zf = value; }
        public float FieldOfView { get => _fov; set => _fov = value; }
        public float AspectRatio { get => _aspectRatio; set => _aspectRatio = value; }
        public float Width { get => _width; set => _width = value; }
        public float Height { get => _height; set => _height = value; }
        public ProjectionType ProjectionType { get => _projectionType; set => _projectionType = value; }      
        public Matrix Local { get => _local; set => _local = value; }
        public Matrix View { get => _view; set => _view = value; }
        public Matrix Proj { get => _proj; set => _proj = value; }               

        private void _UpdateProjection()
        {
            switch (_projectionType)
            {
                case ProjectionType.Orthographic:
                    _proj = Matrix.OrthoRh(_width, _height, _zn, _zf);
                    break;
                case ProjectionType.Perspective:
                    _proj = Matrix.PerspectiveFovRh(_aspectRatio, _fov, _zn, _zf);
                    break;
            }           
        }        

        public void Transform(Matrix transform)
        {
            transform = _local * transform;

            var right = new Vector3(transform.M11, transform.M12, transform.M13);
            right.Normalize();

             var up = new Vector3(transform.M21, transform.M22, transform.M23);
             up.Normalize();

            var front = new Vector3(transform.M31, transform.M32, transform.M33);
            front.Normalize();

            var position = new Vector3(transform.M41, transform.M42, transform.M43);

            //view = Matrix.Invert(m);          
            // Fill in the view matrix entries.            
            float x = -Vector3.Dot(position, right);
            float y = -Vector3.Dot(position, up);
            float z = -Vector3.Dot(position, front);

            _view.M11 = right.X;
            _view.M21 = right.Y;
            _view.M31 = right.Z;
            _view.M41 = x;

            _view.M12 = up.X;
            _view.M22 = up.Y;
            _view.M32 = up.Z;
            _view.M42 = y;

            _view.M13 = front.X;
            _view.M23 = front.Y;
            _view.M33 = front.Z;
            _view.M43 = z;

            _view.M14 = 0f;
            _view.M24 = 0f;
            _view.M34 = 0f;
            _view.M44 = 1f;            
        }

        public static Camera FromOrientation(string name, Vector3 position = default(Vector3), Euler orientation = default(Euler), float zn = 1f, float zf = 1000f)
        {
            Vector3 front, up, right;
            Euler.GetFrame(orientation, out right, out up, out front);
            return new Camera(name, position, right, up, front, zn, zf);
        }

        public Camera SetPerspective(float fov, float aspectRatio)
        {
            this._fov = fov;
            this._aspectRatio = aspectRatio;
            _projectionType = ProjectionType.Perspective;

            _UpdateProjection();

            return this;
        }

        public Camera SetOrthographic(float width, float height)
        {
            this._width = width;
            this._height = height;
            _projectionType = ProjectionType.Orthographic;
            _UpdateProjection();
      

            return this;
        }

        public CameraDto ToDto()
        {
            return new CameraDto
            {
                Id = Id,
                Name = Name,
                AspectRatio = AspectRatio,
                FarPlane = FarPlane,
                NearPlane = NearPlane,
                FieldOfView = FieldOfView,
                Height = Height,
                Width = Width,
                ProjType = (Models.ProjectionType)ProjectionType,
                LocalMatrix = _local.ToList(),                       
            };
        }
    }
}
