using System.Collections.Generic;
using System.Text.RegularExpressions;
using Grayscale.Kifuwarane.Entities.ApplicatedGame;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.JapaneseView;
using Grayscale.Kifuwarane.Entities.Logging;

namespace Grayscale.Kifuwarane.Entities.UseCase
{
    /// <summary>
    /// 符号(*1)を元に、棋譜データの持ち方に変換します。
    /// 
    ///         *1…「▲７六歩」といった記述。
    ///         
    /// </summary>
    public abstract class TuginoItte_JapanFugo
    {
        static readonly Regex regexOfJapaneseRecord = new Regex(
            @"^\s*([▲△]?)(?:([123456789１２３４５６７８９])([123456789１２３４５６７８９一二三四五六七八九]))?(同)?[\s　]*(歩|香|桂|銀|金|飛|角|王|玉|と|成香|成桂|成銀|竜|龍|馬)(右|左|直)?(寄|引|上)?(成|不成)?(打?)",
            RegexOptions.Singleline | RegexOptions.Compiled
        );

        /// <summary>
        /// テキスト形式の符号「▲７六歩△３四歩▲２六歩…」の最初の要素を、切り取ってプロセスに変換します。
        /// 
        /// 再生、コマ送りで利用。
        /// </summary>
        /// <returns></returns>
        public static bool GetData_FromText(
            string text, out string restText, out IMove process, TreeDocument kifuD)
        {
            process = null;
            bool successful = false;
            restText = text;


            //------------------------------------------------------------
            // リスト作成
            //------------------------------------------------------------
            MatchCollection mc = regexOfJapaneseRecord.Matches(text);
            foreach (Match m in mc)
            {
                if (0 < m.Groups.Count)
                {
                    successful = true;

                    // 残りのテキスト
                    restText = text.Substring(0, m.Index) + text.Substring(m.Index + m.Length, text.Length - (m.Index + m.Length));

                    TuginoItte_JapanFugo.GetData_FromTextSub(
                        m.Groups[1].Value,  //▲△
                        m.Groups[2].Value,  //123…9、１２３…９、一二三…九
                        m.Groups[3].Value,  //123…9、１２３…９、一二三…九
                        m.Groups[4].Value,  // “同”
                        m.Groups[5].Value,  //(歩|香|桂|…
                        m.Groups[6].Value,           // 右|左…
                        m.Groups[7].Value,  // 上|引
                        m.Groups[8].Value, //成|不成
                        m.Groups[9].Value,  //打
                        out process,
                        kifuD
                        );
                }

                // 最初の１件だけ処理して終わります。
                break;
            }

            restText = restText.Trim();

            return successful;
        }


        /// <summary>
        /// 次の１手データを作ります(*1)
        /// 
        ///         *1…符号１「▲６８銀上」を元に、「7968」を作ります。
        /// 
        /// ＜再生、コマ送りで呼び出されます＞
        /// </summary>
        /// <returns></returns>
        private static void GetData_FromTextSub(
            string strSengo, //▲△
            string strSuji, //123…9、１２３…９、一二三…九
            string strDan, //123…9、１２３…９、一二三…九
            string strDou, // “同”
            string strSrcSyurui, //(歩|香|桂|…
            string strMigiHidari,           // 右|左…
            string strAgaruHiku, // 上|引
            string strNariFunari, //成|不成
            string strDaHyoji, //打
            out IMove process,
            TreeDocument kifuD
            )
        {
            int lastTeme = kifuD.CountTeme(kifuD.Current8);

            //------------------------------
            // 符号確定
            //------------------------------
            MigiHidari migiHidari = GameTranslator.Str_ToMigiHidari(strMigiHidari);
            AgaruHiku agaruHiku = GameTranslator.Str_ToAgaruHiku(strAgaruHiku);            // 上|引
            NariFunari nariFunari = GameTranslator.Nari_ToBool(strNariFunari);//成
            DaHyoji daHyoji = GameTranslator.Str_ToDaHyoji(strDaHyoji);             //打

            Ks14 srcSyurui = ApplicatedMove.KomaMoji_ToSyurui(strSrcSyurui);


            //------------------------------
            // 
            //------------------------------
            Sengo sengo = GameTranslator.Sengo_ToEnum(strSengo);


            M201 dstMasu;

            if ("同" == strDou)
            {
                // 1手前の筋、段を求めるのに使います。
                dstMasu = kifuD.ElementAt8(kifuD.CountTeme(kifuD.Current8)).TeProcess.Star.Masu;
            }
            else
            {
                dstMasu = M201Util.OkibaSujiDanToMasu(
                    Okiba.ShogiBan,
                    GameTranslator.ArabiaNumericToInt(strSuji),
                    GameTranslator.ArabiaNumericToInt(strDan)
                    );
            }

            // TODO: 駒台にあるかもしれない。
            Okiba srcOkiba1 = Okiba.ShogiBan; //Okiba.FUMEI_FUGO_YOMI_CHOKUGO;// Okiba.SHOGIBAN;
            if (DaHyoji.Visible == daHyoji)
            {
                if (Sengo.Gote == sengo)
                {
                    srcOkiba1 = Okiba.Gote_Komadai;
                }
                else
                {
                    srcOkiba1 = Okiba.Sente_Komadai;
                }
            }

            // 
            M201 dst1 = dstMasu;
            //KomaPos dst1 = KomaPos.Create_GetData_FromTextSub(
            //    sengo,
            //    dstMasuHandle,
            //    srcSyurui,
            //    Kh184.n000_未設定
            //    );

            K40 foundKoma = K40.Error;

            //----------
            // 駒台の駒を(明示的に)打つなら
            //----------
            bool utsu = false;//駒台の駒を打つなら真
            if (DaHyoji.Visible == daHyoji)
            {
                utsu = true;
                goto gt_EndShogiban;
            }

            if (Ks14.H01_Fu == srcSyurui)
            {
                #region 歩
                //************************************************************
                // 歩
                //************************************************************

                //----------
                // 候補マス
                //----------
                //┌─┬─┬─┐
                //│  │  │  │
                //├─┼─┼─┤
                //│  │至│  │
                //├─┼─┼─┤
                //│  │Ｅ│  │
                //└─┴─┴─┘
                bool isE = true;

                IMasus srcAll = new Masus_Set();
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し引(sengo, dst1)); }

                if (TuginoItte_JapanFugo.Hit(sengo, srcSyurui, srcAll, kifuD, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (Ks14.H07_Hisya == srcSyurui)
            {
                #region 飛
                //************************************************************
                // 飛
                //************************************************************
                //----------
                // 候補マス
                //----------
                //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //  │  │  │  │  │  │  │  │  │A7│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A6│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A5│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A4│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │  │  │  │  │  │A3│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A2│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A1│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A0│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │G7│G6│G5│G4│G3│G2│G1│G0│至│C0│C1│C2│C3│C4│C5│C6│C7│
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E0│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E1│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E2│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E3│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │  │  │  │  │  │E4│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E5│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E6│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E7│  │  │  │  │  │  │  │  │
                //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
                bool isA = true;
                bool isC = true;
                bool isE = true;
                bool isG = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isA = false;
                        isE = false;
                        break;
                    case AgaruHiku.Agaru:
                        isA = false;
                        isC = false;
                        isG = false;
                        break;
                    case AgaruHiku.Hiku:
                        isC = false;
                        isE = false;
                        isG = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isA = false;
                        isE = false;
                        isG = false;
                        break;
                    case MigiHidari.Hidari:
                        isA = false;
                        isC = false;
                        isE = false;
                        break;
                    case MigiHidari.Sugu:
                        isA = false;
                        isC = false;
                        isG = false;
                        break;
                }

                IMasus srcAll = new Masus_Set();
                if (isA) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し上(sengo, dst1)); }
                if (isC) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し射(sengo, dst1)); }
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し引(sengo, dst1)); }
                if (isG) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し滑(sengo, dst1)); }

                if (TuginoItte_JapanFugo.Hit(sengo, srcSyurui, srcAll, kifuD, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (Ks14.H08_Kaku == srcSyurui)
            {
                #region 角
                //************************************************************
                // 角
                //************************************************************
                //----------
                // 候補マス
                //----------
                //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //  │H7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │B7│
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │H6│  │  │  │  │  │  │  │  │  │  │  │  │  │B6│  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │H5│  │  │  │  │  │  │  │  │  │  │  │B5│  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │H4│  │  │  │  │  │  │  │  │  │B4│  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │  │H3│  │  │  │  │  │  │  │B3│  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │H2│  │  │  │  │  │B2│  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │H1│  │  │  │B1│  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │H0│  │B0│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │至│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │F0│  │D0│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │F1│  │  │  │D1│  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │F2│  │  │  │  │  │D2│  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │F3│  │  │  │  │  │  │  │D3│  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │F4│  │  │  │  │  │  │  │  │  │D4│  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │F5│  │  │  │  │  │  │  │  │  │  │  │D5│  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │F6│  │  │  │  │  │  │  │  │  │  │  │  │  │D6│  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │F7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │D7│
                //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
                bool isB = true;
                bool isD = true;
                bool isF = true;
                bool isH = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isB = false;
                        isD = false;
                        isF = false;
                        isH = false;
                        break;
                    case AgaruHiku.Agaru:
                        isB = false;
                        isH = false;
                        break;
                    case AgaruHiku.Hiku:
                        isB = false;
                        isH = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isF = false;
                        isH = false;
                        break;
                    case MigiHidari.Hidari:
                        isB = false;
                        isD = false;
                        break;
                    case MigiHidari.Sugu:
                        isD = false;
                        isF = false;
                        break;
                }

                Masus_Set srcAll = new Masus_Set();
                if (isB) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し昇(sengo, dst1)); }
                if (isD) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し沈(sengo, dst1)); }
                if (isF) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し降(sengo, dst1)); }
                if (isH) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し浮(sengo, dst1)); }

                //----------
                // 候補マスＢ
                //----------
                if (TuginoItte_JapanFugo.Hit(sengo, srcSyurui, srcAll, kifuD, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (Ks14.H02_Kyo == srcSyurui)
            {
                #region 香
                //************************************************************
                // 香
                //************************************************************
                //----------
                // 候補マス
                //----------
                //  ┌─┬─┬─┐
                //  │  │至│  │
                //  ├─┼─┼─┤
                //  │  │E0│  │
                //  ├─┼─┼─┤
                //  │  │E1│  │
                //  ├─┼─┼─┤
                //  │  │E2│  │
                //  ├─┼─┼─┤
                //  │  │E3│  │
                //  ├─┼─┼─┤
                //  │  │E4│  │
                //  ├─┼─┼─┤
                //  │  │E5│  │
                //  ├─┼─┼─┤
                //  │  │E6│  │
                //  ├─┼─┼─┤
                //  │  │E7│  │
                //  └─┴─┴─┘
                bool isE = true;

                IMasus srcAll = new Masus_Set();
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し引(sengo, dst1)); }

                if (TuginoItte_JapanFugo.Hit(sengo, srcSyurui, srcAll, kifuD, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (Ks14.H03_Kei == srcSyurui)
            {
                #region 桂
                //************************************************************
                // 桂
                //************************************************************
                //----------
                // 候補マス
                //----------
                //┌─┐　┌─┐
                //│  │　│  │
                //├─┼─┼─┤
                //│　│  │  │
                //├─┼─┼─┤
                //│　│至│  │先手から見た図
                //├─┼─┼─┤
                //│　│  │  │
                //├─┼─┼─┤
                //│Ｊ│　│Ｉ│
                //└─┘　└─┘
                bool isI = true;
                bool isJ = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isI = false;
                        isJ = false;
                        break;
                    case AgaruHiku.Agaru:
                        break;
                    case AgaruHiku.Hiku:
                        isI = false;
                        isJ = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isJ = false;
                        break;
                    case MigiHidari.Hidari:
                        isI = false;
                        break;
                    case MigiHidari.Sugu:
                        isI = false;
                        isJ = false;
                        break;
                }

                IMasus srcAll = new Masus_Set();
                if (isI) { srcAll.AddSupersets(KomanoKidou.SrcKeimatobi_巻戻し跳(sengo, dst1)); }
                if (isJ) { srcAll.AddSupersets(KomanoKidou.SrcKeimatobi_巻戻し駆(sengo, dst1)); }

                if (TuginoItte_JapanFugo.Hit(sengo, srcSyurui, srcAll, kifuD, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (Ks14.H04_Gin == srcSyurui)
            {
                #region 銀
                //************************************************************
                // 銀
                //************************************************************
                //----------
                // 候補マス
                //----------
                //┌─┬─┬─┐
                //│Ｈ│  │Ｂ│
                //├─┼─┼─┤
                //│　│至│  │先手から見た図
                //├─┼─┼─┤
                //│Ｆ│Ｅ│Ｄ│
                //└─┴─┴─┘
                bool isB = true;
                bool isD = true;
                bool isE = true;
                bool isF = true;
                bool isH = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isB = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        isH = false;
                        break;
                    case AgaruHiku.Agaru:
                        isB = false;
                        isH = false;
                        break;
                    case AgaruHiku.Hiku:
                        isD = false;
                        isE = false;
                        isF = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isE = false;
                        isF = false;
                        isH = false;
                        break;
                    case MigiHidari.Hidari:
                        isB = false;
                        isD = false;
                        isE = false;
                        break;
                    case MigiHidari.Sugu:
                        isB = false;
                        isD = false;
                        isF = false;
                        isH = false;
                        break;
                }

                IMasus srcAll = new Masus_Set();
                if (isB) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し昇(sengo, dst1)); }
                if (isD) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し沈(sengo, dst1)); }
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し引(sengo, dst1)); }
                if (isF) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し降(sengo, dst1)); }
                if (isH) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し浮(sengo, dst1)); }

                if (TuginoItte_JapanFugo.Hit(sengo, srcSyurui, srcAll, kifuD, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (
                Ks14.H05_Kin == srcSyurui
                || Ks14.H11_Tokin == srcSyurui
                || Ks14.H12_NariKyo == srcSyurui
                || Ks14.H13_NariKei == srcSyurui
                || Ks14.H14_NariGin == srcSyurui
                )
            {
                #region △金、△と金、△成香、△成桂、△成銀
                //************************************************************
                // △金、△と金、△成香、△成桂、△成銀
                //************************************************************
                //----------
                // 候補マス
                //----------
                //┌─┬─┬─┐
                //│  │Ａ│  │
                //├─┼─┼─┤
                //│Ｇ│至│Ｃ│先手から見た図
                //├─┼─┼─┤
                //│Ｆ│Ｅ│Ｄ│
                //└─┴─┴─┘
                bool isA = true;
                bool isC = true;
                bool isD = true;
                bool isE = true;
                bool isF = true;
                bool isG = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isA = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        break;
                    case AgaruHiku.Agaru:
                        isA = false;
                        isC = false;
                        isG = false;
                        break;
                    case AgaruHiku.Hiku:
                        isC = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        isG = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isA = false;
                        isE = false;
                        isF = false;
                        isG = false;
                        break;
                    case MigiHidari.Hidari:
                        isA = false;
                        isC = false;
                        isD = false;
                        isE = false;
                        break;
                    case MigiHidari.Sugu:
                        isA = false;
                        isC = false;
                        isD = false;
                        isF = false;
                        isG = false;
                        break;
                }

                IMasus srcAll = new Masus_Set();
                if (isA) { srcAll.AddSupersets(KomanoKidou.SrcIppoA_巻戻し上(sengo, dst1)); }
                if (isC) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し射(sengo, dst1)); }
                if (isD) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し沈(sengo, dst1)); }
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し引(sengo, dst1)); }
                if (isF) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し降(sengo, dst1)); }
                if (isG) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し滑(sengo, dst1)); }

                if (TuginoItte_JapanFugo.Hit(sengo, srcSyurui, srcAll, kifuD, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (Ks14.H06_Oh == srcSyurui)
            {
                #region 王
                //************************************************************
                // 王
                //************************************************************

                //----------
                // 候補マス
                //----------
                //┌─┬─┬─┐
                //│Ｈ│Ａ│Ｂ│
                //├─┼─┼─┤
                //│Ｇ│至│Ｃ│先手から見た図
                //├─┼─┼─┤
                //│Ｆ│Ｅ│Ｄ│
                //└─┴─┴─┘
                bool isA = true;
                bool isB = true;
                bool isC = true;
                bool isD = true;
                bool isE = true;
                bool isF = true;
                bool isG = true;
                bool isH = true;

                IMasus srcAll = new Masus_Set();
                if (isA) { srcAll.AddSupersets(KomanoKidou.SrcIppoA_巻戻し上(sengo, dst1)); }
                if (isB) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し昇(sengo, dst1)); }
                if (isC) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し射(sengo, dst1)); }
                if (isD) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し沈(sengo, dst1)); }
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し引(sengo, dst1)); }
                if (isF) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し降(sengo, dst1)); }
                if (isG) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し滑(sengo, dst1)); }
                if (isH) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し浮(sengo, dst1)); }

                // 王は１つです。
                if (TuginoItte_JapanFugo.Hit(sengo, srcSyurui, srcAll, kifuD, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (Ks14.H09_Ryu == srcSyurui)
            {
                #region 竜
                //************************************************************
                // 竜
                //************************************************************

                //----------
                // 候補マス
                //----------
                //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //  │  │  │  │  │  │  │  │  │A7│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A6│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A5│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A4│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │  │  │  │  │  │A3│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A2│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A1│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │Ｈ│A0│Ｂ│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │G7│G6│G5│G4│G3│G2│G1│G0│至│C0│C1│C2│C3│C4│C5│C6│C7│
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │Ｆ│E0│Ｄ│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E1│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E2│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E3│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │  │  │  │  │  │E4│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E5│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E6│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E7│  │  │  │  │  │  │  │  │
                //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
                bool isA = true;
                bool isB = true;
                bool isC = true;
                bool isD = true;
                bool isE = true;
                bool isF = true;
                bool isG = true;
                bool isH = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isA = false;
                        isB = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        isH = false;
                        break;
                    case AgaruHiku.Agaru:
                        isA = false;
                        isB = false;
                        isC = false;
                        isG = false;
                        isH = false;
                        break;
                    case AgaruHiku.Hiku:
                        isC = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        isG = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isA = false;
                        isE = false;
                        isF = false;
                        isG = false;
                        isH = false;
                        break;
                    case MigiHidari.Hidari:
                        isA = false;
                        isB = false;
                        isC = false;
                        isD = false;
                        isE = false;
                        break;
                    case MigiHidari.Sugu:
                        isA = false;
                        isB = false;
                        isC = false;
                        isD = false;
                        isF = false;
                        isG = false;
                        isH = false;
                        break;
                }

                IMasus srcAll = new Masus_Set();
                if (isA) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し上(sengo, dst1)); }
                if (isB) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し昇(sengo, dst1)); }
                if (isC) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し射(sengo, dst1)); }
                if (isD) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し沈(sengo, dst1)); }
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し引(sengo, dst1)); }
                if (isF) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し降(sengo, dst1)); }
                if (isG) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し滑(sengo, dst1)); }
                if (isH) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し浮(sengo, dst1)); }

                if (TuginoItte_JapanFugo.Hit(sengo, srcSyurui, srcAll, kifuD, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (Ks14.H10_Uma == srcSyurui)
            {
                #region 馬
                //************************************************************
                // 馬
                //************************************************************
                //----------
                // 候補マス
                //----------
                //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //  │H7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │B7│
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │H6│  │  │  │  │  │  │  │  │  │  │  │  │  │B6│  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │H5│  │  │  │  │  │  │  │  │  │  │  │B5│  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │H4│  │  │  │  │  │  │  │  │  │B4│  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │  │H3│  │  │  │  │  │  │  │B3│  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │H2│  │  │  │  │  │B2│  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │H1│  │  │  │B1│  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │H0│Ａ│B0│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │Ｇ│至│Ｃ│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │F0│Ｅ│D0│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │F1│  │  │  │D1│  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │F2│  │  │  │  │  │D2│  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │F3│  │  │  │  │  │  │  │D3│  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │F4│  │  │  │  │  │  │  │  │  │D4│  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │F5│  │  │  │  │  │  │  │  │  │  │  │D5│  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │F6│  │  │  │  │  │  │  │  │  │  │  │  │  │D6│  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │F7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │D7│
                //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
                bool isA = true;
                bool isB = true;
                bool isC = true;
                bool isD = true;
                bool isE = true;
                bool isF = true;
                bool isG = true;
                bool isH = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isA = false;
                        isB = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        isH = false;
                        break;
                    case AgaruHiku.Agaru:
                        isA = false;
                        isB = false;
                        isC = false;
                        isG = false;
                        isH = false;
                        break;
                    case AgaruHiku.Hiku:
                        isC = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        isG = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isA = false;
                        isE = false;
                        isF = false;
                        isG = false;
                        isH = false;
                        break;
                    case MigiHidari.Hidari:
                        isA = false;
                        isB = false;
                        isC = false;
                        isD = false;
                        isE = false;
                        break;
                    case MigiHidari.Sugu:
                        isA = false;
                        isB = false;
                        isC = false;
                        isD = false;
                        isF = false;
                        isG = false;
                        isH = false;
                        break;
                }

                IMasus srcAll = new Masus_Set();
                if (isA) { srcAll.AddSupersets(KomanoKidou.SrcIppoA_巻戻し上(sengo, dst1)); }
                if (isB) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し昇(sengo, dst1)); }
                if (isC) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し射(sengo, dst1)); }
                if (isD) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し沈(sengo, dst1)); }
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し引(sengo, dst1)); }
                if (isF) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し降(sengo, dst1)); }
                if (isG) { srcAll.AddSupersets(KomanoKidou.SrcIppo_巻戻し滑(sengo, dst1)); }
                if (isH) { srcAll.AddSupersets(KomanoKidou.SrcKantu_巻戻し浮(sengo, dst1)); }

                if (TuginoItte_JapanFugo.Hit(sengo, srcSyurui, srcAll, kifuD, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else
            {
                #region エラー
                //************************************************************
                // エラー
                //************************************************************

                #endregion
            }

        gt_EndShogiban:

            if (K40.Error == foundKoma && utsu)
            {
                // 駒台の駒を(明示的に)打ちます。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                List<K40> komas = Util_KyokumenReader.Komas_ByOkibaSengoSyurui(kifuD,
                    srcOkiba1, sengo, srcSyurui);

                if (0 < komas.Count)
                {
                    switch (sengo)
                    {
                        case Sengo.Gote:
                            srcOkiba1 = Okiba.Gote_Komadai;
                            break;
                        case Sengo.Sente:
                            srcOkiba1 = Okiba.Sente_Komadai;
                            break;
                        default:
                            srcOkiba1 = Okiba.Empty;
                            break;
                    }

                    foundKoma = komas[0];
                    goto gt_EndSyurui;
                }
            }

        gt_EndSyurui:


            int srcMasuHandle1;

            if (K40.Error != foundKoma)
            {
                // 将棋盤の上に駒がありました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                IKifuElement dammyNode6 = kifuD.ElementAt8(lastTeme);
                PositionKomaHouse house4 = dammyNode6.KomaHouse;

                srcMasuHandle1 = (int)house4.KomaPosAt(foundKoma).Star.Masu;
            }
            else
            {
                // （符号に書かれていませんが）「打」のとき。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                switch (sengo)
                {
                    case Sengo.Gote:
                        srcOkiba1 = Okiba.Gote_Komadai;
                        break;
                    case Sengo.Sente:
                        srcOkiba1 = Okiba.Sente_Komadai;
                        break;
                    default:
                        srcOkiba1 = Okiba.Empty;
                        break;
                }



                List<K40> komaHandles = Util_KyokumenReader.Komas_ByOkibaSengoSyurui(kifuD,
                    srcOkiba1, sengo, srcSyurui);//(2014-10-04 12:46)変更
                // 1個はヒットするはず
                K40 hitKoma = komaHandles[0];//▲！コマ送りボタンを連打すると、エラーになります。

                IKifuElement dammyNode6 = kifuD.ElementAt8(lastTeme);
                PositionKomaHouse house4 = dammyNode6.KomaHouse;
                srcMasuHandle1 = (int)house4.KomaPosAt(hitKoma).Star.Masu;
            }


            Ks14 dstSyurui;
            if (NariFunari.Nari == nariFunari)
            {
                // 成ります
                dstSyurui = KomaSyurui14Array.NariCaseHandle[(int)srcSyurui];
            }
            else
            {
                // そのままです。
                dstSyurui = srcSyurui;
            }


            // １手を、データにします。
            process = MoveImpl.Next3(

                new RO_StarManual(
                    sengo,
                    M201Util.HandleToMasu(srcMasuHandle1),
                    srcSyurui
                ),

                new RO_StarManual(
                    sengo,
                    dstMasu,//符号は将棋盤の升目です。
                    dstSyurui
                ),

                Ks14.H00_Null // 符号からは、取った駒の種類は分からないんだぜ☆　だがバグではない☆　あとで調べる☆
            );
        }


        /// <summary>
        /// 将棋盤上での検索
        /// </summary>
        /// <param name="srcAll">候補マス</param>
        /// <param name="komas"></param>
        /// <returns></returns>
        private static bool Hit(
            Sengo sengo, Ks14 syurui, IMasus srcAll, TreeDocument kifuD, out K40 foundKoma)
        {
            bool hit = false;
            foundKoma = K40.Error;

            int lastTeme = kifuD.CountTeme(kifuD.Current8);

            foreach (M201 masu1 in srcAll.Elements)//筋・段。（先後、種類は入っていません）
            {
                foreach (K40 koma in K40Array.Items_KomaOnly)
                {
                    IKifuElement dammyNode6 = kifuD.ElementAt8(lastTeme);
                    PositionKomaHouse house4 = dammyNode6.KomaHouse;

                    IKomaPos komaP2 = house4.KomaPosAt(koma);

                    if (sengo == komaP2.Star.Sengo
                        && Okiba.ShogiBan == M201Util.GetOkiba(komaP2.Star.Masu)
                        && KomaSyurui14Array.Matches(syurui, Haiyaku184Array.Syurui(komaP2.Star.Haiyaku))
                        && masu1 == komaP2.Star.Masu
                        )
                    {
                        // 候補マスにいた
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        hit = true;
                        foundKoma = koma;// K40Array.Items_All[hKoma];
                        break;
                    }
                }

                //for (int hKoma = 0; hKoma < kifuD.Kifu_Old.KomaDoorsLength; hKoma++)//全駒
                //{
                //    RO_KomaPos komaP2 = kifuD.Kifu_Old.KomaDoorsAt(hKoma);

                //    if (sengo == komaP2.Sengo
                //        && Okiba.ShogiBan == M201Util.GetOkiba(komaP2.Masu)
                //        && KomaSyurui14Array.Matches(syurui, Haiyaku184Array.Syurui(komaP2.Haiyaku))
                //        && masu1 == komaP2.Masu
                //        )
                //    {
                //        // 候補マスにいた
                //        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //        hit = true;
                //        foundKoma = K40Array.Items_All[ hKoma];
                //        break;
                //    }
                //}
            }

            return hit;
        }
    }


}
