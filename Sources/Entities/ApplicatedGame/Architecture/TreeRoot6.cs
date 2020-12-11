using System;
using Grayscale.Kifuwarane.Entities.Log;

namespace Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture
{
    public class TreeRoot6 : TreeNode6
    {
        /// <summary>
        /// 初手局面[0]の手番
        /// </summary>
        public Sengo Sengo
        {
            get
            {
                return this.sengo;
            }
        }
        private Sengo sengo;


        /// <summary>
        /// １手目の手番を先手にしたいときは、初期局面の手番は後手にしてください。
        /// </summary>
        /// <param name="sengo"></param>
        public void SetSengo_Root1(Sengo sengo)
        {
            if (!(this is TreeRoot6))
            {
                string message = "Kifu_Rootクラスではないのに、SetSengo_Rootメソッドを使用しました。\n this.GetType().Name=[" + this.GetType().Name + "]";
                Logger.ErrorLine(LogTags.ErrorLog, message);
                throw new Exception(message);
            }

            this.sengo = sengo;
        }


        public TreeRoot6() :
            base(MoveImpl.NULL_OBJECT,
            new PositionKomaHouse()
            )
        {
            this.sengo = Sengo.Sente;//FIXME:次の手番は先手になります。

            // 平手初期局面
            this.KomaHouse.Reset_ToHirateSyokihaichi();
        }


    }

}
