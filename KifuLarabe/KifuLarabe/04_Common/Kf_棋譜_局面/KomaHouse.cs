using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Grayscale.KifuwaraneLib.Entities.ApplicatedGame;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L03_Communication;

namespace Grayscale.KifuwaraneLib.L04_Common
{
    /// <summary>
    /// 局面
    /// </summary>
    public class KomaHouse
    {
        /// <summary>
        /// 駒台に戻すとき
        /// </summary>
        /// <param name="kifuD"></param>
        /// <param name="starIndex"></param>
        /// <param name="komaP"></param>
        public void SetStarPos(Kifu_Document kifuD, int starIndex, IKomaPos komaP)
        {
            if(this.stars.Count==starIndex)
            {
                this.Stars.Add(komaP);
            }
            else if (this.stars.Count+1 <= starIndex)
            {
                string message = this.GetType().Name + "#SetStarPos：　リストの要素より2多いインデックスを指定されましたので、追加しません。starIndex=[" + starIndex + "] / this.stars.Count=[" + this.stars.Count + "]";
                Logger.ErrorLine(LarabeLoggerTag_Impl.ERROR, message);
                throw new Exception(message);
            }
            else
            {
                this.Stars[starIndex] = komaP;
            }
        }


        public void SetKomaPos(
            Kifu_Document kifuD,
            K40 koma,
            IKomaPos komaP
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
        )
        {
            if (K40.Error == koma)
            {
                string message = this.GetType().Name + "#SetKomaPos：　駒番号isエラー　：　" + memberName + "." + sourceFilePath + "." + sourceLineNumber;
                Logger.ErrorLine(LarabeLoggerTag_Impl.ERROR, message);
                throw new Exception(message);
            }

            this.SetStarPos(kifuD, (int)koma, komaP); //this.Stars[(int)koma] = komaP;
        }


        public IKomaPos KomaPosAt(
            K40 koma
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            if (K40.Error == koma)
            {
                string message = this.GetType().Name + "#KomaPosAt：駒番号isエラー　：　" + memberName + "." + sourceFilePath + "." + sourceLineNumber;
                Logger.ErrorLine(LarabeLoggerTag_Impl.ERROR, message);
                throw new Exception(message);
            }

            return this.StarAt((int)koma);// this.Stars[(int)koma];
        }

        public IKomaPos StarAt(int starIndex)
        {
            IKomaPos found;

            if (starIndex < this.stars.Count)
            {
                found = this.stars[starIndex];
            }
            else
            {
                string message = this.GetType().Name + "#StarAt：　リストの要素より多いインデックスを指定されましたので、取得できません。starIndex=[" + starIndex + "] / this.stars.Count=[" + this.stars.Count + "]";
                Logger.ErrorLine(LarabeLoggerTag_Impl.ERROR, message);
                throw new Exception(message);
            }

            return found;
        }


        /// <summary>
        /// 置き場に置けるもののリストです。駒だけとは限りませんので、４０個以上になることもあります。
        /// </summary>
        public List<IKomaPos> Stars
        {
            get
            {
                return this.stars;
            }
        }
        private List<IKomaPos> stars;


        public KomaHouse()
        {
            this.stars = new List<IKomaPos>();
            this.startpos = "未設定";
        }

        public KomaHouse(List<IKomaPos> stars)
        {
            this.stars = stars;
            this.startpos = "未設定";
        }

        public KomaHouse(IKomaPos[] items)
        {
            this.stars = items.OfType<IKomaPos>().ToList();
            this.startpos = "未設定";
        }

        public void Reset_ToDammy1()
        {
            this.stars = new List<IKomaPos>()
            {
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
            };
        }

        public void Reset_ToDammy40()
        {
            this.stars = new List<IKomaPos>()
            {
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),
            };
        }

        /// <summary>
        /// 平手初期配置にリセットします。
        /// </summary>
        public void Reset_ToHirateSyokihaichi()
        {
            this.stars = new List<IKomaPos>(){

                RO_KomaPos.Reset( new RO_Star( Sengo.Sente, M201.fukuro01, Kh185.n051_底奇王)),//[0]
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro02, Kh185.n051_底奇王)),

                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro03, Kh185.n061_飛)),//[2]
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro04, Kh185.n061_飛)),

                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro05, Kh185.n072_奇角)),//[4]
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro06, Kh185.n072_奇角)),

                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro07, Kh185.n038_底偶金)),//[6]
                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro08, Kh185.n038_底偶金)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro09, Kh185.n038_底偶金)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro10, Kh185.n038_底偶金)),

                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro11, Kh185.n023_底奇銀)),//[10]
                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro12, Kh185.n023_底奇銀)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro13, Kh185.n023_底奇銀)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro14, Kh185.n023_底奇銀)),

                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro15, Kh185.n007_金桂)),//[14]
                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro16, Kh185.n007_金桂)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro17, Kh185.n007_金桂)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro18, Kh185.n007_金桂)),

                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro19, Kh185.n002_香)),//[18]
                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro20, Kh185.n002_香)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro21, Kh185.n002_香)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro22, Kh185.n002_香)),

                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro23, Kh185.n001_歩)),//[22]
                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro24, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro25, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro26, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro27, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro28, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro29, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro30, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Sente,M201.fukuro31, Kh185.n001_歩)),

                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro32, Kh185.n001_歩)),//[31]
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro33, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro34, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro35, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro36, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro37, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro38, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro39, Kh185.n001_歩)),
                RO_KomaPos.Reset(new RO_Star(Sengo.Gote,M201.fukuro40, Kh185.n001_歩)),//[39]
                // 以上、全40駒。
                };

            this.startpos = "startpos";
        }


        /// <summary>
        /// 
        /// </summary>
        public string Log_Kyokumen(
            Kifu_Document kifuD,
            int arrayIndex,
            string memo
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            StringBuilder sb = new StringBuilder();

            RO_KomaPos[,] ban81 = new RO_KomaPos[9, 9];
            List<RO_KomaPos> goteKomadai = new List<RO_KomaPos>();
            List<RO_KomaPos> senteKomadai = new List<RO_KomaPos>();

            foreach (RO_KomaPos starPos in this.Stars)
            {
                if (M201Util.GetOkiba(starPos.Star.Masu) == Okiba.Gote_Komadai)
                {
                    goteKomadai.Add(starPos);
                }
                else if (M201Util.GetOkiba(starPos.Star.Masu) == Okiba.Sente_Komadai)
                {
                    senteKomadai.Add(starPos);
                }
                else if (M201Util.GetOkiba(starPos.Star.Masu) == Okiba.ShogiBan)
                {
                    ban81[
                        Mh201Util.MasuToSuji(starPos.Star.Masu) - 1,
                        Mh201Util.MasuToDan(starPos.Star.Masu) - 1
                        ] = starPos;
                }
            }




            sb.Append("...(^▽^)さて、局面は☆？");

            if (null != memo && "" != memo.Trim())
            {
                sb.Append("　：　");
                sb.Append(memo);
            }
            sb.Append("　：　" + memberName + "." + sourceFilePath + "." + sourceLineNumber);
            sb.AppendLine();

            sb.AppendLine("[" + arrayIndex + "]データ目　葉＝[" + kifuD.CountTeme(kifuD.Current8) + "]手目");

            sb.AppendLine("　９　８　７　６　５　４　３　２　１");
            sb.AppendLine("┏━┯━┯━┯━┯━┯━┯━┯━┯━┓");
            for (int dan = 0; dan < 9; dan++)
            {
                sb.Append("┃");
                for (int suji = 8; suji >= 0; suji--)// 筋は左右逆☆
                {
                    RO_KomaPos masu = ban81[suji, dan];
                    if (null != masu)
                    {
                        sb.Append(masu.ToGaiji());//外字も利用☆
                    }
                    else
                    {
                        sb.Append("  ");//半角2つにするぜ☆　わたしのよく使っているエディターは全角空白を　グレーの□　で表示してしまうんだぜ☆
                    }

                    if (suji == 0)//１筋が最後だぜ☆
                    {
                        sb.Append("┃");
                        sb.AppendLine(GameTranslator.IntToJapanese(dan + 1));
                    }
                    else
                    {
                        sb.Append("│");
                    }
                }

                if (dan == 8)
                {
                    sb.AppendLine("┗━┷━┷━┷━┷━┷━┷━┷━┷━┛");
                }
                else
                {
                    sb.AppendLine("┠─┼─┼─┼─┼─┼─┼─┼─┼─┨");
                }
            }


            // 後手駒台
            sb.Append("▽後手　持ち駒：");
            foreach (RO_KomaPos masu in goteKomadai)
            {
                sb.Append(masu.ToGaiji());
            }
            sb.AppendLine();


            // 先手駒台
            sb.Append("▲先手　持ち駒：");
            foreach (RO_KomaPos masu in senteKomadai)
            {
                sb.Append(masu.ToGaiji());
            }
            sb.AppendLine();
            sb.AppendLine("...(^▽^)ﾄﾞｳﾀﾞｯﾀｶﾅ～☆");
            sb.AppendLine();//空行


            return sb.ToString();
        }










        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 初期配置。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public string Startpos
        {
            get
            {
                return this.startpos;
            }
        }
        /// <summary>
        /// ************************************************************************************************************************
        /// 平手の初期配置にします。
        /// ************************************************************************************************************************
        /// </summary>
        public void SetStartpos()
        {
            this.SetStartpos("startpos");
        }
        /// <summary>
        /// ************************************************************************************************************************
        /// 初期配置を指定します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="startpos"></param>
        public void SetStartpos(string startpos)
        {
            this.startpos = startpos;
        }
        private string startpos;






        public delegate void DELEGATE_KomaHouse_Foreach(Kifu_Document kifuD, RO_KomaPos koma, ref bool toBreak);
        public void Foreach_Items(Kifu_Document kifuD, DELEGATE_KomaHouse_Foreach delegate_KomaHouse_Foreach)
        {
            bool toBreak = false;

            foreach (RO_KomaPos koma in this.Stars)
            {
                delegate_KomaHouse_Foreach(kifuD, koma, ref toBreak);

                if (toBreak)
                {
                    break;
                }
            }
        }

    }
}
