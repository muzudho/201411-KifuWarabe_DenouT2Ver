using System.Text;
using Grayscale.KifuwaraneLib.Entities.PositionTranslation;
using Grayscale.KifuwaraneLib.L03_Communication;

namespace Grayscale.KifuwaraneLib.L04_Common
{
    public abstract class MasusWriter
    {
        /// <summary>
        /// デバッグ用文字列を作ります。
        /// </summary>
        /// <param name="masus"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        public static string DebugString(IMasus masus, string memo)
        {
            StringBuilder sb = new StringBuilder();

            int errorCount = 0;

            // フォルスクリア
            bool[] ban81 = new bool[81];

            // フラグ立て
            foreach (int hMasu in masus.Elements)
            {
                if (Okiba.ShogiBan == PositionTranslator.Masu_ToOkiba( M201Array.Items_All[ hMasu] ))
                {
                    ban81[hMasu] = true;
                }
            }



            sb.AppendLine("...(^▽^)さて、局面は☆？");

            if (null != memo && "" != memo.Trim())
            {
                sb.AppendLine(memo);
            }

            sb.AppendLine("　９　８　７　６　５　４　３　２　１");
            sb.AppendLine("┏━┯━┯━┯━┯━┯━┯━┯━┯━┓");
            for (int dan = 1; dan <= 9; dan++)
            {
                sb.Append("┃");
                for (int suji = 9; suji >= 1; suji--)// 筋は左右逆☆
                {
                    M201 masu = M201Util.OkibaSujiDanToMasu(Okiba.ShogiBan, suji, dan);
                    if (Okiba.ShogiBan== PositionTranslator.Masu_ToOkiba(masu))
                    {
                        if (ban81[(int)masu])
                        {
                            sb.Append("●");
                        }
                        else
                        {
                            sb.Append("  ");
                        }
                    }
                    else
                    {
                        errorCount++;
                        sb.Append("  ");
                    }


                    if (suji == 1)//１筋が最後だぜ☆
                    {
                        sb.Append("┃");
                        sb.AppendLine(PositionTranslator.IntToJapanese(dan));
                    }
                    else
                    {
                        sb.Append("│");
                    }
                }

                if (dan == 9)
                {
                    sb.AppendLine("┗━┷━┷━┷━┷━┷━┷━┷━┷━┛");
                }
                else
                {
                    sb.AppendLine("┠─┼─┼─┼─┼─┼─┼─┼─┼─┨");
                }
            }


            // 後手駒台
            sb.Append("エラー数：");
            sb.AppendLine(errorCount.ToString());
            sb.AppendLine("...(^▽^)ﾄﾞｳﾀﾞｯﾀｶﾅ～☆");


            return sb.ToString();
        }

    }
}
