using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xenon.KifuLarabe;
using Xenon.KifuNarabe.L02_DammyConsole;
using Xenon.KifuLarabe.L03_Communication;
using Xenon.KifuLarabe.L04_Common;
using Xenon.KifuLarabe.L06_KifuIO;
using Xenon.KifuNarabe.L07_Shape;
using Xenon.KifuNarabe.L08_Server;

namespace Xenon.KifuNarabe.L09_Ui
{
    public abstract class Util_MenuDrawer
    {


        public static void DrawKomaokuri1(ref RequestForMain requestForMain, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, LarabeLoggerTag logTag)
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


        public static void DrawKomaokuri2(ref RequestForMain requestForMain, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, LarabeLoggerTag logTag)
        {
            //------------------------------
            // メナス
            //------------------------------
            FlowB_1TumamitaiKoma.Menace(ref requestForMain, shape_PnlTaikyoku, kifuD, logTag);
        }


    }
}
