using FrockRaytracer;
using FrockRaytracer.Objects;
using FrockRaytracer.Objects.Primitives;
using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracerDemo
{
    public class MeshesScene
    {
        public const int SceneID = 3;

        public static World CreateMeshesScene()
        {
            var world = new World(new Camera(new Vector3(3, 4, 0.5f),
                new Quaternion(MathHelper.DegreesToRadians(0), MathHelper.DegreesToRadians(-0), 0)));
            var environment = new HDRTexture(@"assets/textures/kitchen_cross.hdr", 100000);

            world.Environent = new EnvironmentBox(new CubeMapTexture(environment));
            world.Environent.AmbientLight = new Vector3(0.87f, 0.56f, 0.46f) * 0.05f;

            var plane = new Plane(new Vector3(0, -2, 0), Quaternion.Identity)
            {
                Material = {
                    Diffuse = new Vector3(0.2f),
                    IsDielectic = true,
                    Reflectivity = 0.3f,
                    Texture = new DiffuseTexture(@"assets/textures/wall.png", 5f)
                }
            };
            world.addObject(plane);

            var mesh1 = new Mesh(new Vector3(9, 0, 8), Quaternion.Identity)
            {
                Material = {
                    Diffuse = new Vector3(.5f)
                },
                Scale = new Vector3(1f),
            };
            mesh1.ImportMesh("assets/models/lamp.obj");
            world.addObject(mesh1);

            var mesh2 = new Mesh(new Vector3(0, 0, 5), Quaternion.Identity)
            {
                Material =
                {
                    IsMirror = true,
                    Diffuse = new Vector3(1)
                },
                Scale = new Vector3(1)
            };

            mesh2.ImportMesh("assets/models/teapot.obj");
            world.addObject(mesh2);

            var mesh3 = new Mesh(new Vector3(45, 45, 55), Quaternion.Identity)
            {
                Material =
                {
                    Diffuse = new Vector3(.5f, .7f, 1f),
                    IsDielectic = true,
                    Reflectivity = .2f
                },
                Scale = new Vector3(1)
            };
            mesh3.ImportMesh("assets/models/airplane.obj");
            world.addObject(mesh3);

            var mesh4 = new Mesh(new Vector3(-10, 25, 45), Quaternion.Identity)
            {
                Material =
                {
                    Diffuse = new Vector3(1f, .5f, .8f)
                },
                X = 3,
                Z = 1,
                Scale = new Vector3(.7f)
            };
            mesh4.ImportMesh("assets/models/dragon.obj");
            world.addObject(mesh4);

            var light1 = new PointLight(new Vector3(10, 30, 40), Vector3.One, 2500);
            world.addLight(light1);

            var light2 = new PointLight(new Vector3(8, 8f, 0), Vector3.One, 1500);
            world.addLight(light2);

            return world;
        }
    }
}