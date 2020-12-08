using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Grayscale.KifuwaraneEntities.ApplicatedGame;
using Grayscale.KifuwaraneEntities.Log;
using Grayscale.KifuwaraneEntities.Misc;

namespace Grayscale.KifuwaraneEntities.L04_Common
{
    /// <summary>
    /// 配役転換表。
    /// </summary>
    public class Data_HaiyakuTransition
    {
        /// <summary>
        /// 種類ハンドル→升ハンドル→次配役ハンドルの連鎖なんだぜ☆
        /// </summary>
        public static Dictionary<Ks14, Kh185[]> Map
        {
            get
            {
                return Data_HaiyakuTransition.map;
            }
        }
        private static Dictionary<Ks14, Kh185[]> map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syurui"></param>
        /// <param name="masuHandle_shogiban">0～80</param>
        /// <returns></returns>
        public static Kh185 ToHaiyaku(Ks14 syurui, int masuHandle_shogiban)
        {
            Kh185 result;

            if (Mh201Util.OnShogiban(masuHandle_shogiban))
            {
                result = Data_HaiyakuTransition.Map[syurui][(int)masuHandle_shogiban];
            }
            else if (Mh201Util.OnKomadai(masuHandle_shogiban))
            {
                switch(syurui)
                {
                    case Ks14.H01_Fu: result = Kh185.n164_歩打; break;
                    case Ks14.H02_Kyo: result = Kh185.n165_香打; break;
                    case Ks14.H03_Kei: result = Kh185.n166_桂打; break;
                    case Ks14.H04_Gin: result = Kh185.n167_銀打; break;
                    case Ks14.H05_Kin: result = Kh185.n168_金打; break;
                    case Ks14.H06_Oh: result = Kh185.n169_王打; break;
                    case Ks14.H07_Hisya: result = Kh185.n170_飛打; break;
                    case Ks14.H08_Kaku: result = Kh185.n171_角打; break;
                    case Ks14.H09_Ryu: result = Kh185.n170_飛打; break;
                    case Ks14.H10_Uma: result = Kh185.n171_角打; break;
                    case Ks14.H11_Tokin: result = Kh185.n164_歩打; break;
                    case Ks14.H12_NariKyo: result = Kh185.n165_香打; break;
                    case Ks14.H13_NariKei: result = Kh185.n166_桂打; break;
                    case Ks14.H14_NariGin: result = Kh185.n167_銀打; break;
                    default: result = Kh185.n000_未設定; break;
                }
            }
            else if (Mh201Util.OnKomabukuro(masuHandle_shogiban))
            {
                switch (syurui)
                {
                    case Ks14.H01_Fu: result = Kh185.n172_駒袋歩; break;
                    case Ks14.H02_Kyo: result = Kh185.n173_駒袋香; break;
                    case Ks14.H03_Kei: result = Kh185.n174_駒袋桂; break;
                    case Ks14.H04_Gin: result = Kh185.n175_駒袋銀; break;
                    case Ks14.H05_Kin: result = Kh185.n176_駒袋金; break;
                    case Ks14.H06_Oh: result = Kh185.n177_駒袋王; break;
                    case Ks14.H07_Hisya: result = Kh185.n178_駒袋飛; break;
                    case Ks14.H08_Kaku: result = Kh185.n179_駒袋角; break;
                    case Ks14.H09_Ryu: result = Kh185.n180_駒袋竜; break;
                    case Ks14.H10_Uma: result = Kh185.n181_駒袋馬; break;
                    case Ks14.H11_Tokin: result = Kh185.n182_駒袋と金; break;
                    case Ks14.H12_NariKyo: result = Kh185.n183_駒袋杏; break;
                    case Ks14.H13_NariKei: result = Kh185.n184_駒袋圭; break;
                    case Ks14.H14_NariGin: result = Kh185.n185_駒袋全; break;
                    default: result = Kh185.n000_未設定; break;
                }
            }
            else
            {
                result = Kh185.n000_未設定;
            }

            return result;
        }


        public static List<List<string>> Load(string path, Encoding encoding)
        {
            StringBuilder sbDebug = new StringBuilder();

            List<List<string>> rows = new List<List<string>>();

            foreach (string line in File.ReadAllLines(path, encoding))
            {
                rows.Add(CsvLineParserImpl.UnescapeLineToFieldList(line, ','));
            }


            // 最初の2行は削除。
            rows.RemoveRange(0, 2);

            // 各行の先頭3列は削除。
            foreach (List<string> row in rows)
            {
                row.RemoveRange(0, 3);
            }


            //------------------------------
            // データ部だけが残っています。
            //------------------------------


            // コメント行、データ行が交互に出てきます。
            // コメント行を削除します。
            List<List<string>> rows2;
            {
                rows2 = new List<List<string>>();

                int rowCount1 = 0;
                foreach (List<string> row in rows)
                {
                    // 奇数行がデータです。
                    if (rowCount1 % 2 == 1)
                    {
                        rows2.Add(row);
                    }

                    rowCount1++;
                }
            }

            // デバッグ出力
            {
                StringBuilder sb = new StringBuilder();

                foreach (List<string> row2 in rows2)
                {

                    foreach (string field in row2)
                    {
                        sb.Append(field);
                        sb.Append(",");

                    }
                    sb.AppendLine();
                }

                Logger.WriteFile(LogTags.HaichiTenkanHyoOnlyDataLog, sb.ToString());
            }




            Data_HaiyakuTransition.map = new Dictionary<Ks14, Kh185[]>();


            int rowCount2 = 0;
            Kh185[] table81 = null;
            foreach (List<string> row2 in rows2)
            {
                if (rowCount2 % 9 == 0)
                {
                    table81 = new Kh185[81];

                    int syuruiNumber = rowCount2 / 9 + 1;
                    if (15 <= syuruiNumber)
                    {
                        goto gt_EndMethod;
                    }
                    Data_HaiyakuTransition.map.Add(Ks14Array.Items_All[syuruiNumber], table81);
                }


                //----------
                // テーブル作り
                //----------

                int columnCount = 0;
                foreach (string column in row2)
                {
                    // 空っぽの列は無視します。
                    if ("" == column)
                    {
                        goto gt_NextColumn;
                    }

                    // 空っぽでない列の値を覚えます。

                    // 数値型のはずです。
                    int cellValue;
                    if (!int.TryParse(column, out cellValue))
                    {
                        string message = "エラー。\n path=[" + path + "]\n" +
                        "配役転換表に、int型数値でないものが指定されていました。\n" +
                        "rowCount=[" + rowCount2 + "]\n" +
                        "columnCount=[" + columnCount + "]\n";
                        Logger.ErrorLine(LogTags.ErrorLog, message);
                        throw new Exception(message);
                    }

                    int masuHandle = (8 - columnCount) * 9 + (rowCount2 % 9);//0～80

                    sbDebug.AppendLine("(" + rowCount2 + "," + columnCount + ")[" + masuHandle + "]" + cellValue);

                    table81[masuHandle] = Kh185Array.Items[cellValue];

                gt_NextColumn:
                    columnCount++;
                }

                rowCount2++;
            }

        gt_EndMethod:
            Logger.WriteFile(LogTags.HaichiTenkanHyoAllLog, sbDebug.ToString());

            return rows;
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
            sb.AppendLine("    <title>配役転換表</title>");
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

            foreach (KeyValuePair<Ks14, Kh185[]> entry1 in Data_HaiyakuTransition.Map)
            {
                sb.Append("<h1>");
                sb.Append(entry1.Key);
                sb.AppendLine("</h1>");


                sb.Append("<table>");
                // ９一～１一、９二～１二、…９九～１九の順だぜ☆
                for (int dan = 1; dan <= 9; dan++)
                {
                    sb.AppendLine("<tr>");

                    sb.Append("    ");
                    for (int suji = 9; suji >= 1; suji--)
                    {

                        M201 masu = M201Util.OkibaSujiDanToMasu( Okiba.ShogiBan, suji, dan);

                        sb.Append("<td>");


                        Kh185 kh184 = entry1.Value[(int)masu];
                        int haiyakuHandle = (int)kh184;


                        sb.Append("<img src=\"./img/train");


                        if (haiyakuHandle < 10)
                        {
                            sb.Append("00");
                        }
                        else if (haiyakuHandle < 100)
                        {
                            sb.Append("0");
                        }
                        sb.Append(haiyakuHandle);
                        sb.Append(".png\" />");



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
