using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;

using Xenon.KifuLarabe.L04_Common;

namespace Xenon.KifuLarabe
{
    public interface TeProcess : KomaPos
    {

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 先後、升、配役
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        RO_Star SrcStar { get; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// あれば、取った駒の種類。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Ks14 TottaSyurui { get; }

        /// <summary>
        /// ************************************************************************************************************************
        /// SFEN符号表記。（取った駒付き）
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        string ToSfenText_TottaKoma();

        bool isEnableSfen();

        /// <summary>
        /// ************************************************************************************************************************
        /// SFEN符号表記。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        string ToSfenText(
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
        );

        /// <summary>
        /// ************************************************************************************************************************
        /// 元位置。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        TeProcess Src();

        /// <summary>
        /// “打” ＜アクション時＞
        /// </summary>
        /// <returns></returns>
        bool IsDaAction{ get; }

        
        /// <summary>
        /// 成った
        /// </summary>
        /// <returns></returns>
        bool IsNatta_Process{ get; }

    }
}
