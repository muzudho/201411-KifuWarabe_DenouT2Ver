using Grayscale.KifuwaraneEntities.ApplicatedGame.Architecture;
using Grayscale.KifuwaraneEntities.L04_Common;
using Grayscale.KifuwaraneEntities.Log;
using Grayscale.KifuwaraneGui.L07_Shape;

namespace Grayscale.KifuwaraneGui.L09_Ui
{
    public abstract class Util_MenuDrawer
    {


        public static void DrawKomaokuri1(ref RequestForMain requestForMain, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag)
        {
            //------------------------------------------------------------
            // 駒・再描画
            //------------------------------------------------------------
            foreach (K40 koma in requestForMain.RequestRefresh_Komas)
            {
                Shape_BtnKoma btn_koma = Converter09.KomaToBtn(koma, shape_PnlTaikyoku);

                //if (K40Util.OnKoma((int)koma))
                //{
                Ui_02Action.Refresh_KomaLocation(koma, shape_PnlTaikyoku, kifuD, logTag);
                //}
            }
            requestForMain.RequestRefresh_Komas.Clear();
        }


        public static void DrawKomaokuri2(ref RequestForMain requestForMain, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag)
        {
            //------------------------------
            // メナス
            //------------------------------
            FlowB_1TumamitaiKoma.Menace(ref requestForMain, shape_PnlTaikyoku, kifuD, logTag);
        }


    }
}
