using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Grayscale.KifuwaraneEntities;
using Grayscale.KifuwaraneEntities.ApplicatedGame;
using Grayscale.KifuwaraneEntities.L04_Common;
using Grayscale.KifuwaraneEntities.L06_KifuIO;
using Grayscale.KifuwaraneEntities.Log;
using Grayscale.KifuwaraneEntities.Misc;
using Grayscale.KifuwaraneGui.L01_Log;
using Grayscale.KifuwaraneGui.L02_DammyConsole;
using Grayscale.KifuwaraneGui.L07_Shape;

namespace Grayscale.KifuwaraneGui.L09_Ui
{

    /// <summary>
    /// ************************************************************************************************************************
    /// メイン画面です。
    /// ************************************************************************************************************************
    /// </summary>
    [Serializable]
    public partial class Ui_PnlMain : UserControl
    {

        #region プロパティー類

        public SetteiFile SetteiFile
        {
            get
            {
                return this.setteiFile;
            }
        }
        private SetteiFile setteiFile;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// ゲームの流れの状態遷移図はこれです。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public IFlowA FlowA
        {
            get
            {
                return this.flowA;
            }
        }

        public void SetFlowA(IFlowA flowA)
        {
            this.flowA = flowA;
        }

        private IFlowA flowA;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒を動かす状態遷移図はこれです。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public FlowB FlowB
        {
            get
            {
                return this.flowB;
            }
        }

        public void SetFlowB(FlowB flowB, ref RequestForMain requestForMain, Shape_PnlTaikyoku shape_PnlTaikyoku, Kifu_Document kifuD, ILogTag logTag)
        {
            this.flowB = flowB;
            this.flowB.Arrive(this, ref requestForMain, shape_PnlTaikyoku, kifuD, logTag);
        }

        private FlowB flowB;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// グラフィックを描くツールは全部この中です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_PnlTaikyoku Shape_PnlTaikyoku
        {
            get
            {
                return this.shape_PnlTaikyoku;
            }
        }
        private Shape_PnlTaikyoku shape_PnlTaikyoku;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 将棋の状況は全部この中です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Kifu_Document Kifu_Document
        {
            get
            {
                return this.kifu_Document;
            }
        }
        private Kifu_Document kifu_Document;

        #endregion


        #region ゲームエンジンの振りをするメソッド

        /// <summary>
        /// ************************************************************************************************************************
        /// 入力欄のテキストを取得します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        public string ReadLine1()
        {
            return this.txtInput1.Text;
        }

        private const int NSQUARE = 9 * 9;

        /// <summary>
        /// ************************************************************************************************************************
        /// 入力欄のテキストを取得します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        public string ReadLine2(ILogTag logTag)
        {
            int lastTeme = this.Kifu_Document.CountTeme(this.Kifu_Document.Current8);


            // 現在の局面は、これが駒の配列になっています。
            // this.Kifu.Kyokumen.KomaDoors[～]

            //------------------------------------------------------------
            // 表について
            //------------------------------------------------------------

            //
            // 配列の添え字は次の通り。
            //
            //    ９　８　７　６　５　４　３　２　１
            //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
            //  │ 0│ 1│ 2│ 3│ 4│ 5│ 6│ 7│ 8│一
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │ 9│10│11│12│13│14│15│16│17│二
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │18│19│20│21│22│23│24│25│26│三
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │27│28│29│30│31│32│33│34│35│四
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│36│37│38│39│40│41│42│43│44│五
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │45│46│47│48│49│50│51│52│53│六
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │54│55│56│57│58│59│60│61│62│七
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │63│64│65│66│67│68│69│70│71│八
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │72│73│74│75│76│77│78│79│80│九
            //  └─┴─┴─┴─┴─┴─┴─┴─┴─┘

            //------------------------------------------------------------
            // 先手駒について
            //------------------------------------------------------------

            // 先手駒の位置を表にします。
            bool[] wallSTable = new bool[Ui_PnlMain.NSQUARE];

            // 先手駒の利きを表にします。
            bool[] kikiSTable = new bool[Ui_PnlMain.NSQUARE];

            Kifu_Node6 siteiNode = (Kifu_Node6)this.Kifu_Document.ElementAt8(this.Kifu_Document.CountTeme(this.Kifu_Document.Current8));

            foreach (K40 koma in Util_KyokumenReader.Komas_ByOkibaSengo(siteiNode, Okiba.ShogiBan, Sengo.Sente, logTag))
            {
                IKifuElement dammyNode4 = this.Kifu_Document.ElementAt8(lastTeme);
                KomaHouse house5 = dammyNode4.KomaHouse;

                IKomaPos komaP = house5.KomaPosAt(koma);

                int suji = Mh201Util.MasuToSuji(komaP.Star.Masu);
                int dan = Mh201Util.MasuToDan(komaP.Star.Masu);

                // 壁
                wallSTable[(dan - 1) * 9 + (9 - suji)] = true;

                // 利き
                kikiSTable[(dan - 1) * 9 + (9 - suji)] = true;//FIXME:嘘
            }

            //------------------------------------------------------------
            // 後手駒について
            //------------------------------------------------------------

            // 先手駒の位置を表にします。
            bool[] wallGTable = new bool[Ui_PnlMain.NSQUARE];

            // 先手駒の利きを表にします。
            bool[] kikiGTable = new bool[Ui_PnlMain.NSQUARE];

            foreach (K40 koma in Util_KyokumenReader.Komas_ByOkibaSengo(siteiNode, Okiba.ShogiBan, Sengo.Gote, logTag))
            {
                IKifuElement dammyNode5 = this.Kifu_Document.ElementAt8(lastTeme);
                KomaHouse house5 = dammyNode5.KomaHouse;

                IKomaPos komaP = house5.KomaPosAt(koma);

                int suji = Mh201Util.MasuToSuji(komaP.Star.Masu);
                int dan = Mh201Util.MasuToDan(komaP.Star.Masu);

                // 壁
                wallGTable[(dan - 1) * 9 + (9 - suji)] = true;

                // 利き
                kikiGTable[(dan - 1) * 9 + (9 - suji)] = true;//FIXME:嘘
            }


            string tuginoItte = "▲９九王嘘";


            List<K40> komas = Util_KyokumenReader.Komas_ByOkibaSengo(siteiNode, Okiba.ShogiBan, this.Kifu_Document.CountSengo(this.Kifu_Document.CountTeme(this.Kifu_Document.Current8)), logTag);
            if (0<komas.Count)
            {
                K40 koma = komas[ RandomLib.Random.Next(komas.Count)];//ランダムに１つ。

                IKifuElement dammyNode5 = this.Kifu_Document.ElementAt8(lastTeme);
                KomaHouse house6 = dammyNode5.KomaHouse;

                IKomaPos komaP = house6.KomaPosAt(koma);

                MoveImpl tuginoItteData;
                switch (this.kifu_Document.CountSengo(this.Kifu_Document.CountTeme(this.Kifu_Document.Current8)))
                {
                    case Sengo.Gote:
                        {
                            // 後手番です。
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                            int suji = Mh201Util.MasuToSuji(komaP.Star.Masu);
                            int dan = Mh201Util.MasuToDan(komaP.Star.Masu);

                            // 前に１つ突き出させます。
                            tuginoItteData = MoveImpl.Next3(

                                new RO_Star(
                                    this.Kifu_Document.CountSengo(this.Kifu_Document.CountTeme(this.Kifu_Document.Current8)),
                                    M201Util.OkibaSujiDanToMasu(
                                        M201Util.GetOkiba(komaP.Star.Masu),
                                        suji,
                                        dan
                                        ),
                                    komaP.Star.Haiyaku
                                ),

                                new RO_Star(
                                    this.Kifu_Document.CountSengo(this.Kifu_Document.CountTeme(this.Kifu_Document.Current8)),
                                    M201Util.OkibaSujiDanToMasu(
                                        Okiba.ShogiBan,
                                        suji,
                                        dan + 1
                                        ),
                                    komaP.Star.Haiyaku
                                ),

                                Ks14.H00_Null
                            );
                            break;
                        }
                    default:
                        {
                            // 先手番です。
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                            int suji = Mh201Util.MasuToSuji(komaP.Star.Masu);
                            int dan = Mh201Util.MasuToDan(komaP.Star.Masu);

                            // 前に１つ突き出させます。
                            tuginoItteData = MoveImpl.Next3(

                                new RO_Star(
                                    this.kifu_Document.CountSengo(this.kifu_Document.CountTeme(this.kifu_Document.Current8)),
                                    M201Util.OkibaSujiDanToMasu(
                                        M201Util.GetOkiba(komaP.Star.Masu),
                                        suji,
                                        dan
                                        ),
                                    komaP.Star.Haiyaku
                                ),

                                new RO_Star(
                                    this.kifu_Document.CountSengo(this.kifu_Document.CountTeme(this.kifu_Document.Current8)),
                                    M201Util.OkibaSujiDanToMasu(
                                        Okiba.ShogiBan,
                                        suji,
                                        dan - 1
                                        ),
                                    komaP.Star.Haiyaku
                                ),

                                Ks14.H00_Null
                            );
                            break;
                        }
                }


                FugoJ fugoJ = JFugoCreator15Array.ItemMethods[(int)Haiyaku184Array.Syurui(tuginoItteData.SrcStar.Haiyaku)](tuginoItteData, this.Kifu_Document, logTag);//「▲２二角成」なら、馬（dst）ではなくて角（src）。

                //RO_TeProcess last;
                //{
                //    IKifuElement kifuElement = this.Kifu_Document.ElementAt8(this.Kifu_Document.CountTeme(this.Kifu_Document.Current8));

                //    last = kifuElement.TeProcess;
                //}
                Kifu_Node6 node6 = new Kifu_Node6(tuginoItteData,
                    this.kifu_Document.Current8.KomaHouse//FIXME:
                    );
                tuginoItte = fugoJ.ToText_UseDou(node6);//, last
            }


            return tuginoItte;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 出力欄にテキストを出力します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        public void WriteLine(string text)
        {
            this.txtOutput1.Text = text;
        }

        #endregion



        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        public Ui_PnlMain()
        {
            InitializeComponent();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            ILogTag logTag = LogTags.GuiRecordLog;

            // 将棋エンジンからの入力が、input99 に溜まるものとします。
            if (0 < Ui_PnlMain.input99.Length)
            {

                //System.Console.WriteLine("timer input99=[" + input99 + "]");
                Logger.TraceLine(logTag, "timer入力 input99=[" + Ui_PnlMain.input99 + "]");

                this.AppendInput1Text(Ui_PnlMain.input99);
                Ui_PnlMain.input99 = "";


                // 入力欄を空っぽにします。
                RequestForMain requestForMain = new RequestForMain();
                requestForMain.SetAppendInputTextString(Ui_PnlMain.input99);
                this.Response(requestForMain, logTag);


                // ＧＵＩをコマ送りします。
                Ui_01Menu.Komaokuri_GUI(
                    ref requestForMain,
                    shape_PnlTaikyoku,
                    this.Kifu_Document,
                    Util_MenuDrawer.DrawKomaokuri1,
                    Util_MenuDrawer.DrawKomaokuri2,
                    "Ui_PnlMain#timer1_Tick",
                    logTag
                    );

                // 反映させます。
                this.Response(requestForMain, logTag);
            }

        }
        //private static Thread thread99;
        public static string input99 = "";



        /// <summary>
        /// ************************************************************************************************************************
        /// 起動直後の流れです。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_PnlMain_Load(object sender, EventArgs e)
        {
            ILogTag logTag = LogTags.GuiRecordLog;


            this.setteiFile = new SetteiFile();
            if (!this.SetteiFile.Exists())
            {
                // ファイルが存在しませんでした。

                // 作ります。
                this.SetteiFile.Write();
            }

            if (!this.SetteiFile.Read())
            {
                // 読取に失敗しました。
            }

            // デバッグ
            this.SetteiFile.DebugWrite();


            //----------
            // 棋譜
            //----------
            //
            //      先後や駒など、対局に用いられる事柄、物を事前準備しておきます。
            //
            this.kifu_Document = new Kifu_Document();

            //----------
            // ブラシ
            //----------
            //
            //      ボタンや将棋盤などを描画するツールを、事前準備しておきます。
            //
            this.shape_PnlTaikyoku = new Shape_PnlTaikyoku();



            //----------
            // 駒の並べ方
            //----------
            //
            //      平手に並べます。
            //
            SyokiHaichi.ToHirate(this.Kifu_Document,logTag);
            // 再描画
            foreach (Shape_BtnKoma btnKoma in shape_PnlTaikyoku.BtnKomaDoors)
            {
                Ui_02Action.Refresh_KomaLocation(K40Array.Items_All[(int)btnKoma.Koma], shape_PnlTaikyoku, this.Kifu_Document, logTag);
            }

            //----------
            // フェーズ
            //----------
            this.SetFlowA(new FlowA_1Taikyoku());
            RequestForMain requestForMain = new RequestForMain();
            this.SetFlowB(new FlowB_1TumamitaiKoma(), ref requestForMain, this.Shape_PnlTaikyoku, this.Kifu_Document, logTag);

            //----------
            // 監視
            //----------
            this.gameEngineTimer1.Start();

            //----------
            // 将棋エンジンが、コンソールの振りをします。
            //----------
            //
            //      このメインパネルに、コンソールの振りをさせます。
            //      将棋エンジンがあれば、将棋エンジンの入出力を返すように内部を改造してください。
            //
            DammyConsole.SetShogiEngine(this.ReadLine1, this.WriteLine);


            //----------
            // 画面の出力欄
            //----------
            //
            //      出力欄（上下段）を空っぽにします。
            //
            this.WriteLine("");



            // これで、最初に見える画面の準備は終えました。
            // あとは、操作者の入力を待ちます。
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 描画するのはここです。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_PnlMain_Paint(object sender, PaintEventArgs e)
        {
            if (null == this.Shape_PnlTaikyoku)
            {
                goto gt_EndMethod;
            }

            //------------------------------
            // 画面の描画です。
            //------------------------------
            this.Shape_PnlTaikyoku.Paint(sender, e, this.Kifu_Document, LogTags.GuiPaint);

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスが動いたときの挙動です。
        /// ************************************************************************************************************************
        /// 
        ///         マウスが重なったときの、表示物の反応や、将棋データの変更がこの中に書かれています。
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_PnlMain_MouseMove(object sender, MouseEventArgs e)
        {
            ILogTag logTag = LogTags.GuiRecordLog;

            if (null != this.Shape_PnlTaikyoku)
            {
                // このメインパネルに、何かして欲しいという要求は、ここに入れられます。
                RequestForMain requestForMain = new RequestForMain();

                this.FlowB.MouseMove(this, ref requestForMain, e, this.Shape_PnlTaikyoku, this.Kifu_Document, logTag);

                //------------------------------
                // このメインパネルの反応
                //------------------------------
                this.Response(requestForMain,logTag);
            }
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスのボタンを押下したときの挙動です。
        /// ************************************************************************************************************************
        /// 
        ///         マウスボタンが押下されたときの、表示物の反応や、将棋データの変更がこの中に書かれています。
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_PnlMain_MouseDown(object sender, MouseEventArgs e)
        {
            ILogTag logTag = LogTags.GuiRecordLog;

            if (null != shape_PnlTaikyoku)
            {
                // このメインパネルに、何かして欲しいという要求は、ここに入れられます。
                RequestForMain requestForMain = new RequestForMain();


                if (e.Button == MouseButtons.Left)
                {
                    //------------------------------------------------------------
                    // 左ボタン
                    //------------------------------------------------------------
                    IFlowA nextPhase = this.FlowA.MouseLeftButtonDown(this, ref requestForMain, e, shape_PnlTaikyoku, this.Kifu_Document, logTag);

                    if (null != nextPhase)
                    {
                        this.SetFlowA(nextPhase);
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    //------------------------------------------------------------
                    // 右ボタン
                    //------------------------------------------------------------
                    IFlowA nextPhase = this.FlowA.MouseRightButtonDown(this, ref requestForMain, e, shape_PnlTaikyoku, this.Kifu_Document, logTag);

                    if (null != nextPhase)
                    {
                        this.SetFlowA(nextPhase);
                    }
                }


                //------------------------------
                // このメインパネルの反応
                //------------------------------
                this.Response(requestForMain,logTag);
            }
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスのボタンが放されたときの挙動です。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_PnlMain_MouseUp(object sender, MouseEventArgs e)
        {
            ILogTag logTag = LogTags.GuiRecordLog;

            // このメインパネルに、何かして欲しいという要求は、ここに入れられます。
            RequestForMain requestForMain = new RequestForMain();

            //------------------------------
            // マウスボタンが放されたときの、表示物の反応や、将棋データの変更がこの中に書かれています。
            //------------------------------
            if (e.Button == MouseButtons.Left)
            {
                //------------------------------------------------------------
                // 左ボタン
                //------------------------------------------------------------
                IFlowA nextPhaseA = this.FlowA.MouseLeftButtonUp(this, ref requestForMain, e, shape_PnlTaikyoku, this.Kifu_Document, logTag);

                if (null != nextPhaseA)
                {
                    this.SetFlowA(nextPhaseA);
                }

            }
            else if (e.Button == MouseButtons.Right)
            {
                //------------------------------------------------------------
                // 右ボタン
                //------------------------------------------------------------
                IFlowA nextPhaseA = this.FlowA.MouseRightButtonUp(this, ref requestForMain, e, shape_PnlTaikyoku, this.Kifu_Document, logTag);

                if (null != nextPhaseA)
                {
                    this.SetFlowA(nextPhaseA);
                }

            }

            //------------------------------
            // このメインパネルの反応
            //------------------------------
            this.Response(requestForMain,logTag);
        }

        private void SetInput1Text(string value)
        {
            //System.Console.WriteLine("☆セット：" + value);
            this.txtInput1.Text = value;
        }

        private void AppendInput1Text(string value,[CallerMemberName] string memberName = "")
        {
            System.Console.WriteLine("☆アペンド(" + memberName + ")：" + value);
            this.txtInput1.Text += value;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 入力欄の表示・出力欄の表示・再描画
        /// ************************************************************************************************************************
        /// 
        /// このメインパネルに何かして欲しいことがあれば、
        /// RequestForMain に要望を入れて、この関数を呼び出してください。
        ///
        /// 同時には処理できない項目もあります。
        /// </summary>
        /// <param name="requestForMain"></param>
        public void Response(
            RequestForMain requestForMain, ILogTag logTag)
        {
            //------------------------------
            // 入力欄の表示
            //------------------------------
            if (requestForMain.CanInputTextFlag)
            {
                // 指定のテキストで上書きします。
                this.SetInput1Text( requestForMain.RequestInputTextString);
            }
            else if (requestForMain.CanAppendInputTextFlag)
            {
                // 指定のテキストを後ろに足します。
                this.AppendInput1Text( requestForMain.RequestAppendInputTextString);
            }

            //------------------------------
            // 出力欄（上・下段）の表示
            //------------------------------
            if (requestForMain.RequestOutputKifu)
            {
                // 出力欄（上下段）に、棋譜を出力します。
                switch (this.Shape_PnlTaikyoku.SyuturyokuKirikae)
                {
                    case SyuturyokuKirikae.Japanese:
                        this.WriteLine(KirokuGakari.ToJapaneseKifuText(this.Kifu_Document, LogTags.GuiRecordLog));
                        break;
                    case SyuturyokuKirikae.Sfen:
                        this.WriteLine(KirokuGakari.ToSfenKifuText(this.Kifu_Document));
                        break;
                    case SyuturyokuKirikae.Html:
                        this.WriteLine(Ui_01Menu.CreateHtml(this.Kifu_Document));
                        break;
                }

                // ログ
                Logger.TraceLine(logTag, this.txtOutput1.Text);
            }
            else if (requestForMain.RequestClearTxtOutput)
            {
                // 出力欄（上下段）を空っぽにします。
                this.WriteLine("");

                // ログ
                Logger.TraceLine(logTag, "");
                Logger.TraceLine(logTag, "");
            }

            //------------------------------
            // 再描画
            //------------------------------
            if (requestForMain.RequestRefresh)
            {
                this.Refresh();
            }
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 出力欄（上段）でキーボードのキーが押されたときの挙動をここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOutput1_KeyDown(object sender, KeyEventArgs e)
        {
            AspectOriented_TextBox.KeyDown_SelectAll(sender, e);
            ////------------------------------
            //// [Ctrl]+[A] で、全選択します。
            ////------------------------------
            //if (e.KeyCode == System.Windows.Forms.Keys.A & e.Control == true)
            //{
            //    ((TextBox)sender).SelectAll();
            //} 
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 入力欄でキーボードのキーが押されたときの挙動をここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput1_KeyDown(object sender, KeyEventArgs e)
        {
            AspectOriented_TextBox.KeyDown_SelectAll(sender, e);
            ////------------------------------
            //// [Ctrl]+[A] で、全選択します。
            ////------------------------------
            //if (e.KeyCode == System.Windows.Forms.Keys.A & e.Control == true)
            //{
            //    ((TextBox)sender).SelectAll();
            //} 
        }
    }
}
