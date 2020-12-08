using System;
using System.Text.RegularExpressions;
using Grayscale.KifuwaraneLib.Entities.ApplicatedGame;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.Entities.Sfen;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneLib.L06_KifuIO
{
    /// <summary>
    /// 「7g7f」といった記述を、棋譜データの持ち方に変換します。
    /// </summary>
    public abstract class TuginoItte_Sfen
    {
        /// <summary>
        /// 「lnsgkgsn1/1r5b1/ppppppppp/9/9/9/PPPPPPPPP/1B5R1/LNSGKGSNL w - 1」といった記述を解析します。
        /// </summary>
        /// <returns></returns>
        public static bool GetDataStartpos_FromText(string text, out string restText, out SfenStartpos sfenStartpos)
        {
            System.Console.WriteLine("TranslateSfenStartpos: text=" + text);

            bool successful = false;
            sfenStartpos = null;

            restText = text;

            //------------------------------------------------------------
            // リスト作成
            //------------------------------------------------------------
            Regex regex = new Regex(
                @"^\s*" +
                @"([123456789KRBGSNLPkrbgsnlp\+]+)/" +//1段目
                @"([123456789KRBGSNLPkrbgsnlp\+]+)/" +//2段目
                @"([123456789KRBGSNLPkrbgsnlp\+]+)/" +//3段目
                @"([123456789KRBGSNLPkrbgsnlp\+]+)/" +//4段目
                @"([123456789KRBGSNLPkrbgsnlp\+]+)/" +//5段目
                @"([123456789KRBGSNLPkrbgsnlp\+]+)/" +//6段目
                @"([123456789KRBGSNLPkrbgsnlp\+]+)/" +//7段目
                @"([123456789KRBGSNLPkrbgsnlp\+]+)/" +//8段目
                @"([123456789KRBGSNLPkrbgsnlp\+]+) " +//9段目
                @"(b|w) " +//先後
                @"\-?" +
                @"(?:K(\d+))?" +//▲王
                @"(?:R(\d+))?" +//▲飛
                @"(?:B(\d+))?" +//▲角
                @"(?:G(\d+))?" +//▲金
                @"(?:S(\d+))?" +//▲銀
                @"(?:N(\d+))?" +//▲桂
                @"(?:L(\d+))?" +//▲香
                @"(?:P(\d+))?" +//▲歩
                @"(?:k(\d+))?" +//△王
                @"(?:r(\d+))?" +//△飛
                @"(?:b(\d+))?" +//△角
                @"(?:g(\d+))?" +//△金
                @"(?:s(\d+))?" +//△銀
                @"(?:n(\d+))?" +//△桂
                @"(?:l(\d+))?" +//△香
                @"(?:p(\d+))?" +//△歩
                @" (\d+)" +//手目
                @"",
                RegexOptions.Singleline
            );

            MatchCollection mc = regex.Matches(text);
            foreach (Match m in mc)
            {
                if (0 < m.Groups.Count)
                {
                    successful = true;

                    // 残りのテキスト
                    restText = text.Substring(0, m.Index) + text.Substring(m.Index + m.Length, text.Length - (m.Index + m.Length));

                    sfenStartpos = new SfenStartpos(
                        m.Groups[1].Value,  //1段目
                        m.Groups[2].Value,  //2段目
                        m.Groups[3].Value,  //3段目
                        m.Groups[4].Value,  //4段目
                        m.Groups[5].Value,  //5段目
                        m.Groups[6].Value,  //6段目
                        m.Groups[7].Value,  //7段目
                        m.Groups[8].Value,  //8段目
                        m.Groups[9].Value,  //9段目
                        m.Groups[10].Value,  //先後
                        m.Groups[11].Value,  //▲王
                        m.Groups[12].Value,  //▲飛
                        m.Groups[13].Value,  //▲角
                        m.Groups[14].Value,  //▲金
                        m.Groups[15].Value,  //▲銀
                        m.Groups[16].Value,  //▲桂
                        m.Groups[17].Value,  //▲香
                        m.Groups[18].Value,  //▲歩
                        m.Groups[19].Value,  //△王
                        m.Groups[20].Value,  //△飛
                        m.Groups[21].Value,  //△角
                        m.Groups[22].Value,  //△金
                        m.Groups[23].Value,  //△銀
                        m.Groups[24].Value,  //△桂
                        m.Groups[25].Value,  //△香
                        m.Groups[26].Value,  //△歩
                        m.Groups[27].Value  //手目
                        );
                }

                // 最初の１件だけ処理して終わります。
                break;
            }

            restText = restText.Trim();

            return successful;
        }


        /// <summary>
        /// テキスト形式の符号「7g7f 3c3d 6g6f…」の最初の要素を、切り取ってプロセスに変換します。
        /// 
        /// 再生、コマ送りで利用。
        /// </summary>
        /// <returns></returns>
        public static bool GetData_FromText(
            string text,
            out string restText,
            out IMove move,
            Kifu_Document kifuD,
            ILogName logTag
            )
        {
            bool successful = false;
            move = null;
            restText = text;

            //System.Console.WriteLine("TuginoItte_Sfen.GetData_FromText:text=[" + text + "]");

            try
            {
                // Sfenの指し手解析
                Regex regex = new Regex(
                    @"^\s*([123456789PLNSGKRB])([abcdefghi\*])([123456789])([abcdefghi])(\+)?",
                    RegexOptions.Singleline
                );

                MatchCollection mc = regex.Matches(text);
                foreach (Match m in mc)
                {
                    try
                    {
                        if (0 < m.Groups.Count)
                        {
                            successful = true;

                            // 残りのテキスト
                            restText = text.Substring(0, m.Index) + text.Substring(m.Index + m.Length, text.Length - (m.Index + m.Length));

                            var moveB = new SfenMoveBuilder()
                            {
                                Str1st = m.Groups[1].Value.ToCharArray()[0], // 123456789 か、 PLNSGKRB
                                Str2nd = m.Groups[2].Value.ToCharArray()[0], // abcdefghi か、 *
                                Str3rd = m.Groups[3].Value.ToCharArray()[0], // 123456789
                                Str4th = m.Groups[4].Value.ToCharArray()[0], // abcdefghi
                            };
                            if ("+" == m.Groups[5].Value) // + か、無し。
                            {
                                moveB.Str5th = m.Groups[5].Value.ToCharArray()[0];
                            }

                            ApplicatedMove.GetData_FromTextSub(
                                moveB.Build(),
                                out move,
                                kifuD,
                                logTag
                                );
                        }

                        // 最初の１件だけ処理して終わります。
                        break;
                    }
                    catch (Exception ex)
                    {
                        // エラーが起こりました。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        // どうにもできないので  ログだけ取って無視します。
                        string message = "TuginoItte_Sfen.GetData_FromText（A）：" + ex.GetType().Name + "：" + ex.Message + "：text=「" + text + "」　m.Groups.Count=「" + m.Groups.Count + "」";
                        Logger.ErrorLine(Logs.LoggerError, message);

                        // 追加
                        throw;
                    }
                }

                restText = restText.Trim();
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                string message = "TuginoItte_Sfen.GetData_FromText（B）：" + ex.GetType().Name + "：" + ex.Message + "：text=「" + text + "」";
                Logger.ErrorLine(Logs.LoggerError, message);
            }

            return successful;
        }
    }
}
