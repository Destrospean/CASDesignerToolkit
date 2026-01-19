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

namespace Destrospean.CmarNYCBorrowed
{
    public class HSVColor
    {
        float mHue, mSaturation, mValue;

        public float Hue
        {
            get
            {
                return mHue;
            }
            set
            {
                mHue = value;
                while (mHue > 360)
                {
                    mHue -= 360;
                }
                while (mHue < 0)
                {
                    mHue += 360;
                }
            }
        }

        public float Saturation
        {
            get
            {
                return mSaturation;
            }
            set
            {
                mSaturation = value;
                if (mSaturation > 1)
                {
                    mSaturation = 1;
                }
                if (mSaturation < 0)
                {
                    mSaturation = 0;
                }
            }
        }

        public float Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
                if (mValue > 1)
                {
                    mValue = 1;
                }
                if (mValue < 0)
                {
                    mValue = 0;
                }
            }
        }

        public HSVColor(float hue, float saturation, float value, bool corrected = false)
        {
            if (corrected)
            {
                Hue = hue;
                Saturation = saturation;
                Value = value;
                return;
            }
            mHue = hue;
            mSaturation = saturation;
            mValue = value;
        }

        /// <summary>
        /// Create HSV color from red, green, blue values
        /// </summary>
        /// <param name="r">0-255</param>
        /// <param name="g">0-255</param>
        /// <param name="b">0-255</param>
        /// <returns></returns>
        public HSVColor(byte r, byte g, byte b)
        {
            double blue = (double)b / byte.MaxValue,
            green = (double)g / byte.MaxValue,
            hue = 0,
            red = (double)r / byte.MaxValue,
            subtrahend = Math.Min(Math.Min(red, green), blue),
            value = Math.Max(Math.Max(red, green), blue);
            if (subtrahend != value)
            {
                if (red == value && green >= blue)
                {
                    hue = 60 * (green - blue) / (value - subtrahend) + 0;
                }
                else if (red == value && green < blue)
                {
                    hue = 60 * (green - blue) / (value - subtrahend) + 360;
                }
                else if (green == value)
                {
                    hue = 60 * (blue - red) / (value - subtrahend) + 120;
                }
                else if (blue == value)
                {
                    hue = 60 * (red - green) / (value - subtrahend) + 240;
                }
            }
            Hue = (float)hue;
            Value = (float)value;
            if (value == 0)
            {
                Saturation = 0;
            }
            else
            {
                Saturation = (float)((value - subtrahend) / value);
            }
        }

        static byte Clamp(int number)
        {
            return (byte)(number < 0 ? 0 : number > byte.MaxValue ? byte.MaxValue : number);
        }

        public static HSVColor operator +(HSVColor augendHSV, HSVColor summandHSV)
        {
            return new HSVColor(augendHSV.mHue + summandHSV.mHue, augendHSV.mSaturation + summandHSV.mSaturation, augendHSV.mValue + summandHSV.mValue, true);
        }

        public static HSVColor operator -(HSVColor minuendHSV, HSVColor subtrahendHSV)
        {
            return new HSVColor(minuendHSV.mHue - subtrahendHSV.mHue, minuendHSV.mSaturation - subtrahendHSV.mSaturation, minuendHSV.mValue - subtrahendHSV.mValue, true);
        }

        /// <summary>
        /// Returns byte array of red (0-255), green (0-255), blue (0-255)
        /// </summary>
        public byte[] ToRGB()
        {
            double blue,
            green,
            hue = mHue,
            red;
            while (hue < 0)
            {
                hue += 360;
            }
            while (hue >= 360)
            {
                hue -= 360;
            }
            if (mValue <= 0)
            {
                double channel;
                blue = channel = 0;
                green = channel;
                red = channel;
            }
            else if (mSaturation <= 0)
            {
                double channel;
                blue = channel = mValue;
                green = channel;
                red = channel;
            }
            else
            {
                double hueDividedBy60 = hue / 60,
                roundedDown = Math.Floor(hueDividedBy60),
                modifier = hueDividedBy60 - roundedDown,
                channel0 = mValue * (1.0 - mSaturation),
                channel1 = mValue * (1.0 - mSaturation * modifier),
                channel2 = mValue * (1.0 - mSaturation * (1.0 - modifier));
                switch ((int)roundedDown)
                {
                    case -1:
                        red = mValue;
                        green = channel0;
                        blue = channel1;
                        break;
                    case 0:
                        red = mValue;
                        green = channel2;
                        blue = channel0;
                        break;
                    case 1:
                        red = channel1;
                        green = mValue;
                        blue = channel0;
                        break;
                    case 2:
                        red = channel0;
                        green = mValue;
                        blue = channel2;
                        break;
                    case 3:
                        red = channel0;
                        green = channel1;
                        blue = mValue;
                        break;
                    case 4:
                        red = channel2;
                        green = channel0;
                        blue = mValue;
                        break;
                    case 5:
                        red = mValue;
                        green = channel0;
                        blue = channel1;
                        break;
                    case 6:
                        red = mValue;
                        green = channel2;
                        blue = channel0;
                        break;
                    default:
                        double channel;
                        blue = channel = mValue;
                        green = channel;
                        red = channel;
                        break;
                }
            }
            return new byte[]
            {
                Clamp((int)(red * byte.MaxValue)), Clamp((int)(green * byte.MaxValue)), Clamp((int)(blue * byte.MaxValue))
            };
        }
    }
}
