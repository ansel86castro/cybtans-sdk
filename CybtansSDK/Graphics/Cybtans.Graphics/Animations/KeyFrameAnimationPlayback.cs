using System.Collections.Generic;
using Cybtans.Graphics.Common;
using Cybtans.Graphics.Collections;

namespace Cybtans.Graphics.Animations
{
    public struct KeyFramePlaybackDesc
    {
        public KeyFrameAnimation Animation;
        public KeyFrameCursor Cursor;
        public float Blend;
        public float Velocity;

        public KeyFramePlaybackDesc(KeyFrameAnimation animation, KeyFrameCursor cursor = null, float blend = 1.0f, float velocity = 1.0f)
        {
            Animation = animation;
            Cursor = cursor ?? animation.GetCursor(0);
            Blend = blend;
            Velocity = velocity;
        }
    }

    public class KeyFrameAnimationPlayback : IEnumerable<KeyFramePlaybackDesc>, IDynamic
    {
        LinkedList<KeyFramePlaybackDesc> _animations = new LinkedList<KeyFramePlaybackDesc>();

        public KeyFrameAnimationPlayback()
        {

        }

        public KeyFrameAnimationPlayback(KeyFrameAnimation animation, KeyFrameCursor cursor = null, float blend = 1.0f, float velocity = 1.0f)
        {
            AddAnimation(animation, cursor, blend, velocity);
        }

        public KeyFrameAnimationPlayback(KeyFrameAnimation animation, float startTime, float duration, AnimationLooping loop, float blend = 1.0f, float velocity = 1.0f)
        {
            AddAnimation(animation, startTime, duration, loop, blend, velocity);
        }

        public int NbAnimations { get { return _animations.Count; } }

        public KeyFramePlaybackDesc FirstPlayback { get { return _animations.First.Value; } }

        public KeyFrameAnimationPlayback AddAnimation(KeyFrameAnimation animation, KeyFrameCursor cursor = null, float blend = 1.0f, float velocity = 1.0f)
        {
            var desc = new KeyFramePlaybackDesc(animation, cursor, blend, velocity);
            _animations.AddLast(desc);
            return this;
        }

        public KeyFrameAnimationPlayback AddAnimation(KeyFrameAnimation animation, float startTime, float duration, AnimationLooping loop, float blend = 1.0f, float velocity = 1.0f)
        {
            KeyFrameCursor cursor = new KeyFrameCursor(animation)
            {
                StartTime = startTime,
                EndTime = startTime + duration,
                Looping = loop
            };
            KeyFramePlaybackDesc b = new KeyFramePlaybackDesc(animation, cursor, blend, velocity);
            _animations.AddLast(b);
            return this;
        }

        public bool RemoveAnimation(KeyFrameAnimation animation)
        {
            var node = _animations.GetLinkedNode(x => x.Animation == animation);
            if (node != null)
            {
                _animations.Remove(node);
                return true;
            }
            return false;
        }

        public bool SetPlayback(KeyFramePlaybackDesc desc)
        {
            var node = _animations.GetLinkedNode(x => x.Animation == desc.Animation);
            if (node != null)
            {
                node.Value = desc;
                return true;
            }
            return false;
        }

        public bool GetPlayback(KeyFrameAnimation animation, out KeyFramePlaybackDesc desc)
        {
            var node = _animations.GetLinkedNode(x => x.Animation == animation);
            if (node != null)
            {
                desc = node.Value;
                return true;
            }
            desc = new KeyFramePlaybackDesc();
            return false;
        }

        public KeyFrameCursor GetCursor(KeyFrameAnimation animation)
        {
            KeyFramePlaybackDesc desc;
            GetPlayback(animation, out desc);
            return desc.Cursor;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_animations.GetEnumerator());
        }

        IEnumerator<KeyFramePlaybackDesc> IEnumerable<KeyFramePlaybackDesc>.GetEnumerator()
        {
            return new Enumerator(_animations.GetEnumerator());
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(_animations.GetEnumerator());
        }

        public void Update(float deltaT, float blend, float velocity)
        {
            foreach (var item in _animations)
            {
                var cursor = item.Cursor;
                var animation = item.Animation;

                cursor.PlayVelocity = item.Velocity * velocity;
                float blending = item.Blend * blend;
                animation.Update(deltaT, cursor, blending, blending >= 0);
            }
        }

        public void Update(float deltaT)
        {
            foreach (var item in _animations)
            {
                var cursor = item.Cursor;
                var animation = item.Animation;

                cursor.PlayVelocity = item.Velocity;
                float blending = item.Blend;
                animation.Update(deltaT, cursor, blending, blending >= 0);
            }
        }

        public void Reset()
        {
            foreach (var item in _animations)
            {
                item.Cursor.Reset();
            }
        }

        public struct Enumerator : IEnumerator<KeyFramePlaybackDesc>
        {
            LinkedList<KeyFramePlaybackDesc>.Enumerator _listEnumarator;

            public Enumerator(LinkedList<KeyFramePlaybackDesc>.Enumerator enumerator)
            {
                _listEnumarator = enumerator;
            }

            public KeyFramePlaybackDesc Current
            {
                get { return _listEnumarator.Current; }
            }

            public void Dispose()
            {
                _listEnumarator.Dispose();
            }

            object System.Collections.IEnumerator.Current
            {
                get { return _listEnumarator.Current; }
            }

            public bool MoveNext()
            {
                return _listEnumarator.MoveNext();
            }

            void System.Collections.IEnumerator.Reset()
            {
                ((IEnumerator<KeyFramePlaybackDesc>)_listEnumarator).Reset();
            }
        }
    }
}
