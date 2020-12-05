using System;
using System.Text;
using System.Text.RegularExpressions;
using Grayscale.KifuwaraneLib.L01_Log;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneLib.L06_KifuIO
{
    /// <summary>
    /// 「7g7f」といった記述を、棋譜データの持ち方に変換します。
    /// </summary>
    public abstract class TuginoItte_Sfen
    {
        /// <summary>
        /// ************************************************************************************************************************
        /// 「lnsgkgsn1/1r5b1/ppppppppp/9/9/9/PPPPPPPPP/1B5R1/LNSGKGSNL w - 1」といった記述を解析します。
        /// ************************************************************************************************************************
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
        /// ************************************************************************************************************************
        /// テキスト形式の符号「7g7f 3c3d 6g6f…」の最初の要素を、切り取ってプロセスに変換します。
        /// ************************************************************************************************************************
        /// 
        /// 再生、コマ送りで利用。
        /// </summary>
        /// <returns></returns>
        public static bool GetData_FromText(
            string text,
            out string restText,
            out ITeProcess process,
            Kifu_Document kifuD,
            ILarabeLoggerTag logTag
            )
        {
            bool successful = false;
            process = null;
            restText = text;

            //System.Console.WriteLine("TuginoItte_Sfen.GetData_FromText:text=[" + text + "]");

            try
            {



                //------------------------------------------------------------
                // リスト作成
                //------------------------------------------------------------
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

                            TuginoItte_Sfen.GetData_FromTextSub(
                                m.Groups[1].Value,  //123456789 か、 PLNSGKRB
                                m.Groups[2].Value,  //abcdefghi か、 *
                                m.Groups[3].Value,  //123456789
                                m.Groups[4].Value,  //abcdefghi
                                m.Groups[5].Value,  //+
                                out process,
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
                        LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
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
                LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
            }

            return successful;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 符号１「7g7f」を元に、process を作ります。
        /// ************************************************************************************************************************
        /// 
        /// ＜再生、コマ送りで呼び出されます＞
        /// </summary>
        /// <returns></returns>
        private static void GetData_FromTextSub(
            string str1, //123456789 か、 PLNSGKRB
            string str2, //abcdefghi か、 *
            string str3, //123456789
            string str4, //abcdefghi
            string strNari, //+
            out ITeProcess process,
            Kifu_Document kifuD,
            ILarabeLoggerTag logTag
            )
        {
            process = RO_TeProcess.NULL_OBJECT;
            int lastTeme = kifuD.CountTeme(kifuD.Current8);

            try
            {
                Ks14 uttaSyurui; // 打った駒の種類

                int srcSuji = RO_TeProcess.CTRL_NOTHING_PROPERTY_SUJI;
                int srcDan = RO_TeProcess.CTRL_NOTHING_PROPERTY_DAN;

                if ("*" == str2)
                {
                    //>>>>>>>>>> 「打」でした。

                    Converter04.SfenUttaSyurui(str1, out uttaSyurui);

                }
                else
                {
                    //>>>>>>>>>> 指しました。
                    uttaSyurui = Ks14.H00_Null;//打った駒はない☆

                    //------------------------------
                    // 1
                    //------------------------------
                    if (!int.TryParse(str1, out srcSuji))
                    {
                    }

                    //------------------------------
                    // 2
                    //------------------------------
                    srcDan = Converter04.Alphabet_ToInt(str2);
                }

                //------------------------------
                // 3
                //------------------------------
                int suji;
                if (!int.TryParse(str3, out suji))
                {
                }

                //------------------------------
                // 4
                //------------------------------
                int dan;
                dan = Converter04.Alphabet_ToInt(str4);



                K40 koma;

                if ("*" == str2)
                {
                    //>>>>> 「打」でした。

                    // 駒台から、打った種類の駒を取得
                    koma = Util_KyokumenReader.Koma_BySyuruiIgnoreCase(kifuD,
                        Converter04.Sengo_ToKomadai(kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8))),//Okiba.Sente_Komadai,//FIXME:
                        uttaSyurui, logTag);
                    if (K40.Error == koma)
                    {
                        string message = "TuginoItte_Sfen#GetData_FromTextSub：駒台から種類[" + uttaSyurui + "]の駒を掴もうとしましたが、エラーでした。";
                        LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
                        throw new Exception(message);
                    }


                    //// FIXME: 打のとき、srcSuji、srcDan が Int.Min
                    //koma = Util_KyokumenReader.Koma_AtMasu(
                    //    kifuD,
                    //    M201Util.OkibaSujiDanToMasu(Okiba.ShogiBan, srcSuji, srcDan)
                    //    );
                }
                else
                {
                    //>>>>> 打ではないとき
                    koma = Util_KyokumenReader.Koma_AtMasu(
                        kifuD,
                        M201Util.OkibaSujiDanToMasu(Okiba.ShogiBan, srcSuji, srcDan),
                        logTag
                        );

                    if (K40.Error == koma)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("TuginoItte_Sfen#GetData_FromTextSub：将棋盤から [");
                        sb.Append(srcSuji);
                        sb.Append("]筋、[");
                        sb.Append(srcDan);
                        sb.Append("]段 にある駒を掴もうとしましたが、エラーでした。");
                        sb.AppendLine();
                        sb.Append(kifuD.DebugText_Kyokumen7(kifuD,"エラー駒になったとき"));

                        for (int i = 0; i <= kifuD.CountTeme(kifuD.Current8); i++)
                        {
                            IKifuElement dammyNode2 = kifuD.ElementAt8(i);
                            KomaHouse house1 = dammyNode2.KomaHouse;

                            sb.Append(house1.Log_Kyokumen(kifuD, i, "エラー駒になったとき(見直し)"));
                        }

                        string message = sb.ToString();
                        LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
                        throw new Exception(message);
                    }
                }


                Ks14 dstSyurui;
                Ks14 srcSyurui;
                Okiba srcOkiba;
                M201 srcMasu;

                IKifuElement dammyNode3 = kifuD.ElementAt8(lastTeme);
                KomaHouse house2 = dammyNode3.KomaHouse;

                if ("*" == str2)
                {
                    //>>>>> 打った駒の場合

                    dstSyurui = uttaSyurui;
                    srcSyurui = uttaSyurui;
                    switch (kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8)))
                    {
                        case Sengo.Gote:
                            srcOkiba = Okiba.Gote_Komadai;
                            break;
                        case Sengo.Sente:
                            srcOkiba = Okiba.Sente_Komadai;
                            break;
                        default:
                            srcOkiba = Okiba.Empty;
                            break;
                    }

                    K40 srcKoma = Util_KyokumenReader.Koma_BySyuruiIgnoreCase(kifuD, srcOkiba, srcSyurui, logTag);
                    srcMasu = house2.KomaPosAt(srcKoma).Star.Masu;// M201Util.OkibaSujiDanToMasu(srcOkiba, srcSuji, srcDan);
                }
                else
                {
                    //>>>>> 盤上の駒を指した場合

                    dstSyurui = Haiyaku184Array.Syurui(house2.KomaPosAt(koma).Star.Haiyaku);
                    srcSyurui = Haiyaku184Array.Syurui(house2.KomaPosAt(koma).Star.Haiyaku); //駒は「元・種類」を記憶していませんので、「現・種類」を指定します。
                    srcOkiba = Okiba.ShogiBan;
                    srcMasu = M201Util.OkibaSujiDanToMasu(srcOkiba, srcSuji, srcDan);
                }


                //------------------------------
                // 5
                //------------------------------
                if ("+" == strNari)
                {
                    // 成りました
                    dstSyurui = KomaSyurui14Array.NariCaseHandle[(int)dstSyurui];
                }


                //------------------------------
                // 結果
                //------------------------------
                // 棋譜
                process = RO_TeProcess.Next3(

                    new RO_StarManual(
                        kifuD.CountSengo(kifuD.CountTeme( kifuD.Current8)),
                        srcMasu,//FIXME:升ハンドルにしたい
                        srcSyurui
                    ),

                    new RO_StarManual(
                        kifuD.CountSengo(kifuD.CountTeme( kifuD.Current8)),
                        M201Util.OkibaSujiDanToMasu(Okiba.ShogiBan, suji, dan),//符号は将棋盤の升目です。 FIXME:升ハンドルにしたい
                        dstSyurui
                        ),

                    Ks14.H00_Null//符号からは、取った駒は分からない
                );
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので　経路と情報を付け足して　更に外側に投げます。
                string message = ex.GetType().Name + "：" + ex.Message + "　in　TuginoItte_Sfen.GetData_FromTextSub（A）　str1=「" + str1 + "」　str2=「" + str2 + "」　str3=「" + str3 + "」　str4=「" + str4 + "」　strNari=「" + strNari + "」　";
                LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
                throw new Exception(message);
            }

        }


    }


}
