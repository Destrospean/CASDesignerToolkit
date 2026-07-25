/*
    Xmods Data Library, a library to support tools for The Sims 4,
    Copyright (C) 2014  C. Marinetti

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
    The author may be contacted at modthesims.info, username cmarNYC.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Destrospean.S3PIExtensions;
using s3pi.Interfaces;

namespace Destrospean.CmarNYCBorrowed
{
    public static class TextureUtils
    {
        const float kInverseByteMax = 1f / byte.MaxValue,
        kOneThirdInverseByteMax = kInverseByteMax / 3;

        static Dictionary<string, Bitmap> sPreloadedGameImages, sPreloadedImages;

        public delegate Bitmap GetTextureDelegate(IPackage package, IResourceIndexEntry resourceIndexEntry);

        public static object Lock = new object();

        public static Dictionary<string, Bitmap> PreloadedGameImages
        {
            get
            {
                if (sPreloadedGameImages == null)
                {
                    sPreloadedGameImages = new Dictionary<string, Bitmap>();
                }
                return sPreloadedGameImages;
            }
            set
            {
                sPreloadedGameImages = value;
            }
        }

        public static Dictionary<string, Bitmap> PreloadedImages
        {
            get
            {
                if (sPreloadedImages == null)
                {
                    sPreloadedImages = new Dictionary<string, Bitmap>();
                }
                return sPreloadedImages;
            }
            set
            {
                sPreloadedImages = value;
            }
        }

        public static Bitmap GetHSVPatternImage(this IPackage package, PatternInfo pattern, GetTextureDelegate getTextureCallback)
        {
            int height = 256,
            width = 256;
            Bitmap background = pattern.Background == null ? null : package.GetTexture(pattern.Background, getTextureCallback, width, height),
            patternImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var patternBack = new Bitmap[3];
            if (pattern.RGBMask == null)
            {
                return null;
            }
            var rgbMaskArray = package.GetTextureARGBArray(pattern.RGBMask, getTextureCallback, width, height);
            if (rgbMaskArray == null)
            {
                return null;
            }
            for (var i = 0; pattern.Channels != null && i < pattern.Channels.Length; i++)
            {
                patternBack[i] = package.GetTexture(pattern.Channels[i], getTextureCallback, width, height);
            }
            BitmapData bitmapData0 = patternImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, patternImage.PixelFormat),
            bitmapData1 = background == null ? null : background.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, background.PixelFormat),
            bitmapData2 = null,
            bitmapData3 = null,
            bitmapData4 = null;
            byte[] alphaArray = null,
            backArray = background == null ? null : new byte[Math.Abs(bitmapData1.Stride) * background.Height],
            blueArray = null,
            finalArray = new byte[Math.Abs(bitmapData0.Stride) * patternImage.Height],
            greenArray = null;
            var ptr = bitmapData0.Scan0 + (bitmapData0.Stride > 0 ? 0 : bitmapData0.Stride * (patternImage.Height - 1));
            Marshal.Copy(ptr, finalArray, 0, finalArray.Length);
            if (background != null)
            {
                Marshal.Copy(bitmapData1.Scan0 + (bitmapData1.Stride > 0 ? 0 : bitmapData1.Stride * (background.Height - 1)), backArray, 0, backArray.Length);
            }
            if (pattern.ChannelsEnabled != null && pattern.ChannelsEnabled.Length > 0 && pattern.ChannelsEnabled[0] && patternBack[0] != null)
            {
                bitmapData2 = patternBack[0].LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, patternBack[0].PixelFormat);
                greenArray = new byte[Math.Abs(bitmapData2.Stride) * patternBack[0].Height];
                Marshal.Copy(bitmapData2.Scan0 + (bitmapData2.Stride > 0 ? 0 : bitmapData2.Stride * (patternBack[0].Height - 1)), greenArray, 0, greenArray.Length);
            }
            if (pattern.ChannelsEnabled != null && pattern.ChannelsEnabled.Length > 1 && pattern.ChannelsEnabled[1] && patternBack[1] != null)
            {
                bitmapData3 = patternBack[1].LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, patternBack[1].PixelFormat);
                blueArray = new byte[Math.Abs(bitmapData3.Stride) * patternBack[1].Height];
                Marshal.Copy(bitmapData3.Scan0 + (bitmapData3.Stride > 0 ? 0 : bitmapData3.Stride * (patternBack[1].Height - 1)), blueArray, 0, blueArray.Length);
            }
            if (pattern.ChannelsEnabled != null && pattern.ChannelsEnabled.Length > 2 && pattern.ChannelsEnabled[2] && patternBack[2] != null)
            {
                bitmapData4 = patternBack[2].LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, patternBack[2].PixelFormat);
                alphaArray = new byte[Math.Abs(bitmapData4.Stride) * patternBack[2].Height];
                Marshal.Copy(bitmapData4.Scan0 + (bitmapData4.Stride > 0 ? 0 : bitmapData4.Stride * (patternBack[2].Height - 1)), alphaArray, 0, alphaArray.Length);
            }
            HSVColor alphaChannel = pattern.HSV == null || pattern.HSV.Length < 3 ? new HSVColor(0, 0, 0) : new HSVColor(pattern.HSV[2][0] * 360, pattern.HSV[2][1], pattern.HSV[2][2]),
            backChannel = pattern.HSVBG == null ? new HSVColor(0, 0, 0) : new HSVColor(pattern.HSVBG[0] * 360, pattern.HSVBG[1], pattern.HSVBG[2]),
            blueChannel = pattern.HSV == null || pattern.HSV.Length < 2 ? new HSVColor(0, 0, 0) : new HSVColor(pattern.HSV[1][0] * 360, pattern.HSV[1][1], pattern.HSV[1][2]),
            greenChannel = pattern.HSV == null || pattern.HSV.Length < 1 ? new HSVColor(0, 0, 0) : new HSVColor(pattern.HSV[0][0] * 360, pattern.HSV[0][1], pattern.HSV[0][2]);
            for (var i = 0; i < finalArray.Length; i += 4)
            {
                var hsv = backArray == null ? new HSVColor(0, 0, 0) : new HSVColor(backArray[i + 2], backArray[i + 1], backArray[i]);
                byte[] color = (hsv + backChannel).ToRGB(),
                maskArray = BitConverter.GetBytes(rgbMaskArray[i >> 2]);
                if (pattern.ChannelsEnabled != null && pattern.ChannelsEnabled.Length > 0 && pattern.ChannelsEnabled[0] && maskArray[1] > 0)
                {
                    var tempHSV = new HSVColor(greenArray[i + 2], greenArray[i + 1], greenArray[i]);
                    var tempColor = (tempHSV + greenChannel).ToRGB();
                    var weight = maskArray[1] * kInverseByteMax;
                    for (var j = 0; j < 3; j++)
                    {
                        color[j] = (byte)(tempColor[j] * weight + color[j] * (1 - weight));
                    }
                }
                if (pattern.ChannelsEnabled != null && pattern.ChannelsEnabled.Length > 1 && pattern.ChannelsEnabled[1] && maskArray[0] > 0)
                {
                    var tempHSV = new HSVColor(blueArray[i + 2], blueArray[i + 1], blueArray[i]);
                    var tempColor = (tempHSV + blueChannel).ToRGB();
                    var weight = maskArray[0] * kInverseByteMax;
                    for (var j = 0; j < 3; j++)
                    {
                        color[j] = (byte)(tempColor[j] * weight + color[j] * (1 - weight));
                    }
                }
                if (pattern.ChannelsEnabled != null && pattern.ChannelsEnabled.Length > 2 && pattern.ChannelsEnabled[2] && maskArray[3] > 0)
                {
                    var tempHSV = new HSVColor(alphaArray[i + 2], alphaArray[i + 1], alphaArray[i]);
                    var tempColor = (tempHSV + alphaChannel).ToRGB();
                    var weight = maskArray[3] * kInverseByteMax;
                    for (var j = 0; j < 3; j++)
                    {
                        color[j] = (byte)(tempColor[j] * weight + color[j] * (1 - weight));
                    }
                }
                finalArray[i + 2] = color[0];
                finalArray[i + 1] = color[1];
                finalArray[i] = color[2];
            }
            Marshal.Copy(finalArray, 0, ptr, finalArray.Length);
            patternImage.UnlockBits(bitmapData0);
            if (background != null)
            {
                background.UnlockBits(bitmapData1);
            }
            if (pattern.ChannelsEnabled != null && pattern.ChannelsEnabled.Length > 0 && pattern.ChannelsEnabled[0])
            {
                patternBack[0].UnlockBits(bitmapData2);
            }
            if (pattern.ChannelsEnabled != null && pattern.ChannelsEnabled.Length > 1 && pattern.ChannelsEnabled[1])
            {
                patternBack[1].UnlockBits(bitmapData3);
            }
            if (pattern.ChannelsEnabled != null && pattern.ChannelsEnabled.Length > 2 && pattern.ChannelsEnabled[2])
            {
                patternBack[2].UnlockBits(bitmapData4);
            }
            for (var x = 0; x < patternImage.Width; x++)
            {
                for (var y = 0; y < patternImage.Height; y++)
                {
                    patternImage.SetPixel(x, y, Color.FromArgb(patternImage.GetPixel(x, y).ToArgb() | byte.MaxValue << 24));
                }
            }
            return patternImage;
        }

        public static Bitmap GetRGBPatternImage(this IPackage package, PatternInfo pattern, GetTextureDelegate getTextureCallback)
        {
            int height = 256,
            width = 256;
            var colors = pattern.RGBColors;
            var patternImage = new Bitmap(width, height);
            var rectangle = new Rectangle(0, 0, width, height);
            var bitmapData = patternImage.LockBits(rectangle, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            var byteCount = Math.Abs(bitmapData.Stride) * patternImage.Height;
            var maskArray = package.GetTextureARGBArray(pattern.RGBMask, getTextureCallback, width, height);
            if (maskArray == null)
            {
                return null;
            }
            var textureArray = new byte[byteCount];
            for (var i = 0; i < maskArray.Length; i += 4)
            {
                byte[] mask = BitConverter.GetBytes(maskArray[i >> 2]),
                maskControl =
                    {
                        mask[2],
                        mask[1],
                        mask[0],
                        mask[3]
                    };
                for (var j = 0; j < colors.GetLength(0); j++)
                {
                    if (colors[j] != null && maskControl.Length > j && maskControl[j] > 0)
                    {
                        var blend = maskControl[j] * kInverseByteMax;
                        for (var k = 0; k < 3; k++)
                        {
                            var temp = j == 0 ? colors[j][2 - k] : blend * colors[j][2 - k] + (1 - blend) * textureArray[i + k] * kInverseByteMax;
                            temp = temp < 0 ? 0 : temp > 1 ? 1 : temp;
                            textureArray[i + k] = (byte)(temp * byte.MaxValue);
                        }
                    }
                }
                textureArray[i + 3] = byte.MaxValue;
            }
            Marshal.Copy(textureArray, 0, bitmapData.Scan0 + (bitmapData.Stride > 0 ? 0 : bitmapData.Stride * (patternImage.Height - 1)), byteCount);
            patternImage.UnlockBits(bitmapData);
            return patternImage;
        }

        public static Bitmap GetSkinToneImage(this IPackage package, Tone tone, AgeGender age, AgeGender gender, PartType partType, Bitmap colorRamp, float colorSlider, float cutnessSlider, float cleavageSlider, GetTextureDelegate getTextureCallback)
        {
            if (tone == null)
            {
                return null;
            }
            float[][] alphaMatrix =
                {
                    new float[]
                    {
                        1,
                        0,
                        0,
                        0,
                        0
                    },
                    new float[]
                    {
                        0,
                        1,
                        0,
                        0,
                        0
                    },
                    new float[]
                    {
                        0,
                        0,
                        1,
                        0,
                        0
                    },
                    new float[]
                    {
                        0,
                        0,
                        0,
                        1,
                        0
                    },
                    new float[]
                    { 
                        0,
                        0,
                        0,
                        0,
                        1
                    }
                };
            /*
            redMatrix =
                {
                    new[]
                    {
                        1.05f,
                        0,
                        0,
                        0,
                        0
                    },
                    new float[]
                    {
                        0,
                        1,
                        0,
                        0,
                        0
                    },
                    new float[]
                    {
                        0,
                        0,
                        1,
                        0,
                        0
                    },
                    new float[]
                    {
                        0,
                        0,
                        0,
                        1,
                        0
                    },
                    new float[]
                    {
                        0,
                        0,
                        0,
                        0,
                        1
                    }
                };
            */
            var skinSet = tone.GetSkinSet(Species.Human, age, gender, partType);
            var skinToneImage = package.GetTexture(skinSet.LightLink, getTextureCallback, 1024, 1024);
            using (var graphics = Graphics.FromImage(skinToneImage))
            {
                var darkTexture = package.GetTexture(skinSet.DarkLink, getTextureCallback, 1024, 1024);
                alphaMatrix[3][3] = colorSlider;
                var colorMatrix = new ColorMatrix(alphaMatrix);
                var attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                graphics.DrawImage(darkTexture, new Rectangle(0, 0, skinToneImage.Width, skinToneImage.Height), 0, 0, darkTexture.Width, darkTexture.Height, GraphicsUnit.Pixel, attributes);
                darkTexture.Dispose();
                var overlay = package.GetTexture(skinSet.OverlayLink, getTextureCallback, 1024, 1024);
                if (overlay != null)
                {
                    graphics.DrawImage(overlay, new Rectangle(0, 0, skinToneImage.Width, skinToneImage.Height), 0, 0, overlay.Width, overlay.Height, GraphicsUnit.Pixel);
                    overlay.Dispose();
                }
                /*
                if (age > AgeGender.Child)
                {
                    var cut = package.GetTexture(skinSet.CutnessLink, getTextureCallback, 1024, 1024);
                    if (cut != null)
                    {
                        alphaMatrix[3][3] = cutnessSlider;
                        colorMatrix = new ColorMatrix(alphaMatrix);
                        attributes = new ImageAttributes();
                        attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        graphics.DrawImage(cut, new Rectangle(0, 0, details.Width, details.Height), 0, 0, cut.Width, cut.Height, GraphicsUnit.Pixel, attributes);
                        cut.Dispose();
                    }
                    if ((gender & AgeGender.Female) > 0)
                    {
                        var cleavage = package.GetTexture(skinSet.CleavageLink, getTextureCallback, 1024, 1024);
                        if (cleavage != null)
                        {
                            alphaMatrix[3][3] = cleavageSlider;
                            colorMatrix = new ColorMatrix(alphaMatrix);
                            attributes = new ImageAttributes();
                            attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            graphics.DrawImage(cleavage, new Rectangle(0, 0, details.Width, details.Height), 0, 0, cleavage.Width, cleavage.Height, GraphicsUnit.Pixel, attributes);
                            cleavage.Dispose();
                        }
                    }
                }
                colorMatrix = new ColorMatrix(redMatrix);
                attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                graphics.DrawImage(details, new Rectangle(0, 0, details.Width, details.Height), 0, 0, details.Width, details.Height, GraphicsUnit.Pixel, attributes);
                */
            }
            if (colorRamp == null)
            {
                return skinToneImage;
            }
            var skinColor = colorRamp.GetPixel(colorRamp.Width >> 1, (int)(colorRamp.Height * colorSlider));
            float[] color =
                {
                    (float)skinColor.B / byte.MaxValue,
                    (float)skinColor.G / byte.MaxValue,
                    (float)skinColor.R / byte.MaxValue,
                    1
                };
            var rectangle = new Rectangle(0, 0, skinToneImage.Width, skinToneImage.Height);
            var bitmapData = skinToneImage.LockBits(rectangle, ImageLockMode.ReadWrite, skinToneImage.PixelFormat);
            var ptr = bitmapData.Stride > 0 ? bitmapData.Scan0 : bitmapData.Scan0 + bitmapData.Stride * (skinToneImage.Height - 1);
            var byteCount = Math.Abs(bitmapData.Stride) * skinToneImage.Height;
            var detail = new byte[byteCount];
            Marshal.Copy(ptr, detail, 0, byteCount);
            //float contrast = 1.25f,
            //midpoint = .75f;
            for (var i = 0; i < detail.Length; i += 4)
            {
                for (var j = 0; j < 3; j++)
                {
                    var temp = (float)detail[i + j] / byte.MaxValue;
                    temp = temp * color[j] * byte.MaxValue;
                    temp = temp < 0 ? 0 : temp > byte.MaxValue ? byte.MaxValue : temp;
                    detail[i + j] = (byte)temp;
                }
                int columnCount,
                rowCount = Math.DivRem(i >> 2, skinToneImage.Width, out columnCount);
                if (rowCount > 665 && rowCount < 842 && columnCount < 85)
                {
                    detail[i + 3] = 0;
                }
            }
            Marshal.Copy(detail, 0, ptr, byteCount);
            skinToneImage.UnlockBits(bitmapData);
            return skinToneImage;
        }

        public static Bitmap GetTexture(this IPackage package, string key, GetTextureDelegate getTextureCallback, int[] dimensions = null)
        {
            Bitmap image;
            if (!PreloadedGameImages.TryGetValue(key, out image) && !PreloadedImages.TryGetValue(key, out image))
            {
                PackageResourceIndexEntryTuple evaluated;
                try
                {
                    evaluated = package.EvaluateImageResourceKey(key);
                }
                catch (ResourceIndexEntryNotFoundException)
                {
                    return null;
                }
                image = getTextureCallback(evaluated.Package, evaluated.ResourceIndexEntry);
                if (evaluated.Package == package)
                {
                    PreloadedImages[key] = image;
                }
                else
                {
                    PreloadedGameImages[key] = image;
                }
            }
            lock (Lock)
            {
                if (dimensions != null && dimensions[0] != image.Size.Width && dimensions[1] != image.Size.Height)
                {
                    image = new Bitmap(image, new Size(dimensions[0], dimensions[1]));
                }
                return (Bitmap)image.Clone();
            }
        }

        public static Bitmap GetTexture(this IPackage package, string key, GetTextureDelegate getTextureCallback, int width, int height)
        {
            return package.GetTexture(key, getTextureCallback, new[]
                {
                    width,
                    height
                });
        }

        public static Bitmap GetTexture(this IPackage package, TGI tgi, GetTextureDelegate getTextureCallback, int width, int height)
        {
            return package.GetTexture(new ResourceKey(tgi.Type, tgi.Group, tgi.Instance).ReverseEvaluateResourceKey(), getTextureCallback, new[]
                {
                    width,
                    height
                });
        }

        public static uint[] GetTextureARGBArray(this IPackage package, string key, GetTextureDelegate getTextureCallback, int[] dimensions = null)
        {
            var image = package.GetTexture(key, getTextureCallback, dimensions);
            if (image == null)
            {
                return null;
            }
            var bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var byteCount = Math.Abs(bitmapData.Stride) * image.Height;
            var bgraValues = new byte[byteCount];
            var argbValues = new uint[byteCount];
            Marshal.Copy(bitmapData.Scan0, bgraValues, 0, byteCount);
            image.UnlockBits(bitmapData);
            for (var i = 0; i < byteCount; i += 4)
            {
                argbValues[i >> 2] = ((uint)bgraValues[i + 3] << 24) + ((uint)bgraValues[i + 2] << 16) + ((uint)bgraValues[i + 1] << 8) + bgraValues[i];
            }
            return argbValues;
        }

        public static uint[] GetTextureARGBArray(this IPackage package, string key, GetTextureDelegate getTextureCallback, int width, int height)
        {
            return package.GetTextureARGBArray(key, getTextureCallback, new[]
                {
                    width,
                    height
                });
        }

        public static Bitmap GetWithPatternsApplied(this Bitmap multiplier, uint[] maskArray, System.Collections.Generic.List<object> patternImages, bool overlay)
        {
            var multiplierCopy = (Bitmap)multiplier.Clone();
            var rectangle = new Rectangle(0, 0, multiplierCopy.Width, multiplierCopy.Height);
            var bitmapData = multiplierCopy.LockBits(rectangle, ImageLockMode.ReadWrite, multiplierCopy.PixelFormat);
            var byteCount = Math.Abs(bitmapData.Stride) * multiplierCopy.Height;
            var multiplierArray = new byte[byteCount];
            var ptr = bitmapData.Scan0 + (bitmapData.Stride > 0 ? 0 : bitmapData.Stride * (multiplierCopy.Height - 1));
            Marshal.Copy(ptr, multiplierArray, 0, byteCount);
            for (var i = 0; i < byteCount; i += 4)
            {
                var gray = (multiplierArray[i] + multiplierArray[i + 1] + multiplierArray[i + 2]) * kOneThirdInverseByteMax * (overlay ? 1 : 2);
                byte[] mask = BitConverter.GetBytes(maskArray[i >> 2]),
                maskControl =
                    {
                        mask[2],
                        mask[1],
                        mask[0],
                        mask[3]
                    };
                for (var j = 0; j < patternImages.Count; j++)
                {
                    var blend = maskControl[j] * kInverseByteMax;
                    if (patternImages[j] != null && maskControl[j] > 0)
                    {
                        var rgba = patternImages[j] as float[];
                        if (rgba != null)
                        {
                            for (var k = 0; k < 3; k++)
                            {
                                var temp = gray * rgba[2 - k];
                                temp = temp < 0 ? 0 : temp > 1 ? 1 : temp;
                                multiplierArray[i + k] = (byte)((blend * temp + (1 - blend) * multiplierArray[i + k] * kInverseByteMax) * byte.MaxValue);
                            }
                            continue;
                        }
                        var image = patternImages[j] as Bitmap;
                        if (image != null)
                        {
                            int currentX,
                            currentY = Math.DivRem(i >> 2, multiplierCopy.Width, out currentX),
                            height,
                            width;
                            Math.DivRem(currentX, image.Width, out width);
                            Math.DivRem(currentY, image.Height, out height);
                            var color = image.GetPixel(width, height);
                            rgba = new[]
                                {
                                    color.R * kInverseByteMax,
                                    color.G * kInverseByteMax,
                                    color.B * kInverseByteMax,
                                    color.A * kInverseByteMax
                                };
                            for (var k = 0; k < 3; k++)
                            {
                                var temp = gray * rgba[2 - k];
                                temp = temp < 0 ? 0 : temp > 1 ? 1 : temp;
                                multiplierArray[i + k] = (byte)((blend * temp + (1 - blend) * multiplierArray[i + k] * kInverseByteMax) * byte.MaxValue);
                            }
                        }
                    }
                }
            }
            Marshal.Copy(multiplierArray, 0, ptr, byteCount);
            multiplierCopy.UnlockBits(bitmapData);
            return multiplierCopy;
        }
    }
}
