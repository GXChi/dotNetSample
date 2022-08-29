namespace Sample.WinForm
{
    partial class FormSample
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
            this.Install = new System.Windows.Forms.Button();
            this.Start = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.Unstall = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Install
            // 
            this.Install.Location = new System.Drawing.Point(34, 33);
            this.Install.Name = "Install";
            this.Install.Size = new System.Drawing.Size(92, 39);
            this.Install.TabIndex = 0;
            this.Install.Text = "安装服务";
            this.Install.UseVisualStyleBackColor = true;
            this.Install.Click += new System.EventHandler(this.Install_Click);
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(180, 31);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(101, 41);
            this.Start.TabIndex = 0;
            this.Start.Text = "启动服务";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // Stop
            // 
            this.Stop.Location = new System.Drawing.Point(329, 33);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(108, 39);
            this.Stop.TabIndex = 0;
            this.Stop.Text = "停止服务";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // Unstall
            // 
            this.Unstall.Location = new System.Drawing.Point(490, 35);
            this.Unstall.Name = "Unstall";
            this.Unstall.Size = new System.Drawing.Size(114, 37);
            this.Unstall.TabIndex = 0;
            this.Unstall.Text = "卸载服务";
            this.Unstall.UseVisualStyleBackColor = true;
            this.Unstall.Click += new System.EventHandler(this.Unstall_Click);
            // 
            // FormSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 330);
            this.Controls.Add(this.Unstall);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.Install);
            this.Name = "FormSample";
            this.Text = "服务安装卸载";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Install;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button Unstall;
    }
}

