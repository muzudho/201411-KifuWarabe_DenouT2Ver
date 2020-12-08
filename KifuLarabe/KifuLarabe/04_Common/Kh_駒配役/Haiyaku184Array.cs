using System.Collections.Generic;
using System.IO;
using System.Text;
using Grayscale.KifuwaraneEntities.Misc;

namespace Grayscale.KifuwaraneEntities.L04_Common
{
    /// <summary>
    /// 駒配役１８４だぜ☆
    /// 
    /// 「1,歩,1,,,,,,,,,,,,,,,,,,,,,,」といった内容を、
    /// [1]「空間1」に置き換えるぜ☆
    /// 
    /// 駒の情報はそぎ落とすぜ☆　筋の情報だけが残る☆
    /// </summary>
    public abstract class Haiyaku184Array
    {
        /// <summary>
        /// 配役名。
        /// </summary>
        public static List<string> Name { get { return Haiyaku184Array.name; } }
        private static List<string> name;

        /// <summary>
        /// 種類。
        /// </summary>
        public static Ks14 Syurui(Kh185 haiyaku)
        {
            return Haiyaku184Array.syurui[(int)haiyaku];
        }
        public static void AddSyurui(Ks14 syurui)
        {
            Haiyaku184Array.syurui.Add(syurui);
        }
        private static List<Ks14> syurui;


        /// <summary>
        /// 空間フィールド。（１～２４個）
        /// </summary>
        public static Dictionary<Kh185, List<IMasus>> KukanMasus { get { return Haiyaku184Array.kukanMasus; } }
        private static Dictionary<Kh185, List<IMasus>> kukanMasus;


        static Haiyaku184Array()
        {
            Haiyaku184Array.kukanMasus = new Dictionary<Kh185, List<IMasus>>();
            Haiyaku184Array.syurui = new List<Ks14>();
            Haiyaku184Array.name = new List<string>();
        }



        /// <summary>
        /// TODO: 駒の配置と、配役ハンドルをもとに、升を取得できたい。
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<List<string>> Load(string path, Encoding encoding)
        {
            List<List<string>> rows = new List<List<string>>();

            foreach (string line in File.ReadAllLines(path, encoding))
            {
                rows.Add(CsvLineParserImpl.UnescapeLineToFieldList(line, ','));
            }


            // 最初の６行は削除。
            rows.RemoveRange(0, 6);

            // 各行の先頭１列は削除。
            foreach (List<string> row in rows)
            {
                row.RemoveRange(0, 1);
            }



            int haiyaku = 0;
            foreach (List<string> row in rows)
            {
                int fieldNumber = 0;
                foreach (string field in row)
                {
                    switch (fieldNumber)//フィールドある限り空間。
                    {
                        case 0://名前
                            {
                                Haiyaku184Array.Name.Add(field);
                                Haiyaku184Array.KukanMasus.Add( Kh185Array.Items[haiyaku], new List<IMasus>());
                            }
                            break;

                        case 1://駒種類備考
                            break;

                        case 2://駒種類番号
                            {
                                int syuruiHandle;
                                if (int.TryParse(field, out syuruiHandle))
                                {
                                    Haiyaku184Array.AddSyurui( Ks14Array.Items_All[syuruiHandle]);
                                }
                            }
                            break;

                        case 3: // 元の世界 上
                            {
                                for (int iMichi = 1; iMichi <= 9; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 4: // 元の世界 引
                            {
                                for (int iMichi = 10; iMichi <= 18; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 5: // 元の世界 滑
                            {
                                for (int iMichi = 19; iMichi <= 27; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 6: // 元の世界 射
                            {
                                for (int iMichi = 28; iMichi <= 36; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 7: // 奇角交差    昇
                            {
                                for (int iMichi = 37; iMichi <= 43; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 8: // 奇角交差    降
                            {
                                for (int iMichi = 44; iMichi <= 50; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 9: // 奇角交差    沈
                            {
                                for (int iMichi = 51; iMichi <= 57; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 10:    // 奇角交差 浮
                            {
                                for (int iMichi = 58; iMichi <= 64; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 11:    // 偶角交差 昇
                            {
                                for (int iMichi = 65; iMichi <= 72; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 12:    // 偶角交差 降
                            {
                                for (int iMichi = 73; iMichi <= 80; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 13:    // 偶角交差 沈
                            {                                
                                for (int iMichi = 81; iMichi <= 88; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 14:    // 偶角交差 浮
                            {
                                
                                for (int iMichi = 89; iMichi <= 96; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 15:    // 金桂交差 駆
                            {
                                
                                for (int iMichi = 97; iMichi <= 102; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                            // (欠番)金桂交差 退

                            // (欠番)金桂交差 踏


                        case 16:    // 金桂交差 跳
                            {                                
                                for (int iMichi = 115; iMichi <= 120; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 17: // 銀桂交差 駆
                            {                                
                                for (int iMichi = 121; iMichi <= 125; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                            // (欠番)銀桂交差 退

                            // (欠番)銀桂交差 踏

                        case 18:    // 銀桂交差 跳
                            {                                
                                for (int iMichi = 136; iMichi <= 140; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 19:    // 擦金桂交差    駆
                            {                                
                                for (int iMichi = 141; iMichi <= 145; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                            // (欠番)擦金桂交差    退

                            // (欠番)擦金桂交差    踏

                        case 20:    // 擦金桂交差    跳
                            {
                                
                                for (int iMichi = 156; iMichi <= 160; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 21:    // 擦銀桂交差    駆
                            {                                
                                for (int iMichi = 161; iMichi <= 165; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                            // (欠番)擦銀桂交差    退

                            // (欠番)擦銀桂交差    踏

                        case 22:    // 擦銀桂交差    跳
                            {                                
                                for (int iMichi = 176; iMichi <= 180; iMichi++)
                                {
                                    Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[iMichi]);
                                }
                                break;
                            }

                        case 23:    // 歩打面  上
                            {
                                Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[181]);
                                Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[182]);
                                break;
                            }

                            // (欠番)歩打面  引

                        case 24:    // 香打面  上
                            {
                                Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[183]);
                                Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[184]);
                                break;
                            }

                            // (欠番)香打面  引

                        case 25:    // 桂打面  上
                            {
                                Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[185]);
                                Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[186]);
                                break;
                            }

                            // (欠番)桂打面  引

                        case 26:    // 全打面
                            {
                                Haiyaku184Array.KukanMasus[Kh185Array.Items[haiyaku]].Add(Michi187Array.Items[187]);
                                break;
                            }
                    }

                    fieldNumber++;
                }


                haiyaku++;
            }

            return rows;
        }



    }
}
