using FrockRaytracer;
using FrockRaytracer.Objects;
using FrockRaytracer.Objects.Primitives;
using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracerDemo
{
    public class TestScene
    {
        public const int SceneID = 0;
        
         public static World CreateTestScene()
        {
            var world = new World(new Camera(new Vector3(3,4,0.5f), 
                new Quaternion(MathHelper.DegreesToRadians(20), MathHelper.DegreesToRadians(-20), 0)));
            var environment = new HDRTexture(@"assets/textures/uffizi_cross.hdr", 100000);
            
            world.Environent = new EnvironmentBox(new CubeMapTexture(environment));
            world.Environent.AmbientLight = new Vector3(0.87f, 0.56f, 0.46f) * 0.05f;

            var plane = new Plane(new Vector3(0, -2, 0), Quaternion.Identity) {
                Material = {
                    Diffuse = new Vector3(0.2f),
                    //IsDielectic = true,
                    Reflectivity = 0.5f,
                    Texture = new DiffuseTexture(@"assets/textures/wall.png", 5f)
                }
            };
            world.addObject(plane);

            var sphere_matte = new Sphere(new Vector3(-2.5f, 1, 5.5f), 1, Quaternion.Identity) {
                Material = {
                    Diffuse = new Vector3(1, 0.2f, 0.2f),
                    IsGlossy = true,
                    Specular = new Vector3(1, 1f, 1f) * .5f,
                    Shinyness = 8,
                    Texture = new CheckerboardTexture(5, 10)
                }
            };
            //world.addObject(sphere_matte);

            var sphere_mirror = new Sphere(new Vector3(0, 1, 6f), 1, Quaternion.Identity) {
                Material = {
                    IsGlossy = true,
                    Specular = Vector3.One * .2f,
                    Shinyness = 16,
                    IsMirror = true
                }
            };
            //world.addObject(sphere_mirror);

            var sphere_glossy = new Sphere(new Vector3(2.5f, 1, 5.5f), 1, Quaternion.Identity) {
                Material = {
                    Diffuse = new Vector3(0.2f, 0.2f, 1),
                    IsGlossy = true,
                    Specular = new Vector3(0.2f, 0.2f, 1) * .2f,
                    Shinyness = 16,
                    IsDielectic = true,
                    Reflectivity = 0.6f
                }
            };
            //world.addObject(sphere_glossy);

            var sphere_glass = new Sphere(new Vector3(0, 1, 1.5f), 1, Quaternion.Identity) {
                Material = {
                    Absorb = new Vector3(4, 4, 1.5f) * 0.1f,
                    IsGlossy = true,
                    Specular = new Vector3(.2f, .2f, 1f) * .2f,
                    Shinyness = 16,
                    IsDielectic = true,
                    IsRefractive = true,
                    RefractionIndex = 1.3f
                }
            };
            //world.addObject(sphere_glass);

            Vector3 A = new Vector3(-.05f, 0f, 1.5f),
                B = new Vector3(.05f, 0f, 1.5f),
                C = new Vector3(0, .05f, 1.5f),
                D = new Vector3(.1f, .05f, 1.5f),
                E = new Vector3(.05f, 0, 1.5f);
            var polygon = new Polygon((A + B + C) / 3, new Vector3[] {A, B, C}) {
                Material = {
                    Diffuse = new Vector3(0.2f, 0.2f, 0.2f)
                }
            };
            //world.addObject(polygon);

            var mesh = new Mesh(new Vector3(-8, 0, 5), Quaternion.Identity) {
                Material = {
                    Diffuse = new Vector3(.5f)
                },
                Scale = new Vector3(1f),
            };
            mesh.ImportMesh("assets/models/teapot.obj");
            world.addObject(mesh);

            world.Camera.Rotation = new Quaternion(0, -3.14f/2, 0) + Matrix4.LookAt(world.Camera.Position, mesh.Position, Vector3.UnitY).ExtractRotation();
            
            var light = new SphereAreaLight(new Vector3(3, 10, 4), Vector3.One, 1500) {
                Radius = 4f
            };
            //world.addLight(light);

            var light2 = new PointLight(new Vector3(0, 10, 0), Vector3.One, 1500);
            world.addLight(light2);

            var spotl = new SpotLightMultiSample(new Vector3(5, 8, 5), new Vector3(1f, 1f, 0.1f), 1500) {
                Normal = -Vector3.UnitY,
                AngleMin = 10f,
                AngleMax = 25f,
                Radius = 2f
            };
            //world.addLight(spotl);
            
            
            world.addLight(new PointLight(new Vector3(2, 6, 3), Vector3.One, 300));
            world.addLight(new PointLight(new Vector3(-4, 2, 1), Vector3.One, 200));

            return world;
        }
    }
}