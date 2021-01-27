using Cybtans.Graphics.Common;
using Cybtans.Math;
using System;

namespace Cybtans.Graphics.Animations
{

    public unsafe delegate void OutputHandler(float* data, int dimension);

    /// <summary>    
    /// Define a animation curve for a property (channel)
    /// </summary>
    /// 
    [Serializable]
    public class KeyFrameCurve : INameable
    {
        float[] _output;
        int _outDim = 1;
        float[] _inTangent;
        float[] _outTangent;
        InterpolationMethod _interpolationType;
        float[] _result = new float[1];

        public event OutputHandler OutputSample;

        public string Name { get; set; }

        public float[] Output
        {
            get { return _output; }
            set { _output = value; }
        }

        public int OutputDim
        {
            get { return _outDim; }
            set
            {
                _outDim = value;
                _result = new float[_outDim];
            }
        }

        public float[] InTangent
        {
            get { return _inTangent; }
            set { _inTangent = value; }
        }

        public float[] OutTangent
        {
            get { return _outTangent; }
            set { _outTangent = value; }
        }

        public InterpolationMethod InterpolationType
        {
            get { return _interpolationType; }
            set { _interpolationType = value; }
        }

        private unsafe void _OnSample(float* data)
        {
            var onSample = OutputSample;
            if (onSample != null)
            {
                onSample(data, _outDim);
            }
        }

        //private float SampleArray(float[] array, int index)
        //{
        //    if (array == null)
        //        return 0;
        //    return array.Length == 1 ? array[0] : array[index];
        //}

        public void Sample(float s, int lowkey, int hightKey, float[] keys)
        {
            if (_output.Length > 1 && (lowkey >= _output.Length || hightKey >= _output.Length))
                return;

            unsafe
            {
                if (_outDim == 1)
                {
                    float v;
                    if (_output.Length == 1)
                        v = _output[0];
                    else
                    {
                        float p0 = _output[lowkey];
                        float p1 = _output[hightKey];
                        float t0 = _outTangent == null ? 0 : _outTangent.Length == 1 ? _outTangent[0] : _outTangent[lowkey];
                        float t1 = _inTangent == null ? 0 : _inTangent.Length == 1 ? _inTangent[0] : _inTangent[hightKey];

                        switch (_interpolationType)
                        {
                            case InterpolationMethod.STEP:
                                v = p0;
                                break;
                            case InterpolationMethod.LINEAR:
                                v = Numerics.Lerp(p0, p1, s);
                                break;
                            case InterpolationMethod.BEZIER:
                                //v = Numerics.Bezier(p0, p1, t0, t1, s);
                                v = Numerics.Bezier(p0, p1, t0 / 3.0f + p0, p1 - t1 / 3.0f, s);
                                break;
                            case InterpolationMethod.HERMITE:
                                v = Numerics.Hermite(p0, p1, t0, t1, s);
                                break;
                            case InterpolationMethod.QuatSlerpX:
                                v = Quaternion.Slerp(new Quaternion(1, 0, 0, p0), new Quaternion(1, 0, 0, p1), s).W;
                                break;
                            case InterpolationMethod.QuatSlerpY:
                                v = Quaternion.Slerp(new Quaternion(0, 1, 0, p0), new Quaternion(0, 1, 0, p1), s).W;
                                break;
                            case InterpolationMethod.QuatSlerpZ:
                                v = Quaternion.Slerp(new Quaternion(0, 0, 1, p0), new Quaternion(0, 0, 1, p1), s).W;
                                break;
                            default:
                                throw new InvalidOperationException("Interpolation method not implemented");
                        }
                    }
                    _OnSample(&v);
                }
                else
                {
                    fixed (float* pResult = _result)
                    {
                        fixed (float* pOutput = _output)
                        {
                            if (_output.Length == _outDim)
                                Buffer.MemoryCopy(pOutput, pResult, _result.Length, _outDim * sizeof(float));
                            else
                            {
                                float* p0 = pOutput + lowkey * _outDim;
                                float* p1 = pOutput + hightKey * _outDim;
                                float t0 = _outTangent == null ? 0 : _outTangent.Length == 1 ? _outTangent[0] : _outTangent[lowkey];
                                float t1 = _inTangent == null ? 0 : _inTangent.Length == 1 ? _inTangent[0] : _inTangent[hightKey];
                                switch (_interpolationType)
                                {
                                    case InterpolationMethod.STEP:
                                        Buffer.MemoryCopy(p0, pResult, _result.Length, _outDim * sizeof(float));
                                        break;
                                    case InterpolationMethod.LINEAR:
                                        Numerics.Lerp(pResult, p0, p1, s, _outDim);
                                        break;
                                    case InterpolationMethod.BEZIER:
                                        Numerics.Bezier(p0, p1, t0, t1, s, pResult, _outDim);
                                        break;
                                    case InterpolationMethod.HERMITE:
                                        Numerics.Hermite(p0, p1, t0, t1, s, pResult, _outDim);
                                        break;
                                    case InterpolationMethod.QuatSlerp:
                                        *(Quaternion*)pResult = Quaternion.Slerp(*(Quaternion*)p0, *(Quaternion*)p1, s);
                                        break;
                                    default:
                                        throw new InvalidOperationException("Interpolation method not implemented");
                                }
                            }
                            _OnSample(pResult);
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
