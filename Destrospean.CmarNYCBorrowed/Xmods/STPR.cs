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
    public struct STPR
    {
        public float Amount
        {
            get;
            private set;
        }

        public TGI SkinToneKey
        {
            get;
            private set;
        }

        public int Version
        {
            get;
            private set;
        }

        public STPR(System.IO.BinaryReader reader)
        {
            reader.BaseStream.Position = 0;
            Version = reader.ReadInt32();
            Amount = reader.ReadSingle();
            SkinToneKey = new TGI(reader, TGI.TGISequence.IGT);
        }

        public void Write(System.IO.BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(Amount);
            SkinToneKey.Write(writer, TGI.TGISequence.IGT);
        }
    }
}
