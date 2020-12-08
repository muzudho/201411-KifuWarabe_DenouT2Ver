using System.Text;
using Grayscale.KifuwaraneEntities.ApplicatedGame.Architecture;
using Grayscale.KifuwaraneEntities.Log;

namespace Grayscale.KifuwaraneEntities.L06_KifuIO
{
    /// <summary>
    /// 記録係
    /// </summary>
    public abstract class KirokuGakari
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜データを元に、符号リスト１(*1)を出力します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…「▲２六歩△８四歩▲７六歩」といった書き方。
        /// 
        /// </summary>
        /// <param name="fugoList"></param>
        public static string ToJapaneseKifuText(
            TreeDocument kifuD,
            ILogTag logTag
            )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("position ");
            IKifuElement node1 = kifuD.ElementAt8(kifuD.Root7_Teme);
            PositionKomaHouse house1 = node1.KomaHouse;
            sb.Append(house1.Startpos);
            sb.Append(" moves ");

            // 採譜用に、新しい対局を用意します。
            TreeDocument saifuKifuD = new TreeDocument();

            house1.SetStartpos(house1.Startpos);

            // 初期配置
            SyokiHaichi.ToHirate(saifuKifuD,logTag);


            kifuD.ForeachA(kifuD.Current8, (int teme, PositionKomaHouse house, TreeNode6 node6, ref bool toBreak) =>
            {
                if(0==teme)
                {
                    goto gt_EndLoop;
                }


                //RO_TeProcess last;
                //{
                //    IKifuElement kifuElement = saifuKifuD.ElementAt8(saifuKifuD.CountTeme(saifuKifuD.Current8));
                //    last = kifuElement.TeProcess;
                //}
                //RO_TeProcess previousMasu = last; //符号の追加が行われる前に退避

                FugoJ fugo;

                //------------------------------
                // 符号の追加（記録係）
                //------------------------------
                TreeNode6 newNode = saifuKifuD.CreateNodeA(
                    node6.TeProcess.SrcStar,
                    node6.TeProcess.Star,
                    node6.TeProcess.TottaSyurui
                    );
                saifuKifuD.AppendChildA_New(
                    newNode,
                    "KirokuGakari_ToJapaneseKifuText",
                    logTag
                    );

                fugo = JFugoCreator15Array.ItemMethods[(int)Haiyaku184Array.Syurui(node6.TeProcess.SrcStar.Haiyaku)](node6.TeProcess, saifuKifuD, logTag);//「▲２二角成」なら、馬（dst）ではなくて角（src）。
                //Ks14 temp = Haiyaku184Array.Syurui(process.SrcHaiyaku);
                //JFugoCreator15Array.DELEGATE_CreateJFugo delegate_CreateJFugo = JFugoCreator15Array.ItemMethods[(int)temp];
                //fugo = delegate_CreateJFugo(process, saifuKifuD);//「▲２二角成」なら、馬（dst）ではなくて角（src）。

                sb.Append(fugo.ToText_UseDou(node6));

                //// TODO:デバッグ用
                //switch (process.TottaKoma)
                //{
                //    case KomaSyurui.UNKNOWN:
                //    case KomaSyurui.TOTTA_KOMA_NASI:
                //        break;
                //    default:
                //        sb.Append("（");
                //        sb.Append(Converter.SyuruiToFugo(process.TottaKoma));
                //        sb.Append("取り）");
                //        break;
                //}
            gt_EndLoop:
                ;
            });

            //System.Console.WriteLine("size(" + fugoList.Count+")"+sb.ToString());

            return sb.ToString();
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜データを元に、符号リスト２(*1)を出力します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…「position startpos moves 7g7f 3c3d 2g2f」といった書き方。
        /// 
        /// </summary>
        /// <param name="fugoList"></param>
        public static string ToSfenKifuText(TreeDocument kifuD)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("position ");
            IKifuElement node2 = kifuD.ElementAt8(kifuD.Root7_Teme);
            PositionKomaHouse house1 = node2.KomaHouse;
            sb.Append(house1.Startpos);
            sb.Append(" moves ");

            int count = 0;
            kifuD.ForeachA(kifuD.Current8, (int teme, PositionKomaHouse house, TreeNode6 node6, ref bool toBreak) =>
            {
                if (0 == teme)
                {
                    goto gt_EndLoop;
                }

                //if (Kifu_Old.NEW_VERSION)
                //{
                //    if (count==0)
                //    {
                //        // ループの１回目は、開始局面なので飛ばします。
                //        goto gt_EndLoop;
                //    }
                //}
                //else
                //{
                //}

                sb.Append(node6.TeProcess.ToSfenText());

                //// TODO:デバッグ用
                //switch (process.TottaKoma)
                //{
                //    case KomaSyurui.UNKNOWN:
                //    case KomaSyurui.TOTTA_KOMA_NASI:
                //        break;
                //    default:
                //        sb.Append("(");
                //        sb.Append(Converter.SyuruiToSfen(process.Sengo,process.TottaKoma));
                //        sb.Append(")");
                //        break;
                //}

                sb.Append(" ");


            gt_EndLoop:
                count++;
            });


            //System.Console.WriteLine("size(" + fugoList.Count+")"+sb.ToString());

            return sb.ToString();
        }

    }
}
