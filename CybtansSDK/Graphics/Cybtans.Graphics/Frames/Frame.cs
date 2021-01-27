using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cybtans.Entities;
using Cybtans.Graphics.Animations;
using Cybtans.Graphics.Collections;
using Cybtans.Graphics.Common;
using Cybtans.Graphics.Components;
using Cybtans.Graphics.Models;
using Cybtans.Math;

namespace Cybtans.Graphics
{

    public class Frame: IPoseable, IBindeable, IDisposable
    {
        #region Private Members    

        private string _name;
        private Vector3 _localScale = new Vector3(1, 1, 1);
        private Matrix _localRotationMtx = Matrix.Identity;
        private Quaternion _localRotationQuat = Quaternion.Identity;
        private Matrix _worldTransform = Matrix.Identity;
        private Matrix _localTransform = Matrix.Identity;
        private Matrix _bindParentMtx = Matrix.Identity;
        private Matrix _bindAffectorMtx = Matrix.Identity;

        private FrameBounding _boundInfo;      
        private IFrameComponent _nodeObject;
        private Frame _parent;      
        private ObservedDictionary<string, Frame> _nodes;

        private FrameType _nodeType;
        private float _range;        
        private string _tag;
        private IPoseable _affector;

        #endregion

        public Frame(string name)
            : this(name, null)
        {

        }

        public Frame(string name, IFrameComponent component)
        {
            Name = name;

            _nodeObject = component;
            if (component != null)
                component.Frame = this;       
        }

        public Frame(string name,
            Vector3 localPosition,
            Matrix localRotation,
            Vector3 localScale,
            IFrameComponent component) : this(name, component)
        {
            LocalPosition = localPosition;
            LocalRotation = localRotation;
            LocalScale = localScale;

            ComputeLocalPose();
            CommitChanges();
        }

        #region Properties       

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get => _name; set => _name = value; }
      
        public ObservedDictionary<string, Frame> Childrens
        {
            get
            {
                if (_nodes == null)
                {
                    _nodes = new ObservedDictionary<string, Frame>(itemAdded: x =>
                    {
                        if (x._parent != null)
                            x.Remove();
                        x._parent = this;
                    },
                       itemRemoved: x =>
                       {
                           x._parent = null;
                           x._bindParentMtx = Matrix.Identity;
                       },
                       keySelector: x => x.Name);
                }
                return _nodes;
            }
        }

        public Frame Parent { get { return _parent; } }
     
        public FrameType Type { get { return _nodeType; } set { _nodeType = value; } }
      
        public string Tag { get { return _tag; } set { _tag = value; } } 
        
        public IPoseable BindTarget { get => _affector; set => _affector = value; }
        
        public Dictionary<string, object> Props { get; set; }

        public IFrameComponent Component
        {
            get { return _nodeObject; }
            set
            {
                if (_nodeObject != value)
                {
                    if (_nodeObject != null)
                        _nodeObject.Frame = null;

                    if (value != null)
                    {
                        value.Frame = this;
                        _nodeObject = value;

                        var boundable = value.Bounding;
                        if (boundable != null)
                        {
                            if (_boundInfo == null)
                                _boundInfo = new FrameBounding();

                            _boundInfo.LocalSphere = boundable.Sphere;
                            if (boundable.OrientedBox != null)
                            {
                                _boundInfo.OrientedBox = boundable.OrientedBox.Clone();
                            }

                            _boundInfo.Update(_worldTransform);
                        }
                    }
                }
            }
        }    
            

        #region Transforms 

        public Vector3 LocalPosition
        {
            get
            {
                Vector3 v;
                v.X = _localTransform.M41;
                v.Y = _localTransform.M42;
                v.Z = _localTransform.M43;
                return v;
            }
            set
            {
                _localTransform.M41 = value.X;
                _localTransform.M42 = value.Y;
                _localTransform.M43 = value.Z;
            }
        }

        public Vector3 GlobalPosition
        {
            get { return _worldTransform.Translation; }
        }

        public float X
        {
            get { return _localTransform.M41; }
            set
            {
                _localTransform.M41 = value;
            }
        }

        public float Y
        {
            get { return _localTransform.M42; }
            set
            {
                _localTransform.M42 = value;
            }
        }

        public float Z
        {
            get { return _localTransform.M43; }
            set
            {
                _localTransform.M43 = value;
            }
        }


        public Vector3 LocalScale
        {
            get { return _localScale; }
            set
            {
                _localScale = value;              
            }
        }

        public float Sx
        {
            get { return _localScale.X; }
            set
            {
                _localScale.X = value;                
            }
        }

        public float Sy
        {
            get { return _localScale.Y; }
            set
            {
                _localScale.Y = value;                
            }
        }

        public float Sz
        {
            get { return _localScale.Z; }
            set
            {
                _localScale.Z = value;                
            }
        }


        public Matrix LocalRotation
        {
            get { return _localRotationMtx; }
            set
            {
                _localRotationMtx = value;
                _localRotationQuat = Quaternion.RotationMatrix(_localRotationMtx);               
            }
        }

        public Vector3 Right { get { return _localRotationMtx.Right; } set { _localRotationMtx.Right = value; } }

        public Vector3 Up { get { return _localRotationMtx.Up; } set { _localRotationMtx.Up = value; } }

        public Vector3 Front { get { return _localRotationMtx.Front; } set { _localRotationMtx.Front = value; } }

        public Euler LocalEuler
        {
            get { return Euler.FromMatrix(_localRotationMtx); }
            set
            {
                LocalRotation = value.ToMatrix();
            }
        }

        public Quaternion LocalRotationQuat
        {
            get { return _localRotationQuat; }
            set
            {
                _localRotationQuat = value;
                _localRotationMtx = Matrix.RotationQuaternion(Quaternion.Normalize(_localRotationQuat));
            }
        }

        public float Heading
        {
            get { return LocalEuler.Heading; }
            set
            {               
                var orientation = Euler.FromMatrix(_localRotationMtx);
                orientation.Heading = value;
                LocalRotation = orientation.ToMatrix();
            }
        }

        public float Pitch
        {
            get { return LocalEuler.Pitch; }
            set
            {
                value = Euler.NormalizePitch(value);

                var orientation = Euler.FromMatrix(_localRotationMtx);
                orientation.Pitch = value;
                LocalRotation = orientation.ToMatrix();
            }
        }

        public float Roll
        {
            get { return LocalEuler.Roll; }
            set
            {
                value = Euler.NormalizeRoll(value);

                var orientation = Euler.FromMatrix(_localRotationMtx);
                orientation.Roll = value;
                LocalRotation = orientation.ToMatrix();
            }
        }


        /// <summary>
        /// Local space tranform of the node
        /// </summary>             
        public Matrix LocalPose
        {
            get { return _localTransform; }
            set
            {
                _localTransform = value;
                Vector3 trans;
                Matrix.DecomposeTranformationMatrix(value, out _localScale, out _localRotationMtx, out trans);
                _localRotationQuat = Quaternion.RotationMatrix(_localRotationMtx);
            }
        }

        /// <summary>
        /// World space tranform
        /// </summary>                
        public ref Matrix GlobalPose { get {  return ref _worldTransform; } }

        /// <summary>
        /// Allows to transform the node whe the parent is transformed
        /// </summary>             
        public Matrix BindParentPose { get { return _bindParentMtx; } set { _bindParentMtx = value; } }

        /// <summary>
        /// Allows to tranform the node when actor is transformed (by the user or by the phycis engine).
        /// It's updates automaticaly when you tranform the node, or set it to invert of the actor`s global pose.
        /// </summary>                
        public Matrix BindAffectorPose { get { return _bindAffectorMtx; } set { _bindAffectorMtx = value; } }

        #endregion           
        public FrameBounding Bounding { get { return _boundInfo; } }

        public Sphere BoundingSphere { get { return _boundInfo != null ? _boundInfo.GlobalSphere : new Sphere(); } }

        public OrientedBox BoundingBox { get { return _boundInfo != null ? _boundInfo.OrientedBox : null; } }

        public float Range { get { return _range; } set { _range = value; } }

        public bool IsRoot { get { return _nodeType == FrameType.Root; } }

        public bool IsBone { get { return _nodeType == FrameType.Bone; } }

        public bool IsBoneRoot
        {
            get
            {
                return _nodeType == FrameType.Bone &&
                     (_parent == null || _parent._nodeType != FrameType.Bone);
            }
        }

        public FrameAnimationController AnimationController { get; set; }

        #endregion

        public void ComputeBindParentPose()
        {
            _bindParentMtx = _parent != null ? Matrix.Invert(_parent._worldTransform) : Matrix.Identity;
        }

        public bool Remove()
        {
            if (_parent != null)
            {
                return _parent._nodes.Remove(this);
            }
            return false;
        }    

        public Frame FindNode(string name)
        {
            Frame node = null;
            if (Name == name)
                node = this;

            else if (_nodes != null && !_nodes.TryGetValue(name, out node))
            {
                foreach (var item in _nodes)
                {
                    node = item.FindNode(name);
                    if (node != null)
                        break;
                }
            }

            return node;
        }

        public Frame FindNode(Guid id)
        {
            if (Id == Guid.Empty) return null;

            if (Id == id) return this;

            if (_nodes != null)
            {
                foreach (var item in _nodes)
                {
                    var result = item.FindNode(id);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        public Frame FindNode(Predicate<Frame> predicate)
        {
            if (predicate(this))
                return this;

            if (_nodes != null)
            {
                foreach (var item in _nodes)
                {
                    var r = item.FindNode(predicate);
                    if (r != null)
                        return r;
                }
            }
            return null;
        }

      
        public IEnumerable<Frame> FindNodeByTag(string tag)
        {
            foreach (var item in _nodes)
            {
                if (item._tag == tag)
                    yield return item;
                foreach (var child in item.FindNodeByTag(tag))
                {
                    yield return child;
                }
            }
        }

        public IEnumerable<Frame> EnumerateNodesPosOrden()
        {
            if (_nodes != null)
            {
                foreach (var node in _nodes)
                {
                    yield return node;
                    foreach (var item in node.EnumerateNodesPosOrden())
                    {
                        yield return item;
                    }
                }
            }
        }

        public IEnumerable<Frame> EnumerateNodesInPreOrden()
        {
            if (_nodes != null)
            {
                foreach (var node in _nodes)
                {
                    foreach (var item in node.EnumerateNodesPosOrden())
                    {
                        yield return item;
                    }
                    yield return node;
                }
            }
        }

        public void CopyNodes(ICollection<Frame> collection)
        {
            collection.Add(this);
            if (_nodes != null)
            {
                foreach (var item in _nodes)
                {
                    item.CopyNodes(collection);
                }
            }
        }       

        public Frame GetBoneRoot()
        {            
            Frame cursor = this;
            while (cursor.Parent != null && cursor.Parent.Type == FrameType.Bone)
                cursor = cursor.Parent;

            return cursor.Type == FrameType.Bone ? cursor : null;           
        }

        public Frame GetRoot()
        {
            if (_parent == null)
                return this;
            else
                return _parent.GetRoot();
        }
    

        /// <summary>
        /// Transformation orders are scale * rotation * translation
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="rotation"></param>
        /// <param name="translation"></param>
        public void ComputeLocalPose(Vector3 scale, Matrix rotation, Vector3 translation)
        {
            _localScale = scale;
            _localRotationMtx = rotation;
            _localRotationQuat = Quaternion.RotationMatrix(rotation);

            //set translation
            _localTransform.M41 = translation.X;
            _localTransform.M42 = translation.Y;
            _localTransform.M43 = translation.Z;
            _localTransform.M44 = 1;

            //set rotation and scale
            _localTransform.Right = rotation.Right * scale.X;
            _localTransform.Up = rotation.Up * scale.Y;
            _localTransform.Front = rotation.Front * scale.Z;
        }

        public void ComputeLocalPose(Vector3 scale, Quaternion rotation, Vector3 translation)
        {
            _localScale = scale;
            _localRotationMtx = Matrix.RotationQuaternion(rotation);
            _localRotationQuat = rotation;

            //set translation
            _localTransform.M41 = translation.X;
            _localTransform.M42 = translation.Y;
            _localTransform.M43 = translation.Z;
            _localTransform.M44 = 1;

            //set rotation and scale
            _localTransform.Right = _localRotationMtx.Right * scale.X;
            _localTransform.Up = _localRotationMtx.Up * scale.Y;
            _localTransform.Front = _localRotationMtx.Front * scale.Z;
        }

        public void ComputeLocalPose()
        {
            _localTransform.Right = _localRotationMtx.Right * _localScale.X;
            _localTransform.Up = _localRotationMtx.Up * _localScale.Y;
            _localTransform.Front = _localRotationMtx.Front * _localScale.Z;
        }

        private void _ComputeGlobalPose()
        {
            Matrix result;

            if (_parent == null)
                _worldTransform = _localTransform;
            else
            {
                Matrix.Multiply(ref _bindParentMtx, ref _parent._worldTransform, out result);
                Matrix.Multiply(ref _localTransform, ref result, out _worldTransform);               
            }

            if (_affector != null)
            {
                result = _affector.GlobalPose;
                Matrix.Multiply(ref _bindAffectorMtx, ref result, out result);
                Matrix.Multiply(ref _worldTransform, ref result, out _worldTransform);               
            }

            _OnPoseUpdated();
        }

        private void _UpdateGlobalPose(Matrix affectorPose)
        {
            _worldTransform = _localTransform;

            if (_parent != null)
                _worldTransform *= _bindParentMtx * _parent._worldTransform;

            _worldTransform *= _bindAffectorMtx * affectorPose;

            Matrix result;

            if (_parent == null)
                _worldTransform = _localTransform;
            else
            {
                Matrix.Multiply(ref _bindParentMtx, ref _parent._worldTransform, out result);
                Matrix.Multiply(ref _localTransform, ref result, out _worldTransform);
            }

            Matrix.Multiply(ref _bindAffectorMtx, ref affectorPose, out result);
            Matrix.Multiply(ref _worldTransform, ref result, out _worldTransform);

            _OnPoseUpdated();
        }

        public void CommitChanges()
        {
            _ComputeGlobalPose();

            if (_nodes != null)
            {
                foreach (var item in _nodes)
                {
                    item.CommitChanges();
                }
            }
        }

        private void _OnPoseUpdated()
        {
            if (_boundInfo != null)
            {
                _boundInfo.Update(_worldTransform);
            }
        
            if (_nodeObject != null)
                _nodeObject.OnPoseUpdated();
          
        }

        /// <summary>
        /// This method is called when the affector has influenced his affectable instance
        /// and the affactable needs to updates its GlobalPose. For Physics simulated objects this method is called after
        /// a simulation frame is completed
        /// </summary>
        public void UpdateBindPose()
        {
            _UpdateGlobalPose(_affector.GlobalPose);

            if (_nodes != null)
            {
                foreach (var item in _nodes)
                {
                    item.CommitChanges();
                }
            }
        }

      
        //public bool RemoveFromHeirarchy(SceneNode node)
        //{
        //    if (!Remove(node))
        //    {
        //        foreach (var item in nodes)
        //        {
        //            if (item.RemoveFromHeirarchy(node))
        //                return true;
        //        }
        //        return false;
        //    }
        //    return true;

        //}

        public bool Contains(string name)
        {
            if (_nodes == null) return false;
            if (!_nodes.ContainsKey(name))
            {
                foreach (var item in _nodes)
                {
                    if (item.Contains(name))
                        return true;
                }
            }
            return false;
        }

        public bool Contains(Frame node)
        {
            return Contains(node.Name);
        }

        public void Dispose()
        {
            if (_nodeObject != null)
                _nodeObject.Dispose();

            if (_nodes != null)
            {
                foreach (var node in _nodes)
                {
                    node.Dispose();
                }
            }

        }       

        public void Add(Frame frame)
        {
            Childrens.Add(frame);
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }

        public FrameDto ToDto()
        {
            return new FrameDto
            {
                Name = Name,
                Id = Id,
                BindParentTransform = BindParentPose.ToList(),
                ParentId = Parent?.Id,
                Range = Range,
                Tag = Tag,
                Type = (Models.FrameType)Type,
                LocalTransform = _localTransform.ToList(),
                WorldTransform = _worldTransform.ToList(),
                Childrens = Childrens.Select(x => x.ToDto()).ToList(),
                Component = Component?.ToDto()
            };
        }


        #region Helpers


        public Frame CreateChild<T>(T component) where T : IFrameComponent, INameable
        {
            var node = new Frame(component.Name, component);
            Childrens.Add(node);
            return node;
        }

        public Frame CreateChild(string name, IFrameComponent component)
        {
            var node = new Frame(name, component);
            _nodes.Add(node);
            return node;
        }

        public Frame CreateChild(string name, IFrameComponent component, Vector3 localPosition, Matrix localRotation, Vector3 localScale)
        {
            var node = new Frame(name, component);
            node.ComputeLocalPose(localScale, localRotation, localPosition);
            node.CommitChanges();
            Childrens.Add(node);
            return node;
        }

        public Frame CreateChild(string name, IFrameComponent component, Matrix localPose)
        {
            var node = new Frame(name, component);
            node.LocalPose = localPose;
            node.CommitChanges();
            Childrens.Add(node);
            return node;
        }

        public static Frame CreateNode(string name, IFrameComponent component, Matrix localPose)
        {
            var node = new Frame(name, component);
            node.LocalPose = localPose;
            node.CommitChanges();
            return node;
        }

        public static Frame CreateNode(string name, IFrameComponent component, Vector3 localPosition = default,
              Matrix localRotation = default, Vector3 localScale = default)
        {
            var node = new Frame(name, component);
            node.LocalPosition = localPosition;
            node.LocalRotation = localRotation.IsZero ? Matrix.Identity : localRotation;
            node.LocalScale = localScale == Vector3.Zero ? Vector3.One : localScale;
            node.ComputeLocalPose();
            node.CommitChanges();
            return node;
        }     

        #endregion

    
    }

}
