using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grayscale.KifuwaraneLib.L03_Communication
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 駒の相対位置(*1)です。
    /// ************************************************************************************************************************
    /// 
    ///         *1…右、左、直ぐ、非表記。
    /// 
    /// </summary>
    public enum MigiHidari
    {
        /// <summary>
        /// 右
        /// </summary>
        Migi,

        /// <summary>
        /// 左
        /// </summary>
        Hidari,

        /// <summary>
        /// 直
        /// </summary>
        Sugu,

        /// <summary>
        /// 非表記
        /// </summary>
        No_Print
    }
}
