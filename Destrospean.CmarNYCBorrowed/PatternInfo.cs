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

namespace Destrospean.CmarNYCBorrowed
{
    public enum PatternType
    {
        None,
        Colored,
        HSV,
        Solid
    }

    public class PatternInfo
    {
        float[][] mRGBColors;

        public string Background, Name, RGBMask;

        public string[] Channels;

        public bool[] ChannelsEnabled;

        public float[][] HSV, HSVBase, HSVShift;

        public float[] HSVBaseBG, HSVBG, HSVShiftBG, SolidColor;

        public float[][] RGBColors
        {
            get
            {
                var colors = new System.Collections.Generic.List<float[]>();
                for (var i = 0; i < mRGBColors.GetLength(0); i++)
                {
                    if (mRGBColors[i] != null)
                    {
                        colors.Add(mRGBColors[i]);
                    }
                }
                return colors.ToArray();
            }
            set
            {
                mRGBColors = value;
            }
        }

        public PatternType Type
        {
            get
            {
                if (HSVBaseBG != null || HSVBase != null || HSVBG != null || HSV != null || HSVShiftBG != null || HSVShift != null)
                {
                    return PatternType.HSV;
                }
                if (SolidColor != null)
                {
                    return PatternType.Solid;
                }
                if (RGBColors.Length > 1)
                {
                    return PatternType.Colored;
                }
                return PatternType.None;
            }
        }
    }
}
