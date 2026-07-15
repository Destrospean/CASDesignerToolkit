using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Destrospean.S3PIExtensions;
using Gdk;
using s3pi.Interfaces;

namespace Destrospean.DestrospeanCASPEditor
{
    public static class ImageUtils
    {
        public static readonly Dictionary<string, List<Pixbuf>> PreloadedGameImagePixbufs = new Dictionary<string, List<Pixbuf>>(StringComparer.InvariantCultureIgnoreCase),
        PreloadedImagePixbufs = new Dictionary<string, List<Pixbuf>>(StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, Bitmap> PreloadedGameImages = new Dictionary<string, Bitmap>(StringComparer.InvariantCultureIgnoreCase),
        PreloadedImages = new Dictionary<string, Bitmap>(StringComparer.InvariantCultureIgnoreCase);

        struct PreloadVariables
        {
            public Bitmap Image;

            public string ResourceKey;

            public float Scale;

            public PreloadVariables(IPackage package, IResourceIndexEntry resourceIndexEntry, Gtk.Image imageWidget) : this(package, resourceIndexEntry, imageWidget.WidthRequest, imageWidget.HeightRequest)
            {
            }

            public PreloadVariables(IPackage package, IResourceIndexEntry resourceIndexEntry, int width, int height)
            {
                Image = package.GetTexture(resourceIndexEntry);
                ResourceKey = resourceIndexEntry.ReverseEvaluateResourceKey();
                Scale = (float)Math.Min(width, height) / Math.Min(Image.Width, Image.Height);
            }
        }

        static bool PreloadImage(this IPackage package, IResourceIndexEntry resourceIndexEntry, Gtk.Image imageWidget, Dictionary<string, Bitmap> preloadedImages, Dictionary<string, List<Pixbuf>> preloadedImagePixbufs)
        {
            try
            {
                var preloadVariables = new PreloadVariables(package, resourceIndexEntry, imageWidget);
                preloadedImages[preloadVariables.ResourceKey] = preloadVariables.Image;
                var imageCopy = (Bitmap)preloadVariables.Image.Clone();
                var squareCanvasImage = imageCopy.GetInSquareCanvas();
                preloadedImagePixbufs[preloadVariables.ResourceKey] = new List<Pixbuf>
                    {
                        squareCanvasImage.ToPixbuf()
                    };
                imageCopy.Dispose();
                squareCanvasImage.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Pixbuf Colorize(this Pixbuf pixbuf, Gdk.Color color)
        {
            var bitmap = pixbuf.ToBitmap();
            for (var x = 0; x < bitmap.Width; x++)
            {
                for (var y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(bitmap.GetPixel(x, y).A, color.Red >> 8, color.Green >> 8, color.Blue >> 8));
                }
            }
            return bitmap.ToPixbuf();
        }

        public static Bitmap CreateCheckerboard(int width, int height, int checkSize, System.Drawing.Color primary, System.Drawing.Color secondary)
        {
            var checkerboard = new Bitmap(width, height);
            using (var graphics = System.Drawing.Graphics.FromImage(checkerboard))
            {
                graphics.Clear(primary);
            }
            for (var y = 0; y < height; y += checkSize)
            {
                for (var x = ((y / checkSize) & 1) == 0 ? checkSize : 0; x < width; x += 2 * checkSize)
                {
                    for (var i = 0; i < checkSize && y + i < height; i++)
                    {
                        for (var j = 0; j < checkSize && x + j < width; j++)
                        {
                            checkerboard.SetPixel(x + j, y + i, secondary);
                        }
                    }
                }
            }
            return checkerboard;
        }

        public static void DeletePreloadedImages()
        {
            foreach (var key in new List<string>(PreloadedGameImagePixbufs.Keys))
            {
                var pixbufs = PreloadedGameImagePixbufs[key];
                for (var i = pixbufs.Count - 1; i > -1; i--)
                {
                    pixbufs[i].Dispose();
                    pixbufs.RemoveAt(i);
                }
                PreloadedGameImagePixbufs.Remove(key);
            }
            foreach (var key in new List<string>(PreloadedGameImages.Keys))
            {
                PreloadedGameImages[key].Dispose();
                PreloadedGameImages.Remove(key);
            }
            foreach (var key in new List<string>(PreloadedImagePixbufs.Keys))
            {
                var pixbufs = PreloadedImagePixbufs[key];
                for (var i = pixbufs.Count - 1; i > -1; i--)
                {
                    pixbufs[i].Dispose();
                    pixbufs.RemoveAt(i);
                }
                PreloadedImagePixbufs.Remove(key);
            }
            foreach (var key in new List<string>(PreloadedImages.Keys))
            {
                PreloadedImages[key].Dispose();
                PreloadedImages.Remove(key);
            }
        }

        public static Bitmap GetInSquareCanvas(this Bitmap image)
        {
            if (image.Width == image.Height)
            {
                return image;
            }
            var longestDimension = Math.Max(image.Width, image.Height);
            var squareCanvasImage = new Bitmap(longestDimension, longestDimension);
            using (var graphics = System.Drawing.Graphics.FromImage(squareCanvasImage))
            {
                graphics.DrawImage(image, new System.Drawing.Rectangle((squareCanvasImage.Width >> 1) - (image.Width >> 1), (squareCanvasImage.Height >> 1) - (image.Height >> 1), image.Width, image.Height));
            }
            return squareCanvasImage;
        }

        public static Bitmap GetTexture(this IPackage package, IResourceIndexEntry resourceIndexEntry)
        {
            if (resourceIndexEntry.GetResourceTypeTag() != "_IMG")
            {
                return null;
            }
            lock (CmarNYCBorrowed.TextureUtils.Lock)
            {
                Bitmap image;
                var resource = s3pi.WrapperDealer.WrapperDealer.GetResource(0, package, resourceIndexEntry);
                try
                {
                    image = GDImageLibrary._DDS.LoadImage(resource.AsBytes);
                }
                catch (ArgumentNullException)
                {
                    try
                    {
                        using (var dds = TeximpNet.DDS.DDSFile.Read(resource.Stream))
                        {
                            var mipmap = dds.MipChains[0][0];
                            var pixelFormat = PixelFormat.Format32bppArgb;
                            image = new Bitmap(mipmap.Width, mipmap.Height, pixelFormat);
                            var bitmapData = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, pixelFormat);
                            var byteArray = new byte[mipmap.SizeInBytes];
                            System.Runtime.InteropServices.Marshal.Copy(mipmap.Data, byteArray, 0, byteArray.Length);
                            if (dds.Format == TeximpNet.DDS.DXGIFormat.R8G8_UNorm)
                            {
                                var tempByteArray = new byte[byteArray.Length * 2];
                                for (var i = 0; i < byteArray.Length; i += 2)
                                {
                                    tempByteArray[i * 2] = tempByteArray[i * 2 + 1] = tempByteArray[i * 2 + 2] = byteArray[i];
                                    tempByteArray[i * 2 + 3] = byteArray[i + 1];
                                }
                                byteArray = tempByteArray;
                            }
                            System.Runtime.InteropServices.Marshal.Copy(byteArray, 0, bitmapData.Scan0, byteArray.Length);
                            image.UnlockBits(bitmapData);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Destrospean.Logger.WriteError(ex);
                        return new Bitmap(1024, 1024);
                    }
                }
                return image;
            }
        }

        public static bool PreloadGameImage(this IPackage package, IResourceIndexEntry resourceIndexEntry, Gtk.Image imageWidget)
        {
            return package.PreloadImage(resourceIndexEntry, imageWidget, PreloadedGameImages, PreloadedGameImagePixbufs);
        }

        public static bool PreloadImage(this IPackage package, IResourceIndexEntry resourceIndexEntry, Gtk.Image imageWidget)
        {
            return package.PreloadImage(resourceIndexEntry, imageWidget, PreloadedImages, PreloadedImagePixbufs);
        }

        public static Bitmap Scale(this Bitmap image, int width, int height, System.Drawing.Drawing2D.InterpolationMode interpolationMode)
        {
            var scaledImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            if (scaledImage != null)
            {
                using (var graphics = System.Drawing.Graphics.FromImage(scaledImage))
                {
                    graphics.InterpolationMode = interpolationMode;
                    graphics.DrawImage(image, new System.Drawing.Rectangle(0, 0, scaledImage.Width, scaledImage.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                }
            }
            return scaledImage;
        }

        public static Bitmap ToBitmap(this Pixbuf pixbuf)
        {
            return (Bitmap)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Bitmap)).ConvertFrom(pixbuf.SaveToBuffer("png")); 
        }

        public static Pixbuf ToPixbuf(this Bitmap bitmap)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                return new Pixbuf(stream);
            }
        }
    }
}
