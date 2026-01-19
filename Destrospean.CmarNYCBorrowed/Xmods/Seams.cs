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
    public struct Seams
    {
        public uint[][] mBoneHash;

        public string[][] mBoneName;

        public float[][] mBoneWeight;

        public Vector3[] mNormal, mPosition;

        public Seams(string ageGender)
        {
            if (string.Compare(ageGender, "PULOD1") == 0)
            {
                Vector3[] tempP =
                    {
                        new Vector3(-.031278f, .696061f, -.03069f),
                        new Vector3(-.01715f, .697504f, -.039892f), 
                        new Vector3(-.037518f, .692705f, -.014709f),
                        new Vector3(-.031891f, .6852421f, .008956f), 
                        new Vector3(-.037665f, .689176f, -.002054f),
                        new Vector3(-.015221f, .6792201f, .02441f), 
                        new Vector3(-.024111f, .6819131f, .018054f),
                        new Vector3(0, .677699f, .028111f), 
                        new Vector3(0, .697633f, -.042098f),
                        new Vector3(.031278f, .696061f, -.03069f), 
                        new Vector3(.01715f, .697504f, -.039892f),
                        new Vector3(.037518f, .692705f, -.014709f), 
                        new Vector3(.031891f, .6852421f, .008956f),
                        new Vector3(.037665f, .689176f, -.002054f), 
                        new Vector3(.015221f, .6792201f, .02441f),
                        new Vector3(.024111f, .6819131f, .018054f), 
                        new Vector3(0, .450725f, .084889f),
                        new Vector3(-.026126f, .450717f, .082076f), 
                        new Vector3(-.050231f, .451089f, .071738f),
                        new Vector3(-.069655f, .451757f, .050202f), 
                        new Vector3(-.072682f, .452406f, -.021256f),
                        new Vector3(-.078725f, .452308f, .015333f), 
                        new Vector3(0, .452853f, -.051468f),
                        new Vector3(-.020936f, .452878f, -.053291f), 
                        new Vector3(-.048281f, .45267f, -.045821f),
                        new Vector3(.026126f, .450717f, .082076f), 
                        new Vector3(.069655f, .451757f, .050202f),
                        new Vector3(.050231f, .451089f, .071738f), 
                        new Vector3(.072682f, .452406f, -.021256f),
                        new Vector3(.078725f, .452308f, .015333f), 
                        new Vector3(.020936f, .452878f, -.053291f),
                        new Vector3(.048281f, .45267f, -.045821f), 
                        new Vector3(-.069705f, .091365f, .026381f),
                        new Vector3(-.075996f, .091249f, .006737f), 
                        new Vector3(-.054291f, .091519f, .03484f),
                        new Vector3(-.038153f, .09198301f, .029039f), 
                        new Vector3(-.029277f, .092752f, .012425f),
                        new Vector3(-.032156f, .092401f, -.008632f), 
                        new Vector3(-.048907f, .09159001f, -.019313f),
                        new Vector3(-.068639f, .091491f, -.013501f), 
                        new Vector3(.075996f, .091249f, .006737f),
                        new Vector3(.069705f, .091365f, .026381f), 
                        new Vector3(.054291f, .091519f, .03484f),
                        new Vector3(.029277f, .092752f, .012425f), 
                        new Vector3(.038153f, .09198301f, .029039f),
                        new Vector3(.032156f, .092401f, -.008632f), 
                        new Vector3(.048907f, .09159001f, -.019313f),
                        new Vector3(.068639f, .091491f, -.013501f)
                    };
                mPosition = tempP;
                Vector3[] tempN =
                    {
                        new Vector3(-.757879f, -.129651f, -.639382f),
                        new Vector3(-.34021f, -.182772f, -.922416f), 
                        new Vector3(-.955967f, -.129452f, -.263378f),
                        new Vector3(-.8778991f, -.164225f, .449803f), 
                        new Vector3(-.984858f, -.103675f, .138944f),
                        new Vector3(-.416246f, -.762288f, .495637f), 
                        new Vector3(-.738923f, -.494488f, .457684f),
                        new Vector3(0, -.932587f, .360946f), 
                        new Vector3(0, -.20714f, -.978311f),
                        new Vector3(.75788f, -.129651f, -.6393811f), 
                        new Vector3(.340211f, -.182772f, -.922416f),
                        new Vector3(.955968f, -.129452f, -.263378f), 
                        new Vector3(.8778991f, -.164225f, .449803f),
                        new Vector3(.984859f, -.103675f, .138943f), 
                        new Vector3(.416246f, -.762287f, .495638f),
                        new Vector3(.738922f, -.494487f, .457686f), 
                        new Vector3(0, -.153836f, .988096f),
                        new Vector3(-.246222f, -.146825f, .958028f), 
                        new Vector3(-.563352f, -.100154f, .820124f),
                        new Vector3(-.871743f, -.020586f, .489531f), 
                        new Vector3(-.891269f, .074697f, -.447281f),
                        new Vector3(-.998683f, .022238f, .04623f), 
                        new Vector3(0, .184251f, -.982879f),
                        new Vector3(-.080109f, .189392f, -.978628f), 
                        new Vector3(-.502058f, .159632f, -.849974f),
                        new Vector3(.246222f, -.146825f, .958028f), 
                        new Vector3(.871743f, -.020586f, .489531f),
                        new Vector3(.563352f, -.100154f, .820124f), 
                        new Vector3(.891269f, .074697f, -.447281f),
                        new Vector3(.998683f, .022238f, .04623f), 
                        new Vector3(.080109f, .189392f, -.978628f),
                        new Vector3(.502058f, .159632f, -.849974f), 
                        new Vector3(-.753504f, -.141021f, .642141f),
                        new Vector3(-.994094f, -.107919f, -.01138f), 
                        new Vector3(-.081238f, -.146174f, .985918f),
                        new Vector3(.625808f, -.120885f, .770552f), 
                        new Vector3(.974004f, -.109498f, .198306f),
                        new Vector3(.831926f, -.098882f, -.546005f), 
                        new Vector3(.137225f, -.116874f, -.983621f),
                        new Vector3(-.738057f, -.125224f, -.663016f), 
                        new Vector3(.994094f, -.107919f, -.01138f),
                        new Vector3(.753504f, -.141021f, .642141f), 
                        new Vector3(.081238f, -.146174f, .985918f),
                        new Vector3(-.974004f, -.109498f, .198306f), 
                        new Vector3(-.625808f, -.120885f, .770552f),
                        new Vector3(-.831926f, -.098882f, -.546005f), 
                        new Vector3(-.137225f, -.116874f, -.983621f),
                        new Vector3(.738057f, -.125224f, -.663016f)
                    };
                mNormal = tempN;
                string[][] tempB =
                    {
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }
                    };
                mBoneName = tempB;
                uint[][] tempBH =
                    {
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }
                    };
                mBoneHash = tempBH;
                float[][] tempBW =
                    {
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }
                    };
                mBoneWeight = tempBW;
            }
            else if (string.Compare(ageGender, "PULOD2") == 0)
            {
                Vector3[] tempP =
                    {
                        new Vector3(-.01844f, .679836f, .023678f),
                        new Vector3(-.031376f, .684743f, .0099f), 
                        new Vector3(-.037688f, .691183f, -.008308f),
                        new Vector3(0, .677699f, .028111f), 
                        new Vector3(-.029845f, .696691f, -.032878f),
                        new Vector3(-.036594f, .693614f, -.017549f), 
                        new Vector3(0, .697633f, -.042098f),
                        new Vector3(.01844f, .679836f, .023678f), 
                        new Vector3(.031376f, .684743f, .0099f),
                        new Vector3(.037688f, .691183f, -.008308f), 
                        new Vector3(.029845f, .696691f, -.032878f),
                        new Vector3(.036594f, .693614f, -.017549f), 
                        new Vector3(-.026126f, .450717f, .082076f),
                        new Vector3(-.050231f, .451089f, .071738f), 
                        new Vector3(-.069655f, .451757f, .050202f),
                        new Vector3(-.078725f, .452308f, .015333f), 
                        new Vector3(.050231f, .451089f, .071738f),
                        new Vector3(.026126f, .450717f, .082076f), 
                        new Vector3(0, .450725f, .08488901f),
                        new Vector3(.069655f, .451757f, .050202f), 
                        new Vector3(.078725f, .452308f, .015333f),
                        new Vector3(-.072682f, .452406f, -.021256f), 
                        new Vector3(-.048281f, .45267f, -.045821f),
                        new Vector3(-.020936f, .452878f, -.053291f), 
                        new Vector3(.07268201f, .452406f, -.021256f),
                        new Vector3(.048281f, .45267f, -.045821f), 
                        new Vector3(.020936f, .452878f, -.053291f),
                        new Vector3(0, .452853f, -.051468f), 
                        new Vector3(-.054291f, .091519f, .03484f),
                        new Vector3(-.038153f, .09198301f, .029039f), 
                        new Vector3(-.069705f, .091365f, .026381f),
                        new Vector3(-.075996f, .091249f, .006737f), 
                        new Vector3(-.029277f, .092752f, .012425f),
                        new Vector3(-.032156f, .092401f, -.008632f), 
                        new Vector3(-.048907f, .09159001f, -.019313f),
                        new Vector3(-.068639f, .091491f, -.013501f), 
                        new Vector3(.029277f, .092752f, .012425f),
                        new Vector3(.068639f, .091491f, -.013501f), 
                        new Vector3(.048907f, .09159001f, -.019313f),
                        new Vector3(.054291f, .091519f, .03484f), 
                        new Vector3(.038153f, .09198301f, .029039f),
                        new Vector3(.032156f, .092401f, -.008632f), 
                        new Vector3(.075996f, .091249f, .006737f),
                        new Vector3(.069705f, .091365f, .026381f)
                    };
                mPosition = tempP;
                Vector3[] tempN =
                    {
                        new Vector3(-.503272f, -.5352501f, .678398f),
                        new Vector3(-.835081f, -.389278f, .388719f), 
                        new Vector3(-.9769961f, -.212229f, .020934f),
                        new Vector3(0, -.71785f, .696197f), 
                        new Vector3(-.624712f, -.052771f, -.7790701f),
                        new Vector3(-.952426f, -.090944f, -.290884f), 
                        new Vector3(0, -.091163f, -.995836f),
                        new Vector3(.503272f, -.5352501f, .678398f), 
                        new Vector3(.835081f, -.389278f, .388719f),
                        new Vector3(.9769961f, -.212229f, .020934f), 
                        new Vector3(.624712f, -.052771f, -.7790701f),
                        new Vector3(.952426f, -.090944f, -.290885f), 
                        new Vector3(-.219198f, -.151105f, .9639081f),
                        new Vector3(-.547097f, -.094614f, .831705f), 
                        new Vector3(-.883734f, .008286f, .467916f),
                        new Vector3(-.998948f, .019137f, .04168f), 
                        new Vector3(.547097f, -.094614f, .831705f),
                        new Vector3(.253231f, -.135577f, .957858f), 
                        new Vector3(.018778f, -.152735f, .988089f),
                        new Vector3(.884247f, .006201f, .466979f), 
                        new Vector3(.998518f, .029819f, .045519f),
                        new Vector3(-.898744f, .069376f, -.432951f), 
                        new Vector3(-.486671f, .155372f, -.859657f),
                        new Vector3(-.034887f, .168352f, -.985109f), 
                        new Vector3(.87032f, .080455f, -.485871f),
                        new Vector3(.486671f, .155372f, -.859657f), 
                        new Vector3(.08966701f, .170478f, -.9812731f),
                        new Vector3(-.036673f, .159943f, -.9864451f), 
                        new Vector3(-.099911f, -.113575f, .9884931f),
                        new Vector3(.710771f, -.07212101f, .699717f), 
                        new Vector3(-.745001f, -.042036f, .665738f),
                        new Vector3(-.99605f, -.085688f, -.023273f), 
                        new Vector3(.976818f, -.114212f, .181056f),
                        new Vector3(.82801f, -.07409801f, -.555796f), 
                        new Vector3(.239203f, -.02977f, -.970513f),
                        new Vector3(-.755114f, -.055845f, -.653211f), 
                        new Vector3(-.976818f, -.114212f, .181056f),
                        new Vector3(.762378f, -.055102f, -.644782f), 
                        new Vector3(-.239203f, -.02977f, -.970513f),
                        new Vector3(.099911f, -.113575f, .9884931f), 
                        new Vector3(-.710771f, -.07212101f, .699717f),
                        new Vector3(-.82801f, -.07409801f, -.555796f), 
                        new Vector3(.998364f, -.05322601f, -.020874f),
                        new Vector3(.733317f, -.018231f, .679642f)
                    };
                mNormal = tempN;
                string[][] tempB =
                    {
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }
                    };
                mBoneName = tempB;
                uint[][] tempBH =
                    {
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }
                    };
                mBoneHash = tempBH;
                float[][] tempBW =
                    {
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }
                    };
                mBoneWeight = tempBW;
            }
            else if (string.Compare(ageGender, "PULOD3") == 0)
            {
                Vector3[] tempP =
                    {
                        new Vector3(.029845f, .696691f, -.032878f),
                        new Vector3(0, .697633f, -.042098f), 
                        new Vector3(-.029845f, .696691f, -.032878f),
                        new Vector3(-.03486f, .687241f, .003416f), 
                        new Vector3(0, .677699f, .028111f),
                        new Vector3(.03486f, .687241f, .003416f), 
                        new Vector3(.072682f, .452406f, -.021256f),
                        new Vector3(.048281f, .45267f, -.045821f), 
                        new Vector3(.078725f, .452308f, .015333f),
                        new Vector3(-.026126f, .450717f, .082076f), 
                        new Vector3(0, .450725f, .084889f),
                        new Vector3(.069655f, .451757f, .050202f), 
                        new Vector3(.050231f, .451089f, .071738f),
                        new Vector3(.026126f, .450717f, .082076f), 
                        new Vector3(-.048281f, .45267f, -.045821f),
                        new Vector3(-.020936f, .452878f, -.053291f), 
                        new Vector3(.010468f, .452865f, -.05238f),
                        new Vector3(-.07268201f, .452406f, -.021256f), 
                        new Vector3(-.069655f, .451757f, .050202f),
                        new Vector3(-.050231f, .451089f, .071738f), 
                        new Vector3(-.078725f, .452308f, .015333f),
                        new Vector3(.029277f, .092752f, .012425f), 
                        new Vector3(.068639f, .091491f, -.013501f),
                        new Vector3(.048907f, .09159f, -.019313f), 
                        new Vector3(.054291f, .091519f, .03484f),
                        new Vector3(.038153f, .091983f, .029039f), 
                        new Vector3(.032156f, .092401f, -.008632f),
                        new Vector3(.069705f, .091365f, .026381f), 
                        new Vector3(.075996f, .091249f, .006737f),
                        new Vector3(-.068639f, .091491f, -.013501f), 
                        new Vector3(-.069705f, .091365f, .026381f),
                        new Vector3(-.032156f, .092401f, -.008632f), 
                        new Vector3(-.075996f, .091249f, .006737f),
                        new Vector3(-.046222f, .091751f, .031939f), 
                        new Vector3(-.029277f, .092752f, .012425f),
                        new Vector3(-.048907f, .09159f, -.019313f)
                    };
                mPosition = tempP;
                Vector3[] tempN =
                    {
                        new Vector3(.779864f, .084055f, -.62028f),
                        new Vector3(0, -.016788f, -.999859f), 
                        new Vector3(-.7798631f, .084055f, -.62028f),
                        new Vector3(-.9476191f, -.190021f, .256731f), 
                        new Vector3(0, -.799938f, .600083f),
                        new Vector3(.9476191f, -.190021f, .256731f), 
                        new Vector3(.840917f, .034496f, -.540063f),
                        new Vector3(.492832f, .080735f, -.866371f), 
                        new Vector3(.999428f, .009023f, .032605f),
                        new Vector3(-.207677f, -.157772f, .9653901f), 
                        new Vector3(.000504f, -.146607f, .989195f),
                        new Vector3(.794415f, -.035165f, .606357f), 
                        new Vector3(.576211f, -.152654f, .802918f),
                        new Vector3(.240903f, -.071835f, .967887f), 
                        new Vector3(-.498809f, .050566f, -.865236f),
                        new Vector3(-.138132f, .077643f, -.987366f), 
                        new Vector3(.11055f, .156684f, -.981442f),
                        new Vector3(-.902017f, .030054f, -.430653f), 
                        new Vector3(-.854682f, .010594f, .519043f),
                        new Vector3(-.573815f, -.079853f, .815083f), 
                        new Vector3(-.998651f, .024846f, .045585f),
                        new Vector3(-.970656f, -.095312f, .220777f), 
                        new Vector3(.691479f, -.003309f, -.722389f),
                        new Vector3(-.139919f, -.011657f, -.990094f), 
                        new Vector3(.081148f, -.094109f, .992249f),
                        new Vector3(-.647788f, -.140477f, .748757f), 
                        new Vector3(-.842051f, -.054961f, -.53659f),
                        new Vector3(.757687f, -.012879f, .652491f), 
                        new Vector3(.998816f, .045111f, -.018183f),
                        new Vector3(-.676877f, -.016277f, -.7359161f), 
                        new Vector3(-.764426f, -.068986f, .64101f),
                        new Vector3(.8891f, -.05986f, -.453782f), 
                        new Vector3(-.997701f, -.064839f, -.019709f),
                        new Vector3(.365945f, -.091591f, .9261181f), 
                        new Vector3(.970442f, -.090459f, .223737f),
                        new Vector3(.139919f, -.011657f, -.990094f)
                    };
                mNormal = tempN;
                string[][] tempB =
                    {
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }
                    };
                mBoneName = tempB;
                uint[][] tempBH =
                    {
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }
                    };
                mBoneHash = tempBH;
                float[][] tempBW =
                    {
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }
                    };
                mBoneWeight = tempBW;
            }
            else if (string.Compare(ageGender, "CULOD1") == 0)
            {
                Vector3[] tempP =
                    {
                        new Vector3(.037082f, 1.12805f, -.040504f),
                        new Vector3(.023865f, 1.13131f, -.050662f), 
                        new Vector3(.044917f, 1.12229f, -.024376f),
                        new Vector3(.037069f, 1.11055f, .005334f), 
                        new Vector3(.04363f, 1.11633f, -.009510001f),
                        new Vector3(.026963f, 1.10279f, .020057f), 
                        new Vector3(.013509f, 1.09766f, .030423f),
                        new Vector3(0, 1.09573f, .033646f), 
                        new Vector3(0, 1.13223f, -.055818f),
                        new Vector3(-.037082f, 1.12805f, -.040504f), 
                        new Vector3(-.023865f, 1.13131f, -.050662f),
                        new Vector3(-.044917f, 1.12229f, -.024376f), 
                        new Vector3(-.037069f, 1.11055f, .005334f),
                        new Vector3(-.04363f, 1.11633f, -.009509f), 
                        new Vector3(-.013509f, 1.09766f, .030423f),
                        new Vector3(-.026963f, 1.10279f, .020057f), 
                        new Vector3(-.055903f, .766684f, -.060443f),
                        new Vector3(0, .759929f, .091257f), 
                        new Vector3(.031647f, .760024f, .088909f),
                        new Vector3(.090181f, .761507f, .058131f), 
                        new Vector3(.062994f, .760617f, .077069f),
                        new Vector3(.100971f, .763391f, .015379f), 
                        new Vector3(.08496901f, .765584f, -.03306f),
                        new Vector3(0, .766838f, -.064298f), 
                        new Vector3(.024194f, .766911f, -.066105f),
                        new Vector3(.055903f, .766684f, -.060443f), 
                        new Vector3(-.031647f, .760024f, .088909f),
                        new Vector3(-.062994f, .760617f, .077069f), 
                        new Vector3(-.090181f, .761507f, .058131f),
                        new Vector3(-.08496901f, .765584f, -.03306f), 
                        new Vector3(-.100971f, .763391f, .015379f),
                        new Vector3(-.024194f, .766911f, -.066105f), 
                        new Vector3(.099004f, .15644f, -.021123f),
                        new Vector3(.094136f, .156799f, .005412f), 
                        new Vector3(.076363f, .157381f, .018874f),
                        new Vector3(.039954f, .158111f, -.007452f), 
                        new Vector3(.053181f, .15762f, .013923f),
                        new Vector3(.045045f, .157046f, -.036487f), 
                        new Vector3(.068598f, .155829f, -.049488f),
                        new Vector3(.08888301f, .156153f, -.040943f), 
                        new Vector3(-.094136f, .156799f, .005412f),
                        new Vector3(-.099004f, .15644f, -.021123f), 
                        new Vector3(-.076363f, .157381f, .018874f),
                        new Vector3(-.053181f, .15762f, .013923f), 
                        new Vector3(-.039954f, .158111f, -.007452f),
                        new Vector3(-.068598f, .155829f, -.049488f), 
                        new Vector3(-.045045f, .157046f, -.036487f),
                        new Vector3(-.08888301f, .156153f, -.040943f)
                    };
                mPosition = tempP;
                Vector3[] tempN =
                    {
                        new Vector3(.680484f, -.347059f, -.645362f),
                        new Vector3(.345533f, -.412956f, -.842659f), 
                        new Vector3(.923126f, -.241612f, -.299103f),
                        new Vector3(.8849131f, -.367973f, .285524f), 
                        new Vector3(.968635f, -.22283f, .109965f),
                        new Vector3(.736877f, -.632269f, .239266f), 
                        new Vector3(.384592f, -.89435f, .228533f),
                        new Vector3(0, -.990734f, .135814f), 
                        new Vector3(0, -.434829f, -.900513f),
                        new Vector3(-.680488f, -.347058f, -.645358f), 
                        new Vector3(-.345538f, -.412956f, -.842657f),
                        new Vector3(-.923127f, -.241608f, -.299102f), 
                        new Vector3(-.8849131f, -.367978f, .285519f),
                        new Vector3(-.968635f, -.222833f, .109964f), 
                        new Vector3(-.384588f, -.8943521f, .228533f),
                        new Vector3(-.736873f, -.632273f, .239268f), 
                        new Vector3(-.434527f, .185612f, -.881325f),
                        new Vector3(0, -.083619f, .996498f), 
                        new Vector3(.213116f, -.07891901f, .973834f),
                        new Vector3(.818823f, .017723f, .573772f), 
                        new Vector3(.454008f, -.049001f, .889649f),
                        new Vector3(.996437f, .082346f, -.018212f), 
                        new Vector3(.8496801f, .132004f, -.510509f),
                        new Vector3(0, .222727f, -.974881f), 
                        new Vector3(.037308f, .213674f, -.976192f),
                        new Vector3(.434528f, .185612f, -.881325f), 
                        new Vector3(-.213116f, -.07891901f, .973834f),
                        new Vector3(-.454009f, -.049001f, .889649f), 
                        new Vector3(-.818823f, .017723f, .573772f),
                        new Vector3(-.849679f, .132004f, -.510509f), 
                        new Vector3(-.996437f, .082346f, -.018212f),
                        new Vector3(-.037308f, .213674f, -.976192f), 
                        new Vector3(.985398f, -.119768f, -.121024f),
                        new Vector3(.8348781f, -.12883f, .535146f), 
                        new Vector3(.228405f, -.141527f, .963224f),
                        new Vector3(-.960894f, -.13515f, .241696f), 
                        new Vector3(-.564119f, -.13015f, .815371f),
                        new Vector3(-.80319f, -.123556f, -.582769f), 
                        new Vector3(-.047441f, -.095994f, -.994251f),
                        new Vector3(.725233f, -.107829f, -.680007f), 
                        new Vector3(-.8348781f, -.12883f, .535146f),
                        new Vector3(-.985398f, -.119768f, -.121024f), 
                        new Vector3(-.228405f, -.141527f, .963224f),
                        new Vector3(.564119f, -.13015f, .815371f), 
                        new Vector3(.960894f, -.13515f, .241696f),
                        new Vector3(.047441f, -.095994f, -.994251f), 
                        new Vector3(.80319f, -.123556f, -.582769f),
                        new Vector3(-.725233f, -.107829f, -.680007f)
                    };
                mNormal = tempN;
                string[][] tempB =
                    {
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }
                    };
                mBoneName = tempB;
                uint[][] tempBH =
                    {
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }
                    };
                mBoneHash = tempBH;
                float[][] tempBW =
                    {
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }
                    };
                mBoneWeight = tempBW;
            }
            else if (string.Compare(ageGender, "TMLOD1") == 0)
            {
                Vector3[] tempP =
                    {
                        new Vector3(-3E-06f, 1.61974f, .037685f),
                        new Vector3(.016401f, 1.62196f, .034663f), 
                        new Vector3(.032187f, 1.62593f, .026397f),
                        new Vector3(.046979f, 1.6334f, .004885f), 
                        new Vector3(.055556f, 1.63956f, -.014215f),
                        new Vector3(.05586f, 1.64534f, -.037312f), 
                        new Vector3(.04318f, 1.64954f, -.060566f),
                        new Vector3(.024291f, 1.65081f, -.074342f), 
                        new Vector3(0, 1.65119f, -.079405f),
                        new Vector3(-.016401f, 1.62196f, .034663f), 
                        new Vector3(-.032187f, 1.62593f, .026397f),
                        new Vector3(-.046979f, 1.6334f, .004885f), 
                        new Vector3(-.055556f, 1.63956f, -.014215f),
                        new Vector3(-.05586f, 1.64534f, -.037312f), 
                        new Vector3(-.04318f, 1.64954f, -.060566f),
                        new Vector3(-.024291f, 1.65081f, -.074342f), 
                        new Vector3(.104781f, .181469f, .016774f),
                        new Vector3(.079704f, .182745f, .010168f), 
                        new Vector3(.060296f, .183244f, -.020427f),
                        new Vector3(.0778f, .178894f, -.065852f), 
                        new Vector3(.118049f, .178676f, -.06388f),
                        new Vector3(.099726f, .177627f, -.075658f), 
                        new Vector3(.130194f, .179379f, -.036612f),
                        new Vector3(.126548f, .18027f, -.000446f), 
                        new Vector3(0, 1.09736f, .136961f),
                        new Vector3(.030364f, 1.09741f, .134171f), 
                        new Vector3(.140732f, 1.11559f, .007232f),
                        new Vector3(.139733f, 1.11151f, .043776f), 
                        new Vector3(.123358f, 1.10531f, .080452f),
                        new Vector3(.092409f, 1.10131f, .108346f), 
                        new Vector3(.125759f, 1.11838f, -.030516f),
                        new Vector3(.101336f, 1.12057f, -.061763f), 
                        new Vector3(.013782f, 1.1209f, -.08090501f),
                        new Vector3(.054711f, 1.12086f, -.079611f), 
                        new Vector3(0, 1.1209f, -.079728f),
                        new Vector3(.06224401f, 1.10002f, .124916f), 
                        new Vector3(-.030364f, 1.09741f, .134171f),
                        new Vector3(-.140732f, 1.11559f, .007232f), 
                        new Vector3(-.139733f, 1.11151f, .043776f),
                        new Vector3(-.123358f, 1.10531f, .080452f), 
                        new Vector3(-.092409f, 1.10131f, .108346f),
                        new Vector3(-.125759f, 1.11838f, -.030516f), 
                        new Vector3(-.101336f, 1.12057f, -.061763f),
                        new Vector3(-.013782f, 1.1209f, -.08090501f), 
                        new Vector3(-.054711f, 1.12086f, -.079611f),
                        new Vector3(-.06224401f, 1.10002f, .124916f), 
                        new Vector3(-.104781f, .181469f, .016774f),
                        new Vector3(-.079704f, .182745f, .010168f), 
                        new Vector3(-.060296f, .183244f, -.020427f),
                        new Vector3(-.0778f, .178894f, -.065852f), 
                        new Vector3(-.099726f, .177627f, -.075658f),
                        new Vector3(-.118049f, .178676f, -.06388f), 
                        new Vector3(-.130194f, .179379f, -.036612f),
                        new Vector3(-.126548f, .18027f, -.000446f)
                    };
                mPosition = tempP;
                Vector3[] tempN =
                    {
                        new Vector3(-8.4E-05f, -.958624f, .284674f),
                        new Vector3(.372828f, -.8453171f, .382672f), 
                        new Vector3(.710741f, -.630516f, .311924f),
                        new Vector3(.900473f, -.287963f, .325923f), 
                        new Vector3(.982927f, -.161869f, .087483f),
                        new Vector3(.950873f, -.092125f, -.295557f), 
                        new Vector3(.737141f, -.054635f, -.673526f),
                        new Vector3(.393907f, -.024228f, -.9188311f), 
                        new Vector3(-6E-06f, -.022177f, -.999754f),
                        new Vector3(-.372901f, -.845277f, .382691f), 
                        new Vector3(-.71074f, -.630515f, .311929f),
                        new Vector3(-.900473f, -.287963f, .325922f), 
                        new Vector3(-.982927f, -.161868f, .087483f),
                        new Vector3(-.950873f, -.092125f, -.295557f), 
                        new Vector3(-.7371401f, -.054635f, -.673527f),
                        new Vector3(-.393912f, -.024218f, -.9188291f), 
                        new Vector3(.231365f, -.04927f, .971618f),
                        new Vector3(-.550068f, -.041709f, .8340771f), 
                        new Vector3(-.989961f, -.063871f, .126086f),
                        new Vector3(-.711241f, -.110769f, -.694166f), 
                        new Vector3(.740865f, -.088425f, -.665807f),
                        new Vector3(.0573f, -.109279f, -.992358f), 
                        new Vector3(.9872f, -.075213f, -.140639f),
                        new Vector3(.872185f, -.054638f, .486115f), 
                        new Vector3(1.2E-05f, -.08076f, .996733f),
                        new Vector3(.197226f, -.087136f, .976478f), 
                        new Vector3(.978365f, .142778f, -.149722f),
                        new Vector3(.9598801f, .133532f, .246577f), 
                        new Vector3(.757369f, .071106f, .649104f),
                        new Vector3(.584593f, -.027064f, .810875f), 
                        new Vector3(.865046f, .171027f, -.47164f),
                        new Vector3(.580825f, .230373f, -.78075f), 
                        new Vector3(-.064049f, .229331f, -.971239f),
                        new Vector3(.173555f, .244702f, -.953939f), 
                        new Vector3(3.7E-05f, .222686f, -.97489f),
                        new Vector3(.381145f, -.08237f, .9208381f), 
                        new Vector3(-.197214f, -.08712801f, .9764811f),
                        new Vector3(-.978365f, .142778f, -.149722f), 
                        new Vector3(-.9598801f, .133532f, .246577f),
                        new Vector3(-.757369f, .071106f, .649103f), 
                        new Vector3(-.584593f, -.027064f, .810875f),
                        new Vector3(-.865046f, .171027f, -.47164f), 
                        new Vector3(-.580824f, .230373f, -.78075f),
                        new Vector3(.06408601f, .229343f, -.9712331f), 
                        new Vector3(-.173555f, .244702f, -.953939f),
                        new Vector3(-.381145f, -.08237f, .9208381f), 
                        new Vector3(-.231365f, -.04927f, .971618f),
                        new Vector3(.550068f, -.041709f, .8340771f), 
                        new Vector3(.989961f, -.063871f, .126086f),
                        new Vector3(.711241f, -.110769f, -.694166f), 
                        new Vector3(-.0573f, -.109279f, -.992358f),
                        new Vector3(-.740865f, -.088426f, -.665807f), 
                        new Vector3(-.9872f, -.075213f, -.140639f),
                        new Vector3(-.872185f, -.054638f, .486115f)
                    };
                mNormal = tempN;
                string[][] tempB =
                    {
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }
                    };
                mBoneName = tempB;
                uint[][] tempBH =
                    {
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }
                    };
                mBoneHash = tempBH;
                float[][] tempBW =
                    {
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .7499999f,
                            .25f,
                            0,
                            0
                        }
                    };
                mBoneWeight = tempBW;
            }
            else if (string.Compare(ageGender, "TMLOD2") == 0)
            {
                Vector3[] tempP =
                    {
                        new Vector3(.05586f, 1.64498f, -.038803f),
                        new Vector3(1.9E-05f, 1.65119f, -.078971f), 
                        new Vector3(.024291f, 1.65072f, -.074362f),
                        new Vector3(.016401f, 1.62169f, .033673f), 
                        new Vector3(0, 1.61954f, .037085f),
                        new Vector3(.032188f, 1.62531f, .023533f), 
                        new Vector3(.04698f, 1.63262f, .001192f),
                        new Vector3(-.024272f, 1.65049f, -.075583f), 
                        new Vector3(-.055841f, 1.64534f, -.036878f),
                        new Vector3(-.046961f, 1.63263f, .001279f), 
                        new Vector3(-.03217001f, 1.62525f, .023232f),
                        new Vector3(-.016382f, 1.62169f, .033673f), 
                        new Vector3(.07657801f, .182745f, .009359f),
                        new Vector3(.103573f, .181468f, .015965f), 
                        new Vector3(.058715f, .183244f, -.021236f),
                        new Vector3(.074748f, .178894f, -.06774001f), 
                        new Vector3(.115736f, .178676f, -.065768f),
                        new Vector3(.098576f, .177627f, -.078626f), 
                        new Vector3(.127642f, .179379f, -.037421f),
                        new Vector3(.124067f, .18027f, -.001255f), 
                        new Vector3(.141407f, 1.11154f, .043977f),
                        new Vector3(.055366f, 1.121f, -.080893f), 
                        new Vector3(.127266f, 1.1185f, -.03121f),
                        new Vector3(.10255f, 1.12071f, -.062831f), 
                        new Vector3(.142418f, 1.11567f, .006996f),
                        new Vector3(.124837f, 1.10527f, .081095f), 
                        new Vector3(-1.6E-05f, 1.09721f, .138278f),
                        new Vector3(.030728f, 1.09727f, .135455f), 
                        new Vector3(.09351601f, 1.10122f, .109323f),
                        new Vector3(-1.6E-05f, 1.12105f, -.081005f), 
                        new Vector3(-.030727f, 1.09727f, .135455f),
                        new Vector3(-.09351601f, 1.10122f, .109323f), 
                        new Vector3(-.055366f, 1.121f, -.080893f),
                        new Vector3(-.142418f, 1.11567f, .006996f), 
                        new Vector3(-.127266f, 1.1185f, -.03121f),
                        new Vector3(-.124837f, 1.10527f, .081095f), 
                        new Vector3(-.141407f, 1.11154f, .043977f),
                        new Vector3(-.10255f, 1.12071f, -.062831f), 
                        new Vector3(-.127642f, .179379f, -.037421f),
                        new Vector3(-.103573f, .181468f, .015965f), 
                        new Vector3(-.07657801f, .182745f, .009359f),
                        new Vector3(-.058715f, .183244f, -.021236f), 
                        new Vector3(-.074748f, .178894f, -.06774001f),
                        new Vector3(-.098576f, .177627f, -.078626f), 
                        new Vector3(-.115736f, .178676f, -.065768f),
                        new Vector3(-.124067f, .18027f, -.001255f)
                    };
                mPosition = tempP;
                Vector3[] tempN =
                    {
                        new Vector3(.970354f, -.086099f, -.225829f),
                        new Vector3(.012057f, .058264f, -.998228f), 
                        new Vector3(.48151f, .073877f, -.873321f),
                        new Vector3(.411896f, -.596287f, .6890451f), 
                        new Vector3(-.009787f, -.754792f, .655892f),
                        new Vector3(.7823681f, -.288501f, .551967f), 
                        new Vector3(.974071f, -.134416f, .181985f),
                        new Vector3(-.476933f, .06613f, -.876448f), 
                        new Vector3(-.972496f, -.076953f, -.219838f),
                        new Vector3(-.975046f, -.110281f, .192672f), 
                        new Vector3(-.79831f, -.260087f, .54319f),
                        new Vector3(-.36935f, -.419857f, .829036f), 
                        new Vector3(-.537305f, -.042398f, .8423221f),
                        new Vector3(.245816f, -.027222f, .968934f), 
                        new Vector3(-.984544f, -.090375f, .150018f),
                        new Vector3(-.7254111f, -.11686f, -.678323f), 
                        new Vector3(.777585f, -.08394401f, -.623149f),
                        new Vector3(.058185f, -.093447f, -.993923f), 
                        new Vector3(.987937f, -.066375f, -.139907f),
                        new Vector3(.881972f, -.037315f, .469821f), 
                        new Vector3(.973405f, .083741f, .213239f),
                        new Vector3(.108362f, .177358f, -.978162f), 
                        new Vector3(.882184f, .109751f, -.457936f),
                        new Vector3(.561497f, .149884f, -.813791f), 
                        new Vector3(.9875461f, .080759f, -.135024f),
                        new Vector3(.753185f, .050976f, .655831f), 
                        new Vector3(-5.2E-05f, -.111904f, .993719f),
                        new Vector3(.284618f, -.079685f, .955323f), 
                        new Vector3(.5434381f, -.032942f, .838803f),
                        new Vector3(.041023f, .159128f, -.986405f), 
                        new Vector3(-.284658f, -.079678f, .955312f),
                        new Vector3(-.5434381f, -.032936f, .838803f), 
                        new Vector3(-.120274f, .162696f, -.979318f),
                        new Vector3(-.98042f, .115541f, -.159458f), 
                        new Vector3(-.884659f, .127978f, -.44833f),
                        new Vector3(-.753185f, .050976f, .655831f), 
                        new Vector3(-.97486f, .08863f, .204431f),
                        new Vector3(-.5615f, .149883f, -.8137891f), 
                        new Vector3(-.989418f, -.055168f, -.134197f),
                        new Vector3(-.246365f, -.026966f, .968802f), 
                        new Vector3(.536884f, -.042591f, .84258f),
                        new Vector3(.984334f, -.091316f, .150822f), 
                        new Vector3(.725077f, -.117648f, -.678545f),
                        new Vector3(-.059423f, -.093691f, -.993826f), 
                        new Vector3(-.776492f, -.077672f, -.625322f),
                        new Vector3(-.8797581f, -.033082f, .474269f)
                    };
                mNormal = tempN;
                string[][] tempB =
                    {
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }
                    };
                mBoneName = tempB;
                uint[][] tempBH =
                    {
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }
                    };
                mBoneHash = tempBH;
                float[][] tempBW =
                    {
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }
                    };
                mBoneWeight = tempBW;
            }
            else if (string.Compare(ageGender, "TMLOD3") == 0)
            {
                Vector3[] tempP =
                    {
                        new Vector3(.04318f, 1.64954f, -.060132f),
                        new Vector3(1.9E-05f, 1.65119f, -.078971f), 
                        new Vector3(.046883f, 1.63336f, .005422f),
                        new Vector3(0, 1.61974f, .038119f), 
                        new Vector3(-.043161f, 1.64954f, -.060132f),
                        new Vector3(-.046865f, 1.63336f, .005422f), 
                        new Vector3(.113796f, .180885f, .007345f),
                        new Vector3(.07657801f, .182745f, .009359f), 
                        new Vector3(.058715f, .183244f, -.021236f),
                        new Vector3(.074748f, .178894f, -.06774f), 
                        new Vector3(.107147f, .17816f, -.07218f),
                        new Vector3(.127642f, .179379f, -.037421f), 
                        new Vector3(.109125f, 1.10328f, .095175f),
                        new Vector3(.141407f, 1.11154f, .043977f), 
                        new Vector3(-1.6E-05f, 1.12105f, -.081005f),
                        new Vector3(.055366f, 1.121f, -.080893f), 
                        new Vector3(.10255f, 1.12071f, -.062831f),
                        new Vector3(.127266f, 1.1185f, -.03121f), 
                        new Vector3(.142418f, 1.11567f, .006996f),
                        new Vector3(.030728f, 1.09727f, .135455f), 
                        new Vector3(-.055366f, 1.121f, -.080893f),
                        new Vector3(-.10255f, 1.12071f, -.062831f), 
                        new Vector3(-.030727f, 1.09727f, .135455f),
                        new Vector3(-.109125f, 1.10328f, .095172f), 
                        new Vector3(-.142418f, 1.11567f, .006996f),
                        new Vector3(-.127266f, 1.1185f, -.03121f), 
                        new Vector3(-.141407f, 1.11154f, .043977f),
                        new Vector3(-.113796f, .180885f, .007345f), 
                        new Vector3(-.127642f, .179379f, -.037421f),
                        new Vector3(-.107147f, .17816f, -.07218f), 
                        new Vector3(-.074748f, .178894f, -.06774f),
                        new Vector3(-.058715f, .183244f, -.021236f), 
                        new Vector3(-.07657801f, .182745f, .009359f)
                    };
                mPosition = tempP;
                Vector3[] tempN =
                    {
                        new Vector3(.847873f, .239577f, -.472985f),
                        new Vector3(.019774f, .243614f, -.96967f), 
                        new Vector3(.872253f, .09627f, .479486f),
                        new Vector3(-4.8E-05f, -.146386f, .989227f), 
                        new Vector3(-.772506f, .272198f, -.5737101f),
                        new Vector3(-.871677f, .097383f, .480308f), 
                        new Vector3(.48611f, -.052426f, .872323f),
                        new Vector3(-.486869f, -.042079f, .872461f), 
                        new Vector3(-.984399f, -.083306f, .154977f),
                        new Vector3(-.74127f, -.074468f, -.667063f), 
                        new Vector3(.499054f, -.019895f, -.866342f),
                        new Vector3(.995042f, -.058228f, -.080624f), 
                        new Vector3(.623569f, -.007091f, .781736f),
                        new Vector3(.960237f, .03672f, .27676f), 
                        new Vector3(.000417f, .070272f, -.997528f),
                        new Vector3(.139867f, .057758f, -.988484f), 
                        new Vector3(.616095f, .101822f, -.781063f),
                        new Vector3(.863791f, .063345f, -.499851f), 
                        new Vector3(.981219f, .069108f, -.18009f),
                        new Vector3(.284773f, -.079329f, .955307f), 
                        new Vector3(-.139808f, .057599f, -.988502f),
                        new Vector3(-.577201f, .086049f, -.812056f), 
                        new Vector3(-.238538f, -.08225201f, .967643f),
                        new Vector3(-.59139f, -.012066f, .806296f), 
                        new Vector3(-.998455f, .041141f, -.037351f),
                        new Vector3(-.853558f, .039206f, -.51952f), 
                        new Vector3(-.956494f, .041583f, .288774f),
                        new Vector3(-.516952f, -.052924f, .854377f), 
                        new Vector3(-.988768f, -.076827f, -.1282f),
                        new Vector3(-.418126f, -.068249f, -.905822f), 
                        new Vector3(.695477f, -.128762f, -.706918f),
                        new Vector3(.978676f, -.092803f, .183252f), 
                        new Vector3(.400228f, -.01606f, .916275f)
                    };
                mNormal = tempN;
                string[][] tempB =
                    {
                        new string[]
                        {
                            "b__Neck__",
                            "b__HeadNew__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__HeadNew__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__HeadNew__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__HeadNew__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__HeadNew__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__HeadNew__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }
                    };
                mBoneName = tempB;
                uint[][] tempBH =
                    {
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x6E191ACB,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x6E191ACB,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x6E191ACB,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x6E191ACB,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x6E191ACB,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x6E191ACB,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }
                    };
                mBoneHash = tempBH;
                float[][] tempBW =
                    {
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }
                    };
                mBoneWeight = tempBW;
            }
            else if (string.Compare(ageGender, "AMLOD1") == 0)
            {
                Vector3[] tempP =
                    {
                        new Vector3(.059855f, 1.64023f, -.012834f),
                        new Vector3(0, 1.61707f, .042132f), 
                        new Vector3(.016347f, 1.61974f, .03968f),
                        new Vector3(.035934f, 1.62535f, .028437f), 
                        new Vector3(.050681f, 1.63424f, .006397001f),
                        new Vector3(.060255f, 1.64644f, -.038073f), 
                        new Vector3(.047316f, 1.65059f, -.06074701f),
                        new Vector3(.026536f, 1.65177f, -.072129f), 
                        new Vector3(0, 1.65163f, -.077454f),
                        new Vector3(-.016347f, 1.61974f, .03968f), 
                        new Vector3(-.035934f, 1.62535f, .028437f),
                        new Vector3(-.050681f, 1.63424f, .006397001f), 
                        new Vector3(-.059855f, 1.64023f, -.012834f),
                        new Vector3(-.060255f, 1.64644f, -.038073f), 
                        new Vector3(-.047316f, 1.65059f, -.06074701f),
                        new Vector3(-.026536f, 1.65177f, -.072129f), 
                        new Vector3(.106824f, .185431f, .015891f),
                        new Vector3(.08201401f, .186621f, .009767f), 
                        new Vector3(.064931f, .186825f, -.018886f),
                        new Vector3(.080239f, .1832f, -.064115f), 
                        new Vector3(.117716f, .183158f, -.061931f),
                        new Vector3(.102342f, .182097f, -.074045f), 
                        new Vector3(.129913f, .183709f, -.034662f),
                        new Vector3(.124909f, .18437f, -.000229f), 
                        new Vector3(0, 1.09644f, .123577f),
                        new Vector3(.029261f, 1.09722f, .119515f), 
                        new Vector3(.142481f, 1.11602f, -.0007560001f),
                        new Vector3(.141469f, 1.11189f, .036236f), 
                        new Vector3(.124894f, 1.10561f, .073364f),
                        new Vector3(.093564f, 1.10156f, .095144f), 
                        new Vector3(.127324f, 1.11885f, -.038975f),
                        new Vector3(.1026f, 1.12106f, -.068635f), 
                        new Vector3(.055402f, 1.12135f, -.086702f),
                        new Vector3(.01397f, 1.12139f, -.088012f), 
                        new Vector3(0, 1.1214f, -.086814f),
                        new Vector3(.060008f, 1.09925f, .107843f), 
                        new Vector3(-.029261f, 1.09722f, .119515f),
                        new Vector3(-.142481f, 1.11602f, -.000756f), 
                        new Vector3(-.141469f, 1.11189f, .036236f),
                        new Vector3(-.124894f, 1.10561f, .073364f), 
                        new Vector3(-.093564f, 1.10156f, .095144f),
                        new Vector3(-.127324f, 1.11885f, -.038975f), 
                        new Vector3(-.1026f, 1.12106f, -.068635f),
                        new Vector3(-.01397f, 1.12139f, -.088012f), 
                        new Vector3(-.055402f, 1.12135f, -.086702f),
                        new Vector3(-.060008f, 1.09925f, .107844f), 
                        new Vector3(-.106824f, .185431f, .015891f),
                        new Vector3(-.08201401f, .186621f, .009767f), 
                        new Vector3(-.064931f, .186825f, -.018886f),
                        new Vector3(-.080239f, .1832f, -.064115f), 
                        new Vector3(-.102342f, .182097f, -.074045f),
                        new Vector3(-.117716f, .183158f, -.061931f), 
                        new Vector3(-.129913f, .183709f, -.034662f),
                        new Vector3(-.124909f, .18437f, -.000229f)
                    };
                mPosition = tempP;
                Vector3[] tempN =
                    {
                        new Vector3(.983924f, -.117732f, .134287f),
                        new Vector3(0, -.928537f, .371238f), 
                        new Vector3(.41075f, -.8029851f, .431856f),
                        new Vector3(.745924f, -.5387031f, .391659f), 
                        new Vector3(.903421f, -.224648f, .365188f),
                        new Vector3(.9544f, -.040793f, -.29573f), 
                        new Vector3(.692729f, -.011033f, -.721114f),
                        new Vector3(.318631f, .017083f, -.947725f), 
                        new Vector3(0, -.001298f, -.999999f),
                        new Vector3(-.410748f, -.802983f, .431862f), 
                        new Vector3(-.745924f, -.538702f, .39166f),
                        new Vector3(-.903421f, -.224648f, .365191f), 
                        new Vector3(-.983924f, -.117732f, .134287f),
                        new Vector3(-.9544f, -.040793f, -.29573f), 
                        new Vector3(-.692728f, -.011033f, -.721114f),
                        new Vector3(-.318631f, .017083f, -.947725f), 
                        new Vector3(.223485f, -.056773f, .973052f),
                        new Vector3(-.558042f, -.030492f, .829252f), 
                        new Vector3(-.989261f, -.058075f, .134126f),
                        new Vector3(-.714209f, -.097744f, -.693074f), 
                        new Vector3(.747516f, -.106964f, -.655574f),
                        new Vector3(.075609f, -.110545f, -.990991f), 
                        new Vector3(.988306f, -.086328f, -.125696f),
                        new Vector3(.872185f, -.079367f, .482695f), 
                        new Vector3(0, -.014489f, .999895f),
                        new Vector3(.239992f, -.011491f, .9707071f), 
                        new Vector3(.983418f, .097537f, -.15289f),
                        new Vector3(.964854f, .11581f, .235893f), 
                        new Vector3(.717207f, .118146f, .686771f),
                        new Vector3(.448916f, .06612f, .891124f), 
                        new Vector3(.867139f, .123324f, -.482556f),
                        new Vector3(.578218f, .19939f, -.7911431f), 
                        new Vector3(.177612f, .229325f, -.9570081f),
                        new Vector3(-.138265f, .243019f, -.960117f), 
                        new Vector3(0, .221562f, -.975146f),
                        new Vector3(.331916f, .015795f, .943177f), 
                        new Vector3(-.239985f, -.011491f, .9707081f),
                        new Vector3(-.983418f, .097537f, -.15289f), 
                        new Vector3(-.964854f, .115809f, .235892f),
                        new Vector3(-.717207f, .118145f, .686772f), 
                        new Vector3(-.448923f, .06612f, .891121f),
                        new Vector3(-.867139f, .123324f, -.482557f), 
                        new Vector3(-.578218f, .19939f, -.7911431f),
                        new Vector3(.138267f, .243019f, -.960117f), 
                        new Vector3(-.177612f, .229325f, -.9570081f),
                        new Vector3(-.331917f, .015795f, .943176f), 
                        new Vector3(-.223485f, -.056773f, .973052f),
                        new Vector3(.558042f, -.030492f, .829252f), 
                        new Vector3(.989261f, -.058075f, .134126f),
                        new Vector3(.714209f, -.097744f, -.693074f), 
                        new Vector3(-.075609f, -.110545f, -.990991f),
                        new Vector3(-.747516f, -.106964f, -.655574f), 
                        new Vector3(-.988306f, -.086328f, -.125695f),
                        new Vector3(-.872185f, -.079367f, .482695f)
                    };
                mNormal = tempN;
                string[][] tempB =
                    {
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Pelvis__",
                            "b__Spine0__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }
                    };
                mBoneName = tempB;
                uint[][] tempBH =
                    {
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x556B181A,
                            0x6FA96266,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }
                    };
                mBoneHash = tempBH;
                float[][] tempBW =
                    {
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }
                    };
                mBoneWeight = tempBW;
            }
            else if (string.Compare(ageGender, "AMLOD2") == 0)
            {
                Vector3[] tempP =
                    {
                        new Vector3(.059625f, 1.64603f, -.039223f),
                        new Vector3(.026515f, 1.65169f, -.07213901f), 
                        new Vector3(.016757f, 1.61933f, .039248f),
                        new Vector3(-.000122f, 1.61676f, .042038f), 
                        new Vector3(.03728f, 1.6248f, .026519f),
                        new Vector3(.052022f, 1.63337f, .003321f), 
                        new Vector3(8.2E-05f, 1.65163f, -.077434f),
                        new Vector3(-.026345f, 1.65133f, -.072205f), 
                        new Vector3(-.060248f, 1.64634f, -.037663f),
                        new Vector3(-.052004f, 1.63339f, .0034f), 
                        new Vector3(-.037435f, 1.62475f, .026325f),
                        new Vector3(-.016764f, 1.61933f, .039256f), 
                        new Vector3(.082015f, .186621f, .009767f),
                        new Vector3(.106824f, .185431f, .015891f), 
                        new Vector3(.064931f, .186825f, -.018886f),
                        new Vector3(.08024f, .1832f, -.064115f), 
                        new Vector3(.117716f, .183158f, -.061931f),
                        new Vector3(.102342f, .182097f, -.074046f), 
                        new Vector3(.129913f, .183709f, -.034662f),
                        new Vector3(.124909f, .18437f, -.00023f), 
                        new Vector3(.138542f, 1.11165f, .042636f),
                        new Vector3(.057321f, 1.12176f, -.085933f), 
                        new Vector3(.129978f, 1.11885f, -.03228f),
                        new Vector3(.105441f, 1.12106f, -.065227f), 
                        new Vector3(.142226f, 1.116f, .006982f),
                        new Vector3(.121292f, 1.10524f, .075861f), 
                        new Vector3(0, 1.09644f, .123577f),
                        new Vector3(.027615f, 1.09717f, .119743f), 
                        new Vector3(.089572f, 1.10136f, .096654f),
                        new Vector3(0, 1.1221f, -.086729f), 
                        new Vector3(-.027615f, 1.09717f, .119743f),
                        new Vector3(-.089571f, 1.10136f, .096654f), 
                        new Vector3(-.057321f, 1.12176f, -.085933f),
                        new Vector3(-.142226f, 1.116f, .006982f), 
                        new Vector3(-.129978f, 1.11885f, -.03228001f),
                        new Vector3(-.121292f, 1.10524f, .075861f), 
                        new Vector3(-.138542f, 1.11165f, .042636f),
                        new Vector3(-.105441f, 1.12106f, -.065227f), 
                        new Vector3(-.106824f, .185431f, .015891f),
                        new Vector3(-.082015f, .186621f, .009767f), 
                        new Vector3(-.064931f, .186825f, -.018886f),
                        new Vector3(-.08024f, .1832f, -.064115f), 
                        new Vector3(-.102342f, .182097f, -.074046f),
                        new Vector3(-.117716f, .183158f, -.061931f), 
                        new Vector3(-.129913f, .183709f, -.034662f),
                        new Vector3(-.124909f, .18437f, -.00023f)
                    };
                mPosition = tempP;
                Vector3[] tempN =
                    {
                        new Vector3(.961225f, .045321f, -.272014f),
                        new Vector3(.40141f, .16378f, -.901136f), 
                        new Vector3(.413822f, -.630142f, .6570181f),
                        new Vector3(-.007618f, -.775371f, .63146f), 
                        new Vector3(.756307f, -.427702f, .495046f),
                        new Vector3(.969131f, -.132223f, .20809f), 
                        new Vector3(.000283f, .114181f, -.99346f),
                        new Vector3(-.401879f, .163813f, -.900921f), 
                        new Vector3(-.964716f, .03065f, -.261503f),
                        new Vector3(-.967484f, -.139895f, .21072f), 
                        new Vector3(-.761156f, -.422097f, .492418f),
                        new Vector3(-.425292f, -.518403f, .741879f), 
                        new Vector3(-.535535f, -.03055f, .84396f),
                        new Vector3(.232165f, -.037004f, .971972f), 
                        new Vector3(-.984424f, -.076715f, .15819f),
                        new Vector3(-.720797f, -.088778f, -.687437f), 
                        new Vector3(.772631f, -.093266f, -.627967f),
                        new Vector3(.058584f, -.083871f, -.994753f), 
                        new Vector3(.989352f, -.07835101f, -.122649f),
                        new Vector3(.8802871f, -.064184f, .47008f), 
                        new Vector3(.956862f, .099878f, .272836f),
                        new Vector3(.141218f, .165211f, -.976096f), 
                        new Vector3(.899014f, .089207f, -.428737f),
                        new Vector3(.600893f, .125431f, -.789427f), 
                        new Vector3(.9952061f, .073026f, -.06506f),
                        new Vector3(.680461f, .112155f, .7241501f), 
                        new Vector3(0, -.016057f, .999871f),
                        new Vector3(.26851f, .015254f, .963156f), 
                        new Vector3(.433182f, .06799801f, .8987371f),
                        new Vector3(.023115f, .168977f, -.985349f), 
                        new Vector3(-.26851f, .015254f, .963156f),
                        new Vector3(-.433182f, .06799801f, .8987381f), 
                        new Vector3(-.146483f, .158241f, -.976474f),
                        new Vector3(-.991646f, .102549f, -.078237f), 
                        new Vector3(-.904783f, .109092f, -.411663f),
                        new Vector3(-.6804621f, .112155f, .7241501f), 
                        new Vector3(-.958944f, .10174f, .264715f),
                        new Vector3(-.600867f, .125414f, -.789449f), 
                        new Vector3(-.232165f, -.037004f, .971972f),
                        new Vector3(.535535f, -.03055f, .84396f), 
                        new Vector3(.984424f, -.076715f, .15819f),
                        new Vector3(.720797f, -.088778f, -.687437f), 
                        new Vector3(-.058584f, -.083871f, -.994753f),
                        new Vector3(-.770384f, -.087547f, -.631541f), 
                        new Vector3(-.990551f, -.069428f, -.118269f),
                        new Vector3(-.878299f, -.061645f, .474122f)
                    };
                mNormal = tempN;
                string[][] tempB =
                    {
                        new string[]
                        {
                            "b__Head__",
                            "b__HeadDome__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Head__",
                            "b__HeadDome__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Head__",
                            "b__HeadDome__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Head__",
                            "b__HeadDome__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Head__",
                            "b__HeadDome__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Head__",
                            "b__HeadDome__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Head__",
                            "b__HeadDome__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Head__",
                            "b__HeadDome__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Head__",
                            "b__HeadDome__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Head__",
                            "b__HeadDome__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Head__",
                            "b__HeadDome__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Head__",
                            "b__HeadDome__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__NoseTip__",
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__NoseTip__",
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__NoseTip__",
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__NoseTip__",
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__NoseTip__",
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__NoseTip__",
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__NoseTip__",
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__NoseTip__",
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__MouthArea__",
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__MouthArea__",
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__MouthArea__",
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__MouthArea__",
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__MouthArea__",
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__MouthArea__",
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__MouthArea__",
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__MouthArea__",
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }
                    };
                mBoneName = tempB;
                uint[][] tempBH =
                    {
                        new uint[]
                        {
                            0x0F97B21B,
                            0x9D5460CE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x0F97B21B,
                            0x9D5460CE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x0F97B21B,
                            0x9D5460CE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x0F97B21B,
                            0x9D5460CE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x0F97B21B,
                            0x9D5460CE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x0F97B21B,
                            0x9D5460CE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x0F97B21B,
                            0x9D5460CE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x0F97B21B,
                            0x9D5460CE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x0F97B21B,
                            0x9D5460CE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x0F97B21B,
                            0x9D5460CE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x0F97B21B,
                            0x9D5460CE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x0F97B21B,
                            0x9D5460CE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x344AF043,
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x344AF043,
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x344AF043,
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x344AF043,
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x344AF043,
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x344AF043,
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x344AF043,
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x344AF043,
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x78894999,
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x78894999,
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x78894999,
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x78894999,
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x78894999,
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x78894999,
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x78894999,
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x78894999,
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9
                        }
                    };
                mBoneHash = tempBH;
                float[][] tempBW =
                    {
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }
                    };
                mBoneWeight = tempBW;
            }
            else if (string.Compare(ageGender, "AMLOD3") == 0)
            {
                Vector3[] tempP =
                    {
                        new Vector3(.047298f, 1.65059f, -.060313f),
                        new Vector3(0, 1.65163f, -.07702f), 
                        new Vector3(.050566f, 1.6342f, .006934f),
                        new Vector3(0, 1.61706f, .042567f), 
                        new Vector3(-.047298f, 1.65059f, -.060313f),
                        new Vector3(-.050566f, 1.6342f, .006934f), 
                        new Vector3(.115866f, .1849f, .007831f),
                        new Vector3(.082015f, .186621f, .009767f), 
                        new Vector3(.064931f, .186825f, -.018886f),
                        new Vector3(.08024f, .1832f, -.064115f), 
                        new Vector3(.110029f, .182627f, -.067988f),
                        new Vector3(.129913f, .183709f, -.034662f), 
                        new Vector3(.142272f, 1.11516f, .00689f),
                        new Vector3(.138653f, 1.11083f, .042546f), 
                        new Vector3(.10463f, 1.10299f, .087451f),
                        new Vector3(0, 1.1214f, -.086814f), 
                        new Vector3(.05733f, 1.12134f, -.085964f),
                        new Vector3(.105432f, 1.12081f, -.065238f), 
                        new Vector3(.129966f, 1.11835f, -.032312f),
                        new Vector3(.027615f, 1.09717f, .119743f), 
                        new Vector3(-.05733f, 1.12134f, -.085964f),
                        new Vector3(-.105432f, 1.12081f, -.065238f), 
                        new Vector3(-.027615f, 1.09717f, .119743f),
                        new Vector3(-.10463f, 1.10299f, .087451f), 
                        new Vector3(-.142272f, 1.11516f, .00689f),
                        new Vector3(-.129966f, 1.11835f, -.032312f), 
                        new Vector3(-.138653f, 1.11083f, .042546f),
                        new Vector3(-.115866f, .1849f, .007831f), 
                        new Vector3(-.129913f, .183709f, -.034662f),
                        new Vector3(-.110029f, .182627f, -.067988f), 
                        new Vector3(-.08024f, .1832f, -.064115f),
                        new Vector3(-.064931f, .186825f, -.018886f), 
                        new Vector3(-.082015f, .186621f, .009767f)
                    };
                mPosition = tempP;
                Vector3[] tempN =
                    {
                        new Vector3(.845285f, .233816f, -.48044f),
                        new Vector3(.01893f, .235425f, -.971708f), 
                        new Vector3(.867234f, -.011728f, .497763f),
                        new Vector3(0, -.324329f, .945944f), 
                        new Vector3(-.760354f, .280613f, -.585762f),
                        new Vector3(-.866914f, -.011236f, .498331f), 
                        new Vector3(.4669991f, -.075236f, .8810511f),
                        new Vector3(-.480238f, -.031368f, .8765771f), 
                        new Vector3(-.982998f, -.079803f, .165364f),
                        new Vector3(-.737123f, -.056828f, -.673365f), 
                        new Vector3(.502045f, -.024514f, -.864494f),
                        new Vector3(.995165f, -.07114f, -.067713f), 
                        new Vector3(.9898f, .07218f, -.122826f),
                        new Vector3(.932499f, .045287f, .358321f), 
                        new Vector3(.557204f, .037045f, .829548f),
                        new Vector3(.00035f, .086591f, -.996244f), 
                        new Vector3(.151156f, .078383f, -.985397f),
                        new Vector3(.6401221f, .098766f, -.761898f), 
                        new Vector3(.886464f, .058293f, -.459112f),
                        new Vector3(.261532f, -.056021f, .963567f), 
                        new Vector3(-.151203f, .078335f, -.985394f),
                        new Vector3(-.603393f, .085748f, -.79282f), 
                        new Vector3(-.217358f, -.054357f, .9745771f),
                        new Vector3(-.509608f, .032941f, .859776f), 
                        new Vector3(-.998772f, .045617f, .019332f),
                        new Vector3(-.867558f, .038816f, -.495819f), 
                        new Vector3(-.928322f, .056074f, .367525f),
                        new Vector3(-.498906f, -.07706801f, .8632221f), 
                        new Vector3(-.988798f, -.089782f, -.119237f),
                        new Vector3(-.427064f, -.068381f, -.901632f), 
                        new Vector3(.701217f, -.0937f, -.706764f),
                        new Vector3(.977905f, -.08768f, .18977f), 
                        new Vector3(.384164f, -.01735f, .9231011f)
                    };
                mNormal = tempN;
                string[][] tempB =
                    {
                        new string[]
                        {
                            "b__Head__",
                            "b__R_Forearm__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Head__",
                            "b__R_Forearm__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Head__",
                            "b__R_Forearm__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Head__",
                            "b__R_Forearm__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Head__",
                            "b__R_Forearm__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Head__",
                            "b__R_Forearm__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_breast__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Chin__",
                            "b__EyeArea__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Jaw__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }
                    };
                mBoneName = tempB;
                uint[][] tempBH =
                    {
                        new uint[]
                        {
                            0x0F97B21B,
                            0xF0143B40,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x0F97B21B,
                            0xF0143B40,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x0F97B21B,
                            0xF0143B40,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x0F97B21B,
                            0xF0143B40,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x0F97B21B,
                            0xF0143B40,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x0F97B21B,
                            0xF0143B40,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x1EB57A41,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDB898525,
                            0x2506208F,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x405EC0BD,
                            0x57884BB9,
                            0x57884BB9,
                            0x57884BB9
                        }
                    };
                mBoneHash = tempBH;
                float[][] tempBW =
                    {
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            1,
                            0,
                            0,
                            0
                        }
                    };
                mBoneWeight = tempBW;
            }
            else if (string.Compare(ageGender, "AFLOD1") == 0)
            {
                Vector3[] tempP =
                    {
                        new Vector3(.040228f, 1.67105f, -.055092f),
                        new Vector3(.018578f, 1.67452f, -.068158f), 
                        new Vector3(.051864f, 1.66484f, -.036679f),
                        new Vector3(.05338901f, 1.65409f, -.011739f), 
                        new Vector3(.000133f, 1.6753f, -.07139f),
                        new Vector3(.041944f, 1.64318f, .007935001f), 
                        new Vector3(.027021f, 1.63333f, .024957f),
                        new Vector3(.010754f, 1.62413f, .035018f), 
                        new Vector3(0, 1.62259f, .036138f),
                        new Vector3(-.040228f, 1.67105f, -.055092f), 
                        new Vector3(-.018578f, 1.67452f, -.068158f),
                        new Vector3(-.051864f, 1.66484f, -.036679f), 
                        new Vector3(-.05338901f, 1.65409f, -.011739f),
                        new Vector3(-.041944f, 1.64318f, .007935001f), 
                        new Vector3(-.027028f, 1.63334f, .025017f),
                        new Vector3(-.010759f, 1.62415f, .035059f), 
                        new Vector3(.099168f, .185228f, .015239f),
                        new Vector3(.074283f, .185502f, .009548f), 
                        new Vector3(.061592f, .185729f, -.008944f),
                        new Vector3(.067842f, .184506f, -.04200801f), 
                        new Vector3(.092002f, .183107f, -.058299f),
                        new Vector3(.109292f, .183479f, -.048479f), 
                        new Vector3(.117148f, .184221f, .001116f),
                        new Vector3(.121222f, .183809f, -.027694f), 
                        new Vector3(.038936f, 1.20205f, -.067432f),
                        new Vector3(.011133f, 1.20173f, -.069359f), 
                        new Vector3(0, 1.19033f, .09478f),
                        new Vector3(.02438f, 1.19057f, .098728f), 
                        new Vector3(.053599f, 1.19205f, .09192701f),
                        new Vector3(.072243f, 1.194f, .081361f), 
                        new Vector3(.093489f, 1.19666f, .067485f),
                        new Vector3(.10022f, 1.20253f, -.037404f), 
                        new Vector3(.069301f, 1.20283f, -.058755f),
                        new Vector3(.114701f, 1.20218f, -.001555f), 
                        new Vector3(-.024381f, 1.19057f, .098728f),
                        new Vector3(-.0536f, 1.19205f, .09192701f), 
                        new Vector3(-.072244f, 1.194f, .081361f),
                        new Vector3(-.011133f, 1.20173f, -.069359f), 
                        new Vector3(-.038937f, 1.20205f, -.067432f),
                        new Vector3(0, 1.20145f, -.070035f), 
                        new Vector3(-.069302f, 1.20283f, -.058755f),
                        new Vector3(-.114702f, 1.20218f, -.001555f), 
                        new Vector3(-.100221f, 1.20253f, -.037404f),
                        new Vector3(.109921f, 1.1996f, .037028f), 
                        new Vector3(-.09349f, 1.19666f, .067485f),
                        new Vector3(-.109922f, 1.1996f, .037028f), 
                        new Vector3(-.099169f, .185228f, .015239f),
                        new Vector3(-.074284f, .185502f, .009548f), 
                        new Vector3(-.061593f, .185729f, -.008944f),
                        new Vector3(-.06784301f, .184506f, -.04200801f), 
                        new Vector3(-.09200301f, .183107f, -.058299f),
                        new Vector3(-.109293f, .183479f, -.048479f), 
                        new Vector3(-.117149f, .184221f, .001116f),
                        new Vector3(-.121223f, .183809f, -.027694f)
                    };
                mPosition = tempP;
                Vector3[] tempN =
                    {
                        new Vector3(.6708381f, -.170264f, -.721794f),
                        new Vector3(.311132f, -.230031f, -.9221081f), 
                        new Vector3(.924352f, -.15184f, -.350025f),
                        new Vector3(.969749f, -.218947f, .107927f), 
                        new Vector3(.000196f, -.262926f, -.964816f),
                        new Vector3(.843162f, -.373354f, .386891f), 
                        new Vector3(.710006f, -.607994f, .355296f),
                        new Vector3(.4208f, -.792509f, .441426f), 
                        new Vector3(.000241f, -.902018f, .431698f),
                        new Vector3(-.670844f, -.17024f, -.721794f), 
                        new Vector3(-.310838f, -.229983f, -.922219f),
                        new Vector3(-.92436f, -.151811f, -.350018f), 
                        new Vector3(-.969754f, -.218919f, .107939f),
                        new Vector3(-.843253f, -.373541f, .386511f), 
                        new Vector3(-.709915f, -.608452f, .354692f),
                        new Vector3(-.420374f, -.7929701f, .441004f), 
                        new Vector3(.211616f, .00369f, .9773461f),
                        new Vector3(-.5638241f, .020089f, .825651f), 
                        new Vector3(-.972979f, -.017245f, .230247f),
                        new Vector3(-.837718f, -.083248f, -.539721f), 
                        new Vector3(-.023685f, -.137378f, -.990235f),
                        new Vector3(.744515f, -.138084f, -.653169f), 
                        new Vector3(.875347f, -.061439f, .479575f),
                        new Vector3(.970116f, -.138597f, -.199162f), 
                        new Vector3(.177203f, .198655f, -.963917f),
                        new Vector3(.024154f, .245274f, -.969153f), 
                        new Vector3(0, .001119f, .999999f),
                        new Vector3(.076342f, .017702f, .996924f), 
                        new Vector3(.346287f, .06264301f, .936035f),
                        new Vector3(.492957f, .116487f, .862221f), 
                        new Vector3(.715419f, .180331f, .675023f),
                        new Vector3(.7744731f, .160456f, -.611919f), 
                        new Vector3(.438727f, .158261f, -.884574f),
                        new Vector3(.970159f, .198323f, -.139493f), 
                        new Vector3(-.076342f, .017701f, .996924f),
                        new Vector3(-.346286f, .06264301f, .936035f), 
                        new Vector3(-.492957f, .116487f, .86222f),
                        new Vector3(-.024153f, .245274f, -.969153f), 
                        new Vector3(-.177202f, .198655f, -.963917f),
                        new Vector3(0, .266505f, -.963833f), 
                        new Vector3(-.438727f, .158261f, -.884574f),
                        new Vector3(-.970159f, .198324f, -.139493f), 
                        new Vector3(-.774474f, .160456f, -.611919f),
                        new Vector3(.931861f, .214596f, .292547f), 
                        new Vector3(-.715419f, .180331f, .675023f),
                        new Vector3(-.931861f, .214597f, .292547f), 
                        new Vector3(-.211616f, .003686f, .9773461f),
                        new Vector3(.5638241f, .020099f, .825651f), 
                        new Vector3(.97298f, -.017227f, .230247f),
                        new Vector3(.837718f, -.08323501f, -.5397221f), 
                        new Vector3(.023684f, -.137378f, -.990235f),
                        new Vector3(-.744514f, -.138093f, -.653168f), 
                        new Vector3(-.8753461f, -.061455f, .479575f),
                        new Vector3(-.970115f, -.138607f, -.199162f)
                    };
                mNormal = tempN;
                string[][] tempB =
                    {
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Neck__",
                            "b__Head__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__L_Calf__Compress__",
                            "b__L_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__Spine0__",
                            "b__Pelvis__",
                            "b__Spine1__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }, 
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        },
                        new string[]
                        {
                            "b__R_Calf__Compress__",
                            "b__R_Calf__",
                            "b__ROOT_bind__",
                            "b__ROOT_bind__"
                        }
                    };
                mBoneName = tempB;
                uint[][] tempBH =
                    {
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xBC81D5B8,
                            0x0F97B21B,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x631006B6,
                            0x85E195D0,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0x6FA96266,
                            0x556B181A,
                            0xAFAC05CF,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }, 
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        },
                        new uint[]
                        {
                            0xDD534A80,
                            0x81B330DE,
                            0x57884BB9,
                            0x57884BB9
                        }
                    };
                mBoneHash = tempBH;
                float[][] tempBW =
                    {
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .5f,
                            .5f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        },
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        }, 
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        },
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        }, 
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        },
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        }, 
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        },
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        }, 
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        },
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        }, 
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        },
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        }, 
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        },
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        }, 
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        },
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        }, 
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        },
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        }, 
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        },
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        }, 
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        },
                        new float[]
                        {
                            .65625f,
                            .25f,
                            .09375f,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }, 
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        },
                        new float[]
                        {
                            .75f,
                            .25f,
                            0,
                            0
                        }
                    };
                mBoneWeight = tempBW;
            }
            else
            {
                mBoneHash = null;
                mBoneName = null;
                mBoneWeight = null;
                mPosition = null;
                mNormal = null;
            }
        }
    }
}
