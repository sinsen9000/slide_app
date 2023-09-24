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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace slide_app
{
    public partial class Form1 : Form
    {

        public static List<Cue_card> notes;
        public static List<SaveFiles> SaveFile_list = new List<SaveFiles>();
        public static Process VoicevoxProcess, UnityProcess;
        public static string file_name, voice_name, waveFile, dic_voice, bracket_sentence;
        public static DataTable table;
        public static bool is_bracket, is_square;
        public static float Speed, Intonation, prePhonemeLength, postPhonemeLength;
        private bool new_file, not_FileSelect;
        private bool is_cancel = false;
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
            public string Bracket { get; set; } //補足字幕
            public string Voice { get; set; } //音声字幕
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

        public class SaveFiles
        {
            /// <summary>
            /// 乱数列とpptファイルパスの紐づけ
            /// </summary>
            public string random_str { get; set; }
            public string ppt_filename { get; set; }
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
        private int Motion_ID(int count)
        {
            int motion_ID = 0;
            if (count >= 30)
            {
                Random r1 = new System.Random();
                if (r1.Next(1, 11) < 7)
                {
                    Random r2 = new System.Random();
                    motion_ID = r2.Next(1, 4);
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
            void Directory_make(string target_folder)
            {
                if (!Directory.Exists(target_folder))
                {
                    DirectoryInfo di = new DirectoryInfo(target_folder);
                    di.Create();
                }
            }
            using (StreamReader sr = new StreamReader(".\\savefiles.tsv"))
            {

                while (0 <= sr.Peek())
                {
                    //カンマ区切りで分割して配列で格納する
                    var line = sr.ReadLine().Split('\t');
                    if (line is null) continue;
                    else if (line.Count() < 2) break;
                    //リストにデータを追加する
                    SaveFiles s_d = new SaveFiles { random_str = line[0], ppt_filename = line[1] };
                    SaveFile_list.Add(s_d);
                }
                foreach (SaveFiles files in SaveFile_list) OpenFileBox.Items.Add(files.ppt_filename);
            }

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

            ModeCombo.Text = "normal";
            file_name = "";

            Directory_make(".\\slide_image");
            Directory_make(".\\voice");
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
            ofDialog.InitialDirectory = @"C:";
            ofDialog.Title = "スライドを開く";
            ofDialog.Filter = "プレゼンテーションとスライドショー(*.pptx; *.pptm; *.ppt)| *.pptx; *.pptm; *.ppt | すべてのファイル(*.*) | *.* ";
            if (ofDialog.ShowDialog() == DialogResult.OK) 
            {
                foreach (SaveFiles files in SaveFile_list)
                {
                    if (files.ppt_filename == ofDialog.FileName)
                    {
                        DialogResult result = MessageBox.Show("そのスライドは既にスライド画像・音声・表ができています\n削除して新しく作成しますか？", "", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            not_FileSelect = true;
                            File.Delete(Directory.GetCurrentDirectory() + "\\csv\\" + file_name + ".tsv");
                            Directory.Delete(Directory.GetCurrentDirectory() + "\\slide_image\\" + file_name, true);
                            Directory.Delete(Directory.GetCurrentDirectory() + "\\voice\\" + file_name, true);
                            List<string> lines = new List<string>();
                            foreach (SaveFiles j in SaveFile_list)
                            {
                                if (ofDialog.FileName != j.ppt_filename) lines.Add(j.random_str + "\t" + j.ppt_filename);
                            }
                            File.WriteAllLines(".\\savefiles.tsv", lines, Encoding.UTF8);
                            using (StreamReader sr = new StreamReader(".\\savefiles.tsv"))
                            {
                                while (0 <= sr.Peek())
                                {
                                    //カンマ区切りで分割して配列で格納する
                                    var line = sr.ReadLine().Split('\t');
                                    if (line is null) continue;
                                    else if (line.Count() < 2) break;
                                    //リストにデータを追加する
                                    SaveFiles s_d = new SaveFiles { random_str = line[0], ppt_filename = line[1] };
                                    SaveFile_list.Add(s_d);
                                }
                                OpenFileBox.Items.Remove(ofDialog.FileName);
                            }
                            break;
                        }
                        else if (result == DialogResult.No) return;
                    }
                }
                OpenFileBox.Text = ofDialog.FileName;
            }
            else { return; }
            ofDialog.Dispose();
            if (!File.Exists(OpenFileBox.Text))
            {
                MessageBox.Show("ファイルを指定していないか存在しないファイルです", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // スライド画像・ノートの読み込み
            var ppt_file = new ppt.Application().Presentations.Open(OpenFileBox.Text,
                    MsoTriState.msoTrue,
                    MsoTriState.msoTrue,
                    MsoTriState.msoFalse);
            string txt;
            int num = 0;
            notes = new List<Cue_card>();
            string mode = ModeCombo.Text;
            for (int i = 1; i <= ppt_file.Slides.Count; i++) //スライドのインデックスは１から
            {
                txt = ppt_file.Slides[i].NotesPage.Shapes.Placeholders[2].TextFrame.TextRange.Text;
                List<string> temp_lines = txt.Split('\r').ToList();
                foreach (string line in temp_lines) //ノートの分割
                {
                    char[] splitStr = line.ToCharArray();
                    List<string> words = new List<string>();
                    List<string> square_txts = new List<string>();
                    string t;
                    foreach (var word in splitStr)
                    {
                        int note_count = notes.Count();
                        words.Add(word.ToString());
                        if (new Regex(@"(。|？|\?|！|\!|（|「|\(|）|」|\))").IsMatch(word.ToString())) //これら記号は分割記号となる
                        {
                            t = String.Join("", words.ToArray());
                            if (is_bracket)
                            {
                                bracket_sentence = bracket_sentence + t;
                                if (new Regex(@"(）|\))").IsMatch(word.ToString()) && note_count > 0)
                                {
                                    // 字幕文の生成
                                    if (mode == "normal") notes[note_count - 1].Bracket = bracket_sentence[..^1];
                                    else
                                    {
                                        // アクセシビリティ優先の場合は（）内文章は音声文・音声字幕文に含める
                                        notes[note_count - 1].Sentence += ("（" + bracket_sentence[..^1] + "）");
                                        notes[note_count - 1].Voice = notes[notes.Count() - 1].Sentence;
                                        notes[note_count - 1].Pnt = "）";
                                    }
                                    bracket_sentence = "";
                                    is_bracket = false;
                                }
                            }
                            else if (!new Regex("^[ -/:-@[-´{-~]+$").IsMatch(t) && !new Regex("^[！”＃＄％＆’（）＝～｜‘｛＋＊｝＜＞？＿－＾￥＠「；：」、。・]*$").IsMatch(t))
                            {
                                // 音声文の生成
                                if (note_count > 0 && new Regex(@"「").IsMatch(notes[note_count - 1].Sentence) && new Regex(@"」").IsMatch(t))
                                {
                                    // 「」内にある文字は前の文章に付随する
                                    notes[note_count - 1].Sentence += t;
                                    notes[note_count - 1].Voice = notes[note_count - 1].Sentence;
                                }
                                else if (note_count > 0 && ModeCombo.Text == "visual" && new Regex(@"(）|\))").IsMatch(notes[note_count - 1].Pnt))
                                {
                                    // アクセシビリティ優先の場合、()内文字は前の文章に付随する
                                    notes[note_count - 1].Sentence += t;
                                    notes[note_count - 1].Voice = notes[note_count - 1].Sentence;
                                    notes[note_count - 1].Pnt = words[words.Count - 1];
                                }
                                else
                                {
                                    if (new Regex(@"(（|「|\(|「)").IsMatch(t)) t = t[..^1]; //（や「は先頭文字からなくす。
                                    if (note_count > 0 && notes[note_count - 1].Pnt == "「") t = "「" + t;

                                    num += 1;
                                    int motion_ID = Motion_ID(words.Count);
                                    Cue_card m = new Cue_card { No = num, Num = i, Id = motion_ID, Pnt = words[words.Count - 1], Sentence = t, Voice = t, Bracket = "", Size = words.Count };
                                    notes.Add(m);
                                }

                            }
                            else if (ModeCombo.Text == "visual" && new Regex(@"(。|？|\?|！|\!)").IsMatch(t) && note_count > 0)
                            {
                                notes[note_count - 1].Sentence += t;
                                notes[note_count - 1].Voice = notes[note_count - 1].Sentence;
                            }

                            if (new Regex(@"(（|\()").IsMatch(word.ToString())) is_bracket = true;
                            words.Clear();
                        }
                    }
                    t = String.Join("", words.ToArray());
                    if (words.Count() > 0 && t != " ")
                    {
                        int motion_ID = Motion_ID(words.Count);
                        num = num + 1;
                        Cue_card m = new Cue_card { No = num, Num = i, Id = motion_ID, Pnt = "。", Sentence = String.Join("", words.ToArray()), Voice = String.Join("", words.ToArray()), Bracket = "", Size = words.Count };
                        notes.Add(m);
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
            if (!File.Exists(OpenFileBox.Text))
            {
                MessageBox.Show("既に存在しないファイルです\n移動した場合は新たにファイル場所を設定しなおす必要があります", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                List<string> lines = new List<string>();
                foreach (SaveFiles j in SaveFile_list)
                {
                    if (ofDialog.FileName != j.ppt_filename) lines.Add(j.random_str + "\t" + j.ppt_filename);
                    else lines.Add(j.random_str + "\t" + ofDialog.FileName);
                }
                File.WriteAllLines(".\\savefiles.tsv", lines, Encoding.UTF8);
                using (StreamReader sr = new StreamReader(".\\savefiles.tsv"))
                {
                    while (0 <= sr.Peek())
                    {
                        //カンマ区切りで分割して配列で格納する
                        var line = sr.ReadLine().Split('\t');
                        if (line is null) continue;
                        else if (line.Count() < 2) break;
                        //リストにデータを追加する
                        SaveFiles s_d = new SaveFiles { random_str = line[0], ppt_filename = line[1] };
                        SaveFile_list.Add(s_d);
                    }
                    OpenFileBox.Items.Remove(ofDialog.FileName);
                    //OpenFileBox.Items.Clear();
                    //foreach (SaveFiles j in SaveFile_list) OpenFileBox.Items.Add(j.ppt_filename);
                }
            }
            notes = new List<Cue_card>();

            foreach (SaveFiles files in SaveFile_list)
            {
                if (files.ppt_filename == OpenFileBox.Text)
                {
                    using (StreamReader sr = new StreamReader(".\\csv\\" + files.random_str + ".tsv"))
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
                    file_name = files.random_str;
                    break;
                }
            }

            Generate_Grid();
            new_file = false;
            SaveButton.Enabled = true;
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (ModeCombo.Text == "" || VoiceNameCombo.Text == "")
            {
                MessageBox.Show("音源名とアクセシビリティの設定をしていません", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            progressBar1.Value = 0;

            // リスト末尾にVOICEVOXの音源名と締めのあいさつを入れる。音源名の発声はライセンス対策 //
            if (notes[notes.Count - 1].Sentence != "ここまでのご視聴、ありがとうございました。")
            {
                List<Cue_card> last_list = new List<Cue_card>();
                Cue_card m_last_1 = new Cue_card { No = notes[notes.Count - 1].No + 1, Num = notes[notes.Count - 1].Num, Id = 21, Pnt = "。", Sentence = String.Format("この動画は、voicevox、{0}の音声でお送り致しました。", VoiceNameCombo.Text), Bracket = String.Format("VOICEVOX: {0}", VoiceNameCombo.Text), Voice = String.Format("この動画は、voicevox、{0}の音声でお送り致しました。", VoiceNameCombo.Text), Size = 10 };
                last_list.Add(m_last_1);
                notes.Add(m_last_1);
                Cue_card m_last_2 = new Cue_card { No = m_last_1.No + 1, Num = m_last_1.Num, Id = 0, Pnt = "。", Sentence = String.Format("ここまでのご視聴、ありがとうございました。", VoiceNameCombo.Text), Bracket = "", Voice = "ここまでのご視聴、ありがとうございました。", Size = 10 };
                last_list.Add(m_last_2);
                notes.Add(m_last_2);
                foreach (var note in last_list) table.Rows.Add(note.No, note.Num, note.Id, note.Pnt, note.Sentence, note.Voice, note.Bracket);
            }
            // スライド画像を生成する //
            if (new_file) file_name = GeneratePassword(10); //ファイルは適当な文字列。unityで音声や画像を読み込む際、日本語を含んだ文字列は認識できないため
            var ppt_file = new ppt.Application().Presentations.Open(OpenFileBox.Text,
                    MsoTriState.msoTrue,
                    MsoTriState.msoTrue,
                    MsoTriState.msoFalse);
            int width = (int)ppt_file.PageSetup.SlideWidth;
            int height = (int)ppt_file.PageSetup.SlideHeight;
            string file2;
            string dic_image = Directory.GetCurrentDirectory() + "\\slide_image\\" + file_name;
            if (!Directory.Exists(dic_image))
            {
                DirectoryInfo di = new DirectoryInfo(dic_image);  //スライド画像保存フォルダを生成
                di.Create();
            }
            for (int i = 1; i <= ppt_file.Slides.Count; i++)
            {
                file2 = dic_image + String.Format("\\slide{0:0}.jpg", i); //JPEGとして保存
                ppt_file.Slides[i].Export(file2, "jpg", width, height);
            }
            if (ppt_file != null)
            {
                ppt_file.Close(); //ファイルを閉じる
                ppt_file = null;
            }

            dic_voice = Directory.GetCurrentDirectory() + @"\voice\" + file_name;
            if (!Directory.Exists(dic_voice))
            {
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
            List<Cue_card> notes = new List<Cue_card>(); //ResultGridからデータを取得
            foreach (DataGridViewRow row in ResultGrid.Rows)
            {
                if (!row.IsNewRow)
                {
                    Cue_card m = new Cue_card {
                        No = Convert.ToInt32(row.Cells["番号"].Value), // "No"列のデータを整数型に変換
                        Num = Convert.ToInt32(row.Cells["ページ"].Value), // "Num"列のデータを整数型に変換
                        Id = Convert.ToInt32(row.Cells["動作ID"].Value), // "Id"列のデータを整数型に変換
                        Pnt = row.Cells["記号"].Value.ToString(), // "Pnt"列のデータを文字列に変換
                        Sentence = row.Cells["音声の文章"].Value.ToString(), // "Sentence"列のデータを文字列に変換
                        Bracket = row.Cells["補足説明"].Value.ToString(), // "Bracket"列のデータを文字列に変換
                        Voice = row.Cells["字幕用の文章"].Value.ToString(), // "Voice"列のデータを文字列に変換
                        Size = row.Cells["音声の文章"].Value.ToString().Length
                    };
                    notes.Add(m);
                }
            }

            int before_num = 1;
            foreach (var note in notes)
            {
                if (BackVOICEVOX.CancellationPending) return;
                waveFile = String.Format(@"\{0}.wav", note.No);
                if (new Regex(@"(。|？|\?|！|\!)").IsMatch(note.Pnt)) postPhonemeLength = 0.5f;
                if (note.Num != before_num) prePhonemeLength = 0.5f;

                // 音声生成が完了するまで録音は開始できない //
                VoicevoxUtility.RecordSpeech(Form1.dic_voice + waveFile, note.Sentence, Form1.voice_name).Wait();
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
            File.Delete(Directory.GetCurrentDirectory() + "\\csv\\" + file_name + ".tsv");
            Directory.Delete(Directory.GetCurrentDirectory() + "\\slide_image\\" + file_name, true);
            Directory.Delete(Directory.GetCurrentDirectory() + "\\voice\\" + file_name, true);
            
            CancelButton.Enabled = false;
            is_cancel = true;
            BackVOICEVOX.CancelAsync();

            progressBar1.Value = 0;
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
                //CSV出力用変数の作成
                List<string> lines = new List<string>();

                //列名をカンマ区切りで1行に連結
                List<string> header = new List<string>();
                foreach (DataColumn dr in table.Columns) header.Add(dr.ColumnName);
                lines.Add(string.Join("\t", header));

                //列の値をカンマ区切りで1行に連結
                foreach (DataRow dr in table.Rows) lines.Add(string.Join("\t", dr.ItemArray));
                string dic_csv = Directory.GetCurrentDirectory() + "\\csv";
                if (!Directory.Exists(dic_csv))
                {
                    DirectoryInfo di = new DirectoryInfo(dic_csv);
                    di.Create();
                }
                File.WriteAllLines(dic_csv + "\\" + file_name + @".tsv", lines, Encoding.UTF8);

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
                if (new_file)
                {
                    SaveFiles add_file = new SaveFiles { random_str = file_name, ppt_filename = OpenFileBox.Text };
                    SaveFile_list.Add(add_file);
                    lines = new List<string>();
                    foreach (SaveFiles files in SaveFile_list) lines.Add(files.random_str + "\t" + files.ppt_filename);
                    File.WriteAllLines(".\\savefiles.tsv", lines, Encoding.UTF8);
                    OpenFileBox.Items.Add(add_file.ppt_filename);
                }
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
            SaveButton.Enabled = true;
            GenerateButton.Enabled = true;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            
            var ppt_file = new ppt.Application().Presentations.Open(OpenFileBox.Text,
                MsoTriState.msoTrue,
                MsoTriState.msoTrue,
                MsoTriState.msoFalse);
            int width = (int)ppt_file.PageSetup.SlideWidth;
            int height = (int)ppt_file.PageSetup.SlideHeight;
            string file2;
            string dic_image = Directory.GetCurrentDirectory() + "\\slide_image\\" + file_name;
            for (int i = 1; i <= ppt_file.Slides.Count; i++)
            {
                file2 = dic_image + String.Format("\\slide{0:0}.jpg", i); //JPEGとして保存
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
            foreach (DataRow dr in table.Rows) { 
                lines.Add(string.Join("\t", dr.ItemArray)); 
            }
            string dic_csv = Directory.GetCurrentDirectory() + "\\csv";
            if (!Directory.Exists(dic_csv))
            {
                DirectoryInfo di = new DirectoryInfo(dic_csv);
                di.Create();
            }
            File.WriteAllLines(dic_csv + "\\" + file_name + @".tsv", lines, Encoding.UTF8);

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
            if (new_file)
            {
                SaveFiles add_file = new SaveFiles { random_str = file_name, ppt_filename = OpenFileBox.Text };
                SaveFile_list.Add(add_file);
                lines = new List<string>();
                foreach (SaveFiles files in SaveFile_list) lines.Add(files.random_str + "\t" + files.ppt_filename);
                File.WriteAllLines(".\\savefiles.tsv", lines, Encoding.UTF8);
                OpenFileBox.Items.Add(add_file.ppt_filename);
            }
            AvatorButton.Enabled = true;
            GenerateButton.Enabled = true;
        }
    }
}
