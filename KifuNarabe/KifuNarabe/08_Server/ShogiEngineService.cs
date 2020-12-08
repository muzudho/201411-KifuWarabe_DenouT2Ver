using System;
using System.Diagnostics;
using Grayscale.KifuwaraneGui.L09_Ui;
using Grayscale.KifuwaraneLib;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L03_Communication;
using Grayscale.KifuwaraneLib.L04_Common;
using Grayscale.KifuwaraneLib.L06_KifuIO;

namespace Grayscale.KifuwaraneGui.L08_Server
{
    /// <summary>
    /// １つの将棋エンジンと通信します。１対１の関係になります。
    /// </summary>
    public class ShogiEngineService
    {
        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// これが、将棋エンジン（プロセス）です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static Process shogiEngineProcess;

        /// <summary>
        /// 将棋エンジンが起動しているか否かです。
        /// </summary>
        /// <returns></returns>
        public static bool IsLiveShogiEngine()
        {
            return null != ShogiEngineService.shogiEngineProcess && !ShogiEngineService.shogiEngineProcess.HasExited;
        }

        /// <summary>
        /// 将棋エンジンを起動します。
        /// </summary>
        public static void StartShogiEngine(string shogiEngineFileName)
        {
            try
            {
                if (ShogiEngineService.IsLiveShogiEngine())
                {
                    System.Console.WriteLine("将棋エンジンサービスは終了していません。");
                    goto gt_EndMethod;
                }

                //------------------------------
                // ログファイルを削除します。
                //------------------------------
                Logger.RemoveAllLogFile();


                ProcessStartInfo startInfo = new ProcessStartInfo();

                startInfo.FileName = shogiEngineFileName; // 実行するファイル名決め打ち
                //startInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
                startInfo.UseShellExecute = false; // シェル機能を使用しない
                startInfo.RedirectStandardInput = true;//標準入力をリダイレクト
                startInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト

                ShogiEngineService.shogiEngineProcess = Process.Start(startInfo); // アプリの実行開始

                //  OutputDataReceivedイベントハンドラを追加
                ShogiEngineService.shogiEngineProcess.OutputDataReceived += ShogiEngineService.ReceivedData_FromShogiEngine_Async;
                ShogiEngineService.shogiEngineProcess.Exited += ShogiEngineService.ShogiEngineService_Exited;

                // 非同期受信スタート☆！
                ShogiEngineService.shogiEngineProcess.BeginOutputReadLine();

                // 「usi」
                ShogiEngineService.Send("usi");


                // エンジン設定
                // 「setoption」
                //Server.shogiEngineProcess.StandardInput.WriteLine("setoption");


                //
                // サーバー・スレッドはここで終了しますが、
                // 代わりに、Server.shogiEngineProcess プロセスが動き続け、
                // ReceivedData_FromShogiEngine メソッドが非同期に呼び出され続けます。
                //

                //this.thread = new Thread(new ThreadStart(Server.Main2));
                //this.thread.IsBackground = true;
                //this.thread.Start();

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.GetType().Name + "：" + ex.Message);
                System.Console.WriteLine($"shogiEngineFileName: {shogiEngineFileName}");
                

                // 追加。トレース情報が欲しい。
                throw;
            }

        gt_EndMethod:
            ;
        }


        /// <summary>
        /// このサービスを終了したときにする挙動を、ここに書きます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ShogiEngineService_Exited(object sender, System.EventArgs e)
        {
            ShogiEngineService.Shutdown();
        }

        public static void Shutdown()
        {
            //------------------------------------------------------------
            // 将棋エンジンに、終了するように促します。
            //------------------------------------------------------------
            if (ShogiEngineService.IsLiveShogiEngine())
            {
                ShogiEngineService.Send("quit");
            }
        }

        /// <summary>
        /// 手番が替わったときの挙動を、ここに書きます。
        /// </summary>
        public static void Message_ChangeTurn(Kifu_Document kifuD, ILogName logTag)
        {


            //------------------------------------------------------------
            // デバッグログ
            //------------------------------------------------------------
            //LarabeLogger.GetInstance().WriteLineMemo(logTag, kifuD.DebugText_Kyokumen("チェンジターン"));




            if (!ShogiEngineService.IsLiveShogiEngine())
            {
                goto gt_EndMethod;
            }

            switch (kifuD.CountSengo(kifuD.CountTeme(kifuD.Current8)))
            {
                case Sengo.Gote:
                    // 仮に、コンピューターが後手番とします。

                    //------------------------------------------------------------
                    // とりあえず、コンピューターが後手ということにしておきます。
                    //------------------------------------------------------------

                    // 例：「position startpos moves 7g7f」
                    ShogiEngineService.Send(KirokuGakari.ToSfenKifuText(kifuD));

                    ShogiEngineService.Send("go");

                    break;
                default:
                    break;
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// 将棋エンジンから、データを非同期受信(*1)します。
        /// 
        ///         *1…こっちの都合に合わせず、データが飛んできます。
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ReceivedData_FromShogiEngine_Async(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            string line = e.Data;
            //Console.WriteLine("Ｓ：非同期："+line);

            if (null == line)
            {
                // 無視
            }
            else
            {
                //>>>>>>>>>> メッセージを受け取りました。
                Logger.WriteLineR(Logger.DefaultAddress, line);

                if (line.StartsWith("option"))
                {

                }
                else if ("usiok" == line)
                {

                    //------------------------------------------------------------
                    // 「私は将棋サーバーですが、USIプロトコルのponderコマンドには対応していませんので、送ってこないでください」
                    //------------------------------------------------------------
                    ShogiEngineService.Send("setoption name USI_Ponder value false");

                    //------------------------------------------------------------
                    // 「準備はいいですか？」
                    //------------------------------------------------------------
                    ShogiEngineService.Send("isready");
                }
                else if ("readyok" == line)
                {

                    //------------------------------------------------------------
                    // 対局開始！
                    //------------------------------------------------------------
                    ShogiEngineService.Send("usinewgame");

                }
                else if (line.StartsWith("info"))
                {
                }
                else if (line.StartsWith("bestmove resign"))
                {
                    // 将棋エンジンが、投了されました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    //------------------------------------------------------------
                    // あなたの負けです☆
                    //------------------------------------------------------------
                    ShogiEngineService.Send("gameover lose");

                    //------------------------------------------------------------
                    // 将棋エンジンを終了してください☆
                    //------------------------------------------------------------
                    ShogiEngineService.Send("quit");
                }
                else if (line.StartsWith("bestmove"))
                {
                    // 将棋エンジンが、手を指されました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    Ui_PnlMain.input99 += line.Substring("bestmove".Length+"".Length);

                    Logger.TraceLine(Logs.LoggerGui, "USI受信：bestmove input99=[" + Ui_PnlMain.input99 + "]");
                }
                else
                {
                }
            }
        }


        /// <summary>
        /// 将棋エンジンにメッセージを送ります。
        /// </summary>
        /// <param name="line"></param>
        public static void Send(string line)
        {
            ShogiEngineService.shogiEngineProcess.StandardInput.WriteLine(line);

            Logger.WriteLineS(Logger.DefaultAddress,line);
        }

    }


}
