namespace ImageScoreApp
{
    partial class ImageSelectScreen
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.Movie_button = new System.Windows.Forms.Button();
            this.CSV_button = new System.Windows.Forms.Button();
            this.Analyze_Start_button = new System.Windows.Forms.Button();
            this.Cancel_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Start_maskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.End_maskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.MovTextBox = new System.Windows.Forms.TextBox();
            this.ExpTextBox = new System.Windows.Forms.TextBox();
            this.AnaInterval_TextBox = new System.Windows.Forms.TextBox();
            this.AnaInterval_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Movie_button
            // 
            this.Movie_button.Location = new System.Drawing.Point(242, 36);
            this.Movie_button.Name = "Movie_button";
            this.Movie_button.Size = new System.Drawing.Size(80, 30);
            this.Movie_button.TabIndex = 0;
            this.Movie_button.Text = "選択";
            this.Movie_button.UseVisualStyleBackColor = true;
            this.Movie_button.Click += new System.EventHandler(this.button1_Click);
            // 
            // CSV_button
            // 
            this.CSV_button.Location = new System.Drawing.Point(242, 109);
            this.CSV_button.Name = "CSV_button";
            this.CSV_button.Size = new System.Drawing.Size(80, 30);
            this.CSV_button.TabIndex = 3;
            this.CSV_button.Text = "選択";
            this.CSV_button.UseVisualStyleBackColor = true;
            this.CSV_button.Click += new System.EventHandler(this.CSV_button_Click);
            // 
            // Analyze_Start_button
            // 
            this.Analyze_Start_button.Location = new System.Drawing.Point(108, 373);
            this.Analyze_Start_button.Name = "Analyze_Start_button";
            this.Analyze_Start_button.Size = new System.Drawing.Size(100, 25);
            this.Analyze_Start_button.TabIndex = 6;
            this.Analyze_Start_button.Text = "解析開始";
            this.Analyze_Start_button.UseVisualStyleBackColor = true;
            this.Analyze_Start_button.Click += new System.EventHandler(this.Analyze_Start_button_Click);
            // 
            // Cancel_button
            // 
            this.Cancel_button.Location = new System.Drawing.Point(222, 373);
            this.Cancel_button.Name = "Cancel_button";
            this.Cancel_button.Size = new System.Drawing.Size(100, 25);
            this.Cancel_button.TabIndex = 7;
            this.Cancel_button.Text = "キャンセル";
            this.Cancel_button.UseVisualStyleBackColor = true;
            this.Cancel_button.Click += new System.EventHandler(this.Cancel_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "動画ファイルを選択してください。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "解析開始時刻";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(129, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "解析終了時刻";
            // 
            // Start_maskedTextBox
            // 
            this.Start_maskedTextBox.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.Start_maskedTextBox.Location = new System.Drawing.Point(14, 200);
            this.Start_maskedTextBox.Mask = "00:00:00";
            this.Start_maskedTextBox.Name = "Start_maskedTextBox";
            this.Start_maskedTextBox.Size = new System.Drawing.Size(77, 19);
            this.Start_maskedTextBox.TabIndex = 11;
            this.Start_maskedTextBox.TextChanged += new System.EventHandler(this.Start_maskedTextBox_TextChanged);
            // 
            // End_maskedTextBox
            // 
            this.End_maskedTextBox.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.End_maskedTextBox.Location = new System.Drawing.Point(131, 200);
            this.End_maskedTextBox.Mask = "00:00:00";
            this.End_maskedTextBox.Name = "End_maskedTextBox";
            this.End_maskedTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.End_maskedTextBox.Size = new System.Drawing.Size(77, 19);
            this.End_maskedTextBox.TabIndex = 12;
            this.End_maskedTextBox.TextChanged += new System.EventHandler(this.End_maskedTextBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(101, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "～";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(231, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "出力するCSVファイルの場所を選択してください。";
            // 
            // MovTextBox
            // 
            this.MovTextBox.Location = new System.Drawing.Point(12, 42);
            this.MovTextBox.Name = "MovTextBox";
            this.MovTextBox.Size = new System.Drawing.Size(220, 19);
            this.MovTextBox.TabIndex = 15;
            this.MovTextBox.TextChanged += new System.EventHandler(this.MovTextBox_TextChanged);
            // 
            // ExpTextBox
            // 
            this.ExpTextBox.Location = new System.Drawing.Point(12, 115);
            this.ExpTextBox.Name = "ExpTextBox";
            this.ExpTextBox.Size = new System.Drawing.Size(220, 19);
            this.ExpTextBox.TabIndex = 16;
            this.ExpTextBox.TextChanged += new System.EventHandler(this.ExpTextBox_TextChanged);
            // 
            // AnaInterval_TextBox
            // 
            this.AnaInterval_TextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.AnaInterval_TextBox.Location = new System.Drawing.Point(15, 257);
            this.AnaInterval_TextBox.MaxLength = 2;
            this.AnaInterval_TextBox.Name = "AnaInterval_TextBox";
            this.AnaInterval_TextBox.Size = new System.Drawing.Size(30, 19);
            this.AnaInterval_TextBox.TabIndex = 17;
            this.AnaInterval_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AnaInterval_TextBox.TextChanged += new System.EventHandler(this.AnaInterval_TextBox_TextChanged);
            // 
            // AnaInterval_label
            // 
            this.AnaInterval_label.AutoSize = true;
            this.AnaInterval_label.Location = new System.Drawing.Point(14, 240);
            this.AnaInterval_label.Name = "AnaInterval_label";
            this.AnaInterval_label.Size = new System.Drawing.Size(79, 12);
            this.AnaInterval_label.TabIndex = 18;
            this.AnaInterval_label.Text = "解析間隔(sec)";
            // 
            // ImageSelectScreen
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(334, 412);
            this.Controls.Add(this.AnaInterval_label);
            this.Controls.Add(this.AnaInterval_TextBox);
            this.Controls.Add(this.ExpTextBox);
            this.Controls.Add(this.MovTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.End_maskedTextBox);
            this.Controls.Add(this.Start_maskedTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancel_button);
            this.Controls.Add(this.Analyze_Start_button);
            this.Controls.Add(this.CSV_button);
            this.Controls.Add(this.Movie_button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ImageSelectScreen";
            this.Text = "設定画面";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Movie_button;
        private System.Windows.Forms.Button CSV_button;
        private System.Windows.Forms.Button Analyze_Start_button;
        private System.Windows.Forms.Button Cancel_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox Start_maskedTextBox;
        private System.Windows.Forms.MaskedTextBox End_maskedTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox MovTextBox;
        private System.Windows.Forms.TextBox ExpTextBox;
        private System.Windows.Forms.TextBox AnaInterval_TextBox;
        private System.Windows.Forms.Label AnaInterval_label;
    }
}

