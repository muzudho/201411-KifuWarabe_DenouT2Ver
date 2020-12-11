using System.Windows.Forms;
using Grayscale.Kifuwarane.Entities.ApplicatedGame.Architecture;
using Grayscale.Kifuwarane.Entities.Log;
using Grayscale.Kifuwarane.Gui.L07_Shape;
using Grayscale.Kifuwarane.Gui.L09_Ui;

namespace Grayscale.Kifuwarane.Gui
{

    public interface IFlowA
    {
        /// <summary>
        /// マウスの左ボタン押下時。
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        IFlowA MouseLeftButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag);

        /// <summary>
        /// マウスの左ボタンを放した時。
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        IFlowA MouseLeftButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag);

        /// <summary>
        /// マウスの右ボタン押下時。
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        IFlowA MouseRightButtonDown(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag);

        /// <summary>
        /// マウスの右ボタンを放した時。
        /// </summary>
        /// <param name="ui_PnlMain"></param>
        /// <param name="requestForMain"></param>
        /// <param name="e"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        IFlowA MouseRightButtonUp(Ui_PnlMain ui_PnlMain, ref RequestForMain requestForMain, MouseEventArgs e, Shape_PnlTaikyoku shape_PnlTaikyoku, TreeDocument kifuD, ILogTag logTag);
    }
}
