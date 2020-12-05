using System.Runtime.CompilerServices;

using Xenon.KifuLarabe.L04_Common;
using Xenon.KifuLarabe.L06_KifuIO;

namespace Xenon.KifuLarabe
{
    public interface KifuParserA
    {

        DELEGATE_RefreshHirate Delegate_RefreshHirate { get; set; }


        DELEGATE_RefreshShiteiKyokumen Delegate_RefreshShiteiKyokumen { get; set; }


        DELEGATE_IttesasiPaint Delegate_IttesasiPaint { get; set; }


                /// <summary>
        /// １ステップずつ実行します。
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifuD"></param>
        /// <param name="larabeLogger"></param>
        /// <returns></returns>
        string Execute_Step(
            string inputLine,
            Kifu_Document kifuD,
            ref bool isBreak,
            string hint,
            LarabeLoggerTag logTag
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            );

        /// <summary>
        /// 最初から最後まで実行します。
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifuD"></param>
        /// <param name="larabeLogger"></param>
        void Execute_All(
            string inputLine,
            Kifu_Document kifuD,
            string hint,
            LarabeLoggerTag logTag
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            );

    }
}
