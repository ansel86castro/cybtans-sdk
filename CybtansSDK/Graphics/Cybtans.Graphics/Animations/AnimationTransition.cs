using Cybtans.Graphics.Common;
using Cybtans.Math;

namespace Cybtans.Graphics.Animations
{
    public class AnimationTransition : IDynamic
    {
        KeyFrameAnimationPlayback _currentPlayback;
        KeyFrameAnimationPlayback _nextPlayback;
        float _transitionTime;
        float _time;
        public TransitionBlend CurrentBlend = TransitionBlend.Out();
        public TransitionBlend NextBlend = TransitionBlend.In();
        bool _transitionComplete;
        bool _transitionStarted;

        public AnimationTransition()
        {
            _transitionTime = -1;
        }
        public AnimationTransition(float transitionTime = -1)
        {
            _transitionTime = transitionTime;
        }
        public AnimationTransition(KeyFrameAnimationPlayback current, KeyFrameAnimationPlayback next, float transitionTime)
            : this(transitionTime)
        {
            _currentPlayback = current;
            _nextPlayback = next;
        }

        public float TransitionTime { get { return _transitionTime; } set { _transitionTime = value; } }
        public float Time { get { return _time; } }
        public KeyFrameAnimationPlayback CurrentPlayback { get { return _currentPlayback; } set { _currentPlayback = value; } }
        public KeyFrameAnimationPlayback NextPlayback { get { return _nextPlayback; } set { _nextPlayback = value; } }

        public bool CanPerformTransition
        {
            get
            {

                return _time <= _transitionTime;
            }
        }

        public bool TransitionStarted { get { return _transitionStarted; } }
        public bool TransitionComplete { get { return !_transitionStarted && _transitionComplete; } }

        public void Update(float deltaT)
        {
            if (_time <= _transitionTime)
            {
                _transitionStarted = true;
                _transitionComplete = false;

                float s = _time / _transitionTime;

                CurrentBlend.UpdateBlendings(s);
                _currentPlayback.Update(deltaT, CurrentBlend.Blend, CurrentBlend.Velocity);

                NextBlend.UpdateBlendings(s);
                _nextPlayback.Update(deltaT, NextBlend.Blend, NextBlend.Velocity);

                _time += deltaT;
            }
            else
            {
                _time = 0;
                _transitionStarted = false;
                _transitionComplete = true;
            }
        }

        public void Reset()
        {
            ResetTime();
            _currentPlayback.Reset();
            _nextPlayback.Reset();
        }

        public void ResetTime()
        {
            _time = 0;
            _transitionStarted = false;
            _transitionComplete = false;
        }
    }

    public struct TransitionBlend
    {
        public float Blend;
        public float Velocity;
        public float StartBlend;
        public float EndBlend;
        public float StartVelocity;
        public float EndVelocity;

        public TransitionBlend(float startBlend, float startVelocity)
        {
            Blend = 1.0f;
            StartBlend = startBlend;
            Velocity = 1.0f;
            StartVelocity = startVelocity;
            EndBlend = 1.0f - StartBlend;
            EndVelocity = 1.0f - StartVelocity;
        }

        public TransitionBlend(float startBlend, float endBlend, float startVelocity, float endVelocity)
        {
            StartBlend = startBlend;
            EndBlend = endBlend;
            StartVelocity = startVelocity;
            EndVelocity = endVelocity;
            Blend = 0;
            Velocity = 0;
        }

        public static TransitionBlend In()
        {
            return new TransitionBlend(0, 0);
        }

        public static TransitionBlend Out()
        {
            return new TransitionBlend(1, 1);
        }

        public void UpdateBlendings(float s)
        {
            Blend = Numerics.Lerp(StartBlend, 1 - StartBlend, s);
            Velocity = Numerics.Lerp(StartVelocity, 1 - StartVelocity, s);
        }
    }
}
