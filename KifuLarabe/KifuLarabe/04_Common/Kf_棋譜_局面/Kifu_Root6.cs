using System;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L03_Communication;

namespace Grayscale.KifuwaraneLib.L04_Common
{
    public class Kifu_Root6 : Kifu_Node6
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
            if (!(this is Kifu_Root6))
            {
                string message = "Kifu_Rootクラスではないのに、SetSengo_Rootメソッドを使用しました。\n this.GetType().Name=[" + this.GetType().Name + "]";
                LoggerPool.ErrorLine(LarabeLoggerTag_Impl.ERROR, message);
                throw new Exception(message);
            }

            this.sengo = sengo;
        }


        public Kifu_Root6() :
            base(MoveImpl.NULL_OBJECT,
            new KomaHouse()
            )
        {
            this.sengo = Sengo.Sente;//FIXME:次の手番は先手になります。

            // 平手初期局面
            this.KomaHouse.Reset_ToHirateSyokihaichi();
        }


    }

}
