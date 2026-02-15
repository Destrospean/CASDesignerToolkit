using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Destrospean.Graphics.OpenGL
{
    public class Shader
    {
        public int AttributeCount = 0,
        FragmentShaderID = -1,
        ProgramID = -1,
        UniformCount = 0,
        VertexShaderID = -1;

        public readonly Dictionary<string, AttributeInfo> Attributes = new Dictionary<string, AttributeInfo>();

        public readonly Dictionary<string, uint> Buffers = new Dictionary<string, uint>();

        public readonly Dictionary<string, UniformInfo> Uniforms = new Dictionary<string, UniformInfo>();

        public class AttributeInfo
        {
            public int Address = -1,
            Size = 0;

            public string Name = "";

            public ActiveAttribType Type;
        }

        public class UniformInfo
        {
            public int Address = -1,
            Size = 0;

            public string Name = "";

            public ActiveUniformType Type;
        }

        public Shader()
        {
            ProgramID = GL.CreateProgram();
        }

        public Shader(string vertexShader, string fragmentShader, bool fromFile = false)
        {
            ProgramID = GL.CreateProgram();
            if (fromFile)
            {
                LoadShaderFromFile(vertexShader, ShaderType.VertexShader);
                LoadShaderFromFile(fragmentShader, ShaderType.FragmentShader);
            }
            else
            {
                LoadShaderFromString(vertexShader, ShaderType.VertexShader);
                LoadShaderFromString(fragmentShader, ShaderType.FragmentShader);
            }
            Link();
            GenBuffers();
        }

        protected void LoadShader(string code, ShaderType type, out int address)
        {
            address = GL.CreateShader(type);
            GL.ShaderSource(address, code);
            GL.CompileShader(address);
            GL.AttachShader(ProgramID, address);
            System.Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        public void DisableVertexAttribArrays()
        {
            foreach (var attribute in Attributes.Values)
            {
                GL.DisableVertexAttribArray(attribute.Address);
            }
        }

        public void EnableVertexAttribArrays()
        {
            foreach (var attribute in Attributes.Values)
            {
                GL.EnableVertexAttribArray(attribute.Address);
            }
        }

        public void GenBuffers()
        {
            foreach (var attribute in Attributes.Values)
            {
                var buffers = 0u;
                GL.GenBuffers(1, out buffers);
                Buffers.Add(attribute.Name, buffers);
            }
            foreach (var uniform in Uniforms.Values)
            {
                var buffers = 0u;
                GL.GenBuffers(1, out buffers);
                Buffers.Add(uniform.Name, buffers);
            }
        }

        public int GetAttribute(string name)
        {
            if (Attributes.ContainsKey(name))
            {
                return Attributes[name].Address;
            }
            else
            {
                return -1;
            }
        }

        public uint GetBuffer(string name)
        {
            return Buffers.ContainsKey(name) ? Buffers[name] : 0;
        }

        public int GetUniform(string name)
        {
            return Uniforms.ContainsKey(name) ? Uniforms[name].Address : -1;
        }

        public void Link()
        {
            GL.LinkProgram(ProgramID);
            System.Console.WriteLine(GL.GetProgramInfoLog(ProgramID));
            GL.GetProgram(ProgramID, ProgramParameter.ActiveAttributes, out AttributeCount);
            GL.GetProgram(ProgramID, ProgramParameter.ActiveUniforms, out UniformCount);
            for (var i = 0; i < AttributeCount; i++)
            {
                var info = new AttributeInfo();
                info.Name = GL.GetActiveAttrib(ProgramID, i, out info.Size, out info.Type);
                info.Address = GL.GetAttribLocation(ProgramID, info.Name);
                Attributes.Add(info.Name, info);
            }
            for (var i = 0; i < UniformCount; i++)
            {
                var info = new UniformInfo();
                info.Name = GL.GetActiveUniform(ProgramID, i, out info.Size, out info.Type);
                info.Address = GL.GetUniformLocation(ProgramID, info.Name);
                Uniforms.Add(info.Name, info);
            }
        }

        public void LoadShaderFromFile(string filename, ShaderType type)
        {
            using (var streamReader = new System.IO.StreamReader(filename))
            {
                if (type == ShaderType.VertexShader)
                {
                    LoadShader(streamReader.ReadToEnd(), type, out VertexShaderID);
                }
                else if (type == ShaderType.FragmentShader)
                {
                    LoadShader(streamReader.ReadToEnd(), type, out FragmentShaderID);
                }
            }
        }

        public void LoadShaderFromString(string code, ShaderType type)
        {
            if (type == ShaderType.VertexShader)
            {
                LoadShader(code, type, out VertexShaderID);
            }
            else if (type == ShaderType.FragmentShader)
            {
                LoadShader(code, type, out FragmentShaderID);
            }
        }
    }
}

