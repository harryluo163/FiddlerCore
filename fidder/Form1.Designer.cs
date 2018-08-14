namespace fidder
{
    partial class 船货不二抓取系统
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnstart = new System.Windows.Forms.Button();
            this.btnout = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtlog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnstart
            // 
            this.btnstart.Location = new System.Drawing.Point(546, 58);
            this.btnstart.Margin = new System.Windows.Forms.Padding(2);
            this.btnstart.Name = "btnstart";
            this.btnstart.Size = new System.Drawing.Size(56, 18);
            this.btnstart.TabIndex = 0;
            this.btnstart.Text = "开始";
            this.btnstart.UseVisualStyleBackColor = true;
            this.btnstart.Click += new System.EventHandler(this.btnstart_Click);
            // 
            // btnout
            // 
            this.btnout.Location = new System.Drawing.Point(607, 58);
            this.btnout.Margin = new System.Windows.Forms.Padding(2);
            this.btnout.Name = "btnout";
            this.btnout.Size = new System.Drawing.Size(56, 18);
            this.btnout.TabIndex = 1;
            this.btnout.Text = "退出";
            this.btnout.UseVisualStyleBackColor = true;
            this.btnout.Click += new System.EventHandler(this.btnout_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-12, -11);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 41;
            this.label5.Text = "日志记录：";
            // 
            // txtlog
            // 
            this.txtlog.Location = new System.Drawing.Point(21, 88);
            this.txtlog.Margin = new System.Windows.Forms.Padding(2);
            this.txtlog.Multiline = true;
            this.txtlog.Name = "txtlog";
            this.txtlog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtlog.Size = new System.Drawing.Size(680, 328);
            this.txtlog.TabIndex = 42;
            // 
            // 船货不二抓取系统
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 498);
            this.Controls.Add(this.txtlog);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnout);
            this.Controls.Add(this.btnstart);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "船货不二抓取系统";
            this.Text = "船货不二抓取系统";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnstart;
        private System.Windows.Forms.Button btnout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtlog;
    }
}

