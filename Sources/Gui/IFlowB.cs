using System.Windows.Forms;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Log;
using Grayscale.Kifuwarane.Gui.L07_Shape;
using Grayscale.Kifuwarane.Gui.L09_Ui;

namespace Grayscale.Kifuwarane.Gui
{

    /// <summary>
    /// 駒を動かす流れです。
    /// </summary>
    public interface FlowB
    {

        void Arrive(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag);


        /// <summary>
        /// マウス・ムーブ時。
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        FlowB MouseMove(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag);

        /// <summary>
        /// マウスの左ボタン押下時。
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        FlowB MouseLeftButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag);

        /// <summary>
        /// マウスの左ボタンを放した時。
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        FlowB MouseLeftButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag);

        /// <summary>
        /// マウスの右ボタン押下時。
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        FlowB MouseRightButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag);

        /// <summary>
        /// マウスの右ボタンを放した時。
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        FlowB MouseRightButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag);

    }
}
