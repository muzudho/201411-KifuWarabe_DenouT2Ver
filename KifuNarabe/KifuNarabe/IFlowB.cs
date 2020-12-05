using System.Windows.Forms;
using Grayscale.KifuwaraneGui.L07_Shape;
using Grayscale.KifuwaraneGui.L09_Ui;
using Grayscale.KifuwaraneLib;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneGui
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 駒を動かす流れです。
    /// ************************************************************************************************************************
    /// </summary>
    public interface FlowB
    {

        void Arrive(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILoggerFileConf logTag);


        /// <summary>
        /// ************************************************************************************************************************
        /// マウス・ムーブ時。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        FlowB MouseMove(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILoggerFileConf logTag);

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
        FlowB MouseLeftButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILoggerFileConf logTag);

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
        FlowB MouseLeftButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILoggerFileConf logTag);

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
        FlowB MouseRightButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILoggerFileConf logTag);

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
        FlowB MouseRightButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILoggerFileConf logTag);

    }
}
