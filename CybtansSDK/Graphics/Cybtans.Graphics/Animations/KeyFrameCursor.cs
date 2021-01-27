using System;

namespace Cybtans.Graphics.Animations
{
    public enum AnimationLooping
    {
        OneTime,
        Loop,
        PinPon
    }

    [Serializable]
    public class KeyFrameCursor
    {
        private float _time;
        private float _playVelocity = 1;
        private float _playingDir = 1;
        private bool _animationEndFlag;
        private AnimationLooping _looping;
        private float _startTime = -1;
        private float _endTime = -1;
        private bool _timeRestart;
        public event Action<KeyFrameAnimation, KeyFrameCursor> AnimationEnd;

        [NonSerialized]
        KeyFrameAnimation _animation;

        public KeyFrameCursor(KeyFrameAnimation animation)
        {
            if (animation == null) throw new ArgumentNullException("animation");
            _animation = animation;
        }

        public static KeyFrameCursor Create(KeyFrameAnimation animation)
        {
            return new KeyFrameCursor(animation);
        }

        public bool TimeRestart
        {
            get { return _timeRestart; }
        }

        public KeyFrameAnimation Animation { get { return _animation; } internal set { _animation = value; } }

        public float Time { get { return _time; } set { _time = value; } }

        public float PlayVelocity { get { return _playVelocity; } set { _playVelocity = value; } }

        public float PlayDirection { get { return _playingDir; } set { _playingDir = value; } }

        public AnimationLooping Looping
        {
            get { return _looping; }
            set
            {
                _looping = value;
            }
        }

        public float StartTime { get { return _startTime; } set { _startTime = value; } }

        public float EndTime { get { return _endTime; } set { _endTime = value; } }

        public bool Stoped { get { return _animationEndFlag; } }

        public void OnAnimationEnd()
        {
            _animationEndFlag = true;
            if (AnimationEnd != null)
                AnimationEnd(_animation, this);
        }

        public virtual void Reset()
        {
            _time = _startTime < 0 ? 0 : _startTime;
            if (_looping == AnimationLooping.PinPon)
            {
                _playingDir = System.Math.Abs(_playingDir);
            }

            _animationEndFlag = false;
        }

        public virtual void UpdateTime(float elapsed)
        {
            _time += _playVelocity * _playingDir * elapsed;
        }

        public void ValidateTime()
        {
            var lastKeyValue = _animation.LastKeyValue;
            _timeRestart = false;

            if (_endTime < 0) _endTime = lastKeyValue;
            if (_startTime < 0) _startTime = 0;

            if (_time == 0 || _startTime == _endTime)
                _time = _startTime;
            else if (_time > _endTime)
            {
                switch (_looping)
                {
                    case AnimationLooping.Loop:
                        _time = _startTime + (_time - _endTime);
                        _timeRestart = true;
                        break;
                    case AnimationLooping.PinPon:
                        _time = System.Math.Max(0, _endTime - (_time - _endTime));
                        _playingDir = -_playingDir;
                        break;
                    case AnimationLooping.OneTime:
                        _time = _endTime;
                        OnAnimationEnd();
                        break;
                }
            }
            else if (_time < _startTime)
            {
                switch (_looping)
                {
                    case AnimationLooping.Loop:
                        _time = _endTime - (_startTime - _time);
                        _timeRestart = true;
                        break;
                    case AnimationLooping.PinPon:
                        _time = _startTime + (_startTime - _time);
                        _playingDir = -_playingDir;
                        break;
                    case AnimationLooping.OneTime:
                        _time = _startTime;
                        OnAnimationEnd();
                        break;
                }
            }
        }

    }
}
