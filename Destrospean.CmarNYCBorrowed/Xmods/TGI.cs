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
using System.IO;

namespace Destrospean.CmarNYCBorrowed
{
    public class TGI
    {
        public uint Group, Type;

        public ulong Instance;

        public enum TGISequence
        {
            TGI, ITG, IGT
        }

        public TGI()
        {
            Group = 0;
            Instance = 0;
            Type = 0;
        }

        public TGI(BinaryReader reader)
        {
            Type = reader.ReadUInt32();
            Group = reader.ReadUInt32();
            Instance = reader.ReadUInt64();
        }

        public TGI(BinaryReader reader, TGISequence sequence)
        {
            if (sequence == TGISequence.TGI)
            {
                Type = reader.ReadUInt32();
                Group = reader.ReadUInt32();
                Instance = reader.ReadUInt64();
            }
            if (sequence == TGISequence.IGT)
            {
                Instance = reader.ReadUInt64();
                Group = reader.ReadUInt32();
                Type = reader.ReadUInt32();
            }
            if (sequence == TGISequence.ITG)
            {
                Instance = reader.ReadUInt64();
                Type = reader.ReadUInt32();
                Group = reader.ReadUInt32();
            }
        }

        public TGI(string tgi)
        {
            if (String.CompareOrdinal(tgi, " ") <= 0)
            {
                Group = 0;
                Instance = 0;
                Type = 0;
                return;
            }
            var myTGI = tgi.Split('-', ':', '.', ' ', '_');
            for (var i = 0; i < myTGI.Length; i++)
            {
                if (String.CompareOrdinal(myTGI[i].Substring(0, 2), "0x") == 0)
                {
                    myTGI[i] = myTGI[i].Substring(2);
                }
            }
            try
            {
                Group = UInt32.Parse(myTGI[1], System.Globalization.NumberStyles.HexNumber);
                Instance = UInt64.Parse(myTGI[2], System.Globalization.NumberStyles.HexNumber);
                Type = UInt32.Parse(myTGI[0], System.Globalization.NumberStyles.HexNumber);
            }
            catch
            {
                throw new ApplicationException("Can't parse TGI string " + tgi);
            }
        }

        public TGI(TGI tgi)
        {
            Group = tgi.Group;
            Instance = tgi.Instance;
            Type = tgi.Type;
        }

        public TGI(uint type, uint group, ulong instance)
        {
            Group = group;
            Instance = instance;
            Type = type;
        }

        public static TGI[] CopyTGIArray(TGI[] source)
        {
            var temp = new TGI[source.Length];
            Array.Copy(source, temp, source.Length);
            return temp;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TGI);
        }

        public bool Equals(string tgi)
        {
            var temp = new TGI(tgi);
            return (Type == temp.Type & Group == temp.Group & Instance == temp.Instance);
        }

        public bool Equals(TGI tgi)
        {
            return (Type == tgi.Type & Group == tgi.Group & Instance == tgi.Instance);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() + Group.GetHashCode() + Instance.GetHashCode();
        }

        public override string ToString()
        {
            return "0x" + Type.ToString("X8") + "-" + "0x" + Group.ToString("X8") + "-" + "0x" + Instance.ToString("X16");
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Type);
            writer.Write(Group);
            writer.Write(Instance);
        }

        public void Write(BinaryWriter writer, TGISequence sequence)
        {
            if (sequence == TGISequence.TGI)
            {
                writer.Write(Type);
                writer.Write(Group);
                writer.Write(Instance);
            }
            if (sequence == TGISequence.IGT)
            {
                writer.Write(Instance);
                writer.Write(Group);
                writer.Write(Type);
            }
            if (sequence == TGISequence.ITG)
            {
                writer.Write(Instance);
                writer.Write(Type);
                writer.Write(Group);
            }
        }
    }
}
