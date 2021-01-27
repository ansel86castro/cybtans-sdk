
using Cybtans.Math;

namespace Cybtans.Graphics.Animations
{
     public class FrameAnimationController
    {
        #region Private Members

        Frame _frame;

        #region Initial Transforms

        Matrix _initialLocalPose;
        Vector3 _initialTranslation;
        Vector3 _initialScale;
        Quaternion _initialRotation;

        #endregion

        #region Animation Transforms

        Matrix OutPose = Matrix.Identity;
        Vector3 OutTraslation;
        Vector3 OutScale = new Vector3(1, 1, 1);
        Quaternion OutRotation;
        bool UsePose;
        bool UseRotation;

        #endregion

        #endregion

        #region Public Members

        #region Transforms

        public Vector3 Traslation
        {
            get { return OutTraslation; }
            set { OutTraslation = value; }
        }

        public float X
        {
            get { return OutTraslation.X; }
            set { OutTraslation.X = value; }
        }

        public float Y
        {
            get { return OutTraslation.Y; }
            set { OutTraslation.Y = value; }
        }

        public float Z
        {
            get { return OutTraslation.Z; }
            set { OutTraslation.Z = value; }
        }

        public Vector3 Scale
        {
            get { return OutScale; }
            set
            {
                OutScale = value;
            }
        }

        public float ScaleX
        {
            get { return OutScale.X; }
            set
            {
                OutScale.X = value;
            }
        }

        public float ScaleY
        {
            get { return OutScale.Y; }
            set
            {
                OutScale.Y = value;

            }
        }

        public float ScaleZ
        {
            get { return OutScale.Z; }
            set
            {
                OutScale.Z = value;
            }
        }

        public Matrix Pose
        {
            get { return OutPose; }
            set
            {
                OutPose = value;
                UsePose = true;
            }
        }

        public Quaternion Rotation
        {
            get { return OutRotation; }
            set
            {
                OutRotation = value;
                UseRotation = true;
            }
        }
        #endregion     
        
        public Frame Target
        {
            get { return _frame; }
            set
            {
                if (_frame == value)
                    return;

                _frame = value;
                if (_frame != null)
                {
                    SaveTransforms();
                }
            }
        }

        public bool Enable { get; set; }

        #endregion

        public FrameAnimationController()
        {
            Enable = true;
        }

        public void Rotate(Matrix rot, bool rightSide = true)
        {
            var rotQuad = Quaternion.RotationMatrix(rot);
            if (rightSide)
                OutRotation *= rotQuad;
            else
                OutRotation = rotQuad * OutRotation;
            UseRotation = true;
        }

        public void Rotate(Quaternion rot, bool rightSide = true)
        {
            if (rightSide)
                OutRotation *= rot;
            else
                OutRotation = rot * OutRotation;
            UseRotation = true;
        }

        public void SaveTransforms()
        {
            _initialTranslation = _frame.LocalPosition;
            _initialScale = _frame.LocalScale;
            _initialLocalPose = _frame.LocalPose;
            _initialRotation = _frame.LocalRotationQuat;

            OutRotation = Quaternion.Identity;
            OutScale = _initialScale;
            OutTraslation = _initialTranslation;
        }

        public void RestoreTransforms()
        {                  
            _frame.ComputeLocalPose(_initialScale, _initialRotation, _initialTranslation);
        }

        public void OnSample(bool isBlended, float blendWeight = 1.0f)
        {
            if (!Enable) return;

            var rotationQuad = OutRotation;
            var translation = OutTraslation;
            var scale = OutScale;

            if (!UseRotation)
                rotationQuad = _initialRotation;

            if (!isBlended)
            {
                if (UsePose)
                    _frame.LocalPose = OutPose;
                else
                    _frame.ComputeLocalPose(scale, rotationQuad, translation);
            }
            else
            {
                if (UsePose)
                {
                    Matrix diff = OutPose;
                    diff -= _initialLocalPose;
                    diff *= blendWeight;
                    _frame.LocalPose += diff;
                }
                else
                {
                    _frame.LocalScale += (scale - _initialScale) * blendWeight;
                    _frame.LocalRotationQuat += (rotationQuad - _initialRotation) * blendWeight;
                    _frame.LocalPosition += (translation - _initialTranslation) * blendWeight;
                    _frame.ComputeLocalPose();
                }
            }

            //Reset Transforms
            OutPose = Matrix.Identity;
            OutTraslation = _initialTranslation;
            OutScale = _initialScale;
            OutRotation = Quaternion.Identity;
            UsePose = false;
            UseRotation = false;
        }

        public override string ToString()
        {
            if (_frame != null)
                return _frame.ToString();
            return base.ToString();
        }

        public void LinkAnimation(KeyFrameAnimation animation)
        {
            foreach (var curves in animation.Nodes)
            {
                LinkAnimationCurve(curves);
                foreach (var curve in curves.Curves)
                {
                    LinkOutput(curve);
                }
            }
        }

        public void UnLinkAnimation(KeyFrameAnimation animation)
        {
            foreach (var curves in animation.Nodes)
            {
                UnLinkAnimationCurve(curves);
                foreach (var curve in curves.Curves)
                {
                    UnLinkOutput(curve);
                }
            }
        }


        public void LinkAnimationCurve(CurvesContainer animationCurves)
        {
            animationCurves.SampleEnd += OnSample;
        }

        public void UnLinkAnimationCurve(CurvesContainer animationCurves)
        {
            animationCurves.SampleEnd -= OnSample;
        }

        public void LinkOutput(KeyFrameCurve animationCurve)
        {
            unsafe
            {
                OutputHandler hanlder = null;
                string name = animationCurve.Name;
                var part = name.Split('/');
                if (part.Length > 0)
                    name = part[part.Length - 1];

                switch (name)
                {
                    case "translate.X":
                        hanlder = OutputSampleX;
                        break;
                    case "translate.Y":
                        hanlder = OutputSampleY;
                        break;
                    case "translate.Z":
                        hanlder = OutputSampleZ;
                        break;

                    case "scale.X":
                        hanlder = OutputSampleScaleX;
                        break;
                    case "scale.Y":
                        hanlder = OutputSampleScaleY;
                        break;
                    case "scale.Z":
                        hanlder = OutputSampleScaleZ;
                        break;

                    case "rotateX.ANGLE":
                        hanlder = OutputSampleRotateX;
                        break;
                    case "rotateY.ANGLE":
                        hanlder = OutputSampleRotateY;
                        break;
                    case "rotateZ.ANGLE":
                        hanlder = OutputSampleRotateZ;
                        break;

                    case "matrix":
                        hanlder = OutputSampleMatrix;
                        break;
                    case "rotate":
                        hanlder = OutputSampleRotate;
                        break;
                    case "translate":
                        hanlder = OutputSampleTranslate;
                        break;
                    case "scale":
                        hanlder = OutputSampleScale;
                        break;
                }
                if (hanlder != null)
                {
                    animationCurve.OutputSample += hanlder;
                }
            }
        }

        public void UnLinkOutput(KeyFrameCurve animationCurve)
        {
            unsafe
            {
                OutputHandler hanlder = null;
                string name = animationCurve.Name;
                var part = name.Split('/');
                if (part.Length > 0)
                    name = part[part.Length - 1];

                switch (name)
                {
                    case "translate.X":
                        hanlder = OutputSampleX;
                        break;
                    case "translate.Y":
                        hanlder = OutputSampleY;
                        break;
                    case "translate.Z":
                        hanlder = OutputSampleZ;
                        break;

                    case "scale.X":
                        hanlder = OutputSampleScaleX;
                        break;
                    case "scale.Y":
                        hanlder = OutputSampleScaleY;
                        break;
                    case "scale.Z":
                        hanlder = OutputSampleScaleZ;
                        break;

                    case "rotateX.ANGLE":
                        hanlder = OutputSampleRotateX;
                        break;
                    case "rotateY.ANGLE":
                        hanlder = OutputSampleRotateY;
                        break;
                    case "rotateZ.ANGLE":
                        hanlder = OutputSampleRotateZ;
                        break;

                    case "matrix":
                        hanlder = OutputSampleMatrix;
                        break;
                    case "rotate":
                        hanlder = OutputSampleRotate;
                        break;
                    case "translate":
                        hanlder = OutputSampleTranslate;
                        break;
                    case "scale":
                        hanlder = OutputSampleScale;
                        break;
                }
                if (hanlder != null)
                {
                    animationCurve.OutputSample -= hanlder;
                }
            }
        }

        unsafe void OutputSampleX(float* data, int dimension)
        {
            OutTraslation.X = *data;
        }
        unsafe void OutputSampleY(float* data, int dimension)
        {
            OutTraslation.Y = *data;
        }
        unsafe void OutputSampleZ(float* data, int dimension)
        {
            OutTraslation.Z = *data;
        }


        unsafe void OutputSampleScaleX(float* data, int dimension)
        {
            OutScale.X = *data;
        }
        unsafe void OutputSampleScaleY(float* data, int dimension)
        {
            OutScale.Y = *data;
        }
        unsafe void OutputSampleScaleZ(float* data, int dimension)
        {
            OutScale.Z = *data;
        }

        unsafe void OutputSampleRotateX(float* data, int dimension)
        {
            Rotate(Quaternion.RotationAxis(Vector3.UnitX, *data));
        }

        unsafe void OutputSampleRotateY(float* data, int dimension)
        {
            Rotate(Quaternion.RotationAxis(Vector3.UnitY, *data));
        }

        unsafe void OutputSampleRotateZ(float* data, int dimension)
        {
            Rotate(Quaternion.RotationAxis(Vector3.UnitZ, *data));
        }

        unsafe void OutputSampleMatrix(float* data, int dimension)
        {
            OutPose = *(Matrix*)data;
        }

        unsafe void OutputSampleRotate(float* data, int dimension)
        {
            Rotate(*(Quaternion*)data);
        }

        unsafe void OutputSampleTranslate(float* data, int dimension)
        {
            OutTraslation = *(Vector3*)data;
        }

        unsafe void OutputSampleScale(float* data, int dimension)
        {
            OutScale = *(Vector3*)data;
        }
    }
}
