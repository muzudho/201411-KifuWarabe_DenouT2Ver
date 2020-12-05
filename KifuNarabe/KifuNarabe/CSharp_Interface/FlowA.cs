using System.Windows.Forms;
using Grayscale.KifuwaraneGui.L07_Shape;
using Grayscale.KifuwaraneGui.L09_Ui;
using Grayscale.KifuwaraneLib;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneGui
{

    public interface FlowA
    {
        /// <summary>
        /// ************************************************************************************************************************
        /// マウスの左ボタン押下時。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        FlowA MouseLeftButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILarabeLoggerTag logTag);

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスの左ボタンを放した時。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        FlowA MouseLeftButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILarabeLoggerTag logTag);

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスの右ボタン押下時。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        FlowA MouseRightButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILarabeLoggerTag logTag);

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスの右ボタンを放した時。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        FlowA MouseRightButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILarabeLoggerTag logTag);

    }
}
