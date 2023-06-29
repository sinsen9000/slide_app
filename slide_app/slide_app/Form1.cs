using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Office.Core;
using ppt = Microsoft.Office.Interop.PowerPoint;

namespace slide_app
{
    public partial class Form1 : Form
    {
        
        public static List<Cue_card> notes;
        public static Process VoicevoxProcess;
        public static string file_name, waveFile, dic_voice, bracket_sentence;
        public static DataTable table;
        public static bool is_bracket, is_square;
        public static float Speed, Intonation, prePhonemeLength, postPhonemeLength;
        private bool is_DeleteFile;
        private static readonly string passwordChars = "0123456789abcdefghijklmnopqrstuvwxyz";


        public class Cue_card
        {
            /// <summary>
            /// 表形式で出力するためのクラス。csvファイル化
            /// </summary>
            public int No { get; set; } //文章番号
            public int Num { get; set; } //スライドページ
            public int Id { get; set; } //動作ID
            public string Pnt { get; set; } //分割記号（何で分割したか？）
            public string Sentence { get; set; } //文章内容
            public string Bracket { get; set; } //字幕
            public int Size { get; set; } //文章長
        }

        internal class JsonData
        {
            /// <summary>
            /// 設定ファイルのクラス（Setting.json）
            /// </summary>
            public string VisualMode { get; set; }
            public string FileName { get; set; }
            public string VoiceName { get; set; }
            public float VoiceSpeed { get; set; }
            public float VoiceInterval { get; set; }
            public float VoiceIntonation { get; set; }
        }

        public string GeneratePassword(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            Random r = new Random();

            for (int i = 0; i < length; i++){
                int pos = r.Next(passwordChars.Length); //文字の位置をランダムに選択
                char c = passwordChars[pos];            //選択された位置の文字を取得
                sb.Append(c);                           //パスワードに追加
            }

            return sb.ToString();
        }
        public Form1()
        {
            InitializeComponent();
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit); //ApplicationExitイベントハンドラを追加
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VoicevoxProcess = Process.Start(@"C:\Program Files\VOICEVOX\run.exe");
            GenerateButton.Enabled = false;
            void Directory_make(string target_folder){
                if (!Directory.Exists(target_folder))
                {
                    DirectoryInfo di = new DirectoryInfo(target_folder);
                    di.Create();
                }
            }

            //音声設定：速度
            SpeedLabel.Text = "1";
            SpeedBar.Minimum = 0;
            SpeedBar.Maximum = 300;
            SpeedBar.TickFrequency = 5;
            SpeedBar.Value = int.Parse(SpeedLabel.Text)*100;
            Speed = 1f;

            //音声設定：抑揚
            IntonationLabel.Text = "1";
            IntonationBar.Minimum = 0;
            IntonationBar.Maximum = 200;
            IntonationBar.TickFrequency = 5;
            IntonationBar.Value = int.Parse(IntonationLabel.Text)*100;
            Intonation = 1f;

            //音声設定：無音時間
            prePhonemeLength = 0.25f;
            postPhonemeLength = 0.25f;

            Directory_make(".\\slide_image");
            Directory_make(".\\voice");
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            FileLabel.Text = "";
            is_bracket = false;
            is_square = false;
            RecordButton.Enabled = false;
            CancelButton.Enabled = false;
            BackVOICEVOX.DoWork += new DoWorkEventHandler(BackVOICEVOX_DoWork);
            BackVOICEVOX.ProgressChanged +=
                new ProgressChangedEventHandler(BackVOICEVOX_ProgressChanged);
            BackVOICEVOX.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(BackVOICEVOX_RunWorkerCompleted);
            BackVOICEVOX.WorkerReportsProgress = true;
            BackVOICEVOX.WorkerSupportsCancellation = true;

            BackVoiceTest.DoWork += new DoWorkEventHandler(BackVoiceTest_DoWork);
            BackVoiceTest.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(BackVoiceTest_RunWorkerCompleted);

        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            VoicevoxProcess.Kill(); //VOICEVOXのタスクキル
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofDialog = new OpenFileDialog();
            ofDialog.InitialDirectory = @"C:";
            ofDialog.Title = "スライドを開く";
            ofDialog.Filter = "プレゼンテーションとスライドショー(*.pptx; *.pptm; *.ppt)| *.pptx; *.pptm; *.ppt | すべてのファイル(*.*) | *.* ";
            if (ofDialog.ShowDialog() == DialogResult.OK) { OpenFileBox.Text = ofDialog.FileName; }
            else { return; }
            ofDialog.Dispose();

            if (!File.Exists(OpenFileBox.Text))
            {
                MessageBox.Show("ファイルを指定していないか存在しないファイルです", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ResultGrid.DataSource = null; //表の更新
            notes = new List<Cue_card>();
            var ppt_file = new ppt.Application().Presentations.Open(OpenFileBox.Text,
                    MsoTriState.msoTrue,
                    MsoTriState.msoTrue,
                    MsoTriState.msoFalse);

            // 表の作成。スライドのインデックスは１から
            string txt;
            int num = 0;
            for (int i = 1; i <= ppt_file.Slides.Count; i++)
            {
                txt = ppt_file.Slides[i].NotesPage.Shapes.Placeholders[2].TextFrame.TextRange.Text;
                List<string> temp_lines = txt.Split('\r').ToList();
                foreach (string line in temp_lines)
                {
                    char[] splitStr = line.ToCharArray();
                    List<string> words = new List<string>();
                    List<string> square_txts = new List<string>();
                    string t;
                    foreach (var word in splitStr){
                        words.Add(word.ToString());
                        if (new Regex(@"(。|？|\?|！|\!|（|「|\(|）|」|\))").IsMatch(word.ToString())){
                            t = String.Join("", words.ToArray());
                            if (is_bracket){
                                // 字幕文の生成
                                bracket_sentence = bracket_sentence + t;
                                if (new Regex(@"(）|\))").IsMatch(word.ToString())){
                                    notes[notes.Count() - 1].Bracket = bracket_sentence[..^1];
                                    bracket_sentence = "";
                                    is_bracket = false;
                                }
                            }
                            else if (!new Regex("^[ -/:-@[-´{-~]+$").IsMatch(t) && !new Regex("^[！”＃＄％＆’（）＝～｜‘｛＋＊｝＜＞？＿－＾￥＠「；：」、。・]*$").IsMatch(t)){
                                // 音声文の生成
                                if (new Regex(@"「").IsMatch(notes[notes.Count - 1].Sentence) && new Regex(@"」").IsMatch(t)) notes[notes.Count - 1].Sentence = notes[notes.Count - 1].Sentence+t;
                                else{
                                    num = num + 1;
                                    Cue_card m = new Cue_card();
                                    m.No = num;
                                    m.Num = i;
                                    m.Id = 0;
                                    m.Pnt = words[words.Count - 1];
                                    if (new Regex(@"(（|「|\(|「)").IsMatch(t)) t = t[..^1];
                                    if (notes[notes.Count - 1].Pnt == "「") t = "「" + t;
                                    m.Sentence = t;
                                    m.Bracket = "";
                                    m.Size = words.Count;
                                    notes.Add(m);
                                }
                                
                            }
                            if (new Regex(@"(（|\()").IsMatch(word.ToString())) is_bracket = true;
                            words.Clear();
                        }
                    }
                    t = String.Join("", words.ToArray());
                    if (words.Count() > 0 && t != " "){
                        num = num + 1;
                        Cue_card m = new Cue_card {No=num, Num=i,Id=0, Pnt="。", Sentence= String.Join("", words.ToArray()), Bracket="", Size=words.Count};
                        notes.Add(m);
                    }
                }
            }
            if (ppt_file != null){
                ppt_file.Close(); //ファイルを閉じる
                ppt_file = null;
            }
            
            table = new DataTable("Table");
            table.Columns.Add("番号");
            table.Columns.Add("ページ");
            table.Columns.Add("動作ID");
            table.Columns.Add("記号");
            table.Columns.Add("文章");
            table.Columns.Add("字幕");
            foreach (var note in notes) table.Rows.Add(note.No, note.Num, note.Id, note.Pnt, note.Sentence, note.Bracket);
            ResultGrid.DataSource = table;
            ResultGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            GenerateButton.Enabled = true;
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (ModeCombo.Text == "" || VoiceNameCombo.Text == ""){
                MessageBox.Show("音源名とアクセシビリティの設定をしていません", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            progressBar1.Value = 0;

            // リスト末尾にVOICEVOXの音源名と締めのあいさつを入れる。音源名の発声はライセンス対策 //
            List<Cue_card> last_list = new List<Cue_card>();
            Cue_card m_last_1 = new Cue_card { No = notes[notes.Count - 1].No + 1, Num = notes[notes.Count - 1].Num, Id = 0, Pnt = "。", Sentence = String.Format("音声は、voicevoxの{0}でした。", VoiceNameCombo.Text), Bracket = String.Format("VOICEVOX: {0}", VoiceNameCombo.Text), Size = 10 };
            last_list.Add(m_last_1);
            notes.Add(m_last_1);
            Cue_card m_last_2 = new Cue_card { No = m_last_1.No + 1, Num = m_last_1.Num, Id = 0, Pnt = "。", Sentence = String.Format("ここまでのご視聴、ありがとうございました。", VoiceNameCombo.Text), Bracket = String.Format("VOICEVOX: {0}", VoiceNameCombo.Text), Size = 10 };
            last_list.Add(m_last_2);
            notes.Add(m_last_2);
            foreach (var note in last_list) table.Rows.Add(note.No, note.Num, note.Id, note.Pnt, note.Sentence, note.Bracket);

            // スライド画像を生成する //
            List<string> temp_files = OpenFileBox.Text.Split('\\').ToList();
            file_name = GeneratePassword(10); //ファイルは適当な文字列。unityで音声や画像を読み込む際、日本語を含んだ文字列は認識できないため
            var ppt_file = new ppt.Application().Presentations.Open(OpenFileBox.Text,
                    MsoTriState.msoTrue,
                    MsoTriState.msoTrue,
                    MsoTriState.msoFalse);
            int width = (int)ppt_file.PageSetup.SlideWidth;
            int height = (int)ppt_file.PageSetup.SlideHeight;
            string file2;
            string dic_image = Directory.GetCurrentDirectory() + "\\slide_image\\" + file_name;
            if (!Directory.Exists(dic_image)){
                DirectoryInfo di = new DirectoryInfo(dic_image);  //スライド画像保存フォルダを生成
                di.Create();
            }
            for (int i = 1; i <= ppt_file.Slides.Count; i++){
                file2 = dic_image + String.Format("\\slide{0:0}.jpg", i); //JPEGとして保存
                ppt_file.Slides[i].Export(file2, "jpg", width, height);
            }
            if (ppt_file != null){
                ppt_file.Close(); //ファイルを閉じる
                ppt_file = null;
            }

            dic_voice = Directory.GetCurrentDirectory() + @"\voice\" + file_name;
            if (!Directory.Exists(dic_voice)){
                DirectoryInfo di = new DirectoryInfo(dic_voice); //音声保存フォルダを生成
                di.Create();
            }
            OpenFileButton.Enabled = false;
            GenerateButton.Enabled = false;
            StateLabel.Text = "音声生成済み:";
            progressBar1.Maximum = notes.Count();
            CancelButton.Enabled = true;
            BackVOICEVOX.RunWorkerAsync(); //音声生成開始(並列処理)
        }

        private void BackVOICEVOX_DoWork(object sender, DoWorkEventArgs e)
        {
            int before_num = 1;
            foreach (var note in notes){
                if (BackVOICEVOX.CancellationPending) return;
                waveFile = String.Format(@"\{0}.wav", note.No);
                if (new Regex(@"(。|？|\?|！|\!)").IsMatch(note.Pnt)) postPhonemeLength = 0.5f;
                if (note.Num != before_num) prePhonemeLength = 0.5f;

                // 音声生成が完了するまで録音は開始できない //
                VoicevoxUtility.RecordSpeech(Form1.dic_voice + waveFile, note.Sentence, VoiceNameCombo.Text).Wait();
                BackVOICEVOX.ReportProgress(note.No);
                prePhonemeLength = 0.25f;
                postPhonemeLength = 0.25f;
                before_num = note.Num;
            }
        }

        private void ModeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ModeCombo.Text == "normal")
            {
                ModeText.Text = "出力された表通りに音声を作成します。\n" +
                                "字幕はノート内のカッコ内に記載された文章のみを動画下部に表示します。";
            }
            else if (ModeCombo.Text == "visual")
            {
                ModeText.Text = "出力された表通りに音声を作成します。\n" +
                                 "字幕はノート内の全ての文章を動画左部に表示します。";
            }
        }

        private void VoiceTestButton_Click(object sender, EventArgs e)
        {
            VoiceNameCombo.Enabled = false;
            GenerateButton.Enabled = false;
            VoiceTestButton.Enabled = false;
            BackVoiceTest.RunWorkerAsync();

        }

        private void SpeedBar_Scroll(object sender, EventArgs e)
        {
            Speed = SpeedBar.Value / 100f;
            SpeedLabel.Text = "" + Speed;
        }

        private void IntonationBar_Scroll(object sender, EventArgs e)
        {
            Intonation = IntonationBar.Value / 100f;
            IntonationLabel.Text = "" + Intonation;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Directory.Delete(Directory.GetCurrentDirectory() + "\\slide_image\\" + file_name, true);
            Directory.Delete(Directory.GetCurrentDirectory() + "\\voice\\" + file_name, true);
            CancelButton.Enabled = false;
            BackVOICEVOX.CancelAsync();
            
            progressBar1.Value = 0;
        }

        private void RecordButton_Click(object sender, EventArgs e)
        {
            Process.Start(Directory.GetCurrentDirectory() + "\\slide_app\\Capture");
        }

        private void BackVoiceTest_DoWork(object sender, DoWorkEventArgs e)
        {
            VoicevoxUtility.Speek(String.Format("これはテストです。{0}がお話しています。", VoiceNameCombo.Text), VoiceNameCombo.Text).Wait();
        }

        private void BackVoiceTest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GenerateButton.Enabled = true;
            VoiceNameCombo.Enabled = true;
            VoiceTestButton.Enabled = true;
        }

        private void BackVOICEVOX_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FileLabel.Text = String.Format(@"{0}.wav", e.ProgressPercentage);
            progressBar1.Value = e.ProgressPercentage;
        }

        private void BackVOICEVOX_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StateLabel.Text = "完了";
            FileLabel.Text = "";
            OpenFileButton.Enabled = true;
            GenerateButton.Enabled = true;
            RecordButton.Enabled = true;
            CancelButton.Enabled = false;

            if (!is_DeleteFile)
            {
                //CSV出力用変数の作成
                List<string> lines = new List<string>();

                //列名をカンマ区切りで1行に連結
                List<string> header = new List<string>();
                foreach (DataColumn dr in table.Columns) header.Add(dr.ColumnName);
                lines.Add(string.Join(",", header));

                //列の値をカンマ区切りで1行に連結
                foreach (DataRow dr in table.Rows) lines.Add(string.Join(",", dr.ItemArray));
                string dic_csv = Directory.GetCurrentDirectory() + "\\csv";
                if (!Directory.Exists(dic_csv))
                {
                    DirectoryInfo di = new DirectoryInfo(dic_csv);
                    di.Create();
                }
                File.WriteAllLines(dic_csv + "\\" + file_name + @".csv", lines, Encoding.UTF8);

                var options = new JsonSerializerOptions
                {
                    // 日本語を変換するためのエンコード設定
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),

                    // プロパティ名をキャメルケースに変換
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

                    // インデントを付ける
                    WriteIndented = true
                };
                var jsondata = new JsonData
                {
                    VisualMode = ModeCombo.Text,
                    FileName = file_name,
                    VoiceName = VoiceNameCombo.Text,
                    VoiceSpeed = Speed,
                    VoiceInterval = 0f,
                    VoiceIntonation = Intonation
                };
                var jsonString = JsonSerializer.Serialize(jsondata, options);
                File.WriteAllText(@"Setting.json", jsonString);
            }
        }
    }
}
