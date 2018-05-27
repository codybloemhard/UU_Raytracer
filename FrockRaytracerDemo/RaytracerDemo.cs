using System.Drawing;
using FrockRaytracer;
using FrockRaytracer.Objects;
using FrockRaytracer.Structs;
using OpenTK;
using OpenTK.Graphics.ES30;
using OpenTK.Input;

namespace FrockRaytracerDemo
{
    internal class RaytracerDemo : Window
    {
        private const int MOVE_SPHERE_ID = 3;
        
        public static void Main(string[] args)
        {
            using (
                var win = new RaytracerDemo(new Size(1024, 512))) {
                var world = win.World;
                
                var environment = new HDRTexture(@"assets/textures/stpeters_cross.hdr", 50000);
                world.Environent = new EnvironmentBox(new CubeMapTexture(environment));

                /*var plane = new Plane(new Vector3(0, 0, 0), Quaternion.Identity) {
                    Material = {
                        Diffuse = new Vector3(0.2f, 0.2f, 0.2f),
                        Texture = new CheckerboardTexture(1)
                    }
                };
                world.addObject(plane);*/
                
                var sphere_matte = new Sphere(new Vector3(-2.5f, 1, 5.5f), 1, Quaternion.Identity) {
                    Material = {
                        Diffuse = new Vector3(1, 0.2f, 0.2f),
                        IsGlossy = true,
                        Specular = new Vector3(1, 1f, 1f) * .5f,
                        Shinyness = 8,
                        Texture = new CheckerboardTexture(5, 10)
                    }
                };
                world.addObject(sphere_matte);
                
                var sphere_mirror = new Sphere(new Vector3(0, 1, 6f), 1, Quaternion.Identity) {
                    Material = {
                        IsGlossy = true,
                        Specular = Vector3.One * .2f,
                        Shinyness = 16,
                        IsMirror = true
                    }
                };
                world.addObject(sphere_mirror);
                
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
                world.addObject(sphere_glossy);
                
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

                Vector3 A = new Vector3(-1, 0f, 3), B = new Vector3(1, 0f, 3), C = new Vector3(0, 2f, 5);
                var polygon = new Polygon((A + B + C) / 3, new Vector3[] { A, B, C })
                {
                    Material = {
                        Diffuse = new Vector3(0.2f, 0.2f, 0.2f)
                    }
                };
                //world.addObject(polygon);
                
                var light = new Light(new Vector3(0, 1, 0.5f), Vector3.One, 2000);
                world.addLight(light);
                
                win.Run(30.0, 60.0);
            }
        }

        public RaytracerDemo(Size size) : base(size)
        {
            
        }

        public override void Update()
        {
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Down)) {
                World.Camera.RotateBy(new Vector3(3, 0, 0));
                World.Changed = true;
            }

            if (keyState.IsKeyDown(Key.Up)) {
                World.Camera.RotateBy(new Vector3(-3, 0, 0));
                World.Changed = true;
            }

            if (keyState.IsKeyDown(Key.Right)) {
                World.Camera.RotateBy(new Vector3(0, 3, 0));
                World.Changed = true;
            }
            
            if (keyState.IsKeyDown(Key.Left)) {
                World.Camera.RotateBy(new Vector3(0, -3, 0));
                World.Changed = true;
            }

            if (keyState.IsKeyDown(Key.W)) {
                World.Objects[MOVE_SPHERE_ID].Position = World.Objects[MOVE_SPHERE_ID].Position + new Vector3(0, 0, 0.1f);
                World.Changed = true;
            }
            
            if (keyState.IsKeyDown(Key.S)) {
                World.Objects[MOVE_SPHERE_ID].Position = World.Objects[MOVE_SPHERE_ID].Position + new Vector3(0, 0, -0.1f);
                World.Changed = true;
            }

            if (keyState.IsKeyDown(Key.D)) {
                World.Objects[MOVE_SPHERE_ID].Position = World.Objects[MOVE_SPHERE_ID].Position + new Vector3(0.1f, 0, 0);
                World.Changed = true;
            }

            if (keyState.IsKeyDown(Key.A)) {
                World.Objects[MOVE_SPHERE_ID].Position = World.Objects[MOVE_SPHERE_ID].Position + new Vector3(-0.1f, 0, 0);
                World.Changed = true;
            }
        }
    }
}