﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTLS.Types
{
    public struct ResourceEntry
    {
        public uint offInChunk;
        public uint nameOffsetEtc;
        public int cmpSize;
        public int decSize;
        public uint timestamp;
        public uint flags;
    }
}