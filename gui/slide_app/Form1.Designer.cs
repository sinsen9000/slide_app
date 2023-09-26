
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
            OpenFileButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            GenerateButton = new System.Windows.Forms.Button();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            StateLabel = new System.Windows.Forms.Label();
            FileLabel = new System.Windows.Forms.Label();
            BackVOICEVOX = new System.ComponentModel.BackgroundWorker();
            CancelButton = new System.Windows.Forms.Button();
            tabPage1 = new System.Windows.Forms.TabPage();
            ResultGrid = new System.Windows.Forms.DataGridView();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage2 = new System.Windows.Forms.TabPage();
            panel1 = new System.Windows.Forms.Panel();
            VoiceTestButton = new System.Windows.Forms.Button();
            VoiceNameCombo = new System.Windows.Forms.ComboBox();
            label6 = new System.Windows.Forms.Label();
            IntonationLabel = new System.Windows.Forms.Label();
            SpeedLabel = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            IntonationBar = new System.Windows.Forms.TrackBar();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            SpeedBar = new System.Windows.Forms.TrackBar();
            panel2 = new System.Windows.Forms.Panel();
            ModeText = new System.Windows.Forms.RichTextBox();
            label4 = new System.Windows.Forms.Label();
            ModeCombo = new System.Windows.Forms.ComboBox();
            BackVoiceTest = new System.ComponentModel.BackgroundWorker();
            OpenFileBox = new System.Windows.Forms.ComboBox();
            RecordButton = new System.Windows.Forms.Button();
            AvatorButton = new System.Windows.Forms.Button();
            SaveButton = new System.Windows.Forms.Button();
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ResultGrid).BeginInit();
            tabControl1.SuspendLayout();
            tabPage2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)IntonationBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SpeedBar).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // OpenFileButton
            // 
            OpenFileButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            OpenFileButton.Location = new System.Drawing.Point(1030, 33);
            OpenFileButton.Name = "OpenFileButton";
            OpenFileButton.Size = new System.Drawing.Size(122, 29);
            OpenFileButton.TabIndex = 0;
            OpenFileButton.Text = "ファイルを開く";
            OpenFileButton.UseVisualStyleBackColor = true;
            OpenFileButton.Click += OpenFileButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 36);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(66, 20);
            label1.TabIndex = 2;
            label1.Text = "ファイル名";
            // 
            // GenerateButton
            // 
            GenerateButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            GenerateButton.CausesValidation = false;
            GenerateButton.Location = new System.Drawing.Point(1030, 97);
            GenerateButton.Name = "GenerateButton";
            GenerateButton.Size = new System.Drawing.Size(122, 57);
            GenerateButton.TabIndex = 4;
            GenerateButton.Text = "ノート抽出\r\nスライド画像生成";
            GenerateButton.UseVisualStyleBackColor = true;
            GenerateButton.Click += GenerateButton_Click;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            progressBar1.Location = new System.Drawing.Point(22, 495);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(476, 29);
            progressBar1.TabIndex = 6;
            // 
            // StateLabel
            // 
            StateLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            StateLabel.AutoSize = true;
            StateLabel.Location = new System.Drawing.Point(22, 537);
            StateLabel.Name = "StateLabel";
            StateLabel.Size = new System.Drawing.Size(39, 20);
            StateLabel.TabIndex = 8;
            StateLabel.Text = "完了";
            // 
            // FileLabel
            // 
            FileLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            FileLabel.AutoSize = true;
            FileLabel.Location = new System.Drawing.Point(124, 537);
            FileLabel.Name = "FileLabel";
            FileLabel.Size = new System.Drawing.Size(33, 20);
            FileLabel.TabIndex = 9;
            FileLabel.Text = "null";
            // 
            // CancelButton
            // 
            CancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            CancelButton.Location = new System.Drawing.Point(504, 494);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new System.Drawing.Size(80, 29);
            CancelButton.TabIndex = 10;
            CancelButton.Text = "キャンセル";
            CancelButton.UseVisualStyleBackColor = true;
            CancelButton.Click += CancelButton_Click;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(ResultGrid);
            tabPage1.Location = new System.Drawing.Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(1004, 388);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "文章一覧";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // ResultGrid
            // 
            ResultGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ResultGrid.Location = new System.Drawing.Point(32, 12);
            ResultGrid.Name = "ResultGrid";
            ResultGrid.RowHeadersWidth = 51;
            ResultGrid.RowTemplate.Height = 29;
            ResultGrid.Size = new System.Drawing.Size(940, 370);
            ResultGrid.TabIndex = 5;
            ResultGrid.CellBeginEdit += ResultGrid_CellBeginEdit;
            ResultGrid.CellEndEdit += ResultGrid_CellEndEdit2;
            ResultGrid.CurrentCellChanged += ResultGrid_CurrentCellChanged;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Location = new System.Drawing.Point(12, 68);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(1012, 421);
            tabControl1.TabIndex = 11;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(panel1);
            tabPage2.Controls.Add(panel2);
            tabPage2.Location = new System.Drawing.Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(1004, 388);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "各種設定";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add(VoiceTestButton);
            panel1.Controls.Add(VoiceNameCombo);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(IntonationLabel);
            panel1.Controls.Add(SpeedLabel);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(IntonationBar);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(SpeedBar);
            panel1.Location = new System.Drawing.Point(6, 9);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(476, 373);
            panel1.TabIndex = 2;
            // 
            // VoiceTestButton
            // 
            VoiceTestButton.Location = new System.Drawing.Point(361, 216);
            VoiceTestButton.Name = "VoiceTestButton";
            VoiceTestButton.Size = new System.Drawing.Size(94, 29);
            VoiceTestButton.TabIndex = 13;
            VoiceTestButton.Text = "音声テスト";
            VoiceTestButton.UseVisualStyleBackColor = true;
            VoiceTestButton.Click += VoiceTestButton_Click;
            // 
            // VoiceNameCombo
            // 
            VoiceNameCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            VoiceNameCombo.FormattingEnabled = true;
            VoiceNameCombo.Items.AddRange(new object[] { "四国めたん", "ずんだもん", "春日部つむぎ", "波音リツ", "雨晴はう", "玄野武宏", "白上虎太郎", "青山龍星", "冥鳴ひまり", "九州そら", "もち子さん", "剣崎雌雄", "WhiteCUL", "後鬼", "No.7" });
            VoiceNameCombo.Location = new System.Drawing.Point(208, 58);
            VoiceNameCombo.Name = "VoiceNameCombo";
            VoiceNameCombo.Size = new System.Drawing.Size(151, 28);
            VoiceNameCombo.TabIndex = 12;
            VoiceNameCombo.SelectedIndexChanged += VoiceNameCombo_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label6.Location = new System.Drawing.Point(30, 59);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(44, 23);
            label6.TabIndex = 11;
            label6.Text = "音源";
            // 
            // IntonationLabel
            // 
            IntonationLabel.AutoSize = true;
            IntonationLabel.Location = new System.Drawing.Point(34, 182);
            IntonationLabel.Name = "IntonationLabel";
            IntonationLabel.Size = new System.Drawing.Size(33, 20);
            IntonationLabel.TabIndex = 10;
            IntonationLabel.Text = "null";
            // 
            // SpeedLabel
            // 
            SpeedLabel.AutoSize = true;
            SpeedLabel.Location = new System.Drawing.Point(34, 122);
            SpeedLabel.Name = "SpeedLabel";
            SpeedLabel.Size = new System.Drawing.Size(33, 20);
            SpeedLabel.TabIndex = 8;
            SpeedLabel.Text = "null";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label5.Location = new System.Drawing.Point(30, 154);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(44, 23);
            label5.TabIndex = 7;
            label5.Text = "抑揚";
            // 
            // IntonationBar
            // 
            IntonationBar.Location = new System.Drawing.Point(117, 154);
            IntonationBar.Name = "IntonationBar";
            IntonationBar.Size = new System.Drawing.Size(338, 56);
            IntonationBar.TabIndex = 5;
            IntonationBar.Scroll += IntonationBar_Scroll;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label3.Location = new System.Drawing.Point(30, 92);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(44, 23);
            label3.TabIndex = 3;
            label3.Text = "速度";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(3, 2);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(92, 28);
            label2.TabIndex = 2;
            label2.Text = "音声設定";
            // 
            // SpeedBar
            // 
            SpeedBar.Location = new System.Drawing.Point(117, 92);
            SpeedBar.Name = "SpeedBar";
            SpeedBar.Size = new System.Drawing.Size(338, 56);
            SpeedBar.TabIndex = 1;
            SpeedBar.Scroll += SpeedBar_Scroll;
            // 
            // panel2
            // 
            panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel2.Controls.Add(ModeText);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(ModeCombo);
            panel2.Location = new System.Drawing.Point(488, 9);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(510, 211);
            panel2.TabIndex = 14;
            // 
            // ModeText
            // 
            ModeText.Location = new System.Drawing.Point(3, 93);
            ModeText.Name = "ModeText";
            ModeText.ReadOnly = true;
            ModeText.Size = new System.Drawing.Size(502, 113);
            ModeText.TabIndex = 14;
            ModeText.Text = "動画のアクセシビリティの設定をします。";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label4.Location = new System.Drawing.Point(3, 3);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(186, 28);
            label4.TabIndex = 13;
            label4.Text = "動画のアクセシビリティ";
            // 
            // ModeCombo
            // 
            ModeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ModeCombo.FormattingEnabled = true;
            ModeCombo.Items.AddRange(new object[] { "visual", "visual+", "none" });
            ModeCombo.Location = new System.Drawing.Point(176, 59);
            ModeCombo.Name = "ModeCombo";
            ModeCombo.Size = new System.Drawing.Size(151, 28);
            ModeCombo.TabIndex = 13;
            ModeCombo.SelectedIndexChanged += ModeCombo_SelectedIndexChanged;
            // 
            // OpenFileBox
            // 
            OpenFileBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            OpenFileBox.FormattingEnabled = true;
            OpenFileBox.Location = new System.Drawing.Point(84, 33);
            OpenFileBox.Name = "OpenFileBox";
            OpenFileBox.Size = new System.Drawing.Size(936, 28);
            OpenFileBox.TabIndex = 12;
            OpenFileBox.SelectedIndexChanged += OpenFileBox_SelectedIndexChanged;
            // 
            // RecordButton
            // 
            RecordButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            RecordButton.CausesValidation = false;
            RecordButton.Location = new System.Drawing.Point(1030, 497);
            RecordButton.Name = "RecordButton";
            RecordButton.Size = new System.Drawing.Size(122, 57);
            RecordButton.TabIndex = 13;
            RecordButton.Text = "動画確認";
            RecordButton.UseVisualStyleBackColor = true;
            RecordButton.Click += RecordButton_Click_1;
            // 
            // AvatorButton
            // 
            AvatorButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            AvatorButton.CausesValidation = false;
            AvatorButton.Location = new System.Drawing.Point(1030, 428);
            AvatorButton.Name = "AvatorButton";
            AvatorButton.Size = new System.Drawing.Size(122, 57);
            AvatorButton.TabIndex = 14;
            AvatorButton.Text = "アバター起動";
            AvatorButton.UseVisualStyleBackColor = true;
            AvatorButton.Click += AvatorButton_Click;
            // 
            // SaveButton
            // 
            SaveButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            SaveButton.CausesValidation = false;
            SaveButton.Location = new System.Drawing.Point(1030, 166);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new System.Drawing.Size(122, 57);
            SaveButton.TabIndex = 15;
            SaveButton.Text = "上書き保存\r\n（文章一覧）";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1164, 566);
            Controls.Add(SaveButton);
            Controls.Add(AvatorButton);
            Controls.Add(RecordButton);
            Controls.Add(OpenFileBox);
            Controls.Add(tabControl1);
            Controls.Add(CancelButton);
            Controls.Add(GenerateButton);
            Controls.Add(FileLabel);
            Controls.Add(label1);
            Controls.Add(progressBar1);
            Controls.Add(StateLabel);
            Controls.Add(OpenFileButton);
            Name = "Form1";
            Text = "Form1";
            TopMost = true;
            Load += Form1_Load;
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ResultGrid).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)IntonationBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)SpeedBar).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void ResultGrid_CellEndEdit(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.Label label1;
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
        private System.Windows.Forms.ComboBox OpenFileBox;
        private System.Windows.Forms.Button RecordButton;
        private System.Windows.Forms.Button AvatorButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

