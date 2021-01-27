using Cybtans.Graphics.Buffers;
using Cybtans.Math;
using System;
using System.Runtime.InteropServices;

namespace Cybtans.Graphics.Meshes
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct MeshVertex
    {
        [VertexElement(VertexSemantic.Position)]
        public Vector3 Position;

        [VertexElement(VertexSemantic.Normal)]
        public Vector3 Normal;

        [VertexElement(VertexSemantic.Tangent)]
        public Vector3 Tangent;

        [VertexElement(VertexSemantic.TextureCoordinate, 0)]
        public Vector2 TexCoord;

        [VertexElement(VertexSemantic.OcclutionFactor)]
        public float OccFactor;

        public MeshVertex(float x, float y, float z,
                           float tx, float ty, float tz,
                           float nx, float ny, float nz,
                            float u, float v, float occ)
        {
            Position = new Vector3(x, y, z);
            Tangent = new Vector3(tx, ty, tz);
            Normal = new Vector3(nx, ny, nz);
            TexCoord = new Vector2(u, v);
            OccFactor = occ;
        }

        public MeshVertex(Vector3 position = default, Vector3 normal = default, Vector3 tangent = default, Vector2 texCoord = default, float occ = 0)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            TexCoord = texCoord;
            OccFactor = occ;

        }

        public override string ToString()
        {
            return Position.ToString();
            //return "P:" + Position.ToString() + " N:" + Normal + " T:" + Tangent + " Tx:" + TexCoord;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SkinnedVertex
    {
        [VertexElement(VertexSemantic.Position)]
        public Vector3 Position;

        [VertexElement(VertexSemantic.BlendWeights)]
        public Vector4 BlendWeights;

        [VertexElement(VertexSemantic.BlendIndices)]
        public Vector4 BlendIndices;

        [VertexElement(VertexSemantic.Normal)]
        public Vector3 Normal;

        [VertexElement(VertexSemantic.Tangent)]
        public Vector3 Tangent;

        [VertexElement(VertexSemantic.TextureCoordinate, 0)]
        public Vector2 TexCoord;

        [VertexElement(VertexSemantic.TextureCoordinate, 1)]
        public float OccFactor;

        public SkinnedVertex(float x, float y, float z,
                           float tx, float ty, float tz,
                           float nx, float ny, float nz,
                            float u, float v, float occ)
        {
            Position = new Vector3(x, y, z);
            Tangent = new Vector3(tx, ty, tz);
            Normal = new Vector3(nx, ny, nz);
            TexCoord = new Vector2(u, v);
            OccFactor = occ;
            BlendIndices = new Vector4();
            BlendWeights = new Vector4();
        }

        public SkinnedVertex(Vector3 position = default, Vector3 normal = default, Vector3 tangent = default, Vector2 texCoord = default, float occ = 0)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            TexCoord = texCoord;
            OccFactor = occ;
            BlendIndices = new Vector4();
            BlendWeights = new Vector4();
        }

        public override string ToString()
        {
            return Position.ToString();
            //return "P:" + Position.ToString() + " N:" + Normal + " T:" + Tangent + " Tx:" + TexCoord;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct VertexP
    {
        [VertexElement(VertexSemantic.Position)]
        public Vector3 Position;

        public VertexP(Vector3 position)
        {
            Position = position;
        }

        public override string ToString()
        {
            return Position.ToString();
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct VertexPNTTx
    {
        [VertexElement(VertexSemantic.Position)]
        public Vector3 Position;
        [VertexElement(VertexSemantic.Normal)]
        public Vector3 Normal;
        [VertexElement(VertexSemantic.Tangent)]
        public Vector3 Tangent;
        [VertexElement(VertexSemantic.TextureCoordinate)]
        public Vector2 TexCoord;

        public VertexPNTTx(float x, float y, float z,
                           float tx, float ty, float tz,
                           float nx, float ny, float nz,
                            float u, float v)
        {
            Position = new Vector3(x, y, z);
            Tangent = new Vector3(tx, ty, tz);
            Normal = new Vector3(nx, ny, nz);
            TexCoord = new Vector2(u, v);
        }

        public VertexPNTTx(Vector3 position = default, Vector3 normal = default, Vector3 tangent = default, Vector2 texCoord = default)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            TexCoord = texCoord;
        }

        public override string ToString()
        {
            return Position.ToString();
            //return "P:" + Position.ToString() + " N:" + Normal + " T:" + Tangent + " Tx:" + TexCoord;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct VertexPNTx
    {
        [VertexElement(VertexSemantic.Position)]
        public Vector3 Position;

        [VertexElement(VertexSemantic.Normal)]
        public Vector3 Normal;

        [VertexElement(VertexSemantic.TextureCoordinate)]
        public Vector2 TexCoord;

        public VertexPNTx(Vector3 position = default, Vector3 normal = default, Vector2 texCoord = default)
        {
            Position = position;
            Normal = normal;
            TexCoord = texCoord;
        }

        public override string ToString()
        {
            return "P(" + Position.ToString() + ") N(" + Normal.ToString() + ") T(" + TexCoord.ToString() + ")";
        }
    }

    [StructLayout(LayoutKind.Sequential), Serializable]
    public struct VertexPTx
    {
        [VertexElement(VertexSemantic.Position)]
        public Vector3 Position;

        [VertexElement(VertexSemantic.TextureCoordinate)]
        public Vector2 TexCoord;

        public VertexPTx(Vector3 position = default, Vector2 texCoord = default)
        {
            Position = position;
            TexCoord = texCoord;
        }

        public VertexPTx(float x, float y, float z, float u, float v)
        {
            Position = new Vector3(x, y, z);
            TexCoord = new Vector2(u, v);
        }

        public override string ToString()
        {
            return "P" + Position + " Tx" + TexCoord;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct VertexPTxH
    {
        [VertexElement(VertexSemantic.PositionTransformed)]
        public Vector4 Position;

        [VertexElement(VertexSemantic.TextureCoordinate)]
        public Vector2 TexCoord;

        public VertexPTxH(Vector4 position = default, Vector2 texCoord = default)
        {
            Position = position;
            TexCoord = texCoord;
        }

        public override string ToString()
        {
            return "P" + Position + " Tx" + TexCoord;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct VertexPositionColor
    {
        [VertexElement(VertexSemantic.Position)]
        public Vector3 Position;

        [VertexElement(VertexSemantic.Color)]
        public Color4 Color;

        public VertexPositionColor(Vector3 pos, Vector4 color)
        {
            Position = pos;
            Color = (Color4)color;
        }

        public VertexPositionColor(Vector3 Position, Color4 color)
        {
            this.Position = Position;
            Color = color;
        }


        public override string ToString()
        {
            return "P" + Position + " Color" + Color;
        }
    }


   
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct TerrainVertex
    {
        [VertexElement(VertexSemantic.Position)]
        public Vector3 Position;
        [VertexElement(VertexSemantic.Normal)]
        public Vector3 Normal;
        [VertexElement(VertexSemantic.TextureCoordinate, 0)]
        public Vector2 TexCoord;
        [VertexElement(VertexSemantic.TextureCoordinate, 1)]
        public Vector2 BlendTexCoord;
        [VertexElement(VertexSemantic.TextureCoordinate, 2)]
        public float OccFactor;

        public TerrainVertex(Vector3 position = default, Vector3 normal = default, Vector2 texCoord = default, Vector2 blendTexCoord = default, float occ = 0)
        {
            Position = position;
            Normal = normal;
            TexCoord = texCoord;
            BlendTexCoord = blendTexCoord;
            OccFactor = occ;
        }

        public override string ToString()
        {
            return "P(" + Position.ToString() + ") N(" + Normal.ToString() + ") T(" + TexCoord.ToString() + ")";
        }
    }


    [StructLayout(LayoutKind.Sequential), Serializable]
    public struct PointSprite
    {
        [VertexElement(VertexSemantic.Position)]
        public Vector3 Position;
        [VertexElement(VertexSemantic.TextureCoordinate, 0)]
        public Vector2 TexCoord;
        [VertexElement(VertexSemantic.Color)]
        public Color4 Color;
        [VertexElement(VertexSemantic.PointSize)]
        public float Size;
        [VertexElement(VertexSemantic.TextureCoordinate, 1)]
        public float Rotation;

        public override string ToString()
        {
            return Position.ToString();
        }
    }
}
