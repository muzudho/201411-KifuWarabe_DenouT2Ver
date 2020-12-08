using System.Runtime.CompilerServices;
using Grayscale.KifuwaraneEntities.ApplicatedGame.Architecture;
using Grayscale.KifuwaraneEntities.L06_KifuIO;
using Grayscale.KifuwaraneEntities.Log;

namespace Grayscale.KifuwaraneEntities
{
    public interface IKifuParserA
    {

        RefreshHirateDelegate OnRefreshHirate { get; set; }


        RefreshShiteiKyokumenDelegate OnRefreshShiteiKyokumen { get; set; }


        IttesasiPaintDelegate OnIttesasiPaint { get; set; }


                /// <summary>
        /// １ステップずつ実行します。
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifuD"></param>
        /// <param name="larabeLogger"></param>
        /// <returns></returns>
        string Execute_Step(
            string inputLine,
            TreeDocument kifuD,
            ref bool isBreak,
            string hint,
            ILogTag logTag
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
            TreeDocument kifuD,
            string hint,
            ILogTag logTag
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            );

    }
}
