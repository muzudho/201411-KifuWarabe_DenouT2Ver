using System;
using System.Drawing;
using Grayscale.KifuwaraneLib;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneGui.L07_Shape
{


    /// <summary>
    /// ************************************************************************************************************************
    /// 描かれる図画です。将棋の駒を描きます。
    /// ************************************************************************************************************************
    /// </summary>
    [Serializable]
    public class Shape_BtnKoma : Shape_Abstract
    {

        #region プロパティー

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 局面の駒リストでの格納位置番号。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public K40 Koma
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

        #endregion


        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="koma"></param>
        public Shape_BtnKoma(K40 koma)
        {
            this.Koma = koma;
            this.Bounds = new Rectangle(42, 42, 35, 35);
        }


        private void PaintText(Graphics g, IKomaPos komaP, Point location)
        {
            if (null == komaP)
            {
                goto gt_EndMethod;
            }

            if (komaP.Star.Haiyaku == Kh185.n000_未設定)
            {
                // 配役未設定時は、普通に駒を描画します。
                g.DrawString(komaP.Text_Label, new Font(FontFamily.GenericSerif, 20.0f), Brushes.Black, location);
            }
            else
            {
                // 配役設定時は、配役名を描画します。

                string text = Haiyaku184Array.Name[(int)komaP.Star.Haiyaku];
                string text1;
                string text2;
                string text3;

                if (4 < text.Length)
                {
                    text1 = text.Substring(0, 2);
                    text2 = text.Substring(2, 2);
                    text3 = text.Substring(4, text.Length - 4);
                }
                else if (2 < text.Length)
                {
                    text1 = text.Substring(0, 2);
                    text2 = text.Substring(2, text.Length - 2);
                    text3 = "";
                }
                else
                {
                    text1 = text;
                    text2 = "";
                    text3 = "";
                }

                // １行目
                g.DrawString(text1, new Font(FontFamily.GenericSerif, 10.0f), Brushes.Black, location);

                // ２行目
                g.DrawString(text2, new Font(FontFamily.GenericSerif, 10.0f), Brushes.Black, new Point(location.X, location.Y + 11));

                // ３行目
                g.DrawString(text3, new Font(FontFamily.GenericSerif, 10.0f), Brushes.Black, new Point(location.X, location.Y + 22));
            }

        gt_EndMethod:
            ;
        }


        /// <summary>
        /// 駒ボタンの描画はここに書きます。
        /// </summary>
        /// <param name="g1"></param>
        /// <param name="kyokumen"></param>
        public void Paint(Graphics g1, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD,ILogTag logTag)
        {
            if (!this.Visible)
            {
                goto gt_EndMethod;
            }

            int lastTeme = kifuD.CountTeme(kifuD.Current8);

            //----------
            // 背景
            //----------
            if (shape_PnlTaikyoku.HTumandeiruKoma == (int)this.Koma)
            {
                g1.FillRectangle(Brushes.Brown, this.Bounds);
            }
            else if (shape_PnlTaikyoku.MovedKoma == this.Koma)
            {
                g1.FillRectangle(Brushes.DarkKhaki, this.Bounds);
            }
            else if (this.Light)
            {
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

                IKifuElement dammyNode3 = kifuD.ElementAt8(lastTeme);
                KomaHouse house4 = dammyNode3.KomaHouse;

                if (house4.KomaPosAt(this.Koma).Star.Sengo == Sengo.Sente)
                {
                    g1.DrawRectangle(pen, this.Bounds);
                }
                else
                {
                    g1.DrawRectangle(pen, this.Bounds);
                }
            }

            //----------
            // 文字
            //----------

            IKifuElement dammyNode4 = kifuD.ElementAt8(lastTeme);
            KomaHouse house5 = dammyNode4.KomaHouse;

            IKomaPos komaP = house5.KomaPosAt(this.Koma);
            if (komaP.Star.Sengo == Sengo.Sente)
            {
                //----------
                // 先手
                //----------
                //
                // ただ描画するだけ☆
                //

                this.PaintText(g1, komaP, this.Bounds.Location);
            }
            else
            {
                //----------
                // 後手
                //----------
                //
                // １８０度回転させて描画するために、大掛かりになっています。
                //

                //string moji = kyokumen.KomaDoors[this.KomaHandle].Text_Label;

                //----------
                // 使用するフォント
                //----------
                //Font fnt = new Font(FontFamily.GenericSerif, 20.0f);

                //----------
                // 文字の大きさにあった白紙（b）
                //----------
                Graphics bG;
                Bitmap bImg;
                {
                    int w;
                    int h;
                    {
                        //----------
                        // 文字の大きさを調べるための白紙（a）
                        //----------
                        Bitmap aImg = new Bitmap(1, 1);

                        //imgのGraphicsオブジェクトを取得
                        Graphics aG = Graphics.FromImage(aImg);

                        //文字列を描画したときの大きさを計算する
                        w = 48;
                        h = 48;
                        //w = (int)aG.MeasureString(moji, fnt).Width;
                        //h = (int)fnt.GetHeight(aG);

                        //if (w == 0 || h == 0)
                        //{
                        //    System.Console.WriteLine("moji=["+moji+"]");
                        //}

                        //if (w < 1)
                        //{
                        //    w = 1;
                        //}

                        //if (h < 1)
                        //{
                        //    h = 1;
                        //}

                        aG.Dispose();
                        aImg.Dispose();
                    }

                    bImg = new Bitmap(w, h);
                }

                // 文字描画
                bG = Graphics.FromImage(bImg);

                this.PaintText(bG, komaP, new Point(0,0)); //bG.DrawString(moji, fnt, Brushes.Black, 0, 0);

                //----------
                // 回転軸座標
                //----------
                float x = (float)this.Bounds.X + (float)this.Bounds.Width / 2;
                float y = (float)this.Bounds.Y + (float)this.Bounds.Height / 2;

                //----------
                // 回転
                //----------

                // 180度で回転するための座標を計算
                //ラジアン単位に変換
                double d = 180 / (180 / Math.PI);
                //新しい座標位置を計算する
                float x1 = x + bImg.Width * (float)Math.Cos(d);
                float y1 = y + bImg.Width * (float)Math.Sin(d);
                float x2 = x - bImg.Height * (float)Math.Sin(d);
                float y2 = y + bImg.Height * (float)Math.Cos(d);
                //PointF配列を作成
                PointF[] destinationPoints = {new PointF(x + (float)this.Bounds.Width / 2, y + (float)this.Bounds.Height / 2),
                                    new PointF(x1 + (float)this.Bounds.Width / 2, y1 + (float)this.Bounds.Height / 2),
                                    new PointF(x2 + (float)this.Bounds.Width / 2, y2 + (float)this.Bounds.Height / 2)};

                //画像を描画
                g1.DrawImage(bImg, destinationPoints);

                //リソースを解放する
                bImg.Dispose();
                bG.Dispose();
                //fnt.Dispose();
            }

            ////----------
            //// デバッグ用
            ////----------
            //if (true)
            //{
            //    string moji = kyokumen.KomaDoors[this.Handle].SrcOkiba.ToString();

            //    g1.DrawString(moji, new Font(FontFamily.GenericSerif, 12.0f), Brushes.Red, this.Bounds.Location);
            //}

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

    }



}
