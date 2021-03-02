using Cybtans.Graphics.Common;
using System;

namespace Cybtans.Graphics.Animations
{
    public delegate void SampleHandler(bool blended, float blendWeight);

    /// <summary>
    /// Contains Several AnimationCurves for animating an set of objects properties. All the objects must
    /// share the same set of animated properties.
    /// Some propertis may be for example  translation, rotation and scale
    /// The setting of the object properties is handled through an AnimationContext
    /// </summary>  
    public class CurvesContainer : INameable
    {
        float[][] _curveKeys;
        KeyFrameCurve[] _curves;
        int[] _keysIndices;
        SampleInfo[] _keySamples;
        private float _lastKey = -1;

        [NonSerialized]
        KeyFrameAnimation _animation;

        public event SampleHandler SampleEnd;

        public CurvesContainer(string name)
        {
            Name = name;
        }

        public CurvesContainer() { }

        public string Name { get; set; }

        public string Target { get; set; }

        public KeyFrameAnimation Animation
        {
            get { return _animation; }
            set { _animation = value; }
        }

        public KeyFrameCurve[] Curves { get { return _curves; } set { _curves = value; } }
      
        public float[][] CurveKeys
        {
            get { return _curveKeys; }
            set
            {
                _curveKeys = value;
                if (_curveKeys != null)
                    _keySamples = new SampleInfo[_curveKeys.Length];
            }
        }

        public int[] KeysIndices
        {
            get { return _keysIndices; }
            set { _keysIndices = value; }
        }

        public float LastKeyValue
        {
            get
            {
                return _lastKey < 0 ? _lastKey = GetMaxKeyValue() : _lastKey;
            }
        }

        private void OnSampleEnd(bool blended, float blendWeight)
        {
            var action = SampleEnd;
            if (action != null)
            {
                action(blended, blendWeight);
            }
        }

        public void Sample(float time, float blendWeight = 1.0f, bool blended = false)
        {
            #region Compute Keys

            for (int i = 0; i < _curveKeys.Length; ++i)
            {
                SampleInfo info;

                var keys = CurveKeys[i];
                if (keys.Length == 0)
                    continue;

                else if (keys.Length == 1)
                {
                    info.S = 0;
                    info.HightKey = 0;
                    info.LowKey = 0;
                }
                else
                {
                    info.LowKey = _FindKeys(time, keys);
                    info.HightKey = System.Math.Min(info.LowKey + 1, keys.Length - 1);

                    float k0 = keys[info.LowKey];
                    float k1 = keys[info.HightKey];
                    info.S = (time - k0) / (k1 - k0);
                }

                _keySamples[i] = info;
            }

            #endregion

            #region Sample Curves

            for (int i = 0; i < _curves.Length; i++)
            {
                float[] keys;
                SampleInfo info;
                if (_keysIndices != null)
                {
                    keys = _curveKeys[_keysIndices[i]];
                    info = _keySamples[_keysIndices[i]];
                }
                else
                {
                    keys = _curveKeys[0];
                    info = _keySamples[0];
                }

                _curves[i].Sample(info.S, info.LowKey, info.HightKey, keys);
            }

            #endregion

            OnSampleEnd(blended, blendWeight);
        }

        public float GetMaxKeyValue()
        {
            float lastKeyValue = _GetLasKeyValue(_curveKeys[0]);
            foreach (var keys in _curveKeys)
            {
                lastKeyValue = System.Math.Max(lastKeyValue, _GetLasKeyValue(keys));
            }

            return lastKeyValue;
        }

        public float GetMinKeyValue()
        {
            float minKeyValue = _curveKeys[0][0];
            foreach (var keys in _curveKeys)
            {
                minKeyValue = System.Math.Min(minKeyValue, keys[0]);
            }

            return minKeyValue;
        }

        private float _GetLasKeyValue(float[] keys)
        {
            return keys[keys.Length - 1];
        }

        private int _FindKeys(float time, float[] keys)
        {
            if (time <= keys[0])
                return 0;

            int ini = 0;
            int end = keys.Length - 1;

            while (true)
            {
                int diff = end - ini;
                if (diff == 0)
                {
                    if (time >= keys[ini])
                        return ini;
                    else
                        return ini - 1;
                }
                else if (diff == 1)
                {
                    return ini;
                }
                else
                {
                    int middle = ini + end >> 1;
                    if (time > keys[middle])
                        ini = middle;
                    else
                        end = middle;
                }

            }
        }

        struct SampleInfo
        {
            public int LowKey;
            public int HightKey;
            public float S;
        }
    }

}
