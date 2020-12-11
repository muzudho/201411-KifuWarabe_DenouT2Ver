using System.Collections.Generic;
using Grayscale.KifuwaraneEntities.JapaneseView;
using Grayscale.KifuwaraneEntities.Log;

namespace Grayscale.KifuwaraneEntities.ApplicatedGame.Architecture
{
    /// <summary>
    /// J符号作成１５個。
    /// 
    /// ５二金右引　のような文字を作ります。
    /// </summary>
    public abstract class JFugoCreator15Array
    {
        public delegate FugoJ DELEGATE_CreateJFugo(IMove teProcess, TreeDocument kifuD, ILogTag logTag);

        public static DELEGATE_CreateJFugo[] ItemMethods
        {
            get
            {
                return JFugoCreator15Array.itemMethods;
            }
        }
        private static DELEGATE_CreateJFugo[] itemMethods;

        static JFugoCreator15Array()
        {
            // 駒種類ハンドルに対応
            JFugoCreator15Array.itemMethods = new DELEGATE_CreateJFugo[]{
                JFugoCreator15Array.CreateNullKoma,// null,//[0]
                JFugoCreator15Array.CreateFu,//[1]
                JFugoCreator15Array.CreateKyo,
                JFugoCreator15Array.CreateKei,
                JFugoCreator15Array.CreateGin,
                JFugoCreator15Array.CreateKin,
                JFugoCreator15Array.CreateOh,
                JFugoCreator15Array.CreateHisya,
                JFugoCreator15Array.CreateKaku,
                JFugoCreator15Array.CreateRyu,
                JFugoCreator15Array.CreateUma,//[10]
                JFugoCreator15Array.CreateTokin,
                JFugoCreator15Array.CreateNariKyo,
                JFugoCreator15Array.CreateNariKei,
                JFugoCreator15Array.CreateNariGin,
                JFugoCreator15Array.CreateErrorKoma,//[15]
            };
        }


        public static FugoJ CreateNullKoma(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            // エラー
            MigiHidari migiHidari = MigiHidari.No_Print;
            AgaruHiku agaruHiku = AgaruHiku.No_Print;
            NariFunari nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            if (Sengo.Gote == process.Star.Sengo)
            {
                // △後手
            }
            else
            {
                // ▲先手
            }


            //----------
            // TODO: 移動前の駒が成る前かどうか
            //----------
            nari = NariFunari.CTRL_SONOMAMA;

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        /// <summary>
        /// 歩のJ符号を作ります。
        /// </summary>
        /// <param name="process">移動先、移動元、両方のマス番号</param>
        /// <param name="kyokumen"></param>
        /// <returns></returns>
        public static FugoJ CreateFu(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            //************************************************************
            // 歩
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
            //----------
            //┌─┬─┬─┐
            //│  │  │  │
            //├─┼─┼─┤
            //│  │至│  │
            //├─┼─┼─┤
            //│  │Ｅ│  │
            //└─┴─┴─┘
            IMasus srcE = KomanoKidou.SrcIppo_巻戻し引(process.Star.Sengo, process.Star.Masu);
            IMove src = process.Src();

            //----------
            // 棋譜の現局面：競合駒
            //----------
            List<K40> kmE = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcE, logTag);

            if (process.IsDaAction)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (src.ExistsIn(srcE, kifuD, logTag))
            {
                // Ｅにいた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // 歩に右左、引上はありません。
                migiHidari = MigiHidari.No_Print;
                agaruHiku = AgaruHiku.No_Print;
            }
            else
            {
                // どこからか飛んできた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // 歩に右左、引上はありません。
                migiHidari = MigiHidari.No_Print;
                agaruHiku = AgaruHiku.No_Print;
            }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && process.NeverOnaji(kifuD, logTag, kmE)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == process.IsNari && !process.IsDaAction && process.InAitejin)
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariFunari.Funari;
            }
            else if (process.IsNari)
            {
                nari = NariFunari.Nari;
            }
            else
            {
                nari = NariFunari.CTRL_SONOMAMA;
            }

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static FugoJ CreateKyo(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            //************************************************************
            // 香
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

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
            IMasus srcE = KomanoKidou.SrcKantu_巻戻し引(process.Star.Sengo, process.Star.Masu);
            IMove src = process.Src();

            //----------
            // 競合駒
            //----------
            List<K40> kmE = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcE, logTag);

            if (process.IsDaAction)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (src.ExistsIn(srcE, kifuD, logTag))
            {
                //----------
                // 移動前はＥだった
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： 香に右はありません。
            // 「左」解除： 香に左はありません。
            // 「上」解除： 香に上はありません。
            // 「引」解除： 香に引はありません。
            // 「寄」解除： 香に寄はありません。

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && process.NeverOnaji(kifuD, logTag, kmE)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == process.IsNari && !process.IsDaAction && process.InAitejin)
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariFunari.Funari;
            }
            else if (process.IsNari)
            {
                nari = NariFunari.Nari;
            }
            else
            {
                nari = NariFunari.CTRL_SONOMAMA;
            }

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static FugoJ CreateKei(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            //************************************************************
            // 桂
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス
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
            IMasus srcI = KomanoKidou.SrcKeimatobi_巻戻し跳(process.Star.Sengo, process.Star.Masu);
            IMasus srcJ = KomanoKidou.SrcKeimatobi_巻戻し駆(process.Star.Sengo, process.Star.Masu);
            IMove src = process.Src();

            //----------
            // 競合駒
            //----------
            List<K40> kmI = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcI, logTag);
            List<K40> kmJ = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcJ, logTag);

            if (process.IsDaAction)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (src.ExistsIn(srcI, kifuD, logTag))
            {
                //----------
                // 移動前はＩだった
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.Migi;
            }
            else if (src.ExistsIn(srcJ, kifuD, logTag))
            {
                //----------
                // 移動前はＪだった
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ｊに競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && process.NeverOnaji(kifuD, logTag, kmJ)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ｉに競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && process.NeverOnaji(kifuD, logTag, kmI)) { migiHidari = MigiHidari.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && process.NeverOnaji(kifuD, logTag, kmI, kmJ)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == process.IsNari && !process.IsDaAction && process.InAitejin)
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariFunari.Funari;
            }
            else if (process.IsNari)
            {
                nari = NariFunari.Nari;
            }
            else
            {
                nari = NariFunari.CTRL_SONOMAMA;
            }

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static FugoJ CreateGin(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            //************************************************************
            // 銀
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(range masu)
            //----------
            //┌─┬─┬─┐
            //│Ｈ│  │Ｂ│
            //├─┼─┼─┤
            //│　│至│  │先手から見た図
            //├─┼─┼─┤
            //│Ｆ│Ｅ│Ｄ│
            //└─┴─┴─┘
            IMasus srcB = KomanoKidou.SrcIppo_巻戻し昇(process.Star.Sengo, process.Star.Masu);
            IMasus srcD = KomanoKidou.SrcIppo_巻戻し沈(process.Star.Sengo, process.Star.Masu);
            IMasus srcE = KomanoKidou.SrcIppo_巻戻し引(process.Star.Sengo, process.Star.Masu);
            IMasus srcF = KomanoKidou.SrcIppo_巻戻し降(process.Star.Sengo, process.Star.Masu);
            IMasus srcH = KomanoKidou.SrcIppo_巻戻し浮(process.Star.Sengo, process.Star.Masu);
            IMove src = process.Src();

            //----------
            // 競合駒
            //----------
            List<K40> kmB = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcB, logTag);
            List<K40> kmD = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcD, logTag);
            List<K40> kmE = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcE, logTag);
            List<K40> kmF = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcF, logTag);
            List<K40> kmH = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcH, logTag);

            if (process.IsDaAction)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (src.ExistsIn(srcB, kifuD, logTag))
            {
                // 移動前はＢだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Migi;
            }
            else if (src.ExistsIn(srcD, kifuD, logTag))
            {
                // 移動前はＤだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (src.ExistsIn(srcE, kifuD, logTag))
            {
                // 移動前はＥだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Sugu;
            }
            else if (src.ExistsIn(srcF, kifuD, logTag))
            {
                // 移動前はＦだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (src.ExistsIn(srcH, kifuD, logTag))
            {
                // 移動前はＨだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： ①Ｅ、Ｆ、Ｈのどこにも競合駒がなければ。
            //              ②Ｈに競合駒がなく引があるなら。
            //              ③Ｅ、Ｆに競合駒がなく上があるなら。
            if (migiHidari == MigiHidari.Migi && (
                process.NeverOnaji(kifuD, logTag, kmE, kmF, kmH)
                || (process.NeverOnaji(kifuD, logTag, kmH) && agaruHiku == AgaruHiku.Hiku)
                || (process.NeverOnaji(kifuD, logTag, kmE, kmF) && agaruHiku == AgaruHiku.Agaru)
                )) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： ①Ｂ、Ｄ、Ｅのどこにも競合駒がなければ。
            //              ②Ｂに競合駒がなく引があるなら。
            //              ③Ｄ、Ｅに競合駒がなく上があるなら。
            if (migiHidari == MigiHidari.Hidari && (
                process.NeverOnaji(kifuD, logTag, kmB, kmD, kmE)
                || (process.NeverOnaji(kifuD, logTag, kmB) && agaruHiku == AgaruHiku.Hiku)
                || (process.NeverOnaji(kifuD, logTag, kmD, kmE) && agaruHiku == AgaruHiku.Agaru)
                )) { migiHidari = MigiHidari.No_Print; }

            // 「直」解除： Ｄ、Ｆのどちらにも競合駒がなければ。
            if (migiHidari == MigiHidari.Sugu && process.NeverOnaji(kifuD, logTag, kmD, kmF)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ｂ、Ｈのどこにも競合駒がなければ。また、直があるなら。
            if (agaruHiku == AgaruHiku.Agaru && (process.NeverOnaji(kifuD, logTag, kmB, kmH) || migiHidari == MigiHidari.Sugu)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： ①Ｂ、Ｄ、Ｅ、Ｆのどこにも競合駒がなければ。
            //              ②Ｄ、Ｅ、Ｆ、Ｈのどこにも競合駒がなければ。
            //              ③Ｄに競合駒がなく、右があるなら。
            //              ④Ｆに競合駒がなく、左があるなら。
            if (agaruHiku == AgaruHiku.Hiku &&
                (
                process.NeverOnaji(kifuD, logTag, kmB, kmD, kmE, kmF)
                || process.NeverOnaji(kifuD, logTag, kmD, kmE, kmF, kmH)
                || (process.NeverOnaji(kifuD, logTag, kmD) && migiHidari == MigiHidari.Migi)
                || (process.NeverOnaji(kifuD, logTag, kmF) && migiHidari == MigiHidari.Hidari)
                )) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： 銀は寄れません。

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && process.NeverOnaji(kifuD, logTag, kmB, kmD, kmE, kmF, kmH)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == process.IsNari && !process.IsDaAction && process.InAitejin)
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariFunari.Funari;
            }
            else if (process.IsNari)
            {
                nari = NariFunari.Nari;
            }
            else
            {
                nari = NariFunari.CTRL_SONOMAMA;
            }

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static FugoJ CreateKin(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji;

            JFugoCreator15Array.CreateKin_static(process, kifuD, out migiHidari, out agaruHiku, out nari, out daHyoji, logTag);

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static void CreateKin_static(
            IMove process,//移動先、移動元、両方のマス番号
            TreeDocument kifuD,
            out MigiHidari migiHidari, out AgaruHiku agaruHiku, out NariFunari nari, out DaHyoji daHyoji,
            ILogTag logTag
            )
        {
            //************************************************************
            // △金、△と金、△成香、△成桂、△成銀
            //************************************************************
            daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
            //----------
            //┌─┬─┬─┐
            //│  │Ａ│  │
            //├─┼─┼─┤
            //│Ｇ│至│Ｃ│先手から見た図
            //├─┼─┼─┤
            //│Ｆ│Ｅ│Ｄ│
            //└─┴─┴─┘
            IMasus srcA = KomanoKidou.SrcIppoA_巻戻し上(process.Star.Sengo, process.Star.Masu);
            IMasus srcC = KomanoKidou.SrcIppo_巻戻し射(process.Star.Sengo, process.Star.Masu);
            IMasus srcD = KomanoKidou.SrcIppo_巻戻し沈(process.Star.Sengo, process.Star.Masu);
            IMasus srcE = KomanoKidou.SrcIppo_巻戻し引(process.Star.Sengo, process.Star.Masu);
            IMasus srcF = KomanoKidou.SrcIppo_巻戻し降(process.Star.Sengo, process.Star.Masu);
            IMasus srcG = KomanoKidou.SrcIppo_巻戻し滑(process.Star.Sengo, process.Star.Masu);
            IMove src = process.Src();

            //----------
            // 競合駒
            //----------
            List<K40> kmA = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcA, logTag);
            List<K40> kmC = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcC, logTag);
            List<K40> kmD = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcD, logTag);
            List<K40> kmE = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcE, logTag);
            List<K40> kmF = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcF, logTag);
            List<K40> kmG = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcG, logTag);


            if (process.IsDaAction)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (src.ExistsIn(srcA, kifuD, logTag))
            {
                //----------
                // 移動前はＡだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.No_Print;
                //// 打
                //if (process.IsDaAction) { daHyoji = DaHyoji.Visible; }
            }
            else if (src.ExistsIn(srcC, kifuD, logTag))
            {
                //----------
                // 移動前はＣだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Migi;
                //// 打
                //if (process.IsDaAction) { daHyoji = DaHyoji.Visible; }
            }
            else if (src.ExistsIn(srcF, kifuD, logTag))
            {
                //----------
                // 移動前はＤだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
                //// 打
                //if (process.IsDaAction) { daHyoji = DaHyoji.Visible; }
            }
            else if (src.ExistsIn(srcE, kifuD, logTag))
            {
                //----------
                // 移動前はＥだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Sugu;
                //// 打
                //if (process.IsDaAction) { daHyoji = DaHyoji.Visible; }
            }
            else if (src.ExistsIn(srcD, kifuD, logTag))
            {
                //----------
                // 移動前はＦだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
                //// 打
                //if (process.IsDaAction) { daHyoji = DaHyoji.Visible; }
            }
            else if (src.ExistsIn(srcG, kifuD, logTag))
            {
                //----------
                // 移動前はＧだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Hidari;
                //// 打
                //if (process.IsDaAction) { daHyoji = DaHyoji.Visible; }
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }


            // 「右」解除： ①Ｅ、Ｆ、Ｇのどちらにも競合駒がなければ。
            //              ②Ｇに競合駒がなく、寄があるなら。
            //              ③上があり、Ｅ、Ｆのどちらにも競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && (
                process.NeverOnaji(kifuD, logTag, kmE, kmF, kmG)
                || (process.NeverOnaji(kifuD, logTag, kmG) && agaruHiku == AgaruHiku.Yoru)
                || (AgaruHiku.Agaru == agaruHiku && process.NeverOnaji(kifuD, logTag, kmE, kmF))
                )) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： ①Ｃ、Ｄ、Ｅのどちらにも競合駒がなければ。
            //              ②Ｃに競合駒がなく、寄があるなら。
            //              ③上があり、Ｄ、Ｅのどちらにも競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && (
                process.NeverOnaji(kifuD, logTag, kmC, kmD, kmE)
                || (process.NeverOnaji(kifuD, logTag, kmC) && agaruHiku == AgaruHiku.Yoru)
                || (AgaruHiku.Agaru == agaruHiku && process.NeverOnaji(kifuD, logTag, kmD, kmE))
                )) { migiHidari = MigiHidari.No_Print; }

            // 「直」解除： Ｄ、Ｆのどちらにも競合駒がなければ。
            if (migiHidari == MigiHidari.Sugu && process.NeverOnaji(kifuD, logTag, kmD, kmF)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： ①Ａ、Ｃ、Ｇのどこにも競合駒がなければ。
            //              ②直があるなら。
            //              ③Ｃに競合駒がなく、右があるなら。
            //              ④Ｇに競合駒がなく、左があるなら。
            if (agaruHiku == AgaruHiku.Agaru &&
                (
                process.NeverOnaji(kifuD, logTag, kmA, kmC, kmG)
                || migiHidari == MigiHidari.Sugu
                || process.NeverOnaji(kifuD, logTag, kmC) && migiHidari == MigiHidari.Migi
                || process.NeverOnaji(kifuD, logTag, kmG) && migiHidari == MigiHidari.Hidari
                )
                ) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｃ、Ｄ、Ｅ、Ｆ、Ｇのどこにも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && process.NeverOnaji(kifuD, logTag, kmC, kmD, kmE, kmF, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： ①Ａ、Ｄ、Ｅ、Ｆのどこにも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Yoru && process.NeverOnaji(kifuD, logTag, kmA, kmD, kmE, kmF)) { agaruHiku = AgaruHiku.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && process.NeverOnaji(kifuD, logTag, kmA, kmC, kmD, kmE, kmF, kmG)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成れません
            //----------
            nari = NariFunari.CTRL_SONOMAMA;
        }

        public static FugoJ CreateOh(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            //************************************************************
            // 王
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(range masu)
            //----------
            //┌─┬─┬─┐
            //│Ｈ│Ａ│Ｂ│
            //├─┼─┼─┤
            //│Ｇ│至│Ｃ│先手から見た図
            //├─┼─┼─┤
            //│Ｆ│Ｅ│Ｄ│
            //└─┴─┴─┘

            migiHidari = MigiHidari.No_Print;
            agaruHiku = AgaruHiku.No_Print;

            //----------
            // 王は成れません
            //----------
            nari = NariFunari.CTRL_SONOMAMA;

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static FugoJ CreateHisya(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            //************************************************************
            // 飛
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
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
            IMasus srcA = KomanoKidou.SrcKantu_巻戻し上(process.Star.Sengo, process.Star.Masu);
            IMasus srcC = KomanoKidou.SrcKantu_巻戻し射(process.Star.Sengo, process.Star.Masu);
            IMasus srcE = KomanoKidou.SrcKantu_巻戻し引(process.Star.Sengo, process.Star.Masu);
            IMasus srcG = KomanoKidou.SrcKantu_巻戻し滑(process.Star.Sengo, process.Star.Masu);
            IMove src = process.Src();

            //----------
            // 棋譜の現局面：競合駒
            //----------
            List<K40> kmA = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcA, logTag);
            List<K40> kmC = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcC, logTag);
            List<K40> kmE = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcE, logTag);
            List<K40> kmG = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcG, logTag);

            if (process.IsDaAction)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (src.ExistsIn(srcA, kifuD, logTag))
            {
                //----------
                // Ａにいた
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.No_Print;
            }
            else if (src.ExistsIn(srcC, kifuD, logTag))
            {
                //----------
                // Ｃにいた
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Migi;
            }
            else if (src.ExistsIn(srcE, kifuD, logTag))
            {
                //----------
                // Ｅにいた
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.No_Print;
            }
            else if (src.ExistsIn(srcG, kifuD, logTag))
            {
                //----------
                // Ｇにいた
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ｇに競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && process.NeverOnaji(kifuD, logTag, kmG)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ｃに競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && process.NeverOnaji(kifuD, logTag, kmC)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ａ、Ｃ、Ｇに競合駒がなければ。
            //if (agaruHiku == AgaruHiku.Agaru && process.NeverOnaji(kmA) && process.NeverOnaji(kmC) && process.NeverOnaji(kmG)) { agaruHiku = AgaruHiku.None; }
            if (agaruHiku == AgaruHiku.Agaru && process.NeverOnaji(kifuD, logTag, kmA, kmC, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｃ、Ｅ、Ｇに競合駒がなければ。
            //if (agaruHiku == AgaruHiku.Hiku && process.NeverOnaji(kmC) && process.NeverOnaji(kmE) && process.NeverOnaji(kmG)) { agaruHiku = AgaruHiku.None; }
            if (agaruHiku == AgaruHiku.Hiku && process.NeverOnaji(kifuD, logTag, kmC, kmE, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： Ａ、Ｅに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Yoru && process.NeverOnaji(kifuD, logTag, kmA) && process.NeverOnaji(kifuD, logTag, kmE)) { agaruHiku = AgaruHiku.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && process.NeverOnaji(kifuD, logTag, kmA, kmC, kmE, kmG)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == process.IsNari && !process.IsDaAction && process.InAitejin)
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariFunari.Funari;
            }
            else if (process.IsNari)
            {
                nari = NariFunari.Nari;
            }
            else
            {
                nari = NariFunari.CTRL_SONOMAMA;
            }

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static FugoJ CreateKaku(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            //************************************************************
            // 角
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(range masu)
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
            IMasus srcB = KomanoKidou.SrcKantu_巻戻し昇(process.Star.Sengo, process.Star.Masu);
            IMasus srcD = KomanoKidou.SrcKantu_巻戻し沈(process.Star.Sengo, process.Star.Masu);
            IMasus srcF = KomanoKidou.SrcKantu_巻戻し降(process.Star.Sengo, process.Star.Masu);
            IMasus srcH = KomanoKidou.SrcKantu_巻戻し浮(process.Star.Sengo, process.Star.Masu);
            IMove src = process.Src();

            //----------
            // 競合駒
            //----------
            List<K40> kmB = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcB, logTag);
            List<K40> kmD = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcD, logTag);
            List<K40> kmF = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcF, logTag);
            List<K40> kmH = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcH, logTag);


            if (process.IsDaAction)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (src.ExistsIn(srcB, kifuD, logTag))
            {
                //----------
                // 移動前はＢだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Migi;
            }
            else if (src.ExistsIn(srcD, kifuD, logTag))
            {
                //----------
                // 移動前はＤだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (src.ExistsIn(srcF, kifuD, logTag))
            {
                //----------
                // 移動前はＦだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (src.ExistsIn(srcH, kifuD, logTag))
            {
                //----------
                // 移動前はＨだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ｆ、Ｈに競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && process.NeverOnaji(kifuD, logTag, kmF, kmH)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ｂ、Ｄに競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && process.NeverOnaji(kifuD, logTag, kmB, kmD)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ｂ、Ｈに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Agaru && process.NeverOnaji(kifuD, logTag, kmB, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｄ、Ｆに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && process.NeverOnaji(kifuD, logTag, kmD, kmF)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： 角は寄れません。

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && process.NeverOnaji(kifuD, logTag, kmB, kmD, kmF, kmH)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == process.IsNari && !process.IsDaAction && process.InAitejin)
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariFunari.Funari;
            }
            else if (process.IsNari)
            {
                nari = NariFunari.Nari;
            }
            else
            {
                nari = NariFunari.CTRL_SONOMAMA;
            }

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static FugoJ CreateRyu(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            //************************************************************
            // 竜
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
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
            IMasus srcA = KomanoKidou.SrcKantu_巻戻し上(process.Star.Sengo, process.Star.Masu);
            IMasus srcB = KomanoKidou.SrcIppo_巻戻し昇(process.Star.Sengo, process.Star.Masu);
            IMasus srcC = KomanoKidou.SrcKantu_巻戻し射(process.Star.Sengo, process.Star.Masu);
            IMasus srcD = KomanoKidou.SrcIppo_巻戻し沈(process.Star.Sengo, process.Star.Masu);
            IMasus srcE = KomanoKidou.SrcKantu_巻戻し引(process.Star.Sengo, process.Star.Masu);
            IMasus srcF = KomanoKidou.SrcIppo_巻戻し降(process.Star.Sengo, process.Star.Masu);
            IMasus srcG = KomanoKidou.SrcKantu_巻戻し滑(process.Star.Sengo, process.Star.Masu);
            IMasus srcH = KomanoKidou.SrcIppo_巻戻し浮(process.Star.Sengo, process.Star.Masu);
            IMove src = process.Src();

            //----------
            // 競合駒
            //----------
            List<K40> kmA = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcA, logTag);
            List<K40> kmB = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcB, logTag);
            List<K40> kmC = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcC, logTag);
            List<K40> kmD = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcD, logTag);
            List<K40> kmE = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcE, logTag);
            List<K40> kmF = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcF, logTag);
            List<K40> kmG = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcG, logTag);
            List<K40> kmH = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcH, logTag);


            if (process.IsDaAction)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (src.ExistsIn(srcA, kifuD, logTag))
            {
                //----------
                // 移動前はＡだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.No_Print;
            }
            else if (src.ExistsIn(srcB, kifuD, logTag))
            {
                //----------
                // 移動前はＢだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Migi;
            }
            else if (src.ExistsIn(srcC, kifuD, logTag))
            {
                //----------
                // 移動前はＣだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Migi;
            }
            else if (src.ExistsIn(srcD, kifuD, logTag))
            {
                //----------
                // 移動前はＤだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (src.ExistsIn(srcE, kifuD, logTag))
            {
                //----------
                // 移動前はＥだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.No_Print;
            }
            else if (src.ExistsIn(srcF, kifuD, logTag))
            {
                //----------
                // 移動前はＦだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (src.ExistsIn(srcG, kifuD, logTag))
            {
                //----------
                // 移動前はＧだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (src.ExistsIn(srcH, kifuD, logTag))
            {
                //----------
                // 移動前はＨだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ａ、Ｅ、Ｆ、Ｇ、Ｈに１つも競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && process.NeverOnaji(kifuD, logTag, kmA, kmE, kmF, kmG, kmH)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ａ、Ｂ、Ｃ、Ｄ、Ｅに１つも競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && process.NeverOnaji(kifuD, logTag, kmA, kmB, kmC, kmD, kmE)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ａ、Ｂ、Ｃ、Ｇ、Ｈに１つも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Agaru && process.NeverOnaji(kifuD, logTag, kmA, kmB, kmC, kmG, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｃ、Ｄ、Ｅ、Ｆ、Ｇに１つも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && process.NeverOnaji(kifuD, logTag, kmC, kmD, kmE, kmF, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： Ａ、Ｂ、Ｄ、Ｅ、Ｆ、Ｈに１つも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Yoru && process.NeverOnaji(kifuD, logTag, kmA, kmB, kmD, kmE, kmF, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && process.NeverOnaji(kifuD, logTag, kmA, kmB, kmC, kmD, kmE, kmF, kmG, kmH)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成れません
            //----------
            nari = NariFunari.CTRL_SONOMAMA;

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static FugoJ CreateUma(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            //************************************************************
            // 馬
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
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
            IMasus srcA = KomanoKidou.SrcIppoA_巻戻し上(process.Star.Sengo, process.Star.Masu);
            IMasus srcB = KomanoKidou.SrcKantu_巻戻し昇(process.Star.Sengo, process.Star.Masu);
            IMasus srcC = KomanoKidou.SrcIppo_巻戻し射(process.Star.Sengo, process.Star.Masu);
            IMasus srcD = KomanoKidou.SrcKantu_巻戻し沈(process.Star.Sengo, process.Star.Masu);
            IMasus srcE = KomanoKidou.SrcIppo_巻戻し引(process.Star.Sengo, process.Star.Masu);
            IMasus srcF = KomanoKidou.SrcKantu_巻戻し降(process.Star.Sengo, process.Star.Masu);
            IMasus srcG = KomanoKidou.SrcIppo_巻戻し滑(process.Star.Sengo, process.Star.Masu);
            IMasus srcH = KomanoKidou.SrcKantu_巻戻し浮(process.Star.Sengo, process.Star.Masu);
            IMove src = process.Src();

            //----------
            // 競合駒
            //----------
            List<K40> kmA = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcA, logTag);
            List<K40> kmB = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcB, logTag);
            List<K40> kmC = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcC, logTag);
            List<K40> kmD = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcD, logTag);
            List<K40> kmE = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcE, logTag);
            List<K40> kmF = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcF, logTag);
            List<K40> kmG = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcG, logTag);
            List<K40> kmH = Util_KyokumenReader.KomaHandles_EachSrc(kifuD, process.Star.Sengo, process, srcH, logTag);

            if (process.IsDaAction)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (src.ExistsIn(srcB, kifuD, logTag))
            {
                //----------
                // 移動前はＢだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Migi;
            }
            else if (src.ExistsIn(srcD, kifuD, logTag))
            {
                //----------
                // 移動前はＤだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (src.ExistsIn(srcF, kifuD, logTag))
            {
                //----------
                // 移動前はＦだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (src.ExistsIn(srcH, kifuD, logTag))
            {
                //----------
                // 移動前はＨだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Hidari;
            }
            else if (src.ExistsIn(srcA, kifuD, logTag))
            {
                //----------
                // 移動前はＡだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.No_Print;
            }
            else if (src.ExistsIn(srcC, kifuD, logTag))
            {
                //----------
                // 移動前はＣだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Migi;
            }
            else if (src.ExistsIn(srcE, kifuD, logTag))
            {
                //----------
                // 移動前はＥだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.No_Print;
            }
            else if (src.ExistsIn(srcG, kifuD, logTag))
            {
                //----------
                // 移動前はＧだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ａ、Ｅ、Ｆ、Ｇ、Ｈに競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && process.NeverOnaji(kifuD, logTag, kmA, kmE, kmF, kmG, kmH)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ａ、Ｂ、Ｃ、Ｄ、Ｅに競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && process.NeverOnaji(kifuD, logTag, kmA, kmB, kmC, kmD, kmE)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ａ、Ｂ、Ｃ、Ｇ、Ｈに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Agaru && process.NeverOnaji(kifuD, logTag, kmA, kmB, kmC, kmG, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｃ、Ｄ、Ｅ、Ｆ、Ｇに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && process.NeverOnaji(kifuD, logTag, kmC, kmD, kmE, kmF, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： Ａ、Ｂ、Ｄ、Ｅ、Ｆ、Ｈに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Yoru && process.NeverOnaji(kifuD, logTag, kmA, kmB, kmD, kmE, kmF, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && process.NeverOnaji(kifuD, logTag, kmA, kmB, kmC, kmD, kmE, kmF, kmG, kmH)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成れません
            //----------
            nari = NariFunari.CTRL_SONOMAMA;

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static FugoJ CreateTokin(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji;

            JFugoCreator15Array.CreateKin_static(process, kifuD, out migiHidari, out agaruHiku, out nari, out daHyoji, logTag);

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static FugoJ CreateNariKyo(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji;

            JFugoCreator15Array.CreateKin_static(process, kifuD, out migiHidari, out agaruHiku, out nari, out daHyoji, logTag);

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static FugoJ CreateNariKei(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji;


            JFugoCreator15Array.CreateKin_static(process, kifuD, out migiHidari, out agaruHiku, out nari, out daHyoji, logTag);

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static FugoJ CreateNariGin(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariFunari nari;
            DaHyoji daHyoji;

            JFugoCreator15Array.CreateKin_static(process, kifuD, out migiHidari, out agaruHiku, out nari, out daHyoji, logTag);

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

        public static FugoJ CreateErrorKoma(IMove process, TreeDocument kifuD, ILogTag logTag)
        {
            //************************************************************
            // エラー
            //************************************************************
            MigiHidari migiHidari = MigiHidari.No_Print;
            AgaruHiku agaruHiku = AgaruHiku.No_Print;
            NariFunari nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            if (Sengo.Gote == process.Star.Sengo)
            {
                //******************************
                // △後手
                //******************************
            }
            else
            {
                //******************************
                // ▲先手
                //******************************
            }


            //----------
            // TODO: 移動前の駒が成る前かどうか
            //----------
            nari = NariFunari.CTRL_SONOMAMA;

            FugoJ fugo = new FugoJ(
                Haiyaku184Array.Syurui(process.SrcStar.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );
            return fugo;
        }

    }


}
