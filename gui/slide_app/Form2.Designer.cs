namespace slide_app
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            CharaCheck = new System.Windows.Forms.CheckBox();
            label9 = new System.Windows.Forms.Label();
            CaptionCheck = new System.Windows.Forms.CheckBox();
            AudioVoiceCheck = new System.Windows.Forms.CheckBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            FontTextBox = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            HosokuCheck = new System.Windows.Forms.CheckBox();
            label4 = new System.Windows.Forms.Label();
            OkButton = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.Controls.Add(CharaCheck, 0, 5);
            tableLayoutPanel1.Controls.Add(label9, 0, 4);
            tableLayoutPanel1.Controls.Add(CaptionCheck, 0, 3);
            tableLayoutPanel1.Controls.Add(AudioVoiceCheck, 0, 1);
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 2);
            tableLayoutPanel1.Controls.Add(FontTextBox, 2, 3);
            tableLayoutPanel1.Controls.Add(label3, 1, 3);
            tableLayoutPanel1.Controls.Add(HosokuCheck, 2, 1);
            tableLayoutPanel1.Controls.Add(label4, 1, 1);
            tableLayoutPanel1.Controls.Add(OkButton, 2, 5);
            tableLayoutPanel1.Location = new System.Drawing.Point(15, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.Size = new System.Drawing.Size(473, 212);
            tableLayoutPanel1.TabIndex = 25;
            // 
            // CharaCheck
            // 
            CharaCheck.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CharaCheck.AutoSize = true;
            CharaCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            CharaCheck.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            CharaCheck.Location = new System.Drawing.Point(3, 172);
            CharaCheck.Name = "CharaCheck";
            CharaCheck.Size = new System.Drawing.Size(166, 27);
            CharaCheck.TabIndex = 31;
            CharaCheck.Text = "動作有無";
            CharaCheck.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label9.AutoSize = true;
            label9.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label9.Location = new System.Drawing.Point(3, 146);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(166, 23);
            label9.TabIndex = 20;
            label9.Text = "＜キャラクターの動作＞";
            label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CaptionCheck
            // 
            CaptionCheck.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CaptionCheck.AutoSize = true;
            CaptionCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            CaptionCheck.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            CaptionCheck.Location = new System.Drawing.Point(3, 99);
            CaptionCheck.Name = "CaptionCheck";
            CaptionCheck.Size = new System.Drawing.Size(166, 27);
            CaptionCheck.TabIndex = 23;
            CaptionCheck.Text = "動画字幕";
            CaptionCheck.UseVisualStyleBackColor = true;
            CaptionCheck.CheckedChanged += CaptionCheck_CheckedChanged;
            // 
            // AudioVoiceCheck
            // 
            AudioVoiceCheck.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            AudioVoiceCheck.AutoSize = true;
            AudioVoiceCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            AudioVoiceCheck.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            AudioVoiceCheck.Location = new System.Drawing.Point(3, 26);
            AudioVoiceCheck.Name = "AudioVoiceCheck";
            AudioVoiceCheck.Size = new System.Drawing.Size(166, 27);
            AudioVoiceCheck.TabIndex = 22;
            AudioVoiceCheck.Text = "動画音声";
            AudioVoiceCheck.UseVisualStyleBackColor = true;
            AudioVoiceCheck.CheckedChanged += AudioVoiceCheck_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(3, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(126, 23);
            label2.TabIndex = 26;
            label2.Text = "＜動画の音声＞";
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(3, 73);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(126, 23);
            label1.TabIndex = 25;
            label1.Text = "＜動画の字幕＞";
            // 
            // FontTextBox
            // 
            FontTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            FontTextBox.Location = new System.Drawing.Point(340, 99);
            FontTextBox.Name = "FontTextBox";
            FontTextBox.Size = new System.Drawing.Size(125, 27);
            FontTextBox.TabIndex = 27;
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label3.Location = new System.Drawing.Point(249, 96);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(85, 33);
            label3.TabIndex = 28;
            label3.Text = "字幕大きさ";
            // 
            // HosokuCheck
            // 
            HosokuCheck.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            HosokuCheck.AutoSize = true;
            HosokuCheck.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            HosokuCheck.Location = new System.Drawing.Point(340, 26);
            HosokuCheck.Name = "HosokuCheck";
            HosokuCheck.Size = new System.Drawing.Size(18, 27);
            HosokuCheck.TabIndex = 24;
            HosokuCheck.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label4.Location = new System.Drawing.Point(175, 23);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(159, 33);
            label4.TabIndex = 29;
            label4.Text = "補足字幕も音声案内";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OkButton
            // 
            OkButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            OkButton.Location = new System.Drawing.Point(376, 180);
            OkButton.Name = "OkButton";
            OkButton.Size = new System.Drawing.Size(94, 29);
            OkButton.TabIndex = 30;
            OkButton.Text = "適用";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(550, 102);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(8, 8);
            button1.TabIndex = 26;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(504, 231);
            Controls.Add(button1);
            Controls.Add(tableLayoutPanel1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form2";
            Text = "動画のアクセシビリティ";
            Load += Form2_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox CaptionCheck;
        private System.Windows.Forms.CheckBox HosokuCheck;
        private System.Windows.Forms.CheckBox AudioVoiceCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FontTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.CheckBox CharaCheck;
    }
}