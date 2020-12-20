using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Grayscale.Kifuwarane.Gui.L01_Log
{
    /// <summary>
    /// 設定ファイル
    /// 
    /// ファイルパスは Engine.toml へ移動しました。
    /// </summary>
    public class SetteiFile
    {
        /// <summary>
        /// 設定ファイル名です。
        /// </summary>
        public string SetteiFileName
        {
            get
            {
                return this.setteiFileName;
            }
        }
        private string setteiFileName;

        /// <summary>
        /// 設定ファイルのバージョンです。
        /// </summary>
        public string SetteiFileVer
        {
            get
            {
                return this.setteiFileVer;
            }
        }
        private string setteiFileVer;

        /// <summary>
        /// 将棋エンジンの名前です。
        /// </summary>
        public string ShogiEngineName
        {
            get
            {
                return this.shogiEngineName;
            }
        }
        private string shogiEngineName;

        /// <summary>
        /// コンストラクターです。
        /// </summary>
        public SetteiFile()
        {
            this.setteiFileName = "settei.xml";
            this.setteiFileVer = "0.00.0";
            this.shogiEngineName = "The将棋エンジン";
        }

        public void DebugWrite()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($@"設定ファイル          : { this.SetteiFileName}
設定ファイルVer       : { this.SetteiFileVer}
将棋エンジン          : { this.ShogiEngineName}



");
        }

        public bool Exists()
        {
            return File.Exists(this.SetteiFileName);
        }

        public bool Read()
        {
            bool successfule = true;

            XmlDocument xDoc = new XmlDocument();

            try
            {
                xDoc.Load(this.setteiFileName);

                XmlElement xKifunarabe = xDoc.DocumentElement;
                this.setteiFileVer = xKifunarabe.GetAttribute("setteiFileVer");

                XmlNodeList xShogiEngineNodeList = xKifunarabe.GetElementsByTagName("shogiEngine");
                foreach (XmlNode xShogiEngineNode in xShogiEngineNodeList)
                {
                    XmlElement xShogiEngine = (XmlElement)xShogiEngineNode;

                    this.shogiEngineName = xShogiEngine.GetAttribute("name");
                    break;
                }
            }
            catch (Exception ex)
            {
                // エラー
                successfule = false;
                Console.WriteLine(ex.GetType().Name + "　" + ex.Message);
            }

            return successfule;
        }


        public bool Write()
        {
            bool successfule = true;

            XmlDocument xDoc = new XmlDocument();

            // UTF-8 エンコーディングで書くものとします。
            XmlProcessingInstruction xPi = xDoc.CreateProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");
            xDoc.AppendChild(xPi);

            // ルート要素 <kifunarabe> を作成
            XmlElement xKifunarabe = xDoc.CreateElement("kifunarabe");
            xDoc.AppendChild(xKifunarabe);

            // setteiFileVer="1.00.0"
            xKifunarabe.SetAttribute("setteiFileVer", this.SetteiFileVer);

            // コメント
            xKifunarabe.AppendChild(xDoc.CreateComment("v(^-^)vｲｪｰｲ☆ 『将棋ＧＵＩ きふならべ』の設定ファイルなんだぜ☆！ 今は一番上に書いてある ＜shogiEngine＞ を見に行くぜ☆"));

            // <shogiEngine>
            XmlElement xShogiEngine = xDoc.CreateElement("shogiEngine");

            // name="The将棋エンジン"
            xShogiEngine.SetAttribute("file", this.ShogiEngineName);

            xKifunarabe.AppendChild(xShogiEngine);

            // .xmlファイルを保存
            xDoc.Save(this.SetteiFileName);

            return successfule;
        }
    }
}
