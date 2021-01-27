using Cybtans.Graphics.Common;
using System;
using System.Collections.Generic;

namespace Cybtans.Graphics.Animations
{
    public class SecuenceNode : INameable
    {
        LinkedList<SecuenceTransition> _transitions = new LinkedList<SecuenceTransition>();
        List<KeyFramePlaybackDesc> _animations = new List<KeyFramePlaybackDesc>();
        SecuenceTransition _currentTransition;
        string _name;
        int _id;

        public event Action<SecuenceNode, float> UpdateBegin;
        public event Action<SecuenceNode, float> UpdateEnd;
        public event Action<SecuenceNode> Enter;
        public event Action<SecuenceNode> Leave;

        public SecuenceNode(string name, int id = 0, IEnumerable<KeyFramePlaybackDesc> animations = null)
        {
            _name = name;
            _id = id;
            if (animations != null)
            {
                foreach (var item in animations)
                {
                    _animations.Add(item);
                }
            }
        }

        public SecuenceNode(string name, int id = 0, params KeyFramePlaybackDesc[] animations)
            : this(name, id, (IEnumerable<KeyFramePlaybackDesc>)animations)
        {

        }

        public SecuenceNode(string name, KeyFrameAnimation animation, float startTime, float duration, AnimationLooping loop, float blend = 1.0f)
            : this(name)
        {
            Playing(animation, startTime, duration, loop, blend);
        }

        public List<KeyFramePlaybackDesc> Animations { get { return _animations; } }

        public string Name { get { return _name; } set { _name = value; } }

        public int Id { get { return _id; } set { _id = value; } }

        public SecuenceTransition CurrentTransition { get { return _currentTransition; } }

        public void AddTransition(SecuenceTransition transition)
        {
            transition.SourceNode = this;
            _transitions.AddLast(transition);
        }

        public bool RemoveTransition(SecuenceTransition transition)
        {
            return _transitions.Remove(transition);
        }

        public bool RemoveTransitions(SecuenceNode node)
        {
            List<SecuenceTransition> tempTransitions = new List<SecuenceTransition>();

            foreach (var item in _transitions)
            {
                if (item.DestNode == node)
                    tempTransitions.Add(item);
            }

            foreach (var item in tempTransitions)
            {
                _transitions.Remove(item);
            }

            return tempTransitions.Count > 0;
        }

        public SecuenceNode TransitionTo(SecuenceTransition transition)
        {
            _transitions.AddLast(transition);
            return this;
        }

        public SecuenceNode BeforeUpdate(Action<SecuenceNode, float> action)
        {
            UpdateBegin = action;
            return this;
        }

        public SecuenceNode AfterUpdate(Action<SecuenceNode, float> action)
        {
            UpdateEnd = action;
            return this;
        }

        public SecuenceNode Activating(Action<SecuenceNode> action)
        {
            Enter = action;
            return this;
        }

        public SecuenceNode Deactivating(Action<SecuenceNode> action)
        {
            Leave = action;
            return this;
        }

        public void OnBeforeUpdate(float deltaT)
        {
            if (UpdateBegin != null)
                UpdateBegin(this, deltaT);
        }

        public void OnAfterUpdate(float deltaT)
        {
            if (UpdateEnd != null)
                UpdateEnd(this, deltaT);
        }

        public void OnActivating()
        {
            if (Enter != null)
                Enter(this);
        }

        public void OnDeactivating()
        {
            foreach (var item in _animations)
            {
                item.Cursor.Reset();
            }

            if (Leave != null)
                Leave(this);
        }

        public void UpdateAnimations(float deltaT, float blend, float velocity)
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

        public SecuenceNode Update(float deltaT)
        {
            if (_currentTransition == null)
            {
                foreach (var tr in _transitions)
                {
                    if (tr.IsTriggered())
                        _currentTransition = tr;
                }
            }

            SecuenceNode next = null;
            if (_currentTransition != null)
            {
                //it's in transition
                if (!_currentTransition.Blend(deltaT))
                {
                    //the transition has ended
                    //move to the next state
                    next = _currentTransition.DestNode;
                    _currentTransition = null;
                }
            }
            else
            {
                OnBeforeUpdate(deltaT);

                //its playing the state and there aren't any transitions triggered
                UpdateAnimations(deltaT, _animations.Count == 1 ? -1 : 1, 1);

                OnAfterUpdate(deltaT);
            }

            return next;
        }

        public SecuenceNode Playing(KeyFrameAnimation animation, KeyFrameCursor cursor = null, float blend = 1.0f)
        {
            KeyFramePlaybackDesc b = new KeyFramePlaybackDesc(animation, cursor, blend);
            _animations.Add(b);
            return this;
        }

        public SecuenceNode Playing(KeyFrameAnimation animation, float startTime, float duration, AnimationLooping loop, float blend = 1.0f)
        {
            KeyFrameCursor cursor = new KeyFrameCursor(animation)
            {
                StartTime = startTime,
                EndTime = startTime + duration,
                Looping = loop
            };
            KeyFramePlaybackDesc b = new KeyFramePlaybackDesc(animation, cursor, blend);
            _animations.Add(b);
            return this;
        }

        public void Reset()
        {
            foreach (var item in _animations)
            {
                item.Cursor.Reset();
            }
        }

        public float PlayDirection
        {
            get { return _animations[0].Cursor.PlayDirection; }
            set
            {
                foreach (var item in _animations)
                {
                    item.Cursor.PlayDirection = value;
                }
            }
        }

        public float PlayVelocity
        {
            get { return _animations[0].Cursor.PlayVelocity; }
            set
            {
                foreach (var item in _animations)
                {
                    item.Cursor.PlayVelocity = value;
                }
            }
        }

        public AnimationLooping Looping
        {
            get { return _animations[0].Cursor.Looping; }
            set
            {
                foreach (var item in _animations)
                {
                    item.Cursor.Looping = value;
                }
            }
        }

        public override string ToString()
        {
            return _name ?? base.ToString();
        }
    }

}
