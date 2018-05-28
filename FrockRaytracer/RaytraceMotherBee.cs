using System;
using System.Collections.Generic;
using System.Configuration;

namespace FrockRaytracer
{
    public class RaytraceMotherBee
    {
        public Raytracer Raytracer { get; private set; }
        public MultiResolutionRaster Raster { get; private set; }
        public World World => Raytracer.World;

        public uint WorkID { get; private set; }
        public int DebugRow { get; private set; }
        public int AvailableThreads => Environment.ProcessorCount;

        public Random Random;

        private List<RaytraceWorker> Workers = new List<RaytraceWorker>();

        public RaytraceMotherBee(Raytracer raytracer, MultiResolutionRaster raster)
        {
            Raytracer = raytracer;
            Raster = raster;
            Random = new Random(1337);
        }

        /// <summary>
        /// Cancel the current rende rin progress if any
        /// </summary>
        public void Cancel()
        {
            WorkID++;
            Workers.Clear();
        }

        /// <summary>
        /// Update the running workers and and split them if needed.
        /// </summary>
        public void MaintainThreads()
        {
            if(!Settings.IsAsync) return;
            
            bool hasWorkers = Workers.Count > 0;
            for (int i = Workers.Count-1; i >= 0; --i) {
                if (!Workers[i].IsWorking) {
                    Workers.RemoveAt(i);
                    //if(Workers[i].CurrentRow == Workers[i].EndRow) Workers.RemoveAt(i);
                    //else Workers[i].RunAsync(); // Windows killd my worker. Lets restart
                }
            }

            if (Settings.SplitThreads && Workers.Count > 0) {
                int remainingThreads = Math.Min(Workers.Count, AvailableThreads - Workers.Count);
                for (int i = 0; i < remainingThreads; i++) {
                    var worker = Workers[Random.Next(0, remainingThreads)].Split();
                    if (worker != null) {
                        worker.RunAsync();
                        Workers.Add(worker);
                    }
                }
            }

            if (hasWorkers && Workers.Count == 0) {
                DebugRenderer.DebugDraw(Raytracer.DebugData, Raster.CurrentRaster, World);
            }
        }
        
        /// <summary>
        /// Start a full render. It does first check if world has changed or the current render can be upscaled
        /// </summary>
        /// <param name="world"></param>
        /// <param name="raster"></param>
        public void StartRender(World world, MultiResolutionRaster raster)
        {
            // World has not changed and cant upscale
            if(!world.Changed && (raster.CurrentLevel == raster.MaxLevel || Workers.Count > 0)) return;
            // World has not changed. So upscale
            if(!world.Changed) raster.SwitchLevel(raster.CurrentLevel + 1, true);
            if(world.Changed) raster.SwitchLevel(0, false); // World changed. Reset antialias
            ++WorkID;
            Workers.Clear();
            Raytracer.Reset(world);
            Raster = raster;
            World.Changed = false;
            DebugRow = (int) (Settings.RaytraceDebugRow * Raster.Height);

            if (Settings.IsMultithread) {
                var rpt = Raster.Height / AvailableThreads;
                for (int i = 0; i < AvailableThreads; ++i) {
                    Workers.Add(new RaytraceWorker(this, i * rpt, (i + 1) * rpt));
                    Workers[i].RunAsync();
                }
                
                if (!Settings.IsAsync) {
                    foreach (var worker in Workers) worker.Thread.Join();
                    Workers.Clear();
                    DebugRenderer.DebugDraw(Raytracer.DebugData, Raster.CurrentRaster, World);
                }
            }
            else {
                var worker = new RaytraceWorker(this, 0, Raster.Height);
                worker.Run();
                DebugRenderer.DebugDraw(Raytracer.DebugData, Raster.CurrentRaster, World);
            }
        }
        
    }
}