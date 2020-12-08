using System.Collections.Generic;
using Grayscale.KifuwaraneGui.L07_Shape;
using Grayscale.KifuwaraneEntities.L04_Common;

namespace Grayscale.KifuwaraneGui.L09_Ui
{

    /// <summary>
    /// ************************************************************************************************************************
    /// あるデータを、別のデータに変換します。
    /// ************************************************************************************************************************
    /// </summary>
    public abstract class Converter09
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// 駒のハンドル(*1)を元に、ボタンを返します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…将棋の駒１つ１つに付けられた番号です。
        /// 
        /// </summary>
        /// <param name="hKomas"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <returns></returns>
        public static List<Shape_BtnKoma> HKomasToBtns(List<int> hKomas, Shape_PnlTaikyoku shape_PnlTaikyoku)
        {
            List<Shape_BtnKoma> btns = new List<Shape_BtnKoma>();

            foreach (int handle in hKomas)
            {
                Shape_BtnKoma btn = shape_PnlTaikyoku.BtnKomaDoors[handle];
                if (null != btn)
                {
                    btns.Add(btn);
                }
            }

            return btns;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 駒のハンドル(*1)を元に、ボタンを返します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…将棋の駒１つ１つに付けられた番号です。
        /// 
        /// </summary>
        /// <param name="hKoma"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <returns>なければヌル</returns>
        public static Shape_BtnKoma KomaToBtn(K40 koma, Shape_PnlTaikyoku shape_PnlTaikyoku)
        {
            Shape_BtnKoma found = null;

            int hKoma = (int)koma;

            if (0 <= hKoma && hKoma < shape_PnlTaikyoku.BtnKomaDoors.Length)
            {
                found = shape_PnlTaikyoku.BtnKomaDoors[hKoma];
            }

            return found;
        }
    }
}
