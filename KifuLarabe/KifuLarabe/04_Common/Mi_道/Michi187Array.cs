using System.Collections.Generic;
using System.IO;
using Grayscale.KifuwaraneLib.L01_Log;

namespace Grayscale.KifuwaraneLib.L04_Common
{
    /// <summary>
    /// 「1,上一,１九,１八,１七,１六,１五,１四,１三,１二,１一」を、
    /// 「[1]={0,1,2,3,4,5,6,7,8}」に変換して持ちます。
    /// </summary>
    public abstract class Michi187Array
    {
                       public static List<IMasus> Items
        {
            get
            {
                return Michi187Array.items;
            }
        }
        private static List<IMasus> items;

        static Michi187Array()
        {

            //----------
            // 筋１８７
            //----------
            Michi187Array.items = new List<IMasus>();
        }

        private static List<List<string>> ReadCsv(string path)
        {
            List<List<string>> rows = new List<List<string>>();

            foreach (string line in File.ReadAllLines(path))
            {
                rows.Add(CsvLineParserImpl.UnescapeLineToFieldList(line, ','));
            }

            return rows;
        }


        public static List<List<string>> Load(string path)
        {
            List<List<string>> rows = Michi187Array.ReadCsv(path);



            // 最初の１行は削除。
            rows.RemoveRange(0, 1);

            // 各行の先頭２列は削除。
            foreach (List<string> row in rows)
            {
                row.RemoveRange(0, 2);
            }



            Michi187Array.Items.Clear();

            foreach (List<string> row in rows)
            {
                IMasus michi187 = new Masus_Ordered();

                foreach (string field in row)
                {
                    // 「１一」を「1」に変換します。
                    M201 masu81 = M201Util.kanjiToEnum[field];
                    michi187.AddElement(masu81);
                }

                Michi187Array.Items.Add(michi187);
            }

            return rows;
        }


    }
}
