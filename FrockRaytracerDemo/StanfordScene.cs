using FrockRaytracer;
using FrockRaytracer.Objects;
using FrockRaytracer.Objects.Primitives;
using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracerDemo
{
    public class StanfordScene
    {
        public const int SceneID = 1337;
        
        public static World Create()
        {
            var world = new World(new Camera(new Vector3(-6,6,0), 
                new Quaternion(MathHelper.DegreesToRadians(10), MathHelper.DegreesToRadians(30), 0)));
            
            var environment = new HDRTexture(@"assets/textures/stpeters_cross.hdr", 80000);
            world.Environent = new EnvironmentBox(new CubeMapTexture(environment));
            world.Environent.AmbientLight = new Vector3(0.87f, 0.56f, 0.46f) * 0.05f;
            
            
            world.addObject(new Plane(new Vector3(0, 0, 0), Quaternion.Identity) {
                Material = {
                    Diffuse = new Vector3(0.6f),
                    Texture = new DiffuseTexture("assets/textures/wood.png", 8)
                }
            });

            var mesh = new Mesh(new Vector3(0, 0,7), Quaternion.Identity) {
                Material = {
                    Diffuse = new Vector3(1, 0.686f, 0.278f),
                    Specular = new Vector3(1, 0.686f, 0.278f),
                    Shinyness = 20,
                    IsDielectic = true,
                    //Roughness = 0.2f,
                    Reflectivity = 0.6f
                },
                Scale = new Vector3(1f),
            };
            mesh.ImportMesh("assets/models/stanford.obj");
            world.addObject(mesh);

            world.addLight(new PointLight(new Vector3(0, 60, -40), Vector3.One, 120000));
            
//            world.addLight(new PointLight(new Vector3(0, 70, -50), Vector3.One, 120000));
//            world.addLight(new PointLight(new Vector3(30, 90, 40), Vector3.One, 120000));
            
            return world;
        }
    }
}