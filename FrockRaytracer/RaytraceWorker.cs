using System;
using System.Globalization;
using System.Threading;
using FrockRaytracer.Structs;

namespace FrockRaytracer
{
    public class RaytraceWorker
    {
        public int StartRow;
        public int EndRow;
        public int CurrentRow;
        public uint WorkID;
        public Thread Thread;
        public RaytraceMotherBee Mother;
        public bool IsWorking => Thread != null && Thread.IsAlive;

        public RaytraceWorker(RaytraceMotherBee mother, int startRow, int endRow)
        {
            StartRow = startRow;
            EndRow = endRow;
            WorkID = mother.WorkID;
            Mother = mother;
        }

        /// <summary>
        /// Runs the worker synchronously
        /// </summary>
        public void RunAsync()
        {
            Thread = new Thread(Run);
            Thread.Start();
        }

        /// <summary>
        /// Runs the worker synchronously. Worker will render to given rows
        /// </summary>
        public void Run()
        {
            bool is_debug_row = false;
            int debug_column = Settings.RaytraceDebugFrequency;
            for (CurrentRow = StartRow; CurrentRow < EndRow; ++CurrentRow) {
                if (CurrentRow == Mother.DebugRow) is_debug_row = true;
                if(IsCancelled()) return;

                for (int x = 0; x < Mother.Raster.CurrentRaster.WidthHalf; ++x) {
                    if(IsCancelled()) return;

                    bool debug = is_debug_row && x == debug_column;
                    if (debug) debug_column += Settings.RaytraceDebugFrequency;
                    
                    float wt = (float) x / Mother.Raster.CurrentRaster.WidthHalf;
                    float ht = (float) CurrentRow / Mother.Raster.Height;
                    
                    var onPlane = Mother.World.Camera.FOVPlane.PointAt(wt, ht);
                    onPlane.Normalize();
                   
                    var color = Mother.Raytracer.TraceRay(new Ray(Mother.World.Camera.Position, onPlane), debug);
                    
                    if(IsCancelled()) return;
                    Mother.Raster.CurrentRaster.setPixel(x, CurrentRow, color);
                }
            }
        }

        /// <summary>
        /// Check if workmer is cancelled
        /// </summary>
        /// <returns></returns>
        public bool IsCancelled()
        {
            return WorkID != Mother.WorkID;
        }

        /// <summary>
        /// Splits the work in two of possible
        /// </summary>
        /// <returns></returns>
        public RaytraceWorker Split()
        {
            int HalfRow = CurrentRow + (EndRow - CurrentRow)/2;
            if (HalfRow == CurrentRow) return null;
           
            var ret =  new RaytraceWorker(Mother, HalfRow, EndRow);
            EndRow = HalfRow;
            return ret;
        }
    }
}