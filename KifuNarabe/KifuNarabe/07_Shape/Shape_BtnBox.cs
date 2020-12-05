using System.Drawing;

namespace Grayscale.KifuwaraneGui.L07_Shape
{


    /// <summary>
    /// ************************************************************************************************************************
    /// 描かれる図画です。四角いボタンを描きます。
    /// ************************************************************************************************************************
    /// </summary>
    public class Shape_BtnBox : Shape_Abstract
    {

        #region プロパティー

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 表示テキスト
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 光
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public bool Light
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 動かしたい駒
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public bool Select
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// フォントサイズ。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public float FontSize
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
        /// <param name="label"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Shape_BtnBox(string label, int x, int y)
        {
            this.Text = label;
            this.Bounds = new Rectangle(x,y,70,35);
            this.FontSize = 20.0f;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="label"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fontSize"></param>
        public Shape_BtnBox(string label, int x, int y, int width, int height, float fontSize)
        {
            this.Text = label;
            this.Bounds = new Rectangle(x, y, width, height);
            this.FontSize = fontSize;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 描画
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="g1"></param>
        public void Paint(Graphics g1)
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
                g1.FillRectangle(Brushes.Green, this.Bounds);
            }
            else if (this.Light)
            {
                if (null != this.BackColor)
                {
                    g1.FillRectangle(new SolidBrush(this.BackColor), this.Bounds);
                }
            }
            else
            {
                if (null != this.BackColor)
                {
                    g1.FillRectangle(new SolidBrush(this.BackColor), this.Bounds);
                }
            }


            //----------
            // 枠線
            //----------
            {
                Pen pen;
                if (this.Light)
                {
                    pen = Pens.Yellow;
                }
                else
                {
                    pen = Pens.Black;
                }

                g1.DrawRectangle(pen, this.Bounds);
            }

            //----------
            // 文字
            //----------
            g1.DrawString(this.Text, new Font(FontFamily.GenericSerif, this.FontSize), Brushes.Black, this.Bounds.Location);

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
        public bool DeselectByMouse(int x, int y, Shape_PnlTaikyoku shape_PnlTaikyoku)
        {
            bool changed = false;

            if ( this.HitByMouse(x, y)) // マウスが重なっているなら
            {
                if (shape_PnlTaikyoku.SelectFirstTouch)
                {
                    // クリックのマウスアップ
                    shape_PnlTaikyoku.SelectFirstTouch = false;
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
