using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenon.KifuLarabe
{


    /// <summary>
    /// 継承できる列挙型として利用☆
    /// </summary>
    public class LoggerTag_Larabe : LarabeLoggerTag
    {

        public static readonly LarabeLoggerTag DEFAULT = new LoggerTag_Larabe("#log_デフォルト",".txt",true);

        public string FileName { get { return this.FileNameWoe + this.Extension; } }

        public string FileNameWoe { get { return this.fileNameWoe; } }
        private string fileNameWoe;

        public string Extension { get { return this.extension; } }
        private string extension;

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        public bool Enable { get { return this.enable; } }
        private bool enable;

        public LoggerTag_Larabe(string fileNameWoe, string extension, bool enable)
        {
            this.fileNameWoe = fileNameWoe;
            this.extension = extension;
            this.enable = enable;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            LarabeLoggerTag p = obj as LarabeLoggerTag;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (this.FileNameWoe+this.Extension == p.FileNameWoe+p.Extension);
        }
    }


}
