using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Grayscale.KifuwaraneLib.Entities.ApplicatedGame;
using Grayscale.KifuwaraneLib.Entities.Log;
using Grayscale.KifuwaraneLib.L01_Log;
using Grayscale.KifuwaraneLib.L03_Communication;

namespace Grayscale.KifuwaraneLib.L04_Common
{
    /// <summary>
    /// 強制転成
    /// 
    /// 駒配役生成後に生成すること。
    /// </summary>
    public class ForcePromotionArray
    {
        /// <summary>
        /// 配役ハンドル→升ハンドル→次配役ハンドルの連鎖なんだぜ☆
        /// </summary>
        public static Dictionary<Kh185, Dictionary<int, Kh185>> HaiyakuMap
        {
            get
            {
                return ForcePromotionArray.haiyakuMap;
            }
        }
        private static Dictionary<Kh185, Dictionary<int, Kh185>> haiyakuMap;

        public static List<List<string>> Load(string path, Encoding encoding)
        {
            List<List<string>> rows = new List<List<string>>();

            foreach (string line in File.ReadAllLines(path, encoding))
            {
                rows.Add(CsvLineParserImpl.UnescapeLineToFieldList(line, ','));
            }


            // 最初の2行は削除。
            rows.RemoveRange(0, 2);

            // 各行の先頭2列は削除。
            foreach (List<string> row in rows)
            {
                row.RemoveRange(0, 2);
            }


            //------------------------------
            // データ部だけが残っています。
            //------------------------------
            ForcePromotionArray.haiyakuMap = new Dictionary<Kh185, Dictionary<int, Kh185>>();

            int haiyakuHandle = 0;
            int rowCount = 0;
            foreach (List<string> row in rows)
            {
                // 偶数行はコメントなので無視します。
                if(rowCount%2==0)
                {
                    goto gt_NextRow;
                }

                //----------
                // 配役
                //----------
                Dictionary<int, Kh185> map2 = new Dictionary<int, Kh185>();
                ForcePromotionArray.haiyakuMap.Add(
                    Kh185Array.Items[haiyakuHandle],
                    map2
                    );

                int masuHandle = 0;
                foreach(string column in row)
                {
                    // 空っぽの列は無視します。
                    if(""==column)
                    {
                        goto gt_NextColumn;
                    }

                    // 空っぽでない列の値を覚えます。

                    // 数値型のはずです。
                    int haiyakuHandle_target;
                    if(!int.TryParse(column, out haiyakuHandle_target))
                    {
                        string message = "エラー。\n path=[" + path + "]\n" +
                        "強制転成表に、int型数値でないものが指定されていました。\n" +
                        "rowCount=[" + rowCount + "]\n" +
                        "masuHandle=[" + masuHandle + "]\n";
                        LoggerPool.ErrorLine(LarabeLoggerTag_Impl.ERROR, message);
                        throw new Exception(message);
                    }

                    map2.Add(masuHandle, Kh185Array.Items[ haiyakuHandle_target]);

                gt_NextColumn:
                    masuHandle++;
                }


                haiyakuHandle++;

            gt_NextRow:
                rowCount++;
            }

            return rows;
        }



        /// <summary>
        /// ロードした内容を確認するときに使います。
        /// </summary>
        /// <returns></returns>
        public static string DebugString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<Kh185, Dictionary<int, Kh185>> entry1 in ForcePromotionArray.HaiyakuMap)
            {
                sb.Append(entry1.Key);
                sb.Append("：　");

                foreach (KeyValuePair<int, Kh185> entry2 in entry1.Value)
                {
                    sb.Append(
                        GameTranslator.SqToJapanese(entry2.Key)
                        );
                    sb.Append(".");
                    sb.Append(entry2.Value);
                    sb.Append("　");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }


        /// <summary>
        /// ロードした内容を確認するときに使います。
        /// </summary>
        /// <returns></returns>
        public static string DebugHtml()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("    <title>強制転成表</title>");
            sb.AppendLine("    <style type=\"text/css\">");
            sb.AppendLine("            /* 将棋盤 */");
            sb.AppendLine("            table{");
            sb.AppendLine("                border-collapse:collapse;");
            sb.AppendLine("                border:2px #2b2b2b solid;");
            sb.AppendLine("            }");
            sb.AppendLine("            td{");
            sb.AppendLine("                border:1px #2b2b2b solid;");
            sb.AppendLine("                background-color:#ffcc55;");
            sb.AppendLine("                width:48px; height:48px;");
            sb.AppendLine("            }");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");

            foreach (KeyValuePair<Kh185, Dictionary<int, Kh185>> entry1 in ForcePromotionArray.HaiyakuMap)
            {
                sb.Append("<h1>");
                sb.Append(entry1.Key);
                sb.AppendLine("</h1>");


                sb.Append("<table>");
                // ９一～１一、９二～１二、…９九～１九の順だぜ☆
                for (int dan = 1; dan <= 9; dan++ )
                {
                    sb.AppendLine("<tr>");

                    sb.Append("    ");
                    for (int suji = 9; suji >= 1; suji--)
                    {

                        M201 masu = M201Util.OkibaSujiDanToMasu( Okiba.ShogiBan, suji, dan);

                        sb.Append("<td>");

                        if (entry1.Value.ContainsKey((int)masu))
                        {
                            // 強制転成が起こるマスなら、画像を出します。


                            Kh185 kh184 = entry1.Value[(int)masu];
                            int haiyakuHandle = (int)kh184;

                            
                            sb.Append("<img src=\"./img/train");


                            if (haiyakuHandle<10)
                            {
                                sb.Append("00");
                            }
                            else if (haiyakuHandle < 100)
                            {
                                sb.Append("0");
                            }
                            sb.Append(haiyakuHandle);
                            sb.Append(".png\" />");


                        }

                        sb.Append("</td>");
                    }
                    sb.AppendLine();
                    sb.AppendLine("</tr>");

                }
                sb.AppendLine("</table>");
            }


            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }


    }


}
