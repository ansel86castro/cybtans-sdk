using Cybtans.Graphics.Buffers;
using Cybtans.Graphics.Common;
using Cybtans.Graphics.Components;
using Cybtans.Graphics.Models;
using Cybtans.Math;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Cybtans.Graphics.Meshes
{
    public enum CoordMappingType
    {
        None, Spherical, Cylindrical
    }

    public class Mesh : IDisposable, INameable
    {
        MeshPart[]? _layers = new MeshPart[0];
        string[]? _materialSlotsNames;
        int[]? _adjacency;
        int _vertexCount;
        int _faceCount;
        private VertexDefinition _vd;
        VertexBuffer? _vb;
        IndexBuffer? _ib;
        MeshPart[][]? _materialLayersLookup = new MeshPart[0][]; //material per layer lookup            
        Boundings _volume = new Boundings();
        MeshPrimitive _primitive = MeshPrimitive.TriangleList;

     
        public Mesh(VertexDefinition vd)
        {
            _vd = vd;
            _vb = new VertexBuffer(vd);
        }

        public MeshPrimitive Primitive => _primitive;

        public int VertexCount { get { return _vb?.VertexCount ?? 0; } }

        public int FaceCount { get { return (_ib?.IndicesCount ?? 0) / 3; } }

        public int LayerCount { get { return _layers?.Length ?? 0; } }
    
        public int MaterialSlots { get { return _materialLayersLookup?.Length ?? 0; } }

        public MeshPart[]? Layers
        {
            get { return _layers; }
            set
            {
                _layers = value;

                for (int i = 0; i < _layers.Length; i++)
                    _layers[i].LayerId = i;

                _SetupMaterialsLayers();
            }
        }

        public VertexBuffer? VertexBuffer { get { return _vb; } set { _vb = value; } }

        public IndexBuffer? IndexBuffer { get { return _ib; } set { _ib = value; } }

        public int[]? Adjacency { get { return _adjacency; } set { _adjacency = value; } }

        public VertexDefinition? VertexDescriptor => _vb?.VertexDefinition;

        public string[]? MaterialSlotNames { get { return _materialSlotsNames; } set { _materialSlotsNames = value; } }

        public Boundings? Bounding => _volume;

        public string? Name { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();

        #region Public       

        public void ComputeBoundingVolumenes()
        {
            if (_volume == null)
            {
                _volume = new Boundings();
            }

            if (_vb == null)
                return;

            _volume.ComputeBoundingVolumenes(_vb);
        }     

        public void ComputeNormals()
        {
            if (_vb == null || _ib == null)
                return;

            using var trianglePos = _vb.GetIndexedBufferView<Vector3>(_ib, VertexSemantic.Position);
            using var triangleNormals = _vb.GetIndexedBufferView<Vector3>(_ib, VertexSemantic.Normal);

            for (int i = 0; i < trianglePos.Count; i += 3)
            {
                var p0 = trianglePos[i];
                var p1 = trianglePos[i + 1];
                var p2 = trianglePos[i + 2];

                Vector3 normal = Triangle.ComputeFaceNormal(p0, p1, p2);

                triangleNormals[i] += normal;
                triangleNormals[i + 1] += normal;
                triangleNormals[i + 2] += normal;
            }

            var normals = _vb.GetVertexBufferView<Vector3>(VertexSemantic.Normal);

            for (int i = 0; i < normals.Count; i++)
            {
                normals[i] = Vector3.Normalize(normals[i]);
            }

        }

        public void ComputeTangents()
        {
            if (_vb == null || _ib == null)
                return;

            using var trianglePos = _vb.GetIndexedBufferView<Vector3>(_ib, VertexSemantic.Position);
            using var triangleTangent = _vb.GetIndexedBufferView<Vector3>(_ib, VertexSemantic.Tangent);
            using var triangleTexC = _vb.GetIndexedBufferView<Vector2>(_ib, VertexSemantic.TextureCoordinate);

            for (int i = 0; i < trianglePos.Count; i += 3)
            {
                var p0 = trianglePos[i];
                var p1 = trianglePos[i + 1];
                var p2 = trianglePos[i + 2];

                Vector3 tangent = Triangle.ComputeFaceTangent(trianglePos[i], triangleTexC[i],
                                                            trianglePos[i + 1], triangleTexC[i + 1],
                                                             trianglePos[i + 2], triangleTexC[i + 2]);

                triangleTangent[i] += tangent;
                triangleTangent[i + 1] += tangent;
                triangleTangent[i + 2] += tangent;
            }

            var tangents = _vb.GetVertexBufferView<Vector3>(VertexSemantic.Tangent);

            for (int i = 0; i < tangents.Count; i++)
            {
                tangents[i] = Vector3.Normalize(tangents[i]);
            }



        }

        public void ComputeTextureCoords(CoordMappingType mapping)
        {
            if (_vb == null || _ib == null)
                return;

            using var positions = _vb.GetVertexBufferView<Vector3>(VertexSemantic.Position);
            using var texCoords = _vb.GetVertexBufferView<Vector2>(VertexSemantic.TextureCoordinate);


            var box = _volume.OrientedBox;
            Vector3 maxValues = box.GlobalTraslation + box.Extends;
            Vector3 minValues = box.GlobalTraslation - box.Extends;
            float height = maxValues.Y - minValues.Y;
            Vector3 pos;
            Vector2 tc = new Vector2();
            var center = box.GlobalTraslation;
            for (int i = 0; i < _vertexCount; i++)
            {
                #region Map
                switch (mapping)
                {
                    case CoordMappingType.None:
                        tc = (Vector2)positions[i];
                        break;
                    case CoordMappingType.Spherical:
                        //translate the center of the object to the origin
                        pos = positions[i] - center;
                        //compute the sphical coordinate of the vertex
                        Vector3 spherical = Vector3.CartesianToSpherical(pos);
                        // u =theta / 2PI
                        tc.X = spherical.Y / Numerics.TwoPI;
                        // v = phi / PI
                        tc.Y = spherical.X / Numerics.PI;
                        break;
                    case CoordMappingType.Cylindrical:
                        //translate the center of the object to the origin
                        pos = positions[i] - center;
                        //compute the sphical coordinate of the vertex
                        Vector3 cylindrical = Vector3.CartesianToCylindrical(pos);
                        // u =theta / 2PI
                        tc.X = cylindrical.X / Numerics.TwoPI;
                        // v = (y - minY)/ (maxY - minY)
                        tc.Y = (cylindrical.Y - minValues.Y) / height;
                        break;
                }
                #endregion
                texCoords[i] = tc;
            }

        }

        public MeshPart[] GetLayersByMaterial(int materialSlot)
        {
            if (_materialLayersLookup == null || materialSlot >= _materialLayersLookup.Length) throw new IndexOutOfRangeException("meshMaterialindex");
            return _materialLayersLookup[materialSlot];
        }

        public void DefragmentParts()
        {
            if (_vb == null || _ib == null || _layers == null ||  _layers.Length == 1)
                return;

            var vds = _vb.Pin();
            var ids = _ib.Pin();

            byte[] newIndices = new byte[_ib.Length];
            List<uint> vertList = new List<uint>(_vertexCount);

            var is16BitIndices = _ib.Is16BitsIndices;
            //guarda para el viejo indice el nuevo indice 
            Dictionary<uint, uint> indexHash = new Dictionary<uint, uint>(newIndices.Length);

            int componentCount = _layers.Length;
            int k = 0;

            unsafe
            {
                byte* indices = (byte*)ids;
                fixed (byte* pNewIndices = newIndices)
                {
                    foreach (var c in _layers)
                    {
                        int vertCount = 0;
                        uint oldindex = 0;
                        uint newIndex = 0;
                        uint oldStartIndex = (uint)c.StartIndex;

                        c.startIndex = k;
                        c.startVertex = vertList.Count;

                        //recorrer cada indice de la componente.
                        for (int i = 0; i < c.PrimitiveCount * 3; i++)
                        {
                            //antiguo indice
                            oldindex = _ib.Is16BitsIndices ? ((ushort*)indices)[oldStartIndex + i] : ((uint*)indices)[oldStartIndex + i];

                            if (!indexHash.ContainsKey(oldindex))
                            {
                                newIndex = (uint)vertList.Count;
                                indexHash.Add(oldindex, newIndex);
                                if (is16BitIndices)
                                    ((ushort*)pNewIndices)[k] = (ushort)newIndex;
                                else
                                    ((uint*)pNewIndices)[k] = newIndex;

                                vertList.Add(oldindex);
                                vertCount++;
                            }
                            else
                            {
                                newIndex = indexHash[oldindex];
                                if (is16BitIndices)
                                    ((ushort*)pNewIndices)[k] = (ushort)newIndex;
                                else
                                    ((uint*)pNewIndices)[k] = newIndex;
                            }
                            k++;
                        }

                        c.vertexCount = vertCount;
                        indexHash.Clear();
                    }
                }


                int size = _vd.Size;
                byte[] vertexes = new byte[vertList.Count * size];
                fixed (byte* pVertexes = vertexes)
                {
                    for (int i = 0; i < vertList.Count; i++)
                    {
                        byte* pVertex = (byte*)((uint)vds + vertList[i] * (uint)size);
                        byte* pDestVertex = pVertexes + i * size;
                        for (int j = 0; j < size; j++)
                        {
                            *(pDestVertex + j) = *(pVertex + j);
                        }
                    }
                }

                _vb.UnPin();
                _ib.UnPin();

                _vb.SetData(vertexes);
                _ib.SetData(newIndices);
            }
        }

        public unsafe void BlendLayers()
        {

            Dictionary<int, List<MeshPart>> materialList = new Dictionary<int, List<MeshPart>>(_materialLayersLookup.Length);
            for (int i = 0; i < _materialLayersLookup.Length; i++)
                materialList.Add(i, new List<MeshPart>(_materialLayersLookup[i]));

            List<MeshPart> newLayers = new List<MeshPart>();

            var ds = _ib.Pin();

            byte* pIndices = (byte*)ds;
            byte[] newIndices = new byte[_ib.Length];

            fixed (byte* pNewIndices = newIndices)
            {
                int iIndex = 0;
                Dictionary<uint, bool> vertexLookup = new Dictionary<uint, bool>();

                foreach (var v in materialList)
                {
                    vertexLookup.Clear();

                    MeshPart newLayer = new MeshPart();
                    if (newLayer == null)
                    {
                        _ib.UnPin();
                        return;
                    }
                    newLayers.Add(newLayer);
                    newLayer.materialIndex = v.Key;
                    newLayer.startIndex = iIndex;
                    newLayer.startVertex = int.MaxValue;

                    foreach (var layer in v.Value)
                    {
                        newLayer.primitiveCount += layer.primitiveCount;
                        newLayer.startVertex = System.Math.Min(newLayer.startVertex, layer.startVertex);

                        int indicesCount = layer.primitiveCount * 3;
                        for (int i = layer.startIndex; i < layer.startIndex + indicesCount; i++)
                        {
                            uint vertexIndex;
                            if (_ib.Is16BitsIndices)
                            {
                                vertexIndex = ((ushort*)pIndices)[i];
                                ((ushort*)pNewIndices)[iIndex] = ((ushort*)pIndices)[i];
                            }
                            else
                            {
                                vertexIndex = ((uint*)pIndices)[i];
                                ((uint*)pNewIndices)[iIndex] = ((uint*)pIndices)[i];
                            }
                            iIndex++;
                            vertexLookup[vertexIndex] = true;
                        }
                    }
                    newLayer.vertexCount = vertexLookup.Keys.Count;
                    newLayer.indexCount = newLayer.primitiveCount * 3;
                    materialList[v.Key].Clear();
                    materialList[v.Key].Add(newLayer);
                }

                Layers = newLayers.ToArray();

                _ib.UnPin();
                _ib.SetData(newIndices);
            }

        }

        public void Dispose()
        {
            if (_vb != null)
                _vb.Dispose();
            if (_ib != null)
                _ib.Dispose();
        }

        #endregion

        /// <summary>
        /// arrange the layers by materials 
        /// </summary>
        private void _SetupMaterialsLayers()
        {
            var materialsIndex = _layers.Select(x => x.materialIndex).Distinct().ToArray();

            //store for each material index the list of layers
            List<MeshPart>[] materialLayers = new List<MeshPart>[materialsIndex.Length];
            for (int i = 0; i < materialsIndex.Length; i++)
            {
                materialLayers[i] = new List<MeshPart>();
                foreach (var layer in _layers)
                {
                    if (layer.materialIndex == i)
                        materialLayers[i].Add(layer);
                }
            }
            _materialLayersLookup = new MeshPart[materialsIndex.Length][];
            for (int i = 0; i < _materialLayersLookup.Length; i++)
            {
                _materialLayersLookup[i] = materialLayers[i].ToArray();
            }
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }

        public void CreateVertexBuffer(ReadOnlySpan<byte>buffer)
        {
            _vb ??= new VertexBuffer(_vd);
            _vb.SetData(buffer);
        }

        public void CreateVertexBuffer<T>(Span<T> data) where T:unmanaged
        {
            _vd = VertexDefinition.GetDefinition<T>();
            _vb = new VertexBuffer(_vd);

            _vb.SetData(data);
        }

        public void CreateIndexBuffer(int[] buffer)
        {
            _ib ??= new IndexBuffer();
            _ib.SetData(buffer);
        }

        public void CreateIndexBuffer(short[] buffer)
        {
            _ib ??= new IndexBuffer();
            _ib.SetData(buffer);
        }

        public void CreateIndexBuffer(ushort[] buffer)
        {
            _ib ??= new IndexBuffer();
            _ib.SetData(buffer);
        }

        public void CreateIndexBuffer(uint[] buffer)
        {
            _ib ??= new IndexBuffer();
            _ib.SetData(buffer);
        }

        public MeshDto ToDto()
        {
            return new MeshDto
            {
                Name = Name,
                Id = Id,
                Adjacency = _adjacency?.ToList(),
                FaceCount = FaceCount,
                Primitive = (Models.MeshPrimitive)Primitive,
                VertexCount = VertexCount,
                MaterialSlots = _materialSlotsNames?.ToList(),
                Layers = _layers.Select(x => x.ToDto()).ToList(),
                VertexDeclaration = VertexDescriptor?.ToDto(),
                IndexBuffer = IndexBuffer?.ToArray(),
                VertexBuffer = VertexBuffer?.ToArray(),
                SixteenBitsIndices = IndexBuffer?.Is16BitsIndices ?? false
            };
        }
    }

}
