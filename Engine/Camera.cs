using System;
using System.Collections.Generic;
using System.Drawing;
using Engine.Helpers;
using Engine.Objects;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
    struct Plane3D
    {
        
    }
    
    public class Camera : ITransformative
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public float Fovy { get; set; }
        public float Aspect { get; set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }

        public Camera(Vector3 position, Quaternion rotation, float fovy = 1.6f, float aspect = 16f/9f, float zNear = .1f, float zFar = 100000000)
        {
            Position = position;
            Rotation = rotation;
            Fovy = fovy;
            Aspect = aspect;
            ZNear = zNear;
            ZFar = zFar;
        }

        public Matrix4 TransformMatrix(Matrix4 matrix)
        {
            matrix *= Matrix4.CreateFromQuaternion(Rotation);
            matrix *= Matrix4.CreateTranslation(Position);
            matrix *= Matrix4.CreatePerspectiveFieldOfView(Fovy, Aspect, ZNear, ZFar);
            return matrix;
        }
        
        /// <summary>
        /// Cull the objects to the only ones we need.
        /// TODO: actually do that
        /// *Warning: Objects are in a hierarchy not a flat array*
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public IEnumerable<ITransformative> Cull(List<ITransformative> objects)
        {
            return objects;
        }

        /// <summary>
        /// Clear the screen
        /// </summary>
        public void Clear()
        {
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
        }

        /// <summary>
        /// http://slideplayer.com/slide/5356366/17/images/42/OpenGL+Perspective+gluPerspective(fovy,+aspect,+near,+far);.jpg
        /// </summary>
        /// <param name="fovy"></param>
        /// <param name="aspect"></param>
        /// <param name="zNear"></param>
        /// <param name="zFar"></param>
        public void SetFOV(float fovy = 1.6f, float aspect = 16f / 9f, float zNear = .1f, float zFar = 100000000)
        {
            Fovy = fovy;
            Aspect = aspect;
            ZNear = zNear;
            ZFar = zFar;
        }

        /// <summary>
        /// Sets camera field of view from an angle
        /// </summary>
        /// <param name="angle">Horizontal field of view angle</param>
        // ReSharper disable once InconsistentNaming
        public void SetFOVAngle(float angle)
        {
            Fovy = angle * MathHelper.DegreesToRadians(angle) / Aspect;
        }

        /// <summary>
        /// Returns the two opposite points on the near clipping plane
        /// </summary>
        /// <returns></returns>
        public Tuple<Vector3, Vector3> GetNearClippingPlane()
        {
            var rotationMatrix = Matrix3.CreateFromQuaternion(Rotation);

            float halfHeight = (float) (Math.Atan(Fovy) * ZNear * 2);
            float halfWidth = (float) (Math.Atan(Fovy * Aspect) * ZNear * 2); // Fovy * Aspect = Fovx
            
            var leftTop = (new Vector3(-halfWidth, halfHeight, ZNear) + Position) * rotationMatrix;
            var rightBottom = (new Vector3(halfWidth, -halfHeight, ZNear) + Position) * rotationMatrix;
            return new Tuple<Vector3, Vector3>(leftTop, rightBottom);
        }
    }
}