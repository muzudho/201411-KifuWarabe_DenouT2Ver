namespace Grayscale.Kifuwarane.Gui.L09_Ui
{
    partial class Ui_PnlMain
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtOutput1 = new System.Windows.Forms.TextBox();
            this.txtInput1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gameEngineTimer1 = new System.Windows.Forms.Timer(this.components);
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtOutput1
            // 
            this.txtOutput1.Location = new System.Drawing.Point(64, 728);
            this.txtOutput1.Multiline = true;
            this.txtOutput1.Name = "txtOutput1";
            this.txtOutput1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput1.Size = new System.Drawing.Size(714, 30);
            this.txtOutput1.TabIndex = 0;
            this.txtOutput1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtOutput1_KeyDown);
            // 
            // txtInput1
            // 
            this.txtInput1.Location = new System.Drawing.Point(21, 3);
            this.txtInput1.Multiline = true;
            this.txtInput1.Name = "txtInput1";
            this.txtInput1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInput1.Size = new System.Drawing.Size(688, 30);
            this.txtInput1.TabIndex = 2;
            this.txtInput1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput1_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "入\r\n力";
            // 
            // gameEngineTimer1
            // 
            this.gameEngineTimer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txtHistory
            // 
            this.txtHistory.Location = new System.Drawing.Point(1001, 45);
            this.txtHistory.Multiline = true;
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHistory.Size = new System.Drawing.Size(70, 713);
            this.txtHistory.TabIndex = 4;
            // 
            // Ui_PnlMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.txtHistory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtInput1);
            this.Controls.Add(this.txtOutput1);
            this.DoubleBuffered = true;
            this.Name = "Ui_PnlMain";
            this.Size = new System.Drawing.Size(1074, 761);
            this.Load += new System.EventHandler(this.Ui_PnlMain_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Ui_PnlMain_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Ui_PnlMain_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Ui_PnlMain_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Ui_PnlMain_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOutput1;
        private System.Windows.Forms.TextBox txtInput1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer gameEngineTimer1;
        private System.Windows.Forms.TextBox txtHistory;
    }
}
