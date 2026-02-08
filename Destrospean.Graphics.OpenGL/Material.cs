using OpenTK;

namespace Destrospean.Graphics.OpenGL
{
    public class Material
    {
        public Vector3 AmbientColor = new Vector3(),
        DiffuseColor = new Vector3(),
        SpecularColor = new Vector3();

        public string AmbientMap = "",
        DiffuseMap = "",
        NormalMap = "",
        OpacityMap = "",
        Shader = "",
        SpecularMap = "";

        public bool HasTransparency = false;

        public float Opacity = 1,
        SpecularExponent = 1;

        public Material()
        {
        }

        public Material(Vector3 ambient, Vector3 diffuse, Vector3 specular, float specularExponent = 1, float opacity = 1)
        {
            AmbientColor = ambient;
            DiffuseColor = diffuse;
            Opacity = opacity;
            SpecularColor = specular;
            SpecularExponent = specularExponent;
        }
    }
}
