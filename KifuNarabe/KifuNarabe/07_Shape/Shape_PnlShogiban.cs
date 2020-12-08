using System.Collections.Generic;
using System.Drawing;
using Grayscale.KifuwaraneEntities;
using Grayscale.KifuwaraneEntities.ApplicatedGame;
using Grayscale.KifuwaraneEntities.Sfen;
using Grayscale.KifuwaraneEntities.L03_Communication;
using Grayscale.KifuwaraneEntities.L04_Common;

namespace Grayscale.KifuwaraneGui.L07_Shape
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 描かれる図画です。将棋盤を描きます。
    /// ************************************************************************************************************************
    /// </summary>
    public class Shape_PnlShogiban : Shape_Abstract
    {

        #region プロパティー

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 升の横幅。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public int MasuWidth
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 升の縦幅。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public int MasuHeight
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// 升目
        /// </summary>
        public List<Shape_BtnMasu> MasuList
        {
            get;
            set;
        }

        /// <summary>
        /// 光らせる利き升ハンドル。
        /// </summary>
        public IMasus KikiBan
        {
            get;
            set;
        }

        /// <summary>
        /// 枡毎の、利いている駒ハンドルのリスト。
        /// </summary>
        public Dictionary<int,List<int>> HMasu_KikiKomaList
        {
            get;
            set;
        }


        /// <summary>
        /// 将棋盤の枡の数。
        /// </summary>
        public const int NSQUARE = 9 * 9;

        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Shape_PnlShogiban(int x, int y)
        {
            this.Bounds = new Rectangle(x, y, 1, 1);
            this.MasuWidth = 40;
            this.MasuHeight = 40;

            this.MasuList = new List<Shape_BtnMasu>();
            this.KikiBan = new Masus_Set();
            this.HMasu_KikiKomaList = new Dictionary<int, List<int>>();

            //----------
            // 升目
            //----------
            for (int suji = 1; suji < 10; suji++)
            {
                for (int dan = 1; dan < 10; dan++)
                {
                    this.MasuList.Add(new Shape_BtnMasu(Okiba.ShogiBan, suji, dan, this.SujiToX(suji), this.DanToY(dan)));
                }
            }

            //----------
            // 枡に利いている駒への逆リンク（の入れ物を用意）
            //----------
            this.ClearHMasu_KikiKomaList();
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 枡に利いている駒への逆リンク（の入れ物を用意）
        /// ************************************************************************************************************************
        /// </summary>
        public void ClearHMasu_KikiKomaList()
        {
            this.HMasu_KikiKomaList.Clear();

            for (int masuIndex = 0; masuIndex < Shape_PnlShogiban.NSQUARE; masuIndex++)
            {
                this.HMasu_KikiKomaList.Add(masuIndex, new List<int>());
            }
        }



        /// <summary>
        /// ************************************************************************************************************************
        /// 筋を指定すると、ｘ座標を返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="suji"></param>
        /// <returns></returns>
        public int SujiToX(int suji)
        {
            return (9 - suji) * this.MasuWidth + this.Bounds.X;
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
            return (dan - 1) * this.MasuHeight + this.Bounds.Y;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋盤の描画はここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="g"></param>
        public void Paint(Graphics g)
        {
            if (!this.Visible)
            {
                goto gt_EndMethod;
            }

            //----------
            // 筋の数字
            //----------
            for (int i = 0; i < 9; i++)
            {
                g.DrawString(GameTranslator.IntToArabic(i + 1), new Font("ＭＳ ゴシック", 25.0f), Brushes.Black, new Point((8 - i) * this.MasuWidth + this.Bounds.X - 8, -1 * this.MasuHeight + this.Bounds.Y));
            }

            //----------
            // 段の数字
            //----------
            for (int i = 0; i < 9; i++)
            {
                g.DrawString(GameTranslator.IntToJapanese(i + 1), new Font("ＭＳ ゴシック", 23.0f), Brushes.Black, new Point(9 * this.MasuWidth + this.Bounds.X, i * this.MasuHeight + this.Bounds.Y));
                g.DrawString(GameTranslator.IntToAlphabet(i + 1), new Font("ＭＳ ゴシック", 11.0f), Brushes.Black, new Point(9 * this.MasuWidth + this.Bounds.X, i * this.MasuHeight + this.Bounds.Y));
            }


            //----------
            // 水平線
            //----------
            for (int i = 0; i < 10; i++)
            {
                g.DrawLine(Pens.Black,
                    0 * this.MasuWidth + this.Bounds.X,
                    i * this.MasuHeight + this.Bounds.Y,
                    9 * this.MasuHeight + this.Bounds.X,
                    i * this.MasuHeight + this.Bounds.Y
                    );
            }

            //----------
            // 垂直線
            //----------
            for (int i = 0; i < 10; i++)
            {
                g.DrawLine(Pens.Black,
                    i * this.MasuWidth + this.Bounds.X,
                    0 * this.MasuHeight + this.Bounds.Y,
                    i * this.MasuHeight + this.Bounds.X,
                    9 * this.MasuHeight + this.Bounds.Y
                    );
            }


            //----------
            // 升目
            //----------
            for (int masuHandle = 0; masuHandle < this.MasuList.Count; masuHandle++)
            {
                Shape_BtnMasu cell = this.MasuList[masuHandle];

                IMasus masus2 = new Masus_Set();
                masus2.AddElement(M201Array.Items_All[ masuHandle]);
                bool isKiki = this.KikiBan.ContainsAll(masus2);

                int kikiSu = this.HMasu_KikiKomaList[masuHandle].Count;


                cell.Paint(g, isKiki, kikiSu);
            }



        gt_EndMethod:
            ;
        }


    }
}
