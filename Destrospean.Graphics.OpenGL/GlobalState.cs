using System;
using System.Collections.Generic;
using Destrospean.S3PIExtensions;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Destrospean.Graphics.OpenGL
{
    public static class GlobalState
    {
        const int kMaxLights = 5;

        public static string ActiveShader = "default";

        public static Camera Camera = new Camera();

        public static Vector3[] ColorData, NormalData, NormalDeltaDataFat, NormalDeltaDataFit, NormalDeltaDataSpecial, NormalDeltaDataThin, VertexData, VertexDeltaDataFat, VertexDeltaDataFit, VertexDeltaDataSpecial, VertexDeltaDataThin;

        public static Vector3 CurrentRotation = Vector3.Zero;

        public static bool GLInitialized = false,
        Locked = false;

        public static int CurrentLODIndex = 0,
        IBOElements;

        public static int[] IndexData;

        public static List<Light> Lights = new List<Light>();

        public static object Lock = new object();

        public static readonly Dictionary<string, Material> LockedMaterials = new Dictionary<string, Material>(StringComparer.InvariantCultureIgnoreCase),
        Materials = new Dictionary<string, Material>(StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, Volume> LockedMeshes = new Dictionary<string, Volume>(),
        Meshes = new Dictionary<string, Volume>();

        public static readonly Dictionary<string, Shader> Shaders = new Dictionary<string, Shader>();

        public static Vector2[] TextureCoordinateData;

        public static readonly Dictionary<string, int> TextureIDs = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

        public static Matrix4 ViewMatrix = Matrix4.Identity;

        public static void DeleteTexture(string key)
        {
            int textureID;
            if (TextureIDs.TryGetValue(key, out textureID))
            {
                GL.DeleteTexture(textureID);
                TextureIDs.Remove(key);
            }
        }

        public static void DeleteTextures()
        {
            foreach (var textureID in TextureIDs.Values)
            {
                GL.DeleteTexture(textureID);
            }
            TextureIDs.Clear();
        }

        public static void InitProgram()
        {
            GL.GenBuffers(1, out IBOElements);
            var backportedFunctions = @"
                mat3 inverse(mat3 m)
                {
                    vec3 c0 = m[0];
                    vec3 c1 = m[1];
                    vec3 c2 = m[2];
                    vec3 v0 = cross(c1, c2);
                    vec3 v1 = cross(c2, c0);
                    vec3 v2 = cross(c0, c1);
                    float inv_det = 1.0 / dot(c0, v0);
                    return mat3(v0.x * inv_det, v0.y * inv_det, v0.z * inv_det, v1.x * inv_det, v1.y * inv_det, v1.z * inv_det, v2.x * inv_det, v2.y * inv_det, v2.z * inv_det);
                }

                mat4 inverse(mat4 m)
                {
                    float c00 = m[2][2] * m[3][3] - m[3][2] * m[2][3];
                    float c01 = m[1][2] * m[3][3] - m[3][2] * m[1][3];
                    float c02 = m[1][2] * m[2][3] - m[2][2] * m[1][3];
                    float c03 = m[2][1] * m[3][3] - m[3][1] * m[2][3];
                    float c04 = m[1][1] * m[3][3] - m[3][1] * m[1][3];
                    float c05 = m[1][1] * m[2][3] - m[2][1] * m[1][3];
                    float c06 = m[2][1] * m[3][2] - m[3][1] * m[2][2];
                    float c07 = m[1][1] * m[3][2] - m[3][1] * m[1][2];
                    float c08 = m[1][1] * m[2][2] - m[2][1] * m[1][2];
                    float c09 = m[2][0] * m[3][3] - m[3][0] * m[2][3];
                    float c10 = m[1][0] * m[3][3] - m[3][0] * m[1][3];
                    float c11 = m[1][0] * m[2][3] - m[2][0] * m[1][3];
                    float c12 = m[2][0] * m[3][2] - m[3][0] * m[2][2];
                    float c13 = m[1][0] * m[3][2] - m[3][0] * m[1][2];
                    float c14 = m[1][0] * m[2][2] - m[2][0] * m[1][2];
                    float c15 = m[2][0] * m[3][1] - m[3][0] * m[2][1];
                    float c16 = m[1][0] * m[3][1] - m[3][0] * m[1][1];
                    float c17 = m[1][0] * m[2][1] - m[2][0] * m[1][1];
                    vec4 f0 = vec4(c00, c00, c01, c02);
                    vec4 f1 = vec4(c03, c03, c04, c05);
                    vec4 f2 = vec4(c06, c06, c07, c08);
                    vec4 f3 = vec4(c09, c09, c10, c11);
                    vec4 f4 = vec4(c12, c12, c13, c14);
                    vec4 f5 = vec4(c15, c15, c16, c17);
                    vec4 v0 = vec4(m[1][0], m[0][0], m[0][0], m[0][0]);
                    vec4 v1 = vec4(m[1][1], m[0][1], m[0][1], m[0][1]);
                    vec4 v2 = vec4(m[1][2], m[0][2], m[0][2], m[0][2]);
                    vec4 v3 = vec4(m[1][3], m[0][3], m[0][3], m[0][3]);
                    vec4 i0 = v1 * f0 - v2 * f1 + v3 * f2;
                    vec4 i1 = v0 * f0 - v2 * f3 + v3 * f4;
                    vec4 i2 = v0 * f1 - v1 * f3 + v3 * f5;
                    vec4 i3 = v0 * f2 - v1 * f4 + v2 * f5;
                    vec4 signA = vec4(1.0, -1.0, 1.0, -1.0);
                    vec4 signB = vec4(-1.0, 1.0, -1.0, 1.0);
                    mat4 inv = mat4(i0 * signA, i1 * signB, i2 * signA, i3 * signB);
                    return inv * 1.0 / dot(m[0], inv[0]);
                }

                mat3 transpose(mat3 m)
                {
                    return mat3(vec3(m[0].x, m[1].x, m[2].x), vec3(m[0].y, m[1].y, m[2].y), vec3(m[0].z, m[1].z, m[2].z));
                }

                mat4 transpose(mat4 m)
                {
                    mat4 result;
                    result[0][0] = m[0][0];
                    result[0][1] = m[1][0];
                    result[0][2] = m[2][0];
                    result[0][3] = m[3][0];
                    result[1][0] = m[0][1];
                    result[1][1] = m[1][1];
                    result[1][2] = m[2][1];
                    result[1][3] = m[3][1];
                    result[2][0] = m[0][2];
                    result[2][1] = m[1][2];
                    result[2][2] = m[2][2];
                    result[2][3] = m[3][2];
                    result[3][0] = m[0][3];
                    result[3][1] = m[1][3];
                    result[3][2] = m[2][3];
                    result[3][3] = m[3][3];
                    return result;
                }";
            Shaders.Add("default", new Shader(@"
                #version 110

                attribute vec3 vPosition;
                attribute vec3 vColor;
                varying vec4 color;
                uniform mat4 modelview;
     
                void main()
                {
                    gl_Position = modelview * vec4(vPosition, 1.0);
                    color = vec4(vColor, 1.0);
                }", @"
                #version 110

                varying vec4 color;
     
                void main()
                {
                    gl_FragColor = color;
                }"));
            Shaders.Add("textured", new Shader(@"
                #version 110

                attribute vec3 vPosition;
                attribute vec3 vDeltaPositionFat;
                attribute vec3 vDeltaPositionFit;
                attribute vec3 vDeltaPositionSpecial;
                attribute vec3 vDeltaPositionThin;
                attribute vec2 texcoord;
                varying vec2 f_texcoord;
                uniform mat4 modelview;
                uniform vec4 morphWeights;
                uniform bool hasMorphs;

                void main()
                {
                    vec3 morphPos = vPosition;
                    if (hasMorphs)
                    {{
                        morphPos = morphPos + vDeltaPositionFat * morphWeights.x;
                        morphPos = morphPos + vDeltaPositionFit * morphWeights.y;
                        morphPos = morphPos + vDeltaPositionSpecial * morphWeights.z;
                        morphPos = morphPos + vDeltaPositionThin * morphWeights.w;
                    }}
                    gl_Position = modelview * vec4(morphPos, 1.0);
                    f_texcoord = texcoord;
                }", @"
                #version 110

                varying vec2 f_texcoord;
                uniform sampler2D maintexture;
                uniform bool hasTransparency;
                uniform vec3 skin_color;
     
                void main()
                {
                    vec4 texcolor = texture2D(maintexture, f_texcoord);
                    if (texcolor.a < 0.1)
                    {{
                        if (hasTransparency)
                        {{
                            discard;
                        }}
                        else
                        {{
                            texcolor = vec4(skin_color, 1.0);
                        }}
                    }}
                    gl_FragColor = texcolor;
                }"));
            Shaders.Add("normal", new Shader(@"
                #version 110

                attribute vec3 vPosition;
                attribute vec3 vNormal;
                varying vec3 v_norm;
                uniform mat4 modelview;
     
                void main()
                {
                    gl_Position = modelview * vec4(vPosition, 1.0);
                    v_norm = normalize(mat3(modelview[0].xyz, modelview[1].xyz, modelview[2].xyz) * vNormal);
                    v_norm = vNormal;
                }", @"
                #version 110

                varying vec3 v_norm;
     
                void main()
                {
                    vec3 n = normalize(v_norm);
                    gl_FragColor = vec4(0.5 + 0.5 * n, 1.0);
                }"));
            Shaders.Add("lit", new Shader(string.Format(@"
                #version 110

                attribute vec3 vPosition;
                attribute vec3 vDeltaPositionFat;
                attribute vec3 vDeltaPositionFit;
                attribute vec3 vDeltaPositionSpecial;
                attribute vec3 vDeltaPositionThin;
                attribute vec3 vNormal;
                attribute vec3 vDeltaNormalFat;
                attribute vec3 vDeltaNormalFit;
                attribute vec3 vDeltaNormalSpecial;
                attribute vec3 vDeltaNormalThin;
                attribute vec2 texcoord;
                varying vec3 v_norm;
                varying vec3 v_pos;
                varying vec2 f_texcoord;
                uniform mat4 modelview;
                uniform mat4 model;
                uniform mat4 view;
                uniform vec4 morphWeights;
                uniform bool hasMorphs;

                {0}

                void main()
                {{
                    vec3 morphPos = vPosition;
                    if (hasMorphs)
                    {{
                        morphPos = morphPos + vDeltaPositionFat * morphWeights.x;
                        morphPos = morphPos + vDeltaPositionFit * morphWeights.y;
                        morphPos = morphPos + vDeltaPositionSpecial * morphWeights.z;
                        morphPos = morphPos + vDeltaPositionThin * morphWeights.w;
                    }}
                    gl_Position = modelview * vec4(morphPos, 1.0);
                    f_texcoord = texcoord;
                    mat3 normMatrix = transpose(inverse(mat3(model[0].xyz, model[1].xyz, model[2].xyz)));
                    v_norm = vNormal;
                    if (hasMorphs)
                    {{
                        v_norm = v_norm + vDeltaNormalFat * morphWeights.x;
                        v_norm = v_norm + vDeltaNormalFit * morphWeights.y;
                        v_norm = v_norm + vDeltaNormalSpecial * morphWeights.z;
                        v_norm = v_norm + vDeltaNormalThin * morphWeights.w;
                    }}
                    v_norm = normMatrix * v_norm;
                    v_pos = (model * vec4(morphPos, 1.0)).xyz;
                }}", backportedFunctions), string.Format(@"
                #version 110

                struct Light
                {{
                    vec3 position;
                    vec3 color;
                    float ambientIntensity;
                    float diffuseIntensity;
                    int type;
                    vec3 direction;
                    float coneAngle;
                    float linearAttenuation;
                    float quadraticAttenuation;
                    float radius;
                }};
                varying vec3 v_norm;
                varying vec3 v_pos;
                varying vec2 f_texcoord;
                uniform sampler2D maintexture;
                uniform bool hasAmbientMap;
                uniform bool hasSkinAmbientMap;
                uniform bool hasSpecularMap;
                uniform bool hasSkinSpecularMap;
                uniform bool hasTransparency;
                uniform sampler2D map_ambient;
                uniform sampler2D map_skin_ambient;
                uniform sampler2D map_specular;
                uniform sampler2D map_skin_specular;
                uniform mat4 view;
                uniform vec3 material_ambient;
                uniform vec3 material_diffuse;
                uniform vec3 material_specular;
                uniform float material_specExponent;
                uniform Light lights[5];
                uniform vec3 skin_color;

                {0}

                void main()
                {{
                    bool isSkin = false;
                    vec3 n = normalize(v_norm);
                    vec4 texcolor = texture2D(maintexture, f_texcoord);
                    if (texcolor.a < 0.1)
                    {{
                        if (hasTransparency)
                        {{
                            discard;
                        }}
                        else
                        {{
                            isSkin = true;
                            texcolor = vec4(skin_color, 1.0);
                        }}
                    }}
                    gl_FragColor = vec4(0.0, 0.0, 0.0, texcolor.a);
                    for (int i = 0; i < 5; i++)
                    {{
                        if (lights[i].color == vec3(0.0, 0.0, 0.0))
                        {{
                            continue;
                        }}
                        vec3 lightvec = normalize(lights[i].position - v_pos);
                        vec4 lightcolor = vec4(0.0, 0.0, 0.0, 1.0);
                        if (lights[i].type == 0)
                        {{
                            lightvec = lights[i].direction;
                        }}
                        vec4 light_ambient = lights[i].ambientIntensity * vec4(lights[i].color, 0.0);
                        vec4 light_diffuse = lights[i].diffuseIntensity * vec4(lights[i].color, 0.0);
                        if (isSkin)
                        {{
                            lightcolor = lightcolor + texcolor * light_ambient * vec4(0.25, 0.25, 0.25, 0.0);
                        }}
                        else if (hasAmbientMap)
                        {{
                            lightcolor = lightcolor + texcolor * light_ambient * vec4(material_ambient, 0.0) * texture2D(map_ambient, f_texcoord).r;
                        }}
                        else
                        {{
                            lightcolor = lightcolor + texcolor * light_ambient * vec4(material_ambient, 0.0);
                        }}
                        float lambertmaterial_diffuse = max(dot(n, lightvec), 0.0);
                        bool inConeOrNotSpotlight = lights[i].type != 2 || degrees(acos(dot(lightvec, lights[i].direction))) < lights[i].coneAngle;
                        if (inConeOrNotSpotlight)
                        {{
                            lightcolor = lightcolor + light_diffuse * texcolor * vec4(material_diffuse, 0.0) * lambertmaterial_diffuse;
                        }}
                        vec3 reflectionvec = normalize(reflect(-lightvec, v_norm));
                        vec3 viewvec = normalize(vec3(inverse(view) * vec4(0.0, 0.0, 0.0, 1.0)) - v_pos); 
                        float material_specularreflection = max(dot(v_norm, lightvec), 0.0) * pow(max(dot(reflectionvec, viewvec), 0.0), material_specExponent);
                        if (hasSkinSpecularMap && isSkin)
                        {{
                            material_specularreflection = material_specularreflection * texture2D(map_skin_specular, f_texcoord).r;
                        }}
                        else if (hasSpecularMap)
                        {{
                            material_specularreflection = material_specularreflection * texture2D(map_specular, f_texcoord).r;
                        }}
                        if (inConeOrNotSpotlight)
                        {{
                            lightcolor = lightcolor + vec4(material_specular * lights[i].color, 0.0) * material_specularreflection;
                        }}
                        float distancefactor = distance(lights[i].position, v_pos);
                        gl_FragColor = gl_FragColor + lightcolor * 1.0 / (1.0 + distancefactor * lights[i].linearAttenuation + distancefactor * distancefactor * lights[i].quadraticAttenuation);
                        gl_FragColor.a = texcolor.a;
                    }}
                }}", backportedFunctions)));
            ActiveShader = Common.ApplicationSettings.UseAdvancedOpenGLShaders ? "lit" : "textured";
            Lights.Add(new Light(new Vector3(0, 1, 3), Vector3.One)
                {
                    QuadraticAttenuation = .05f
                });
            Lights.Add(new Light(new Vector3(0, 1, -3), Vector3.One)
                {
                    Direction = new Vector3(0, 0, 1),
                    QuadraticAttenuation = .05f
                });
            Camera.Position = new Vector3(0, 1, 4);
        }

        public static int LoadTexture(string key, System.Drawing.Bitmap image = null)
        {
            if (image == null)
            {
                return CmarNYCBorrowed.TextureUtils.PreloadedGameImages.TryGetValue(key, out image) || CmarNYCBorrowed.TextureUtils.PreloadedImages.TryGetValue(key, out image) ? LoadTexture(key, image) : -1;
            }
            try
            {
                if (!GLInitialized)
                {
                    return -1;
                }
                int textureID;
                if (!TextureIDs.TryGetValue(key, out textureID))
                {
                    GL.GenTextures(1, out textureID);
                    TextureIDs.Add(key, textureID);
                }
                GL.BindTexture(TextureTarget.Texture2D, textureID);
                var bitmapData = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                image.UnlockBits(bitmapData);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                return textureID;
            }
            catch (Exception ex)
            {
                Common.ProgramUtils.WriteError(ex);
                return -1;
            }
        }

        public static void OnRenderFrame(int width, int height)
        {
            lock (Lock)
            {
                GL.Viewport(0, 0, width, height);
                GL.ClearColor(System.Drawing.Color.CornflowerBlue);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Enable(EnableCap.DepthTest);
                var indexAt = 0;
                foreach (var meshKey in new List<string>((Locked ? LockedMeshes : Meshes).Keys))
                {
                    Volume mesh;
                    if (!(Locked ? LockedMeshes : Meshes).TryGetValue(meshKey, out mesh))
                    {
                        continue;
                    }
                    var shader = mesh.Material.Shader == "" ? ActiveShader : mesh.Material.Shader;
                    GL.UseProgram(Shaders[shader].ProgramID);
                    Shaders[shader].EnableVertexAttribArrays();
                    var casPartVolume = mesh as Sims3.Sim.CASPartVolume;
                    if (Shaders[shader].GetUniform("morphWeights") != -1)
                    {
                        if (casPartVolume == null)
                        {
                            GL.Uniform1(Shaders[shader].GetUniform("hasMorphs"), 0);
                        }
                        else
                        {
                            GL.Uniform1(Shaders[shader].GetUniform("hasMorphs"), 1);
                            GL.Uniform4(Shaders[shader].GetUniform("morphWeights"), new Vector4(casPartVolume.ParentSim.Fat, casPartVolume.ParentSim.Fit, casPartVolume.ParentSim.Special, casPartVolume.ParentSim.Thin));
                        }
                    }
                    if (Shaders[shader].GetUniform("skin_color") != -1)
                    {
                        GL.Uniform3(Shaders[shader].GetUniform("skin_color"), casPartVolume == null ? Vector3.One : new Vector3(casPartVolume.ParentSim.SkinColor[0], casPartVolume.ParentSim.SkinColor[1], casPartVolume.ParentSim.SkinColor[2]));
                    }
                    if (Shaders[shader].GetUniform("hasTransparency") != -1)
                    {
                        GL.Uniform1(Shaders[shader].GetUniform("hasTransparency"), Convert.ToInt32(mesh.Material.HasTransparency));
                    }
                    GL.BindTexture(TextureTarget.Texture2D, mesh.MainTextureID);
                    GL.UniformMatrix4(Shaders[shader].GetUniform("modelview"), false, ref mesh.ModelViewProjectionMatrix);
                    if (Shaders[shader].GetUniform("light_ambientIntensity") != -1)
                    {
                        GL.Uniform1(Shaders[shader].GetUniform("light_ambientIntensity"), Lights[0].AmbientIntensity);
                    }
                    if (Shaders[shader].GetUniform("light_color") != -1)
                    {
                        GL.Uniform3(Shaders[shader].GetUniform("light_color"), ref Lights[0].Color);
                    }
                    if (Shaders[shader].GetUniform("light_diffuseIntensity") != -1)
                    {
                        GL.Uniform1(Shaders[shader].GetUniform("light_diffuseIntensity"), Lights[0].DiffuseIntensity);
                    }
                    if (Shaders[shader].GetUniform("light_position") != -1)
                    {
                        GL.Uniform3(Shaders[shader].GetUniform("light_position"), ref Lights[0].Position);
                    }
                    for (var i = 0; i < Math.Min(Lights.Count, kMaxLights); i++)
                    {
                        if (Shaders[shader].GetUniform("lights[" + i + "].ambientIntensity") != -1)
                        {
                            GL.Uniform1(Shaders[shader].GetUniform("lights[" + i + "].ambientIntensity"), Lights[i].AmbientIntensity);
                        }
                        if (Shaders[shader].GetUniform("lights[" + i + "].color") != -1)
                        {
                            GL.Uniform3(Shaders[shader].GetUniform("lights[" + i + "].color"), ref Lights[i].Color);
                        }
                        if (Shaders[shader].GetUniform("lights[" + i + "].coneAngle") != -1)
                        {
                            GL.Uniform1(Shaders[shader].GetUniform("lights[" + i + "].coneAngle"), Lights[i].ConeAngle);
                        }
                        if (Shaders[shader].GetUniform("lights[" + i + "].diffuseIntensity") != -1)
                        {
                            GL.Uniform1(Shaders[shader].GetUniform("lights[" + i + "].diffuseIntensity"), Lights[i].DiffuseIntensity);
                        }
                        if (Shaders[shader].GetUniform("lights[" + i + "].direction") != -1)
                        {
                            GL.Uniform3(Shaders[shader].GetUniform("lights[" + i + "].direction"), ref Lights[i].Direction);
                        }
                        if (Shaders[shader].GetUniform("lights[" + i + "].linearAttenuation") != -1)
                        {
                            GL.Uniform1(Shaders[shader].GetUniform("lights[" + i + "].linearAttenuation"), Lights[i].LinearAttenuation);
                        }
                        if (Shaders[shader].GetUniform("lights[" + i + "].position") != -1)
                        {
                            GL.Uniform3(Shaders[shader].GetUniform("lights[" + i + "].position"), ref Lights[i].Position);
                        }
                        if (Shaders[shader].GetUniform("lights[" + i + "].quadraticAttenuation") != -1)
                        {
                            GL.Uniform1(Shaders[shader].GetUniform("lights[" + i + "].quadraticAttenuation"), Lights[i].QuadraticAttenuation);
                        }
                        if (Shaders[shader].GetUniform("lights[" + i + "].type") != -1)
                        {
                            GL.Uniform1(Shaders[shader].GetUniform("lights[" + i + "].type"), (int)Lights[i].Type);
                        }
                    }
                    if (Shaders[shader].GetAttribute("maintexture") != -1)
                    {
                        GL.Uniform1(Shaders[shader].GetAttribute("maintexture"), mesh.MainTextureID);
                    }
                    if (Shaders[shader].GetUniform("map_ambient") != -1)
                    {
                        if (mesh.AmbientMapID == -1)
                        {
                            GL.Uniform1(Shaders[shader].GetUniform("hasAmbientMap"), 0);
                        }
                        else
                        {
                            GL.ActiveTexture(TextureUnit.Texture3);
                            GL.BindTexture(TextureTarget.Texture2D, mesh.AmbientMapID);
                            GL.Uniform1(Shaders[shader].GetUniform("map_ambient"), 3);
                            GL.Uniform1(Shaders[shader].GetUniform("hasAmbientMap"), 1);
                            GL.ActiveTexture(TextureUnit.Texture0);
                        }
                    }
                    if (Shaders[shader].GetUniform("map_skin_ambient") != -1)
                    {
                        if (casPartVolume == null || casPartVolume.SkinAmbientMapID == -1)
                        {
                            GL.Uniform1(Shaders[shader].GetUniform("hasSkinAmbientMap"), 0);
                        }
                        else
                        {
                            GL.ActiveTexture(TextureUnit.Texture4);
                            GL.BindTexture(TextureTarget.Texture2D, casPartVolume.SkinAmbientMapID);
                            GL.Uniform1(Shaders[shader].GetUniform("map_skin_ambient"), 4);
                            GL.Uniform1(Shaders[shader].GetUniform("hasSkinAmbientMap"), 1);
                            GL.ActiveTexture(TextureUnit.Texture0);
                        }
                    }
                    if (Shaders[shader].GetUniform("map_specular") != -1)
                    {
                        if (mesh.SpecularMapID == -1)
                        {
                            GL.Uniform1(Shaders[shader].GetUniform("hasSpecularMap"), 0);
                        }
                        else
                        {
                            GL.ActiveTexture(TextureUnit.Texture1);
                            GL.BindTexture(TextureTarget.Texture2D, mesh.SpecularMapID);
                            GL.Uniform1(Shaders[shader].GetUniform("map_specular"), 1);
                            GL.Uniform1(Shaders[shader].GetUniform("hasSpecularMap"), 1);
                            GL.ActiveTexture(TextureUnit.Texture0);
                        }
                    }
                    if (Shaders[shader].GetUniform("map_skin_specular") != -1)
                    {
                        if (casPartVolume == null || casPartVolume.SkinSpecularMapID == -1)
                        {
                            GL.Uniform1(Shaders[shader].GetUniform("hasSkinSpecularMap"), 0);
                        }
                        else
                        {
                            GL.ActiveTexture(TextureUnit.Texture2);
                            GL.BindTexture(TextureTarget.Texture2D, casPartVolume.SkinSpecularMapID);
                            GL.Uniform1(Shaders[shader].GetUniform("map_skin_specular"), 2);
                            GL.Uniform1(Shaders[shader].GetUniform("hasSkinSpecularMap"), 1);
                            GL.ActiveTexture(TextureUnit.Texture0);
                        }
                    }
                    if (Shaders[shader].GetUniform("material_ambient") != -1)
                    {
                        GL.Uniform3(Shaders[shader].GetUniform("material_ambient"), ref mesh.Material.AmbientColor);
                    }
                    if (Shaders[shader].GetUniform("material_diffuse") != -1)
                    {
                        GL.Uniform3(Shaders[shader].GetUniform("material_diffuse"), ref mesh.Material.DiffuseColor);
                    }
                    if (Shaders[shader].GetUniform("material_specExponent") != -1)
                    {
                        GL.Uniform1(Shaders[shader].GetUniform("material_specExponent"), mesh.Material.SpecularExponent);
                    }
                    if (Shaders[shader].GetUniform("material_specular") != -1)
                    {
                        GL.Uniform3(Shaders[shader].GetUniform("material_specular"), ref mesh.Material.SpecularColor);
                    }
                    if (Shaders[shader].GetUniform("model") != -1)
                    {
                        GL.UniformMatrix4(Shaders[shader].GetUniform("model"), false, ref mesh.ModelMatrix);
                    }
                    if (Shaders[shader].GetUniform("view") != -1)
                    {
                        GL.UniformMatrix4(Shaders[shader].GetUniform("view"), false, ref ViewMatrix);
                    }
                    GL.DrawElements(BeginMode.Triangles, mesh.IndexCount, DrawElementsType.UnsignedInt, indexAt * sizeof(uint));
                    indexAt += mesh.IndexCount;
                    Shaders[shader].DisableVertexAttribArrays();
                }
                GL.Flush();
                OpenTK.Graphics.GraphicsContext.CurrentContext.SwapBuffers();
            }
        }

        public static void OnUpdateFrame(CmarNYCBorrowed.Action processInputCallback, float fov, float aspectRatio)
        {
            lock (Lock)
            {
                processInputCallback();
                List<Vector3> colors = new List<Vector3>(),
                deltaNormalsFat = new List<Vector3>(),
                deltaNormalsFit = new List<Vector3>(),
                deltaNormalsThin = new List<Vector3>(),
                deltaNormalsSpecial = new List<Vector3>(),
                deltaVerticesFat = new List<Vector3>(),
                deltaVerticesFit = new List<Vector3>(),
                deltaVerticesThin = new List<Vector3>(),
                deltaVerticesSpecial = new List<Vector3>(),
                normals = new List<Vector3>(),
                vertices = new List<Vector3>();
                var indices = new List<int>();
                var textureCoordinates = new List<Vector2>();
                var vertexCount = 0;
                foreach (var mesh in new List<Volume>((Locked ? LockedMeshes : Meshes).Values))
                {
                    colors.AddRange(mesh.ColorData);
                    indices.AddRange(mesh.GetIndices(vertexCount));
                    normals.AddRange(mesh.Normals);
                    textureCoordinates.AddRange(mesh.TextureCoordinates);
                    vertices.AddRange(mesh.Vertices);
                    var casPartVolume = mesh as Sims3.Sim.CASPartVolume;
                    if (casPartVolume == null)
                    {
                        deltaNormalsFat.AddRange(mesh.Normals);
                        deltaNormalsFit.AddRange(mesh.Normals);
                        deltaNormalsSpecial.AddRange(mesh.Normals);
                        deltaNormalsThin.AddRange(mesh.Normals);
                        deltaVerticesFat.AddRange(mesh.Vertices);
                        deltaVerticesFit.AddRange(mesh.Vertices);
                        deltaVerticesSpecial.AddRange(mesh.Vertices);
                        deltaVerticesThin.AddRange(mesh.Vertices);
                    }
                    else
                    {
                        deltaNormalsFat.AddRange(casPartVolume.DeltaNormalsFat);
                        deltaNormalsFit.AddRange(casPartVolume.DeltaNormalsFit);
                        deltaNormalsSpecial.AddRange(casPartVolume.DeltaNormalsSpecial);
                        deltaNormalsThin.AddRange(casPartVolume.DeltaNormalsThin);
                        deltaVerticesFat.AddRange(casPartVolume.DeltaVerticesFat);
                        deltaVerticesFit.AddRange(casPartVolume.DeltaVerticesFit);
                        deltaVerticesSpecial.AddRange(casPartVolume.DeltaVerticesSpecial);
                        deltaVerticesThin.AddRange(casPartVolume.DeltaVerticesThin);
                    }
                    vertexCount += mesh.VertexCount;
                }
                ColorData = colors.ToArray();
                IndexData = indices.ToArray();
                NormalData = normals.ToArray();
                NormalDeltaDataFat = deltaNormalsFat.ToArray();
                NormalDeltaDataFit = deltaNormalsFit.ToArray();
                NormalDeltaDataThin = deltaNormalsThin.ToArray();
                NormalDeltaDataSpecial = deltaNormalsSpecial.ToArray();
                TextureCoordinateData = textureCoordinates.ToArray();
                VertexData = vertices.ToArray();
                VertexDeltaDataFat = deltaVerticesFat.ToArray();
                VertexDeltaDataFit = deltaVerticesFit.ToArray();
                VertexDeltaDataThin = deltaVerticesThin.ToArray();
                VertexDeltaDataSpecial = deltaVerticesSpecial.ToArray();
                GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders[ActiveShader].GetBuffer("vPosition"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(VertexData.Length * Vector3.SizeInBytes), VertexData, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(Shaders[ActiveShader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);
                if (Shaders[ActiveShader].GetAttribute("vColor") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders[ActiveShader].GetBuffer("vColor"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(ColorData.Length * Vector3.SizeInBytes), ColorData, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(Shaders[ActiveShader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
                }
                if (Shaders[ActiveShader].GetAttribute("texcoord") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders[ActiveShader].GetBuffer("texcoord"));
                    GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(TextureCoordinateData.Length * Vector2.SizeInBytes), TextureCoordinateData, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(Shaders[ActiveShader].GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
                }
                if (Shaders[ActiveShader].GetAttribute("vNormal") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders[ActiveShader].GetBuffer("vNormal"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(NormalData.Length * Vector3.SizeInBytes), NormalData, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(Shaders[ActiveShader].GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, true, 0, 0);
                }
                if (Shaders[ActiveShader].GetAttribute("vDeltaNormalFat") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders[ActiveShader].GetBuffer("vDeltaNormalFat"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(NormalDeltaDataFat.Length * Vector3.SizeInBytes), NormalDeltaDataFat, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(Shaders[ActiveShader].GetAttribute("vDeltaNormalFat"), 3, VertexAttribPointerType.Float, true, 0, 0);
                }
                if (Shaders[ActiveShader].GetAttribute("vDeltaNormalFit") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders[ActiveShader].GetBuffer("vDeltaNormalFit"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(NormalDeltaDataFit.Length * Vector3.SizeInBytes), NormalDeltaDataFit, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(Shaders[ActiveShader].GetAttribute("vDeltaNormalFit"), 3, VertexAttribPointerType.Float, true, 0, 0);
                }
                if (Shaders[ActiveShader].GetAttribute("vDeltaNormalSpecial") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders[ActiveShader].GetBuffer("vDeltaNormalSpecial"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(NormalDeltaDataSpecial.Length * Vector3.SizeInBytes), NormalDeltaDataSpecial, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(Shaders[ActiveShader].GetAttribute("vDeltaNormalSpecial"), 3, VertexAttribPointerType.Float, true, 0, 0);
                }
                if (Shaders[ActiveShader].GetAttribute("vDeltaNormalThin") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders[ActiveShader].GetBuffer("vDeltaNormalThin"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(NormalDeltaDataThin.Length * Vector3.SizeInBytes), NormalDeltaDataThin, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(Shaders[ActiveShader].GetAttribute("vDeltaNormalThin"), 3, VertexAttribPointerType.Float, true, 0, 0);
                }
                if (Shaders[ActiveShader].GetAttribute("vDeltaPositionFat") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders[ActiveShader].GetBuffer("vDeltaPositionFat"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(VertexDeltaDataFat.Length * Vector3.SizeInBytes), VertexDeltaDataFat, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(Shaders[ActiveShader].GetAttribute("vDeltaPositionFat"), 3, VertexAttribPointerType.Float, true, 0, 0);
                }
                if (Shaders[ActiveShader].GetAttribute("vDeltaPositionFit") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders[ActiveShader].GetBuffer("vDeltaPositionFit"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(VertexDeltaDataFit.Length * Vector3.SizeInBytes), VertexDeltaDataFit, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(Shaders[ActiveShader].GetAttribute("vDeltaPositionFit"), 3, VertexAttribPointerType.Float, true, 0, 0);
                }
                if (Shaders[ActiveShader].GetAttribute("vDeltaPositionSpecial") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders[ActiveShader].GetBuffer("vDeltaPositionSpecial"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(VertexDeltaDataSpecial.Length * Vector3.SizeInBytes), VertexDeltaDataSpecial, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(Shaders[ActiveShader].GetAttribute("vDeltaPositionSpecial"), 3, VertexAttribPointerType.Float, true, 0, 0);
                }
                if (Shaders[ActiveShader].GetAttribute("vDeltaPositionThin") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders[ActiveShader].GetBuffer("vDeltaPositionThin"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(VertexDeltaDataThin.Length * Vector3.SizeInBytes), VertexDeltaDataThin, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(Shaders[ActiveShader].GetAttribute("vDeltaPositionThin"), 3, VertexAttribPointerType.Float, true, 0, 0);
                }
                foreach (var mesh in new List<Volume>((Locked ? LockedMeshes : Meshes).Values))
                {
                    mesh.Rotation = CurrentRotation;
                    mesh.CalculateModelMatrix();
                    mesh.ViewProjectionMatrix = Camera.ViewMatrix * Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, 1, 40);
                    mesh.ModelViewProjectionMatrix = mesh.ModelMatrix * mesh.ViewProjectionMatrix;
                }
                GL.UseProgram(Shaders[ActiveShader].ProgramID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBOElements);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(IndexData.Length * sizeof(int)), IndexData, BufferUsageHint.StaticDraw);
                ViewMatrix = Camera.ViewMatrix;
                System.Threading.Thread.Sleep(1);
            }
        }
    }
}
