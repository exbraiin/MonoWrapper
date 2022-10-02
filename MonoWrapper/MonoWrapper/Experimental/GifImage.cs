#if BUG
using Microsoft.Xna.Framework.Graphics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Drawing;
using Image = System.Drawing.Image;

namespace MonoWrapper.Experimental
{
    public class GifImage
    {
        private int frame;
        private float timer;

        private readonly float[] delays;
        private readonly Texture2D[] frames;

        public int Frames => frames.Length;
        public float Duration => delays.Sum();
        public Texture2D Frame => frames[frame];

        public GifImage (Texture2D[] frames, float[] delays)
        {
            frame = 0;
            timer = 0;
            this.delays = delays;
            this.frames = frames;
        }

        public void Update ()
        {
            timer += Time.ElapsedTime;
            if (timer < delays[frame]) return;
            timer = 0;
            frame = (frame + 1) % frames.Length;
        }

        public static GifImage Load (string path)
        {
            using (var image = Image.FromFile(path))
            {
                var dimension = new FrameDimension(image.FrameDimensionsList[0]);
                var count = image.GetFrameCount(dimension);

                var delays = new float[count];
                var frames = new Texture2D[count];

                for (int i = 0; i < count; ++i)
                {
                    image.SelectActiveFrame(dimension, i);
                    var property = image.GetPropertyItem(0x5100);
                    var delay = (property.Value[0] + property.Value[1] * 256) / 100f;

                    delays[i] = delay;
                    using (var frame = image.Clone() as Image)
                    using (var stream = ImageToStream(frame, ImageFormat.Gif))
                        frames[i] = Texture2D.FromStream(Window.GraphicsDevice, stream);
                }
                return new GifImage(frames, delays);
            }
        }

        private static MemoryStream ImageToStream (Image image, ImageFormat format)
        {
            var stream = new MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }
    }
}
#endif