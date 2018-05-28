using System;
using System.Collections.Generic;

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

        private List<RaytraceWorker> Workers = new List<RaytraceWorker>();

        public RaytraceMotherBee(Raytracer raytracer, MultiResolutionRaster raster)
        {
            Raytracer = raytracer;
            Raster = raster;
        }

        public void MaintainThreads()
        {
            bool hasWorkers = Workers.Count > 0;
            for (int i = Workers.Count-1; i >= 0; --i) {
                if(!Workers[i].IsWorking) Workers.RemoveAt(i);
            }

            if (Settings.SplitThreads && Workers.Count > 0) {
                int remainingThreads = Math.Min(Workers.Count, AvailableThreads - Workers.Count);
                for (int i = 0; i < remainingThreads; i++) {
                    var worker = Workers[i].Split();
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

        public void StartRender(World world, MultiResolutionRaster raster)
        {
            if(!world.Changed && (raster.CurrentLevel == raster.MaxLevel || Workers.Count > 0)) return;
            if(!world.Changed) raster.SwitchLevel(raster.CurrentLevel + 1, true);
            if(world.Changed) raster.SwitchLevel(0, false);
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