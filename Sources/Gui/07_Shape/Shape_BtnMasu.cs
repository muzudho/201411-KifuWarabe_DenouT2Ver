using System.Drawing;
using Grayscale.Kifuwarane.Entities.ApplicatedGame;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;

namespace Grayscale.Kifuwarane.Gui.L07_Shape
{


    /// <summary>
    /// ************************************************************************************************************************
    /// 描かれる図画です。ボタンとして押せる升目を描きます。
    /// ************************************************************************************************************************
    /// </summary>
    public class Shape_BtnMasu : Shape_Abstract
    {

        #region プロパティー

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 座標
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///         升目の位置。
        /// 
        /// </summary>
        public M201 Zahyo
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 光。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public bool Light
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 動かしたい駒。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public bool Select
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 動かしたい駒。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public bool SelectFirstTouch
        {
            get;
            set;
        }

        #endregion


        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="suji"></param>
        /// <param name="dan"></param>
        public Shape_BtnMasu(Okiba okiba, int suji, int dan)
        {
            this.Zahyo = M201Util.OkibaSujiDanToMasu(
                    okiba,
                    suji,
                    dan
                    );
            this.Bounds = new Rectangle(42, 42, 35, 35);
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="suji"></param>
        /// <param name="dan"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Shape_BtnMasu(Okiba okiba, int suji, int dan, int x, int y)
            : this(okiba, suji, dan)
        {
            this.Bounds = new Rectangle(x, y, this.Bounds.Width, this.Bounds.Height);
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 升ボタンの描画はここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="g1"></param>
        public void Paint(Graphics g1, bool kiki, int kikiSu)
        {
            if (!this.Visible)
            {
                goto gt_EndMethod;
            }

            //----------
            // 背景
            //----------
            if (this.Select)
            {
                g1.FillRectangle(Brushes.Brown, this.Bounds);
            }
            else if(kiki)
            {
                g1.FillRectangle(Brushes.YellowGreen, this.Bounds);
            }
            else if(0<kikiSu)
            {
                //int level = kikiSu * 5;
                int level = (kikiSu-1) * 40;
                if (120 < level)
                {
                    level = 120;
                }
                g1.FillRectangle(new SolidBrush(Color.FromArgb(255, 255-30-level, 255-10-level, 255-10-level)), this.Bounds);
            }
            else if (this.Light)
            {
            }

            //----------
            // 枠線
            //----------
            {
                if (this.Light)
                {
                    g1.DrawRectangle(Pens.Yellow, this.Bounds);
                }
            }

        gt_EndMethod:
            ;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// マウスが重なった駒は、光フラグを立てます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void LightByMouse(int x, int y)
        {
            if (this.HitByMouse(x,y)) // マウスが重なっているなら
            {
                this.Light = true;
            }
            else // マウスが重なっていないなら
            {
                this.Light = false;
            }
        }




        /// <summary>
        /// ************************************************************************************************************************
        /// 動かしたい駒の解除
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool DeselectByMouse(int x, int y)
        {
            bool changed = false;

            if ( this.HitByMouse(x, y)) // マウスが重なっているなら
            {
                if (this.SelectFirstTouch)
                {
                    // クリックのマウスアップ
                    this.SelectFirstTouch = false;
                }
                else
                {
                    // 選択解除のマウスアップ
                    this.Select = false;
                    changed = true;
                }
            }
            else
            {
                // 何もしない
            }

            return changed;
        }
    }


}
