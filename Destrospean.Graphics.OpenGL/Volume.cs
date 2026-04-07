using System.Collections.Generic;
using OpenTK;

namespace Destrospean.Graphics.OpenGL
{
    public class Volume
    {
        public int AmbientMapID, LODIndex, MainTextureID, SpecularMapID;

        public Vector3[] ColorData, Normals, Vertices;

        public int ColorDataCount
        {
            get
            {
                return ColorData.Length;
            }
        }

        public List<int[]> Faces = new List<int[]>();

        public string GroupID, Key;

        public int IndexCount
        {
            get
            {
                return Faces.Count * 3;
            }
        }

        public bool IsTextured = false;

        public Material Material = new Material();

        public Matrix4 ModelMatrix = Matrix4.Identity,
        ModelViewProjectionMatrix = Matrix4.Identity,
        ViewProjectionMatrix = Matrix4.Identity;

        public int NormalCount
        {
            get
            {
                return Normals.Length;
            }
        }

        public object Object;

        public Vector3 Position = Vector3.Zero,
        Rotation = Vector3.Zero,
        Scale = Vector3.One;

        public int TextureCoordinateCount
        {
            get
            {
                return TextureCoordinates.Length;
            }
        }

        public Vector2[] TextureCoordinates;

        public int VertexCount
        {
            get
            {
                return Vertices.Length;
            }
        }

        public void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.Scale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }

        public void CalculateNormals()
        {
            Vector3[] normals = new Vector3[VertexCount],
            vertices = Vertices;
            var indices = GetIndices();
            for (var i = 0; i < IndexCount; i += 3)
            {
                Vector3 a = vertices[indices[i]],
                b = vertices[indices[i + 1]],
                c = vertices[indices[i + 2]];
                normals[indices[i]] += Vector3.Cross(b - a, c - a);
                normals[indices[i + 1]] += Vector3.Cross(b - a, c - a);
                normals[indices[i + 2]] += Vector3.Cross(b - a, c - a);
            }
            for (var i = 0; i < NormalCount; i++)
            {
                normals[i].Normalize();
            }
            Normals = normals;
        }

        public int[] GetIndices(int offset = 0)
        {
            var indices = new List<int>();
            foreach (var face in Faces)
            {
                indices.Add(face[0] + offset);
                indices.Add(face[1] + offset);
                indices.Add(face[2] + offset);
            }
            return indices.ToArray();
        }
    }
}
