using OpenTK;

namespace FrockRaytracer.Structs
{
    public struct Material
    {
        public Vector3 Diffuse;
        public Vector3 Specular;
        public Vector3 Absorb;
        public bool IsGlossy;
        public float Shinyness;
        public float Roughness;
        public bool IsDielectic;
        public bool IsMirror;
        public float Reflectivity;
        public bool IsRefractive;
        public float RefractionIndex;

        public Material(Vector3 diffuse, Vector3 specular, Vector3 absorb, bool isGlossy, float shinyness,
            float roughness, bool isDielectic, bool isMirror, float reflectivity, bool isRefractive,
            float refractionIndex)
        {
            Diffuse = diffuse;
            Specular = specular;
            Absorb = absorb;
            IsGlossy = isGlossy;
            Shinyness = shinyness;
            Roughness = roughness;
            IsDielectic = isDielectic;
            IsMirror = isMirror;
            Reflectivity = reflectivity;
            IsRefractive = isRefractive;
            RefractionIndex = refractionIndex;
        }

        public static Material Default()
        {
            return new Material(Vector3.One, Vector3.One, Vector3.One, false, 3, 0, false, false, 0, false,
                Constants.LIGHT_IOR);
        }
    }
}