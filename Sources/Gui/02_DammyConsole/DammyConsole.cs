namespace Grayscale.Kifuwarane.Gui.L02_DammyConsole
{

    /// <summary>
    /// 引数を受け取らず、１行の文字列を返すだけのメソッド型です。
    /// </summary>
    /// <returns></returns>
    public delegate string ReadTextLine();

    /// <summary>
    /// 引数として１行のテキストを１つ受け取り、何も返さないメソッド型メンバーです。
    /// </summary>
    /// <returns></returns>
    public delegate void WriteTextLine(string text);

    /// <summary>
    /// ************************************************************************************************************************
    /// 将棋エンジンの振り(*1)をしています。
    /// ************************************************************************************************************************
    /// 
    ///         *1…将棋エンジンがあれば、これを“皮”（ラッピング）にすると、ＧＵＩに対応すると思います。
    /// 
    /// </summary>
    public class DammyConsole
    {

        #region プロパティ類

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 将棋エンジン(*1)をこの中に入れておきます。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///         *1…その実、将棋エンジンの振りをさせて、このメインパネルのメソッドが入っています。
        /// 
        /// </summary>
        public static DammyConsole DefaultDammyConsole
        {
            get
            {
                return DammyConsole.defaultDammyConsole;
            }
        }

        public static void SetShogiEngine(
            ReadTextLine readTextLineDelegate,
            WriteTextLine writeTextLineDelegate
            )
        {
            DammyConsole.defaultDammyConsole = new DammyConsole(readTextLineDelegate, writeTextLineDelegate);
        }
        private static DammyConsole defaultDammyConsole;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 入力先１です。　引数を受け取らず、１行の文字列を返すだけのメソッド型メンバーです。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        private ReadTextLine readTextLineDelegate;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 出力先です。　引数として１行のテキストを１つ受け取り、何も返さないメソッド型メンバーです。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        private WriteTextLine writeTextLineDelegate;

        #endregion


        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        public DammyConsole(
            ReadTextLine readTextLineDelegate,
            WriteTextLine writeTextLineDelegate
            )
        {
            this.readTextLineDelegate = readTextLineDelegate;
            this.writeTextLineDelegate = writeTextLineDelegate;
        }



        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// １行の文字列を返します。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public string ReadLine1()
        {
            string value;

            if (null == this.readTextLineDelegate)
            {
                // 入力先が設定されていませんでした。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                value = "";
                goto gt_EndMethod;
            }

            value = this.readTextLineDelegate();

        gt_EndMethod:
            return value;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 出力欄（上段）に１行のテキストをセットします。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public void WriteTextLine(string text)
        {
            if (null == this.writeTextLineDelegate)
            {
                // 出力先１が設定されていませんでした。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                goto gt_EndMethod;
            }

            this.writeTextLineDelegate(text);

        gt_EndMethod:
            ;
        }

    }
}
