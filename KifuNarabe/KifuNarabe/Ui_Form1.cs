using System;
using System.Reflection;
using System.Windows.Forms;
using Grayscale.KifuwaraneGui.L08_Server;

namespace Grayscale.KifuwaraneGui.L09_Ui
{
    [Serializable]
    public partial class Ui_Form1 : Form
    {

        /// <summary>
        /// コンストラクターです。
        /// </summary>
        public Ui_Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ウィンドウが表示される直前にしておく準備をここに書きます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_Form1_Load(object sender, EventArgs e)
        {
            //------------------------------
            // タイトルバーに表示する、「タイトル 1.00.0」といった文字を設定します。
            //------------------------------
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = String.Format("{0} {1}.{2}.{3}", this.Text, version.Major, version.Minor.ToString("00"), version.Build);
        }

        /// <summary>
        /// ウィンドウが閉じられる直前にしておくことを、ここに書きます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShogiEngineService.Shutdown();
        }

        private void Ui_Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ShogiEngineService.Shutdown();
        }
    }
}
