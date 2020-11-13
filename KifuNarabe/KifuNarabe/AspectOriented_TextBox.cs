using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace Xenon.KifuNarabe.L09_Ui
{
    public abstract class AspectOriented_TextBox
    {

        /// <summary>
        /// 全選択　[Ctrl]+[A]
        /// </summary>
        public static void KeyDown_SelectAll(object sender, KeyEventArgs e)
        {
            //------------------------------
            // [Ctrl]+[A] で、全選択します。
            //------------------------------
            if (e.KeyCode == System.Windows.Forms.Keys.A & e.Control == true)
            {
                ((TextBox)sender).SelectAll();
            }
        }

    }
}
