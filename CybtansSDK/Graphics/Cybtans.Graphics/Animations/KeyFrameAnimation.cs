using System;
using System.Collections.Generic;
using Cybtans.Graphics.Collections;
using Cybtans.Graphics.Common;

namespace Cybtans.Graphics.Animations
{
    public enum PlayingState
    {
        Stoped = 0,
        Playing = 1,
    };

   
    public partial class KeyFrameAnimation:INameable
    {
        List<KeyFrameCursor> _cursors = new List<KeyFrameCursor>(1);
        ObservedDictionary<string, CurvesContainer> _nodes;
        float _lastKeyValue = -1;

        public event Action<KeyFrameAnimation> AnimationStepEnd;

        public string Name { get; set; }

        public KeyFrameAnimation(string name)            
        {
            Name = name;
            //Creates the Defaul cursor
            _cursors.Add(KeyFrameCursor.Create(this));

            _nodes = new ObservedDictionary<string, CurvesContainer>(
                itemAdded: x =>
                {
                    x.Animation = this;
                },
                itemRemoved: x =>
                {
                    if (x.Animation == this) x.Animation = null;
                },
                keySelector: x => x.Name);
        }


        public ObservedDictionary<string, CurvesContainer> Nodes { get { return _nodes; } }

        public float LastKeyValue
        {
            get { return _lastKeyValue < 0 ? _lastKeyValue = _GetMaxKeyValue() : _lastKeyValue; }
            protected set { _lastKeyValue = value; }
        }

        public bool HasEnded(int cursorIndex)
        {
            return GetCursor(cursorIndex).Time == LastKeyValue;
        }

        public KeyFrameCursor GetCursor(int cursorIndex) { return _cursors[cursorIndex]; }

        public int NbCursors { get { return _cursors.Count; } }

        public KeyFrameCursor CreateCursor()
        {
            var cursor = new KeyFrameCursor(this);
            _cursors.Add(cursor);
            return cursor;
        }

        public void Update(float elapsedTime, float blendWeight = 1.0f, bool blended = false)
        {
            Update(elapsedTime, _cursors[0], blendWeight, blended);
        }

        public void Update(float elapsedTime, int stateIndex, float blendWeight = 1.0f, bool blended = false)
        {
            var state = _cursors[stateIndex];
            Update(elapsedTime, _cursors[stateIndex], blendWeight, blended);
        }

        public void Update(float elapsedTime, KeyFrameCursor state, float blendWeight = 1.0f, bool blended = false)
        {
            state.ValidateTime();
            Sample(state.Time, blendWeight, blended);
            state.UpdateTime(elapsedTime);
        }

        public void Sample(float time, float blendWeight = 1.0f, bool blended = false)
        {
            foreach (var node in _nodes)
            {
                node.Sample(time, blendWeight, blended);
            }

            _OnAnimationStepEnd();
        }

        protected float _GetMaxKeyValue()
        {
            float lastKeyValue = _nodes[0].LastKeyValue;
            foreach (var node in _nodes)
            {
                lastKeyValue = System.Math.Max(lastKeyValue, node.LastKeyValue);
            }

            return lastKeyValue;
        }

        private void _OnAnimationStepEnd()
        {
            if (AnimationStepEnd != null)
                AnimationStepEnd(this);
        }
    }
}
