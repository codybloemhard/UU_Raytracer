namespace FrockRaytracer
{
    public class MultiResolutionRaster
    {
        public float[] ResolutionLevels { get; private set; }
        public Raster[] Rasters;
        public int CurrentLevel { get; set; }
        public int MaxLevel => Rasters.Length - 1;
        
        public int BaseWidth { get; private set; }
        public int BaseHeight { get; private set; }
        
        public int Width => CurrentRaster.Width;
        public int Height => CurrentRaster.Height;
        public Raster CurrentRaster => Rasters[CurrentLevel];
       
        public MultiResolutionRaster() { }

        public void Resize(int width, int height, float[] resolutionLevels)
        {
            if (width == BaseWidth && height == BaseHeight && resolutionLevels == ResolutionLevels) return;
            CurrentLevel = 0;
            BaseWidth = width;
            BaseHeight = height;
            ResolutionLevels = resolutionLevels;
            Rasters = new Raster[ResolutionLevels.Length];
            for (int i = 0; i < ResolutionLevels.Length; i++) {
                Rasters[i] = new Raster((int) (BaseWidth*ResolutionLevels[i]), (int) (BaseHeight * ResolutionLevels[i]));
            }
        }

        public void SwitchLevel(int level, bool copy = false, bool clear = false)
        {
            if(CurrentLevel == level || level >= Rasters.Length) return;
            if (copy) {
                var TargetRaster = Rasters[level];
                var SourceRaster = Rasters[CurrentLevel];
                float scale = (float) SourceRaster.Width / TargetRaster.Width;

                for (int x = 0; x < TargetRaster.Width; x++)
                for (int y = 0; y < TargetRaster.Height; y++) {
                    // TODO: just increment it everytime by 3 since pixels flow liek that already
                    int xs = (int) (x * scale), ys = (int) (y * scale);
                    int offset = (x + TargetRaster.Width * y) * 3;
                    int offset_s = (xs + SourceRaster.Width * ys) * 3;
                    TargetRaster.Pixels[offset++] = (byte) (SourceRaster.Pixels[offset_s++]*0.7f);
                    TargetRaster.Pixels[offset++] = (byte) (SourceRaster.Pixels[offset_s++]*0.7f);
                    TargetRaster.Pixels[offset] = (byte) (SourceRaster.Pixels[offset_s]*0.7f);
                }
            } else if (clear) {
                Rasters[level].Clear();
            }

            CurrentLevel = level;
        }
    }
}