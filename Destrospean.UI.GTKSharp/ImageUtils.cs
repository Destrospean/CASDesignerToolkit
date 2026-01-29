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

        public static Bitmap Base64StringToBitmap(string base64String)
        {
            using (var stream = new System.IO.MemoryStream(Convert.FromBase64String(base64String)))
            {
                return (Bitmap)Bitmap.FromStream(stream);
            }
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
                    catch (System.OverflowException)
                    {
                        image = new Bitmap(1024, 1024);
                    }
                }
                return image;
            }
        }

        public static bool PreloadGameImage(this IPackage package, IResourceIndexEntry resourceIndexEntry, Gtk.Image imageWidget)
        {
            try
            {
                var preloadVariables = new PreloadVariables(package, resourceIndexEntry, imageWidget);
                PreloadedGameImages[preloadVariables.ResourceKey] = preloadVariables.Image;
                var squareCanvasImage = preloadVariables.Image.GetInSquareCanvas();
                PreloadedGameImagePixbufs[preloadVariables.ResourceKey] = new List<Pixbuf>
                    {
                        squareCanvasImage.ToPixbuf().ScaleSimple((int)(squareCanvasImage.Width * preloadVariables.Scale), (int)(squareCanvasImage.Height * preloadVariables.Scale), InterpType.Bilinear)
                    };
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool PreloadImage(this IPackage package, IResourceIndexEntry resourceIndexEntry, Gtk.Image imageWidget)
        {
            try
            {
                var preloadVariables = new PreloadVariables(package, resourceIndexEntry, imageWidget);
                PreloadedImages[preloadVariables.ResourceKey] = preloadVariables.Image;
                var squareCanvasImage = preloadVariables.Image.GetInSquareCanvas();
                PreloadedImagePixbufs[preloadVariables.ResourceKey] = new List<Pixbuf>
                    {
                        squareCanvasImage.ToPixbuf().ScaleSimple((int)(squareCanvasImage.Width * preloadVariables.Scale), (int)(squareCanvasImage.Height * preloadVariables.Scale), InterpType.Bilinear)
                    };
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Bitmap Scale(this Bitmap image, int width, int height, System.Drawing.Drawing2D.InterpolationMode interpolationMode)
        {
            var imageCopy = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            if (imageCopy != null)
            {
                using (var graphics = System.Drawing.Graphics.FromImage(imageCopy))
                {
                    graphics.InterpolationMode = interpolationMode;
                    graphics.DrawImage(image, new System.Drawing.Rectangle(0, 0, imageCopy.Width, imageCopy.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                }
            }
            return imageCopy;
        }

        public static Bitmap ToBitmap(this Pixbuf pixbuf)
        {
            return (Bitmap)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Bitmap)).ConvertFrom(pixbuf.SaveToBuffer("png")); 
        }

        public static Pixbuf ToPixbuf(this Bitmap bitmap)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                return new Pixbuf(stream);
            }
        }
    }
}
