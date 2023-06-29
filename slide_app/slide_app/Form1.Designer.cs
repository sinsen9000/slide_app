
namespace slide_app
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OpenFileButton = new System.Windows.Forms.Button();
            this.OpenFileBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RecordButton = new System.Windows.Forms.Button();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.StateLabel = new System.Windows.Forms.Label();
            this.FileLabel = new System.Windows.Forms.Label();
            this.BackVOICEVOX = new System.ComponentModel.BackgroundWorker();
            this.CancelButton = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ResultGrid = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.VoiceTestButton = new System.Windows.Forms.Button();
            this.VoiceNameCombo = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.IntonationLabel = new System.Windows.Forms.Label();
            this.SpeedLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.IntonationBar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SpeedBar = new System.Windows.Forms.TrackBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ModeText = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ModeCombo = new System.Windows.Forms.ComboBox();
            this.BackVoiceTest = new System.ComponentModel.BackgroundWorker();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IntonationBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedBar)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenFileButton.Location = new System.Drawing.Point(1030, 33);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(122, 29);
            this.OpenFileButton.TabIndex = 0;
            this.OpenFileButton.Text = "ファイルを開く";
            this.OpenFileButton.UseVisualStyleBackColor = true;
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFileButton_Click);
            // 
            // OpenFileBox
            // 
            this.OpenFileBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenFileBox.Location = new System.Drawing.Point(84, 33);
            this.OpenFileBox.Name = "OpenFileBox";
            this.OpenFileBox.Size = new System.Drawing.Size(940, 27);
            this.OpenFileBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "ファイル名";
            // 
            // RecordButton
            // 
            this.RecordButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RecordButton.Location = new System.Drawing.Point(1030, 494);
            this.RecordButton.Name = "RecordButton";
            this.RecordButton.Size = new System.Drawing.Size(122, 57);
            this.RecordButton.TabIndex = 3;
            this.RecordButton.Text = "録画";
            this.RecordButton.UseVisualStyleBackColor = true;
            this.RecordButton.Click += new System.EventHandler(this.RecordButton_Click);
            // 
            // GenerateButton
            // 
            this.GenerateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GenerateButton.CausesValidation = false;
            this.GenerateButton.Location = new System.Drawing.Point(1030, 97);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(122, 57);
            this.GenerateButton.TabIndex = 4;
            this.GenerateButton.Text = "ノート抽出\r\nスライド画像生成";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar1.Location = new System.Drawing.Point(22, 495);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(476, 29);
            this.progressBar1.TabIndex = 6;
            // 
            // StateLabel
            // 
            this.StateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StateLabel.AutoSize = true;
            this.StateLabel.Location = new System.Drawing.Point(22, 537);
            this.StateLabel.Name = "StateLabel";
            this.StateLabel.Size = new System.Drawing.Size(39, 20);
            this.StateLabel.TabIndex = 8;
            this.StateLabel.Text = "完了";
            // 
            // FileLabel
            // 
            this.FileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FileLabel.AutoSize = true;
            this.FileLabel.Location = new System.Drawing.Point(124, 537);
            this.FileLabel.Name = "FileLabel";
            this.FileLabel.Size = new System.Drawing.Size(33, 20);
            this.FileLabel.TabIndex = 9;
            this.FileLabel.Text = "null";
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CancelButton.Location = new System.Drawing.Point(504, 494);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(80, 29);
            this.CancelButton.TabIndex = 10;
            this.CancelButton.Text = "キャンセル";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ResultGrid);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1004, 388);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "文章一覧";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ResultGrid
            // 
            this.ResultGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultGrid.Location = new System.Drawing.Point(32, 12);
            this.ResultGrid.Name = "ResultGrid";
            this.ResultGrid.RowHeadersWidth = 51;
            this.ResultGrid.RowTemplate.Height = 29;
            this.ResultGrid.Size = new System.Drawing.Size(940, 370);
            this.ResultGrid.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 68);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1012, 421);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1004, 388);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "各種設定";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.VoiceTestButton);
            this.panel1.Controls.Add(this.VoiceNameCombo);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.IntonationLabel);
            this.panel1.Controls.Add(this.SpeedLabel);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.IntonationBar);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.SpeedBar);
            this.panel1.Location = new System.Drawing.Point(6, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(476, 373);
            this.panel1.TabIndex = 2;
            // 
            // VoiceTestButton
            // 
            this.VoiceTestButton.Location = new System.Drawing.Point(361, 216);
            this.VoiceTestButton.Name = "VoiceTestButton";
            this.VoiceTestButton.Size = new System.Drawing.Size(94, 29);
            this.VoiceTestButton.TabIndex = 13;
            this.VoiceTestButton.Text = "音声テスト";
            this.VoiceTestButton.UseVisualStyleBackColor = true;
            this.VoiceTestButton.Click += new System.EventHandler(this.VoiceTestButton_Click);
            // 
            // VoiceNameCombo
            // 
            this.VoiceNameCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VoiceNameCombo.FormattingEnabled = true;
            this.VoiceNameCombo.Items.AddRange(new object[] {
            "四国めたん",
            "ずんだもん",
            "春日部つむぎ",
            "波音リツ",
            "雨晴はう",
            "玄野武宏",
            "白上虎太郎",
            "青山龍星",
            "冥鳴ひまり",
            "九州そら",
            "もち子さん",
            "剣崎雌雄",
            "WhiteCUL",
            "後鬼",
            "No.7"});
            this.VoiceNameCombo.Location = new System.Drawing.Point(208, 58);
            this.VoiceNameCombo.Name = "VoiceNameCombo";
            this.VoiceNameCombo.Size = new System.Drawing.Size(151, 28);
            this.VoiceNameCombo.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(30, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 23);
            this.label6.TabIndex = 11;
            this.label6.Text = "音源";
            // 
            // IntonationLabel
            // 
            this.IntonationLabel.AutoSize = true;
            this.IntonationLabel.Location = new System.Drawing.Point(34, 182);
            this.IntonationLabel.Name = "IntonationLabel";
            this.IntonationLabel.Size = new System.Drawing.Size(33, 20);
            this.IntonationLabel.TabIndex = 10;
            this.IntonationLabel.Text = "null";
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.AutoSize = true;
            this.SpeedLabel.Location = new System.Drawing.Point(34, 122);
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.Size = new System.Drawing.Size(33, 20);
            this.SpeedLabel.TabIndex = 8;
            this.SpeedLabel.Text = "null";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(30, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 23);
            this.label5.TabIndex = 7;
            this.label5.Text = "抑揚";
            // 
            // IntonationBar
            // 
            this.IntonationBar.Location = new System.Drawing.Point(117, 154);
            this.IntonationBar.Name = "IntonationBar";
            this.IntonationBar.Size = new System.Drawing.Size(338, 56);
            this.IntonationBar.TabIndex = 5;
            this.IntonationBar.Scroll += new System.EventHandler(this.IntonationBar_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(30, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "速度";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(3, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 28);
            this.label2.TabIndex = 2;
            this.label2.Text = "音声設定";
            // 
            // SpeedBar
            // 
            this.SpeedBar.Location = new System.Drawing.Point(117, 92);
            this.SpeedBar.Name = "SpeedBar";
            this.SpeedBar.Size = new System.Drawing.Size(338, 56);
            this.SpeedBar.TabIndex = 1;
            this.SpeedBar.Scroll += new System.EventHandler(this.SpeedBar_Scroll);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.ModeText);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.ModeCombo);
            this.panel2.Location = new System.Drawing.Point(488, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(510, 211);
            this.panel2.TabIndex = 14;
            // 
            // ModeText
            // 
            this.ModeText.Location = new System.Drawing.Point(3, 93);
            this.ModeText.Name = "ModeText";
            this.ModeText.ReadOnly = true;
            this.ModeText.Size = new System.Drawing.Size(502, 113);
            this.ModeText.TabIndex = 14;
            this.ModeText.Text = "動画のアクセシビリティの設定をします。";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(186, 28);
            this.label4.TabIndex = 13;
            this.label4.Text = "動画のアクセシビリティ";
            // 
            // ModeCombo
            // 
            this.ModeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModeCombo.FormattingEnabled = true;
            this.ModeCombo.Items.AddRange(new object[] {
            "normal",
            "visual"});
            this.ModeCombo.Location = new System.Drawing.Point(176, 59);
            this.ModeCombo.Name = "ModeCombo";
            this.ModeCombo.Size = new System.Drawing.Size(151, 28);
            this.ModeCombo.TabIndex = 13;
            this.ModeCombo.SelectedIndexChanged += new System.EventHandler(this.ModeCombo_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 566);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.RecordButton);
            this.Controls.Add(this.FileLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.StateLabel);
            this.Controls.Add(this.OpenFileBox);
            this.Controls.Add(this.OpenFileButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IntonationBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedBar)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.TextBox OpenFileBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button RecordButton;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.Label FileLabel;
        private System.ComponentModel.BackgroundWorker BackVOICEVOX;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView ResultGrid;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox VoiceNameCombo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label IntonationLabel;
        private System.Windows.Forms.Label SpeedLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar IntonationBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar SpeedBar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox ModeText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ModeCombo;
        private System.Windows.Forms.Button VoiceTestButton;
        private System.ComponentModel.BackgroundWorker BackVoiceTest;
    }
}

