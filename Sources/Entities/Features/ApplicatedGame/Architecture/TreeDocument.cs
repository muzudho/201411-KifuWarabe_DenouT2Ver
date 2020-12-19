using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Grayscale.Kifuwarane.Entities.Logging;
using Grayscale.Kifuwarane.Entities.UseCase;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture
{
    /// <summary>
    /// 棋譜。
    /// </summary>
    public class TreeDocument
    {
        /// <summary>
        /// ツリー構造になっている本譜の葉ノード。
        /// 根を「startpos」等の初期局面コマンドとし、次の節からは棋譜の符号「2g2f」等が連なっている。
        /// </summary>
        public IKifuElement Current8 { get { return this.current8; } }
        private IKifuElement current8;

        public TreeDocument()
        {
            this.current8 = new TreeRoot6();

            Logging.Logger.Trace("リンクトリストは作られた"+this.DebugText_Kyokumen7(this, "ルートが追加されたはずだぜ☆"), SpecifyLogFiles.LinkedList);
        }



        public void Move_Previous()
        {
            this.current8 = this.Current8.Previous;
        }


        /// <summary>
        /// 現在の要素を切り取って返します。なければヌル。
        /// 
        /// カレントは、１手前に戻ります。
        /// 
        /// </summary>
        /// <returns>ルートしかないリストの場合、ヌルを返します。</returns>
        public IKifuElement PopCurrent1()
        {
            IKifuElement deleteeElement = null;

            if (this.Current8 is TreeRoot6)
            {
                // やってはいけない操作は、例外を返すようにします。
                throw new Exception("ルート局面を削除しようとしました。");

                //// ルート局面は削除させません。
                //goto gt_EndMethod;
            }

            //>>>>> ラスト要素がルートでなかったら

            // 一手前の要素（必ずあるはずです）
            deleteeElement = this.Current8;
            // 残されたリストの最後の要素の、次リンクを切ります。
            deleteeElement.Previous.Next2.Clear();

            // カレントを、１つ前の要素に替えます。
            this.current8 = deleteeElement.Previous;


            Logging.Logger.Trace("リンクトリストの最後の要素が削除された", SpecifyLogFiles.LinkedList);

        // gt_EndMethod:
            return deleteeElement;
        }





        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 現在の先後
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Sengo CountSengo(int teme)//IKifuElement element
        {
            Sengo result;

            //int teme = this.CountTeme(element);//this.Current8

            switch (this.GetRoot8().Sengo)
            {
                case Sengo.Sente:
                    if (teme % 2 == 0)
                    {
                        // 手目が偶数なら先手
                        result = Sengo.Sente;
                    }
                    else
                    {
                        result = Sengo.Gote;
                    }
                    break;

                case Sengo.Gote:
                    if (teme % 2 == 0)
                    {
                        // 手目が偶数なら後手
                        result = Sengo.Gote;
                    }
                    else
                    {
                        result = Sengo.Sente;
                    }
                    break;

                default:
                    throw new Exception("先後エラー");
            }


            return result;
        }




        /// <summary>
        /// 駒を駒袋に移動させます。
        /// 
        /// 駒袋には、不成にして入れておきます。
        /// </summary>
        public void SetKyokumen_ToKomabukuro3(TreeDocument kifuD)
        {
            this.ClearA();//this.Clear8();

            int lastTeme = kifuD.CountTeme(kifuD.Current8);

            foreach (K40 koma in K40Array.Items_KomaOnly)
            {
                PositionKomaHouse house1 = kifuD.ElementAt8(lastTeme).KomaHouse;

                IKomaPos komaP = house1.KomaPosAt(koma);

                M201 akiMasu = KifuIO.GetKomadaiKomabukuroSpace(Okiba.KomaBukuro, kifuD);

                house1.SetKomaPos(kifuD, koma, house1.KomaPosAt(koma).Next(
                    komaP.Star.Sengo,
                    akiMasu,
                    Haiyaku184Array.Syurui(komaP.Star.Haiyaku),
                    "局面_ClearToKomabukuro"
                    ));
            }
        }


        /// <summary>
        /// [ここから採譜]機能
        /// </summary>
        public void SetStartpos_KokokaraSaifu(TreeDocument kifuD, Sengo sengo)
        {
            StringBuilder sb = new StringBuilder();

            for (int dan = 1; dan <= 9; dan++)
            {
                int spaceCount = 0;

                for (int suji = 9; suji >= 1; suji--)
                {
                    // 将棋盤上のどこかにある駒？
                    K40 hKoma = Util_KyokumenReader.Koma_AtMasu(kifuD,
                        M201Util.OkibaSujiDanToMasu(Okiba.ShogiBan, suji, dan)
                        );

                    if (K40.Error != hKoma)
                    {
                        if (0 < spaceCount)
                        {
                            sb.Append(spaceCount);
                            spaceCount = 0;
                        }

                        PositionKomaHouse house1 = kifuD.ElementAt8(kifuD.CountTeme(kifuD.Current8)).KomaHouse;

                        sb.Append(KomaSyurui14Array.SfenText(
                            Haiyaku184Array.Syurui(house1.KomaPosAt(hKoma).Star.Haiyaku),
                            house1.KomaPosAt(hKoma).Star.Sengo
                            ));
                    }
                    else
                    {
                        spaceCount++;
                    }

                }

                if (0 < spaceCount)
                {
                    sb.Append(spaceCount);
                    spaceCount = 0;
                }

                if (dan != 9)
                {
                    sb.Append("/");
                }
            }

            sb.Append(" ");

            //------------------------------------------------------------
            // 先後
            //------------------------------------------------------------
            switch (sengo)
            {
                case Sengo.Gote:
                    sb.Append("w");
                    break;
                default:
                    sb.Append("b");
                    break;
            }

            sb.Append(" ");

            //------------------------------------------------------------
            // 持ち駒
            //------------------------------------------------------------
            {
                int mK = 0;
                int mR = 0;
                int mB = 0;
                int mG = 0;
                int mS = 0;
                int mN = 0;
                int mL = 0;
                int mP = 0;

                int mk = 0;
                int mr = 0;
                int mb = 0;
                int mg = 0;
                int ms = 0;
                int mn = 0;
                int ml = 0;
                int mp = 0;


                int lastTeme = kifuD.CountTeme(kifuD.Current8);

                // 先手
                List<K40> komasS = Util_KyokumenReader.Komas_ByOkiba(kifuD, Okiba.Sente_Komadai);
                foreach (K40 koma in komasS)
                {
                    IKifuElement dammyNode3 = kifuD.ElementAt8(lastTeme);
                    PositionKomaHouse house2 = dammyNode3.KomaHouse;

                    Ks14 syurui = KomaSyurui14Array.FunariCaseHandle(Haiyaku184Array.Syurui(house2.KomaPosAt(koma).Star.Haiyaku));

                    if (Ks14.H06_Oh == syurui)
                    {
                        mK++;
                    }
                    else if (Ks14.H07_Hisya == syurui)
                    {
                        mR++;
                    }
                    else if (Ks14.H08_Kaku == syurui)
                    {
                        mB++;
                    }
                    else if (Ks14.H05_Kin == syurui)
                    {
                        mG++;
                    }
                    else if (Ks14.H04_Gin == syurui)
                    {
                        mS++;
                    }
                    else if (Ks14.H03_Kei == syurui)
                    {
                        mN++;
                    }
                    else if (Ks14.H02_Kyo == syurui)
                    {
                        mL++;
                    }
                    else if (Ks14.H01_Fu == syurui)
                    {
                        mP++;
                    }
                    else
                    {
                    }
                }

                // 後手
                List<K40> komasG = Util_KyokumenReader.Komas_ByOkiba(kifuD, Okiba.Gote_Komadai);
                foreach (K40 koma in komasG)
                {
                    IKifuElement dammyNode3 = kifuD.ElementAt8(lastTeme);
                    PositionKomaHouse house2 = dammyNode3.KomaHouse;

                    Ks14 syurui = KomaSyurui14Array.FunariCaseHandle(Haiyaku184Array.Syurui(house2.KomaPosAt(koma).Star.Haiyaku));

                    if (Ks14.H06_Oh == syurui)
                    {
                        mk++;
                    }
                    else if (Ks14.H07_Hisya == syurui)
                    {
                        mr++;
                    }
                    else if (Ks14.H08_Kaku == syurui)
                    {
                        mb++;
                    }
                    else if (Ks14.H05_Kin == syurui)
                    {
                        mg++;
                    }
                    else if (Ks14.H04_Gin == syurui)
                    {
                        ms++;
                    }
                    else if (Ks14.H03_Kei == syurui)
                    {
                        mn++;
                    }
                    else if (Ks14.H02_Kyo == syurui)
                    {
                        ml++;
                    }
                    else if (Ks14.H01_Fu == syurui)
                    {
                        mp++;
                    }
                    else
                    {
                    }

                }

                if (0 == mK + mR + mB + mG + mS + mN + mL + mP + mk + mr + mb + mg + ms + mn + ml + mp)
                {
                    sb.Append("-");
                }
                else
                {
                    if (0 < mK)
                    {
                        sb.Append("K");
                        sb.Append(mK);
                    }

                    if (0 < mR)
                    {
                        sb.Append("R");
                        sb.Append(mR);
                    }

                    if (0 < mB)
                    {
                        sb.Append("B");
                        sb.Append(mB);
                    }

                    if (0 < mG)
                    {
                        sb.Append("G");
                        sb.Append(mG);
                    }

                    if (0 < mS)
                    {
                        sb.Append("S");
                        sb.Append(mS);
                    }

                    if (0 < mN)
                    {
                        sb.Append("N");
                        sb.Append(mN);
                    }

                    if (0 < mL)
                    {
                        sb.Append("L");
                        sb.Append(mL);
                    }

                    if (0 < mP)
                    {
                        sb.Append("P");
                        sb.Append(mP);
                    }

                    if (0 < mk)
                    {
                        sb.Append("k");
                        sb.Append(mk);
                    }

                    if (0 < mr)
                    {
                        sb.Append("r");
                        sb.Append(mr);
                    }

                    if (0 < mb)
                    {
                        sb.Append("b");
                        sb.Append(mb);
                    }

                    if (0 < mg)
                    {
                        sb.Append("g");
                        sb.Append(mg);
                    }

                    if (0 < ms)
                    {
                        sb.Append("s");
                        sb.Append(ms);
                    }

                    if (0 < mn)
                    {
                        sb.Append("n");
                        sb.Append(mn);
                    }

                    if (0 < ml)
                    {
                        sb.Append("l");
                        sb.Append(ml);
                    }

                    if (0 < mp)
                    {
                        sb.Append("p");
                        sb.Append(mp);
                    }
                }

            }

            // 手目
            sb.Append(" 1");

            //------------------------------------------------------------
            // 棋譜を空に
            //------------------------------------------------------------
            this.ClearA();// this.Clear8();

            IKifuElement dammyNode6 = kifuD.ElementAt8(kifuD.Root7_Teme);
            PositionKomaHouse house3 = dammyNode6.KomaHouse;

            house3.SetStartpos(sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public string DebugText_Kyokumen7(
            TreeDocument kifuD,
            string memo
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            PositionKomaHouse house4 = kifuD.ElementAt8(kifuD.CountTeme(kifuD.Current8)).KomaHouse;

            return house4.Log_Kyokumen(kifuD, kifuD.CountTeme(kifuD.Current8), memo);
        }

        /// <summary>
        /// 記録係が利用します。
        /// </summary>
        /// <param name="teme">手目</param>
        /// <param name="item"></param>
        /// <param name="toBreak"></param>
        public delegate void DELEGATE_Foreach(int teme, PositionKomaHouse house, TreeNode6 item, ref bool toBreak);
        public void ForeachA(IKifuElement endNode, DELEGATE_Foreach delegate_Foreach)
        {
            bool toBreak = false;

            List<IKifuElement> list8 = new List<IKifuElement>();

            //
            // ツリー型なので、１本のリストに変換するために工夫します。
            //
            // カレントからルートまで遡り、それを逆順にすれば、本譜になります。
            //

            while (null!=endNode)//ルートを含むところまで遡ります。
            {
                list8.Add(endNode); // リスト作成

                endNode = endNode.Previous;
            }


            list8.Reverse();

            int teme = 0;//初期局面が[0]

            foreach (TreeNode6 item in list8)//正順になっています。
            {
                PositionKomaHouse house = item.KomaHouse;

                delegate_Foreach(teme, house, item, ref toBreak);
                if (toBreak)
                {
                    break;
                }

                teme++;
            }

        //大量に呼び出される。
        //OldLinkedList.logger.WriteLineMemo(LarabeLoggerTag.LINKED_LIST, "リンクトリストのforeachが終了した");

        }

        /// <summary>
        /// 動作確認用に
        /// </summary>
        /// <param name="kifuD"></param>
        /// <param name="item"></param>
        /// <param name="hint"></param>
        /// <param name="logTag"></param>
        public void AppendChild_Main(
            TreeDocument kifuD,
            //TeProcess item,
            //KomaHouse newHouse,
            TreeNode6 newNode,
            string hint
            )
        {


            // SFENをキーに、次ノードを増やします。
            this.Current8.Next2.Add(newNode.TeProcess.ToSfenText(), newNode);


            newNode.Previous = this.Current8;
            this.current8 = newNode;

            // ここでノードが追加されますが、局面は進んでいません。
            // ログは、Ittesasiの最後に取ってください。
            //OldLinkedList.logger.WriteLineMemo(LarabeLoggerTag.LINKED_LIST, "リンクトリストに、ノードは追加された item=[" + item.ToSfenText() + "] memberName=["+memberName+"] sourceFilePath=["+sourceFilePath+"] sourceLineNumber=["+sourceLineNumber+"]");
            //Kifu_Document.KOMA_DOORS_LOGGER.WriteLineMemo(LarabeLoggerTag.LINKED_LIST, kifuD.DebugText_Kyokumen("ノードが追加されたぜ☆ hint=[" + hint+"]"));

            Logging.Logger.Trace( "ノードが１つ追加されたぜ☆ｗｗ　：　[" + kifuD.CountTeme(kifuD.Current8) + "]手目　：　hint=[" + hint + "]", SpecifyLogFiles.LinkedList);
            //　：　棋譜＝"+ KirokuGakari.ToJapaneseKifuText(kifuD, logTag)
            //　：　呼出箇所＝" + memberName + "." + sourceFilePath + "." + sourceLineNumber
        }


        public TreeNode6 CreateNodeA(
            RO_Star srcStar,
            RO_Star dstStar,
            Ks14 tottaSyurui
            )
        {
            // 最後尾をコピーして、最後尾に付け加えます。
            PositionKomaHouse newHouse;
            {
                PositionKomaHouse latestHouse = this.ElementAt8(this.CountTeme(this.Current8)).KomaHouse;

                //Array dst = new RO_KomaPos[lastHouse.Stars.Length];
                //Array.Copy(lastHouse.Stars, dst, lastHouse.Stars.Length);
                //newHouse = new KomaHouse((RO_KomaPos[])dst);// RO_KomaPos[K40Array.Items_KomaOnly.Length];

                newHouse = new PositionKomaHouse(latestHouse.Stars);

                Logging.Logger.Trace( newHouse.Log_Kyokumen(this, this.CountTeme(this.Current8), "増えたニュー局面"), SpecifyLogFiles.LinkedList);
            }



            MoveImpl item = MoveImpl.New(
                srcStar,
                dstStar,
                tottaSyurui
            );


            if (null == newHouse)
            {
                throw new Exception("ノードを追加しようとしましたが、指定されたnewHouseがヌルです。");
            }

            return new TreeNode6(item, newHouse);
        }

        /// <summary>
        /// 棋譜に符号を追加します。
        /// 
        /// KifuIO を通して使ってください。
        /// 
        /// ①コマ送り用。
        /// ②「成り」フラグの更新用。
        /// ③マウス操作用
        /// 
        /// </summary>
        /// <param name="process"></param>
        public void AppendChildA_New(
            //RO_Star srcStar,
            //RO_Star dstStar,
            //Ks14 tottaSyurui,
            TreeNode6 newNode,
            string hint
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            this.AppendChild_Main(this, newNode, hint + "：Kifu_Document.cs_Kifu_Document#Add_Old3a");

            Logging.Logger.Trace( "リンクトリストに、ノードは追加された hint=[" + hint + "] te=[" + newNode.TeProcess.ToSfenText() + "] memberName=[" + memberName + "] sourceFilePath=[" + sourceFilePath + "] sourceLineNumber=[" + sourceLineNumber + "]", SpecifyLogFiles.LinkedList);
        }

        /// <summary>
        /// 取った駒を差替えます。
        /// 
        /// 棋譜読取時用です。マウス操作時は、流れが異なるので使えません。
        /// </summary>
        /// <param name="process"></param>
        public void AppendChildB_Swap(
            Ks14 tottaSyurui,
            PositionKomaHouse newHouse,
            string hint,
            ILogFile logFile

            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            if (this.CountTeme(this.Current8) < 1)
            {
                // ルートしか無いなら
                goto gt_EndMethod;
            }

            IMove oldLastTe = this.PopCurrent1().TeProcess;

            MoveImpl item = MoveImpl.New(
                oldLastTe.SrcStar,
                oldLastTe.Star,
                tottaSyurui//ここを差替えます。
                );

            if (null == newHouse)
            {
                throw new Exception("ノードを追加しようとしましたが、指定されたnewHouseがヌルです。");
            }

            TreeNode6 newNode = new TreeNode6(item, newHouse);//, Converter04.AlternateSengo(this.Current2.Sengo)
            this.AppendChild_Main(this, newNode, "Kifu_Document.cs_Kifu_Document#Add_Old3b_WhenKifuRead");
            //this.Add8(this, item, null, "Kifu_Document.cs_Kifu_Document#Add_Old3b_WhenKifuRead", logTag);

            Logging.Logger.Trace( "リンクトリストの、最終ノードは差し替えられた hint=[" + hint + "] item=[" + item.ToSfenText() + "] memberName=[" + memberName + "] sourceFilePath=[" + sourceFilePath + "] sourceLineNumber=[" + sourceLineNumber + "]", logFile);

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// 棋譜を空っぽにします。
        /// </summary>
        public void ClearA()
        {
            // ルートまで遡ります。
            while (!(this.Current8 is TreeRoot6))
            {
                this.current8 = this.Current8.Previous;
            }

            // ルートの次の手を全クリアーします。
            this.Current8.Next2.Clear();

            Logging.Logger.Trace( "リンクトリストは、クリアーされた", SpecifyLogFiles.LinkedList);
        }

        public TreeRoot6 GetRoot8()
        {
            IKifuElement cur = this.Current8;

            while (!(cur is TreeRoot6))
            {
                cur = cur.Previous;

                if (null == cur)
                {
                    throw new Exception("ルートに遡ろうとしたら、ヌルでした。");
                }
            }

            return (TreeRoot6)cur;
        }

        public int Root7_Teme { get { return 0; } }

        /// <summary>
        /// 何手目か。
        /// 
        /// 新版では、初期局面（ルート）は必ず含まれていることから、オリジン1です。
        /// </summary>
        public int CountTeme(IKifuElement element)
        {
            // [0]初期局面 は必ず入っているので、ループが１回も回らないということはないはず。
            int countTeme = -1;

            this.ForeachA(element, (int teme2, PositionKomaHouse house, TreeNode6 node6, ref bool toBreak) =>
            {
                countTeme = teme2;
            });

            if (-1 == countTeme)
            {
                throw new Exception("手目を調べるのに失敗しました。\n[0]初期局面 は必ず入っているので、ループが１回も回らないということはないはずですが、-1手目になりました。");
            }

            // ログ出すぎ
            //Kifu_Document.TREE_LOGGER.WriteLineMemo(LarabeLoggerTag.LINKED_LIST, "リンクトリストの高さを調べられた Count=[" + count + "]");
            return countTeme;
        }

        public IKifuElement ElementAt8(int teme1)
        {
            IKifuElement found6 = null;

            this.ForeachA(this.Current8, (int teme2, PositionKomaHouse house5, TreeNode6 node6, ref bool toBreak) =>
            {
                if (teme1 == teme2) //新Verは 0 にも対応。 teme1が 0 のとき、配列Verは 1 スタートなので、スルーされるので、このループに入る前に処理しておきます。
                {
                    found6 = node6;
                    toBreak = true;
                }

            });

            if (null == found6)
            {
                throw new Exception($"[{teme1}]の局面ノード6はヌルでした。");
            }

            return found6;
        }
    }
}
