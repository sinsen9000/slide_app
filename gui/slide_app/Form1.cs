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
using MeCab;
using Slide_app;

namespace slide_app
{
    public partial class Form1 : Form
    {
        public static List<Cue_card> notes, target_notes;
        //public static List<SaveFiles> SaveFile_list = new List<SaveFiles>();
        public static Process VoicevoxProcess, UnityProcess;
        public static string voice_name, waveFile, dic_voice, bracket_sentence;
        public static DataTable table;
        public static bool is_bracket, is_square;
        public static float Speed, Intonation, prePhonemeLength, postPhonemeLength;
        private bool new_file, not_FileSelect;
        private bool is_cancel = false;
        private static readonly string passwordChars = "0123456789abcdefghijklmnopqrstuvwxyz";
        private string dic_csv = "";
        private readonly List<int> emotion_list = new List<int> {0,14,15,12,16,10,11,13,0}; //[なし, 喜び, 悲しみ, 期待, 驚き, 怒り, 恐れ, 嫌悪, 信頼]
        private string project_dict = "", project_name = "", ppt_file_name = "", ppt_path = "";
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
            public string Bracket { get; set; } //補足字幕
            public string Voice { get; set; } //音声字幕
            public int Size { get; set; } //文章長
        }

        internal class JsonData
        {
            /// <summary>
            /// 設定ファイルのクラス（Setting.json）
            /// </summary>
            public string targetPath { get; set; }
        }

        public class ProjectJson
        {
            /// <summary>
            /// 設定ファイルのクラス（Project.json）
            /// </summary>
            public string videoAudio { get; set; }
            public string hosokuAudio { get; set; }
            public string videoCaption { get; set; }
            public string captionFont { get; set; }
            public string motion { get; set; }
            public string fileName { get; set; }
            public string voiceName { get; set; }
            public float voiceSpeed { get; set; }
            public float voiceInterval { get; set; }
            public float voiceIntonation { get; set; }
        }

        private string ReplaceFileName(string target_file)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (char i in invalidChars)
            {
                target_file = target_file.Replace(i.ToString(), "_");
            }
            Console.WriteLine(target_file);
            return target_file;
        }

        public string GeneratePassword(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            Random r = new Random();

            for (int i = 0; i < length; i++)
            {
                int pos = r.Next(passwordChars.Length); //文字の位置をランダムに選択
                char c = passwordChars[pos];            //選択された位置の文字を取得
                sb.Append(c);                           //パスワードに追加
            }

            return sb.ToString();
        }
        private int Motion_ID(List<int> motion_list, string sentence="",int count=0)
        {
            int motion_ID = 0;
            string ConjunctionText = "", InterjectionText = "";
            if (sentence.Contains("こちら"))
            {
                return 23;
            }

            var tagger = MeCabTagger.Create();
            foreach (var node in tagger.ParseToNodes(sentence))
            {
                if (0 < node.CharType)
                {
                    if (node.Feature.Contains("接続詞"))
                    {
                        ConjunctionText = node.Surface;
                        break;
                    }
                    else if (node.Feature.Contains("感動詞"))
                    {
                        InterjectionText = node.Surface;
                        break;
                    }
                }
            }
            //Debug.WriteLine(resultText.TrimEnd(','));
            if (ConjunctionText != "")
            {
                if (ConjunctionText.Contains("しかし") || ConjunctionText.Contains("しかしながら") || ConjunctionText.Contains("でも") || ConjunctionText.Contains("だが"))
                {
                    motion_ID = 26;
                }
                else if (ConjunctionText.Contains("では"))
                {
                    motion_ID = 20;
                }
                else if (ConjunctionText.Contains("まず"))
                {
                    motion_ID = 3;
                }
                else if (ConjunctionText.Contains("それでは"))
                {
                    motion_ID = 2;
                }
            }
            else if (InterjectionText != "")
            {
                if (InterjectionText.Contains("はい") || InterjectionText.Contains("ええ"))
                {
                    motion_ID = 25;
                }
                else if (InterjectionText.Contains("いいえ"))
                {
                    motion_ID = 26;
                }
            }
            else if (count >= 20 || motion_list.Count == 0)
            {
                Random r1 = new System.Random();
                int num = r1.Next(1, 11);
                if (num == 1 || num == 3 || num == 5 || num == 7 || num == 9)
                {
                    var sampleData = new SentimentModel.ModelInput() { Sentence = sentence };
                    var result = SentimentModel.Predict(sampleData); //感情推論
                    motion_ID = emotion_list[(int)result.PredictedLabel];
                    if (!motion_list.Contains(motion_ID)) return motion_ID;
                }
                while (true)
                {
                    Random r2 = new System.Random();
                    motion_ID = r2.Next(1, 5);
                    if (!motion_list.Contains(motion_ID)) break;
                }
            }
            return motion_ID;
        }

        public Form1()
        {
            InitializeComponent();
            VoicevoxProcess = Process.Start(@"C:\Program Files\VOICEVOX\run.exe");
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit); //ApplicationExitイベントハンドラを追加
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateButton.Enabled = false;

            
            DirectoryInfo directoryInfo = new DirectoryInfo(@".\projects"); //フォルダ情報取得
            var subDirectories = directoryInfo.GetDirectories(); //サブディレクトリ情報取得
            string[] folderNames = new string[subDirectories.Length]; //フォルダ名配列を作成
            for (int i = 0; i < subDirectories.Length; i++) { folderNames[i] = subDirectories[i].Name; } //フォルダ名を取得して配列に格納
            foreach (string files in folderNames) OpenFileBox.Items.Add(files);

            //音声設定：速度
            SpeedLabel.Text = "1";
            SpeedBar.Minimum = 0;
            SpeedBar.Maximum = 300;
            SpeedBar.TickFrequency = 5;
            SpeedBar.Value = int.Parse(SpeedLabel.Text) * 100;
            Speed = 1f;

            //音声設定：抑揚
            IntonationLabel.Text = "1";
            IntonationBar.Minimum = 0;
            IntonationBar.Maximum = 200;
            IntonationBar.TickFrequency = 5;
            IntonationBar.Value = int.Parse(IntonationLabel.Text) * 100;
            Intonation = 1f;

            //音声設定：無音時間
            prePhonemeLength = 0.25f;
            postPhonemeLength = 0.25f;

            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            FileLabel.Text = "";
            is_bracket = false;
            is_square = false;
            not_FileSelect = false;
            CancelButton.Enabled = false;
            SaveButton.Enabled = false;
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
            this.TopMost = !this.TopMost;
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            VoicevoxProcess.Kill(); //VOICEVOXのタスクキル
            UnityProcess.Kill();
        }

        private void Generate_Grid()
        {
            // 表の作成 //
            ResultGrid.DataSource = null; //表の更新
            table = new DataTable("Table");
            table.Columns.Add("番号");
            table.Columns.Add("ページ");
            table.Columns.Add("動作ID");
            table.Columns.Add("記号");
            table.Columns.Add("音声の文章");
            table.Columns.Add("字幕用の文章");
            table.Columns.Add("補足説明");
            foreach (var note in notes) table.Rows.Add(note.No, note.Num, note.Id, note.Pnt, note.Sentence, note.Voice, note.Bracket);
            ResultGrid.DataSource = table;

            ResultGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ResultGrid.Columns["番号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ResultGrid.Columns["ページ"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ResultGrid.Columns["動作ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ResultGrid.Columns["記号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ResultGrid.Columns["音声の文章"].Width = 400;
            ResultGrid.Columns["字幕用の文章"].Width = 400;
            ResultGrid.Columns["補足説明"].Width = 400;
            ResultGrid.Columns["音声の文章"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ResultGrid.Columns["字幕用の文章"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ResultGrid.Columns["補足説明"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            GenerateButton.Enabled = true;
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofDialog = new OpenFileDialog();
            ofDialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\projects\";
            ofDialog.Title = "スライドを開く";
            ofDialog.Filter = "プレゼンテーションとスライドショー(*.pptx; *.pptm; *.ppt)| *.pptx; *.pptm; *.ppt | すべてのファイル(*.*) | *.* ";
            if (ofDialog.ShowDialog() == DialogResult.OK)
            {
                ppt_file_name = ofDialog.SafeFileName; //pptのファイル名
                project_name = ReplaceFileName(ppt_file_name); //フォルダの名前
                ppt_path = ofDialog.FileName; //pptのパス（絶対パス）
                project_dict = Directory.GetCurrentDirectory() + @"\projects\" + project_name;

                if (Directory.Exists(project_dict))
                {
                    DialogResult result = MessageBox.Show("既に同じプロジェクト名が使用されています\n中身のデータを上書きしますか", "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        not_FileSelect = true;
                        if (File.Exists($"{project_dict}\\use_save.tsv")) File.Delete($"{project_dict}\\use_save.tsv");
                        if (File.Exists($"{project_dict}\\use_video.tsv")) File.Delete($"{project_dict}\\use_video.tsv");
                        if (Directory.Exists($"{project_dict}\\slide_image")) Directory.Delete($"{project_dict}\\slide_image", true);
                        if (Directory.Exists($"{project_dict}\\voice")) Directory.Delete($"{project_dict}\\voice", true);
                    }
                    else if (result == DialogResult.No) return;
                }
                OpenFileBox.Text = project_name;
            }
            else { return; }
            ofDialog.Dispose();
            if (!File.Exists(ppt_path))
            {
                MessageBox.Show("ファイルを指定していないか存在しないファイルです", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveButton.Enabled = false;
            var ppt_file = new ppt.Application().Presentations.Open(ppt_path,
                    MsoTriState.msoTrue,
                    MsoTriState.msoTrue,
                    MsoTriState.msoFalse); //スライド画像・ノートの読み込み
            string txt;
            int num = 0;
            notes = new List<Cue_card>();
            List<string> words = new List<string>();
            List<string> branket_words = new List<string>();

            void add_text(string normal_sentence = "", string branket_text = "", int i = 0)
            {
                num += 1;
                int word_count = words.Count;
                string point = "。";
                List<int> motion_list = new List<int>();
                if (notes.Count > 0)
                {
                    int num = 0;
                    foreach (Cue_card _index in Enumerable.Reverse(notes).ToList())
                    {
                        motion_list.Add(_index.Id);
                        num += 1;
                        if (num == 2) break;
                    }
                }
                int motion_ID = Motion_ID(motion_list, normal_sentence, word_count);
                if (new Regex(@"(（|\()").IsMatch(words[word_count - 1])) point = words[word_count - 1];

                Cue_card m = new Cue_card { No = num, Num = i, Id = motion_ID, Pnt = point, Sentence = normal_sentence, Voice = normal_sentence, Bracket = branket_text, Size = words.Count };
                notes.Add(m);
                words.Clear();
                branket_words.Clear();
            }

            for (int i = 1; i <= ppt_file.Slides.Count; i++) //スライドのインデックスは１から
            {
                txt = ppt_file.Slides[i].NotesPage.Shapes.Placeholders[2].TextFrame.TextRange.Text;
                List<string> temp_lines = txt.Split('\r').ToList();
                foreach (string line in temp_lines) //ノートの分割
                {
                    char[] splitStr = line.ToCharArray();
                    string target_word, normal_sentence, branket_text;
                    int note_count;
                    foreach (var word in splitStr)
                    {
                        note_count = notes.Count();
                        target_word = word.ToString();
                        if (new Regex(@"(（|\(|）|\))").IsMatch(target_word) && note_count > 0)
                        {
                            if (new Regex(@"(（|\()").IsMatch(target_word)) is_bracket = true;
                            else if (new Regex(@"(）|\))").IsMatch(target_word))
                            {
                                words.Add("<branket>");
                                is_bracket = false;
                            }
                            continue;
                        }
                        if (is_bracket)
                        {
                            branket_words.Add(target_word);
                            continue;
                        }

                        words.Add(target_word);
                        if (new Regex(@"(。|．|？|\?|！|\!)").IsMatch(target_word)) //分割記号となる対象が出た場合、
                        {
                            normal_sentence = String.Join("", words.ToArray());
                            branket_text = String.Join("", branket_words.ToArray());
                            add_text(normal_sentence, branket_text, i);
                        }
                    }
                    normal_sentence = String.Join("", words.ToArray());
                    if (words.Count() > 0 && normal_sentence != " ") //ノート最後の行で文章があれば登録
                    {
                        normal_sentence = String.Join("", words.ToArray());
                        branket_text = String.Join("", branket_words.ToArray());
                        add_text(normal_sentence, branket_text, i);
                    }
                }
            }
            if (ppt_file != null)
            {
                ppt_file.Close(); //ファイルを閉じる
                ppt_file = null;
            }
            Generate_Grid();
            new_file = true;
            not_FileSelect = false;
        }

        private void OpenFileBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (not_FileSelect) return;
            project_name = OpenFileBox.Text; //フォルダの名前
            project_dict = Directory.GetCurrentDirectory() + @"\projects\" + project_name;
            ProjectJson project_json = new ProjectJson();

            if (File.Exists($"{project_dict}\\Project.json"))
            {
                using var jsonStream = File.OpenRead($"{project_dict}\\Project.json");
                project_json = JsonSerializer.Deserialize<ProjectJson>(jsonStream);
                ppt_file_name = project_json.fileName; //pptのファイル名
                ppt_path = $"{project_dict}\\{ppt_file_name}"; //pptのパス（絶対パス）
            }
            else
            {
                MessageBox.Show("プロジェクトファイルの一部が欠損しています\nプロジェクトフォルダ内のPowerPointファイルを指定し、再設定して下さい", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                OpenFileDialog ofDialog = new OpenFileDialog();
                ofDialog.InitialDirectory = @"C:";
                ofDialog.Title = "スライドを開く";
                ofDialog.Filter = "プレゼンテーションとスライドショー(*.pptx; *.pptm; *.ppt)| *.pptx; *.pptm; *.ppt | すべてのファイル(*.*) | *.* ";
                if (ofDialog.ShowDialog() == DialogResult.OK) { OpenFileBox.Text = ofDialog.FileName; }
                else { return; }
                ofDialog.Dispose();

                ppt_file_name = ofDialog.SafeFileName; //pptのファイル名
                project_name = ReplaceFileName(ppt_file_name); //フォルダの名前
                ppt_path = ofDialog.FileName; //pptのパス（絶対パス）
                project_dict = Directory.GetCurrentDirectory() + @"\projects\" + project_name;

                project_json.videoAudio = "False";
                project_json.hosokuAudio = "False";
                project_json.videoCaption = "False";
                project_json.captionFont = "24";
                project_json.motion = "False";
                project_json.fileName = ppt_file_name;
                project_json.voiceName = "四国めたん";
                project_json.voiceSpeed = 1f;
                project_json.voiceInterval = 0f;
                project_json.voiceIntonation = 1f;
            }
            
            VoiceNameCombo.Text = project_json.voiceName;
            SpeedLabel.Text = project_json.voiceSpeed.ToString();
            Speed = project_json.voiceSpeed;
            SpeedBar.Value = (int) float.Parse(SpeedLabel.Text) * 100;
            IntonationLabel.Text = project_json.voiceIntonation.ToString();
            Intonation = project_json.voiceIntonation;
            IntonationBar.Value = (int) float.Parse(IntonationLabel.Text) * 100;

            notes = new List<Cue_card>();
            using (StreamReader sr = new StreamReader($"{project_dict}\\use_save.tsv"))
            {
                bool isFirstLineSkip = true;
                while (0 <= sr.Peek())
                {
                    //カンマ区切りで分割して配列で格納する
                    var line = sr.ReadLine().Split('\t');
                    if (line is null) continue;
                    if (isFirstLineSkip)
                    {
                        isFirstLineSkip = false;
                        continue;
                    }
                    //リストにデータを追加する
                    Cue_card m = new Cue_card { No = int.Parse(line[0]), Num = int.Parse(line[1]), Id = int.Parse(line[2]), Pnt = line[3], Sentence = line[4], Voice = line[5], Bracket = line[6], Size = line[4].Length };
                    notes.Add(m);
                }
            }
            Generate_Grid();
            new_file = false;
            SaveButton.Enabled = true;
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (VoiceNameCombo.Text == "")
            {
                MessageBox.Show("音声名を設定してください", "エラー",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            progressBar1.Value = 0;
            Form2 form2 = new Form2(this); //アクセシビリティに関する設定画面を出す
            form2.FormClosed += new FormClosedEventHandler(Form2_FormClosed);
            form2.Show();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!Directory.Exists(project_dict))
            {
                DirectoryInfo di = new DirectoryInfo(project_dict);  //スライド画像保存フォルダを生成
                di.Create();
            }

            // リスト末尾にVOICEVOXの音源名と締めのあいさつを入れる。音源名の発声はライセンス対策 //
            if (notes[notes.Count - 1].Sentence != "ここまでのご視聴、ありがとうございました。")
            {
                List<Cue_card> last_list = new List<Cue_card>();
                Cue_card m_last_1 = new Cue_card { No = notes[notes.Count - 1].No + 1, Num = notes[notes.Count - 1].Num, Id = 21, Pnt = "。", Sentence = String.Format("この動画は、VOICEVOX: {0}の音声でお送り致しました。", VoiceNameCombo.Text), Bracket = "", Voice = String.Format("この動画は、VOICEVOX: {0}の音声でお送り致しました。", VoiceNameCombo.Text), Size = 10 };
                last_list.Add(m_last_1);
                notes.Add(m_last_1);
                Cue_card m_last_2 = new Cue_card { No = m_last_1.No + 1, Num = m_last_1.Num, Id = 0, Pnt = "。", Sentence = String.Format("ここまでのご視聴、ありがとうございました。", VoiceNameCombo.Text), Bracket = "", Voice = "ここまでのご視聴、ありがとうございました。", Size = 10 };
                last_list.Add(m_last_2);
                notes.Add(m_last_2);
                foreach (var note in last_list) table.Rows.Add(note.No, note.Num, note.Id, note.Pnt, note.Sentence, note.Voice, note.Bracket);
            }

            // スライド画像を生成する //
            string dic_image = $"{project_dict}\\slide_image";
            if (!Directory.Exists(dic_image))
            {
                DirectoryInfo di = new DirectoryInfo(dic_image);  //スライド画像保存フォルダを生成
                di.Create();
            }
            var ppt_file = new ppt.Application().Presentations.Open(ppt_path,
                    MsoTriState.msoTrue,
                    MsoTriState.msoTrue,
                    MsoTriState.msoFalse);
            string file2;
            for (int i = 1; i <= ppt_file.Slides.Count; i++)
            {
                file2 = dic_image + String.Format("\\slide{0:0}.jpg", i); //JPEGとして保存
                ppt_file.PageSetup.SlideWidth = 16 * 64;
                ppt_file.PageSetup.SlideHeight = 9 * 64;
                int width = (int)ppt_file.PageSetup.SlideWidth;
                int height = (int)ppt_file.PageSetup.SlideHeight;
                ppt_file.Slides[i].Export(file2, "jpg", width, height);
            }
            if (ppt_file != null)
            {
                ppt_file.Close(); //ファイルを閉じる
                ppt_file = null;
            }

            dic_voice = $"{project_dict}\\voice";
            if (!Directory.Exists(dic_voice))
            {
                DirectoryInfo di = new DirectoryInfo(dic_voice); //音声保存フォルダを生成
                di.Create();
            }
            OpenFileButton.Enabled = false;
            GenerateButton.Enabled = false;
            SaveButton.Enabled = false;
            StateLabel.Text = "音声生成済み:";
            progressBar1.Maximum = notes.Count();
            CancelButton.Enabled = true;
            BackVOICEVOX.RunWorkerAsync(); //音声生成開始(並列処理)
        }

        private void BackVOICEVOX_DoWork(object sender, DoWorkEventArgs e)
        {
            target_notes = new List<Cue_card>(); //ResultGridからデータを取得
            foreach (DataGridViewRow row in ResultGrid.Rows)
            {
                if (!row.IsNewRow)
                {
                    Cue_card m = new Cue_card
                    {
                        No = Convert.ToInt32(row.Cells["番号"].Value), // "No"列のデータを整数型に変換
                        Num = Convert.ToInt32(row.Cells["ページ"].Value), // "Num"列のデータを整数型に変換
                        Id = Convert.ToInt32(row.Cells["動作ID"].Value), // "Id"列のデータを整数型に変換
                        Pnt = row.Cells["記号"].Value.ToString(), // "Pnt"列のデータを文字列に変換
                        Sentence = row.Cells["音声の文章"].Value.ToString(), // "Sentence"列のデータを文字列に変換
                        Bracket = row.Cells["補足説明"].Value.ToString(), // "Bracket"列のデータを文字列に変換
                        Voice = row.Cells["字幕用の文章"].Value.ToString(), // "Voice"列のデータを文字列に変換
                        Size = row.Cells["音声の文章"].Value.ToString().Length
                    };
                    if (m.Sentence.Contains("<branket>"))
                    {
                        string temp_Sentence = m.Sentence;
                        string temp_Voice = m.Voice;
                        if (HosokuLabel.Text == "True")
                        {
                            m.Sentence = temp_Sentence.Replace("<branket>", $"（{m.Bracket}）");
                            m.Voice = temp_Voice.Replace("<branket>", $"（{m.Bracket}）");
                            m.Bracket = "";
                        }
                        else
                        {
                            m.Sentence = temp_Sentence.Replace("<branket>", "");
                            m.Voice = temp_Voice.Replace("<branket>", "");
                        }
                    }
                    if (CharacterLabel.Text == "False") m.Id = 0;
                    target_notes.Add(m);
                }
            }

            try
            {
                int before_num = 1;
                foreach (var note in target_notes)
                {
                    if (BackVOICEVOX.CancellationPending) return;
                    waveFile = String.Format(@"\{0}.wav", note.No);
                    if (new Regex(@"(。|？|\?|！|\!)").IsMatch(note.Pnt)) postPhonemeLength = 0.5f;
                    if (note.Num != before_num) prePhonemeLength = 0.5f;

                    // 音声生成が完了するまで録音は開始できない //
                    VoicevoxUtility.RecordSpeech(dic_voice + waveFile, note.Sentence, voice_name).Wait();
                    BackVOICEVOX.ReportProgress(note.No);
                    prePhonemeLength = 0.25f;
                    postPhonemeLength = 0.25f;
                    before_num = note.Num;
                }
            }
            catch (Exception)
            {
                BackVOICEVOX.CancelAsync();
            }
        }

        private void ModeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void VoiceNameCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            voice_name = VoiceNameCombo.Text;
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
            File.Delete($"{project_dict}\\use_video.tsv");
            Directory.Delete($"{project_dict}\\slide_image", true);
            Directory.Delete($"{project_dict}\\voice", true);

            CancelButton.Enabled = false;
            is_cancel = true;
            BackVOICEVOX.CancelAsync();

            progressBar1.Value = 0;
            StateLabel.Text = "作成キャンセル";
        }

        private void BackVoiceTest_DoWork(object sender, DoWorkEventArgs e)
        {
            VoicevoxUtility.Speek(String.Format("これはテストです。{0}がお話しています。", Form1.voice_name), Form1.voice_name).Wait();
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
            CancelButton.Enabled = false;
            if (!is_cancel) //最後まで遂行した場合
            {
                List<string> lines = new List<string>();　//CSV出力用変数の作成
                List<string> header = new List<string>();
                foreach (DataColumn dr in table.Columns) header.Add(dr.ColumnName);
                lines.Add(string.Join("\t", header)); //列名をカンマ区切りで1行に連結
                foreach (DataRow dr in table.Rows) lines.Add(string.Join("\t", dr.ItemArray));
                File.WriteAllLines($"{project_dict}\\use_save.tsv", lines, Encoding.UTF8); //表->tsv保存。保存の復帰に使う

                lines.Clear();
                List<string> temp;
                lines.Add("番号\tページ\t動作ID\t記号\t音声の文章\t字幕用の文章\t補足説明");
                foreach (Cue_card dr in target_notes)
                {
                    temp = new List<string> {dr.No.ToString(), dr.Num.ToString(), dr.Id.ToString(), dr.Pnt, dr.Voice, dr.Sentence, dr.Bracket};
                    lines.Add(string.Join("\t", temp));
                }
                File.WriteAllLines($"{project_dict}\\use_video.tsv", lines, Encoding.UTF8); //音声List->tsv保存。カンペに使う

                var options = new JsonSerializerOptions
                {
                    // 日本語を変換するためのエンコード設定
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),

                    // プロパティ名をキャメルケースに変換
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

                    // インデントを付ける
                    WriteIndented = true
                };
                var jsondata = new ProjectJson
                {
                    videoAudio = AudioVoiceLabel.Text,
                    hosokuAudio = HosokuLabel.Text,
                    videoCaption = CaptionLabel.Text,
                    captionFont = FontLabel.Text,
                    motion = CaptionLabel.Text,
                    fileName = project_name,
                    voiceName = VoiceNameCombo.Text,
                    voiceSpeed = Speed,
                    voiceInterval = 0f,
                    voiceIntonation = Intonation
                };
                var jsonString = JsonSerializer.Serialize(jsondata, options);
                File.WriteAllText($"{project_dict}\\Project.json", jsonString);

                var save_json = new JsonData
                {
                    targetPath = project_name
                };
                jsonString = JsonSerializer.Serialize(save_json, options);
                File.WriteAllText($"Setting.json", jsonString);
                File.Copy(ppt_path, $"{project_dict}\\{ppt_file_name}");
                OpenFileBox.Items.Add(project_name);
                SaveButton.Enabled = true;
                new_file = false;
            }
            else is_cancel = false;

        }

        private void RecordButton_Click_1(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Directory.GetCurrentDirectory() + "\\Capture");
        }

        private void AvatorButton_Click(object sender, EventArgs e)
        {
            VoicevoxProcess.Kill(); //VOICEVOXのタスクキル
            UnityProcess = Process.Start(Directory.GetCurrentDirectory() + @"\unity_app\slide_app.exe");
            UnityProcess.WaitForExit();
            VoicevoxProcess = Process.Start(@"C:\Program Files\VOICEVOX\run.exe");
        }

        private void ResultGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            var dgv = (DataGridView)sender;
            if (dgv.IsCurrentCellDirty)
            {
                dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void ResultGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            SaveButton.Enabled = false;
            GenerateButton.Enabled = false;

        }
        private void ResultGrid_CellEndEdit2(object sender, DataGridViewCellEventArgs e)
        {
            if (!new_file) SaveButton.Enabled = true;
            GenerateButton.Enabled = true;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            AvatorButton.Enabled = false;
            GenerateButton.Enabled = false;

            var ppt_file = new ppt.Application().Presentations.Open(ppt_path,
                MsoTriState.msoTrue,
                MsoTriState.msoTrue,
                MsoTriState.msoFalse);
            string file2;
            string dic_image = $"{project_dict}\\slide_image";
            for (int i = 1; i <= ppt_file.Slides.Count; i++)
            {
                file2 = dic_image + String.Format("\\slide{0:0}.jpg", i); //JPEGとして保存
                ppt_file.PageSetup.SlideWidth = 16 * 72;
                ppt_file.PageSetup.SlideHeight = 9 * 72;
                int width = (int)ppt_file.PageSetup.SlideWidth;
                int height = (int)ppt_file.PageSetup.SlideHeight;
                ppt_file.Slides[i].Export(file2, "jpg", width, height);
            }
            if (ppt_file != null)
            {
                ppt_file.Close(); //ファイルを閉じる
                ppt_file = null;
            }

            //CSV出力用変数の作成
            List<string> lines = new List<string>();
            table = (DataTable)ResultGrid.DataSource; //ResultGridからデータを取得

            //列名をカンマ区切りで1行に連結
            List<string> header = new List<string>();
            foreach (DataColumn dr in table.Columns) header.Add(dr.ColumnName);
            lines.Add(string.Join("\t", header));

            //列の値をカンマ区切りで1行に連結
            foreach (DataRow dr in table.Rows)
            {
                lines.Add(string.Join("\t", dr.ItemArray));
            }
            File.WriteAllLines($"{project_dict}\\use_save.tsv", lines, Encoding.UTF8);

            var options = new JsonSerializerOptions
            {
                // 日本語を変換するためのエンコード設定
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),

                // プロパティ名をキャメルケースに変換
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

                // インデントを付ける
                WriteIndented = true
            };
            var jsondata = new ProjectJson
            {
                videoAudio = AudioVoiceLabel.Text,
                hosokuAudio = HosokuLabel.Text,
                videoCaption = CaptionLabel.Text,
                captionFont = FontLabel.Text,
                motion = CaptionLabel.Text,
                fileName = ppt_file_name,
                voiceName = VoiceNameCombo.Text,
                voiceSpeed = Speed,
                voiceInterval = 0f,
                voiceIntonation = Intonation
            };
            var jsonString = JsonSerializer.Serialize(jsondata, options);
            File.WriteAllText($"{project_dict}\\Project.json", jsonString);
            AvatorButton.Enabled = true;
            GenerateButton.Enabled = true;
        }
    }
}
