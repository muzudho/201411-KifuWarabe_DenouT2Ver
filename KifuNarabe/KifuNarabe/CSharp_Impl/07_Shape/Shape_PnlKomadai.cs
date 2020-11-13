using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

using Xenon.KifuLarabe.L03_Communication;
using Xenon.KifuLarabe.L04_Common;

namespace Xenon.KifuNarabe.L07_Shape
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 描かれる図画です。駒台、駒袋を描きます。
    /// ************************************************************************************************************************
    /// </summary>
    public class Shape_PnlKomadai : Shape_Abstract
    {


        #region プロパティー

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// １升の横幅。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public int MasuWidth
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// １升の縦幅。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public int MasuHeight
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒台が先後どちらのものか。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Sengo Sengo
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// すべての升目１つ１つのリスト。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public List<Shape_BtnMasu> MasuList
        {
            get
            {
                return this.masuList;
            }
        }
        private List<Shape_BtnMasu> masuList;

        #endregion


        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="sengo"></param>
        public Shape_PnlKomadai( Okiba okiba, int x, int y, Sengo sengo)
        {
            this.Bounds = new Rectangle(x, y, 1, 1);
            this.MasuWidth = 40;
            this.MasuHeight = 40;

            this.Sengo = sengo;

            //----------
            // 升目
            //----------
            this.masuList = new List<Shape_BtnMasu>();

            for (int suji = 1; suji <= M201Util.KOMADAI_LAST_SUJI; suji++)
            {
                for (int dan = 1; dan <= M201Util.KOMADAI_LAST_DAN; dan++)
                {
                    this.MasuList.Add(new Shape_BtnMasu(okiba, suji, dan, this.SujiToX( suji), this.DanToY(dan)));
                }
            }
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 筋を指定すると、ｘ座標を返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="suji"></param>
        /// <returns></returns>
        public int SujiToX( int suji)
        {
            return (M201Util.KOMADAI_LAST_SUJI - suji) * this.MasuWidth + this.Bounds.X;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 段を指定すると、ｙ座標を返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="dan"></param>
        /// <returns></returns>
        public int DanToY(int dan)
        {
            return (dan-1) * this.MasuHeight + this.Bounds.Y;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 駒台の描画は、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="g"></param>
        public void Paint( Graphics g)
        {
            if (!this.Visible)
            {
                goto gt_EndMethod;
            }

            //----------
            // 水平線
            //----------
            for (int i = 0; i <= M201Util.KOMADAI_LAST_DAN; i++)
            {
                g.DrawLine(Pens.Black,
                                0 * this.MasuWidth + this.Bounds.X,
                                i * this.MasuHeight + this.Bounds.Y,
                    M201Util.KOMADAI_LAST_SUJI * this.MasuHeight + this.Bounds.X,
                                i * this.MasuHeight + this.Bounds.Y
                    );
            }

            //----------
            // 垂直線
            //----------
            for (int i = 0; i <= M201Util.KOMADAI_LAST_SUJI; i++)
            {
                g.DrawLine(Pens.Black,
                                i * this.MasuWidth + this.Bounds.X,
                                0 * this.MasuHeight + this.Bounds.Y,
                                i * this.MasuHeight + this.Bounds.X,
                    M201Util.KOMADAI_LAST_DAN * this.MasuHeight + this.Bounds.Y
                    );
            }

            //----------
            // 升目
            //----------
            foreach (Shape_BtnMasu cell in this.MasuList)
            {
                cell.Paint(g, false, 0);
            }


        gt_EndMethod:
            ;
        }


    }
}
