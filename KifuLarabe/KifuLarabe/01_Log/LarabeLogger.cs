using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace Grayscale.KifuwaraneLib.L01_Log
{
    /// <summary>
    /// ロガー
    /// </summary>
    public class LarabeLogger
    {
        public static LarabeLogger GetInstance()
        {
            if (null == LarabeLogger.instance)
            {
                LarabeLogger.instance = new LarabeLogger("#log_default(" + System.Diagnostics.Process.GetCurrentProcess() + ")", ".txt", true);
            }

            return LarabeLogger.instance;
        }
        private static LarabeLogger instance;



        private const bool PRINT_TIMESTAMP = false;


        /// <summary>
        /// デフォルト・ログ・ファイル
        /// </summary>
        private ILoggerFileConf defaultFile;


        /// <summary>
        /// タグの登録。リムーブに使用。
        /// </summary>
        private List<ILoggerFileConf> tagList;


        private LarabeLogger(string defaultLogFileNameWoe,string extension,bool enable)
        {
            this.defaultFile = new LibLoggerFileConf(defaultLogFileNameWoe, extension, enable);

            this.tagList = new List<ILoggerFileConf>();
        }


        public void AddLogFile(ILoggerFileConf tag)
        {
            this.tagList.Add(tag);
        }


        /// <summary>
        /// ログファイルを削除します。(連番がなければ)
        /// </summary>
        public void RemoveFile()
        {
            try
            {
                System.IO.File.Delete(this.defaultFile.FileName);

                foreach (ILoggerFileConf tag in this.tagList)
                {
                    System.IO.File.Delete(tag.FileNameWoe+tag.Extension);
                }
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = this.GetType().Name + "#RemoveFile：" + ex.Message;
                LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
            }

        }


        /// <summary>
        /// エラーを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public void WriteLineError(ILoggerFileConf tag, string line)
        {
            if (null == tag)
            {
                tag = this.defaultFile;
            }

            if (!tag.Enable)
            {
                // ログ出力オフ
                goto gt_EndMethod;
            }

            // ログ追記 TODO:非同期
            try
            {
                StringBuilder sb = new StringBuilder();

                // タイムスタンプ
                if (LarabeLogger.PRINT_TIMESTAMP)
                {
                    sb.Append(DateTime.Now.ToString());
                    sb.Append(" : ");
                }

                sb.Append(line);
                sb.AppendLine();

                string message = sb.ToString();
                MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                System.IO.File.AppendAllText(tag.FileName, message);
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = this.GetType().Name + "#WriteLineError：" + ex.Message;
                LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
            }

        gt_EndMethod:
            ;
        }


        /// <summary>
        /// メモを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public void WriteLineMemo(ILoggerFileConf tag, string line)
        {
            if (null == tag)
            {
                tag = this.defaultFile;
            }

            if (!tag.Enable)
            {
                // ログ出力オフ
                goto gt_EndMethod;
            }

            // ログ追記 TODO:非同期
            try
            {
                StringBuilder sb = new StringBuilder();

                // タイムスタンプ
                if (LarabeLogger.PRINT_TIMESTAMP)
                {
                    sb.Append(DateTime.Now.ToString());
                    sb.Append(" : ");
                }

                sb.Append(line);
                sb.AppendLine();

                System.IO.File.AppendAllText(tag.FileName, sb.ToString());
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = this.GetType().Name + "#WriteLineMemo：" + ex.Message;
                LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
            }

        gt_EndMethod:
            ;
        }


        /// <summary>
        /// サーバーへ送ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public void WriteLineS(
            string line,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            // ログ追記 TODO:非同期
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now.ToString());
                sb.Append("<   ");
                sb.Append(line);
                sb.Append("：");
                sb.Append(memberName);
                sb.Append("：");
                sb.Append(sourceFilePath);
                sb.Append("：");
                sb.Append(sourceLineNumber);
                sb.AppendLine();

                System.IO.File.AppendAllText(this.defaultFile.FileName, sb.ToString());
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = this.GetType().Name + "#WriteLineS：" + ex.Message;
                LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
            }
        }


        /// <summary>
        /// サーバーから受け取ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public void WriteLineR(
            string line,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            // ログ追記 TODO:非同期
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now.ToString());
                sb.Append("  > ");
                sb.Append(line);
                sb.Append("：");
                sb.Append(memberName);
                sb.Append("：");
                sb.Append(sourceFilePath);
                sb.Append("：");
                sb.Append(sourceLineNumber);
                sb.AppendLine();

                System.IO.File.AppendAllText(this.defaultFile.FileName, sb.ToString());
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = this.GetType().Name + "#WriteLineR：" + ex.Message;
                LarabeLogger.GetInstance().WriteLineError(LarabeLoggerTag_Impl.ERROR, message);
            }
        }
    }
}
