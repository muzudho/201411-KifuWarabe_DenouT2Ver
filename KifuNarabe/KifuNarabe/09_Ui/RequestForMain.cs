using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Grayscale.KifuwaraneEntities.L04_Common;

namespace Grayscale.KifuwaraneGui.L09_Ui
{

    /// <summary>
    /// ************************************************************************************************************************
    /// このメインパネルに、何かして欲しいという要求は、ここに入れられます。
    /// ************************************************************************************************************************
    /// </summary>
    public class RequestForMain
    {

        #region プロパティ類
        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 入力欄のテキストを上書きしたいときに設定(*1)します。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///         *1…ヌルなら、要求フラグは偽になります。
        /// </summary>
        public string RequestInputTextString
        {
            get
            {
                return this.requestInputTextString;
            }
            set
            {
                string str = value;

                if (null == str)
                {
                    this.canInputTextFlag = false;
                }
                else
                {
                    this.canInputTextFlag = true;
                }

                this.requestInputTextString = value;
            }
        }
        private string requestInputTextString;

        /// <summary>
        /// フラグ。読取専用。
        /// </summary>
        public bool CanInputTextFlag
        {
            get
            {
                return this.canInputTextFlag;
            }
        }
        private bool canInputTextFlag;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 入力欄の後ろにテキストを付け足したいときに設定(*1)します。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///         *1…ヌルなら、要求フラグは偽になります。
        /// 
        /// </summary>
        public string RequestAppendInputTextString
        {
            get
            {
                return this.requestAppendInputTextString;
            }
        }

        public void SetAppendInputTextString(string value, [CallerMemberName] string memberName = "")
        {
            if (null == value)
            {
                this.canAppendInputTextFlag = false;
            }
            else
            {
                this.canAppendInputTextFlag = true;
            }

            System.Console.WriteLine("☆セットアペンド("+memberName+")："+value);
            this.requestAppendInputTextString = value;
        }
        private string requestAppendInputTextString;

        /// <summary>
        /// フラグ。読取専用。
        /// </summary>
        public bool CanAppendInputTextFlag
        {
            get
            {
                return canAppendInputTextFlag;
            }
        }
        private bool canAppendInputTextFlag;


        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// 出力欄をクリアーしたいときは、真にしてください。
        ///------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public bool RequestClearTxtOutput
        {
            get;
            set;
        }

        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// 出力欄に棋譜を出力したいときは、真にしてください。
        ///------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public bool RequestOutputKifu
        {
            get;
            set;
        }

        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// メインパネルを再描画したいときは、真にしてください。
        ///------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public bool RequestRefresh
        {
            get;
            set;
        }

        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// 手番が交代していたら真です。
        ///------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public bool ChangedTurn
        {
            get;
            set;
        }

        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// リフレッシュして欲しい駒
        ///------------------------------------------------------------------------------------------------------------------------
        ///
        /// 要素は駒ハンドル。
        /// 
        /// </summary>
        public HashSet<K40> RequestRefresh_Komas
        {
            get;
            set;
        }

        #endregion


        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクタです。
        /// ************************************************************************************************************************
        /// </summary>
        public RequestForMain()
        {
            this.RequestRefresh_Komas = new HashSet<K40>();
        }

    }


}
