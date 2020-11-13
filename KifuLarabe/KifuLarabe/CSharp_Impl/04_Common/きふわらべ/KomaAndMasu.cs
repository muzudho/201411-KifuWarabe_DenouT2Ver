﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenon.KifuLarabe.L04_Common
{
    public class KomaAndMasu
    {

        public K40 Koma { get { return this.koma; } }
        private K40 koma;

        public M201 Masu { get { return this.masu; } }
        public M201 masu;

        public KomaAndMasu(K40 koma, M201 masu)
        {
            this.koma = koma;
            this.masu = masu;
        }
    }
}
