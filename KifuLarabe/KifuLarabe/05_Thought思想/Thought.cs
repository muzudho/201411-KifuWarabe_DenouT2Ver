using System.Collections.Generic;
using System.Text;
using Grayscale.KifuwaraneEntities.ApplicatedGame;
using Grayscale.KifuwaraneEntities.L04_Common;
using Grayscale.KifuwaraneEntities.Log;

namespace Grayscale.KifuwaraneEntities.L05_Thought
{
    public abstract class Thought
    {
        public static KomaAndMasusDictionary GetPotentialMovesByKoma(
            IKifuElement siteiNode,//IKifuElement siteiNode = kifuD.ElementAt8(kifuD.CountTeme(kifuD.Current8));
            List<K40> komas, ILogTag logTag)
        {
            KomaAndMasusDictionary komaAndMove = new KomaAndMasusDictionary();// 「どの駒を、どこに進める」の一覧

            foreach (K40 koma in komas)
            {
                // ポテンシャル・ムーブを調べます。
                IMasus masus_PotentialMove = Rule01_PotentialMove_15Array.ItemMethods[
                    (int)Haiyaku184Array.Syurui(siteiNode.KomaHouse.KomaPosAt(koma).Star.Haiyaku)
                    ]//←ポテンシャル・ムーブ取得関数を選択。歩とか。
                    (
                    siteiNode.KomaHouse.KomaPosAt(koma).Star.Sengo,
                    siteiNode.KomaHouse.KomaPosAt(koma).Star.Masu
                    );

                if (!masus_PotentialMove.IsEmptySet())
                {
                    // 空でないなら
                    //komaAndMove.AddUnique(koma, masus_PotentialMove);
                    komaAndMove.AddOverwrite(koma, masus_PotentialMove);
                }
            }

            return komaAndMove;
        }


        /// <summary>
        /// 自分の駒がある升。
        /// </summary>
        /// <param name="kifuD"></param>
        /// <param name="selfSengo"></param>
        /// <param name="sbGohosyu"></param>
        /// <returns></returns>
        public static IMasus Masus_BySengoOkiba(
            IKifuElement siteiNode,// = kifuD.ElementAt8(kifuD.CountTeme(kifuD.Current8));
            Sengo selfSengo,
            Okiba okiba,
            StringBuilder sbGohosyu,
            ILogTag logTag
            )
        {
            //------------------------------------------------------------
            // 自分の駒がある枡。
            //------------------------------------------------------------
            IMasus hMasus_Self = new Masus_Set();
            {
                foreach (K40 koma in Util_KyokumenReader.Komas_BySengo(siteiNode, selfSengo, logTag))// 自分の駒だけを抽出。
                {

                    if (K40Util.OnKoma((int)koma))//エラーは除外
                    {
                        KomaHouse house1 = siteiNode.KomaHouse;

                        if (GameTranslator.Masu_ToOkiba(house1.KomaPosAt(koma).Star.Masu).HasFlag(okiba))
                        {
                            hMasus_Self.AddElement(house1.KomaPosAt(koma).Star.Masu);
                        }
                    }
                }

                sbGohosyu.AppendLine("┏━━━━━━━━━━┓自分の駒のある枡");
                sbGohosyu.AppendLine(hMasus_Self.LogString_Concrete());
                sbGohosyu.AppendLine("┗━━━━━━━━━━┛自分の駒のある枡");
            }

            return hMasus_Self;
        }


    }


}
