using System.Collections.Generic;

namespace Cybtans.Graphics.Animations
{
    public class FrameAnimationManager
    {
        struct Data
        {
            public FrameAnimationController Controller;
        }

        SortedList<Frame, Data> _controllers = new SortedList<Frame, Data>();
        List<KeyFrameAnimation> _animations = new List<KeyFrameAnimation>();
        HashSet<Frame> _roots = new HashSet<Frame>();

        public List<KeyFrameAnimation> Animations { get { return _animations; } }

        public HashSet<Frame> Roots { get { return _roots; } }

        public void AddController(FrameAnimationController controller)
        {
            _controllers.Add(controller.Target, new Data
            {
                Controller = controller
            });
        }

        public FrameAnimationController GetController(Frame frame)
        {
            Data data;
            _controllers.TryGetValue(frame, out data);
            return data.Controller;
        }

        public FrameAnimationController GetController(int index)
        {
            return _controllers.Values[index].Controller;
        }

        public int NbControllers { get { return _controllers.Count; } }

        public IEnumerable<FrameAnimationController> EnumerateControllers()
        {
            foreach (var item in _controllers.Values)
            {
                yield return item.Controller;
            }
        }

        public void RestoreTransforms()
        {
            foreach (var item in _controllers)
            {
                item.Value.Controller.RestoreTransforms();
            }
        }

        public void Initialize()
        {
            foreach (var item in _controllers)
            {
                var frame = item.Value.Controller.Target;
                _roots.Add(frame.IsBone ? frame.GetBoneRoot() : frame.GetRoot());
            }
        }

        public void CommitChanges()
        {
            foreach (var root in _roots)
            {
                root.CommitChanges();
            }
        }

    }
}
