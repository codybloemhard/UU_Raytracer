﻿using System;
using FrockRaytracer;
using FrockRaytracer.Objects;
using FrockRaytracer.Objects.Primitives;
using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracerDemo
{
    public static class SpheresScene
    {
        public const int SceneID = 1;

        
        public static World CreateSphereScene()
        {
            var world = new World(new Camera(new Vector3(3,4,0.5f), 
                new Quaternion(MathHelper.DegreesToRadians(20), MathHelper.DegreesToRadians(-20), 0)));
            world.Environent = new Environent();
            
            world.addObject(new Plane(new Vector3(0, 0, 0), Quaternion.Identity) {
                Material = {
                    Diffuse = new Vector3(0.6f),
                    Texture = new CheckerboardTexture()
                }
            });
            
            world.addObject(new Plane(new Vector3(0, 0, 10), new Quaternion(MathHelper.DegreesToRadians(90), 0 ,0)) {
                Material = {
                    Diffuse = new Vector3(0.6f),
                }
            });
            
            world.addObject(new Plane(new Vector3(0, 0, 0), new Quaternion(MathHelper.DegreesToRadians(-90), 0 ,0)) {
                Material = {
                    Diffuse = new Vector3(0.6f),
                }
            });

            world.addObject(new Plane(new Vector3(-6, 0, 0), new Quaternion(0, 0 , MathHelper.DegreesToRadians(90))) {
                Material = {
                    Diffuse = new Vector3(0.6f),
                }
            });
            
            world.addObject(new Plane(new Vector3(6, 0, 0), new Quaternion(0, 0 , MathHelper.DegreesToRadians(-90))) {
                Material = {
                    Diffuse = new Vector3(0.6f),
                }
            });
            
            world.addObject(new Plane(new Vector3(0, 12, 0), new Quaternion(MathHelper.DegreesToRadians(180), 0 , 0)) {
                Material = {
                    Diffuse = new Vector3(0.6f),
                }
            });
            
            
            // Mirror
            world.addObject(new Sphere(new Vector3(3, 8f, 7), 1.5f, Quaternion.Identity) {
                Material = {
                    IsMirror = true,
                    IsGlossy = true,
                    Shinyness = 30,
                }
            });
            
            // Mirror rough
            world.addObject(new Sphere(new Vector3(-3, 7f, 7), 1.5f, Quaternion.Identity) {
                Material = {
                    Diffuse = new Vector3(0.988f, 0.69f, 0.702f),
                    Specular = new Vector3(0.988f, 0.69f, 0.702f),
                    IsGlossy = true,
                    Shinyness = 30,
                    IsDielectic = true,
                    Roughness = 0.3f,
                    Reflectivity = 0.6f
                }
            });
            
            // Checker ball
            world.addObject(new Sphere(new Vector3(-5, 2.5f, 4), 1f, Quaternion.Identity) {
                Material = {
                    Specular = Vector3.One,
                    IsGlossy = true,
                    Shinyness = 20,
                    IsDielectic = true,
                    Reflectivity = 0.3f,
                    Texture = new CheckerboardTexture(3, 6)
                }
            });
            
            // Purple
            world.addObject(new Sphere(new Vector3(-2, 1f, 6), 1f, Quaternion.Identity) {
                Material = {
                    Diffuse = new Vector3(0.651f, 0.51f, 1f),
                    Specular = new Vector3(0.651f, 0.51f, 1f)*0.2f,
                    IsGlossy = true,
                    Shinyness = 1,
                    IsDielectic = true,
                    Reflectivity = 0.1f,
                }
            });
            
            // Purple
            world.addObject(new Sphere(new Vector3(4, 1f, 4), 1f, Quaternion.Identity) {
                Material = {
                    Diffuse = new Vector3(0.039f, 0.529f, 0.329f),
                    Specular = new Vector3(0.039f, 0.529f, 0.329f)*0.7f,
                    IsGlossy = true,
                    Shinyness = 10,
                    IsDielectic = true,
                    Reflectivity = 0.02f,
                }
            });
            
            // Glass
            world.addObject(new Sphere(new Vector3(1.2f, 1f, 5), 0.8f, Quaternion.Identity) {
                Material = {
                    Absorb = (Vector3.One - new Vector3(0.992f, 0.906f, 0.298f))*0.8f,
                    IsGlossy = true,
                    Shinyness = 10,
                    IsDielectic = true,
                    IsRefractive = true,
                    RefractionIndex = 1.5f
                }
            });
            
            world.addObject(new Sphere(new Vector3(.5f, 1.4f, 2), 1.2f, Quaternion.Identity) {
                Material = {
                    Absorb = Vector3.Zero,
                    IsGlossy = true,
                    Shinyness = 10,
                    IsDielectic = true,
                    IsRefractive = true,
                    RefractionIndex = 1.2f,
                }
            });
            
            
            world.addLight(new PointLight(new Vector3(2, 6, 3), Vector3.One, 300));
            world.addLight(new PointLight(new Vector3(-4, 2, 1), Vector3.One, 200));
            world.addLight(new SphereAreaLight(new Vector3(0, 9, 6), new Vector3(0.965f, 0.847f, 0.682f), 300, 256 ));


            return world;
        }
    }
}