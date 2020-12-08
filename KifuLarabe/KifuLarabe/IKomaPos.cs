using System.Collections.Generic;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;

namespace Grayscale.KifuwaraneLib
{
    public interface IKomaPos
    {
        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 先後、升、配役
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        RO_Star Star { get; }


        
        /// <summary>
        /// 進める駒の種類と、進める先の升を指定することで、駒を次の配役に変換します。
        /// </summary>
        /// <param name="sengo"></param>
        /// <param name="dstMasu"></param>
        /// <param name="dstHaiyaku"></param>
        /// <param name="hint"></param>
        /// <returns></returns>
        IKomaPos Next(Sengo sengo, M201 dstMasu, Ks14 currentSyurui, string hint);


        /// <summary>
        /// 不成ケース
        /// </summary>
        /// <returns></returns>
        Ks14 ToFunariCase();


        
        /// <summary>
        /// 不一致判定：　先後、駒種類  が、自分と同じものが　＜ひとつもない＞
        /// </summary>
        /// <returns></returns>
        bool NeverOnaji(Kifu_Document kifuD, ILog logTag, params List<K40>[] komaGroupArgs);


        /// <summary>
        /// 成り
        /// </summary>
        bool IsNari { get; }

        /// <summary>
        /// ************************************************************************************************************************
        /// 相手陣に入っていれば真。
        /// ************************************************************************************************************************
        /// 
        ///         後手は 7,8,9 段。
        ///         先手は 1,2,3 段。
        /// </summary>
        /// <returns></returns>
        bool InAitejin{ get; }



        /// <summary>
        /// ************************************************************************************************************************
        /// 含まれるか判定。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="masu2Arr"></param>
        /// <returns></returns>
        bool ExistsIn(IMasus masu2Arr, Kifu_Document kifuD, ILog logTag);


        
        /// <summary>
        /// 外字を利用した、デバッグ用の駒の名前１文字だぜ☆
        /// </summary>
        /// <returns></returns>
        char ToGaiji();



        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋盤上にあれば真。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        bool OnShogiban { get; }


        /// <summary>
        /// 駒の表面の文字。
        /// </summary>
        string Text_Label { get; }


        /// <summary>
        /// 成れる駒なら真。
        /// </summary>
        bool IsNareruKoma { get; }

    }
}
