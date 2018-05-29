using FrockRaytracer;
using FrockRaytracer.Objects;
using FrockRaytracer.Objects.Primitives;
using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracerDemo
{
    public class MirrorRoomScene
    {
        public const int SceneID = 0;
        
         public static World CreateTestScene()
        {
            var world = new World(new Camera(new Vector3(-2f), 
                new Quaternion(MathHelper.DegreesToRadians(35), MathHelper.DegreesToRadians(-35), 0)));
            var environment = new HDRTexture(@"assets/textures/uffizi_cross.hdr", 100000);

            world.Environent = new EnvironmentBox(new CubeMapTexture(environment));
            world.Environent.AmbientLight = new Vector3(0.87f, 0.56f, 0.46f) * 0.05f;

            var sphere = new Sphere(new Vector3(0), 1, Quaternion.Identity)
            {
                Material = {
                    Diffuse = new Vector3(1.0f, .5f, .5f)
                }
            };
            //world.addObject(sphere);
            var mesh = new Mesh(Vector3.Zero, Quaternion.Identity) {
                Material = {
                    Diffuse = new Vector3(1, 0.5f, 0.5f)
                },
                Scale = new Vector3(.5f)
            };
            mesh.ImportMesh("assets/models/teapot.obj");
            world.addObject(mesh);

            world.addObject(new Plane(new Vector3(0, -3, 0), Quaternion.Identity)
            {
                Material = {
                    Diffuse = new Vector3(1),
                    IsDielectic = true,
                    Reflectivity = .7f
                }
            });

            world.addObject(new Plane(new Vector3(0, 3, 0), new Quaternion(MathHelper.DegreesToRadians(180), 0, 0))
            {
                Material = {
                    Diffuse = new Vector3(1),
                    IsDielectic = true,
                    Reflectivity = .7f
                }
            });

            world.addObject(new Plane(new Vector3(0, 0, 3), new Quaternion(MathHelper.DegreesToRadians(90), 0, 0))
            {
                Material = {
                    Diffuse = new Vector3(1),
                    IsDielectic = true,
                    Reflectivity = .7f
                }
            });

            world.addObject(new Plane(new Vector3(0, 0, -3), new Quaternion(MathHelper.DegreesToRadians(-90), 0, 0))
            {
                Material = {
                    Diffuse = new Vector3(1),
                    IsDielectic = true,
                    Reflectivity = .7f
                }
            });

            world.addObject(new Plane(new Vector3(-3, 0, 0), new Quaternion(0, 0, MathHelper.DegreesToRadians(90)))
            {
                Material = {
                    Diffuse = new Vector3(1),
                    IsDielectic = true,
                    Reflectivity = .7f
                }
            });

            world.addObject(new Plane(new Vector3(3, 0, 0), new Quaternion(0, 0, MathHelper.DegreesToRadians(-90)))
            {
                Material = {
                    Diffuse = new Vector3(1),
                    IsDielectic = true,
                    Reflectivity = .7f
                }
            });

            world.addLight(new SphereAreaLight(new Vector3(1.5f, -1.5f, 1.5f), new Vector3(0.5f, 0.4f, 0.4f), 150) { Radius = .5f });
            world.addLight(new SphereAreaLight(new Vector3(-1.5f, 1.5f, -1.5f), new Vector3(0.5f, 0.4f, 0.4f), 150) { Radius = .5f });

            return world;
        }
    }
}