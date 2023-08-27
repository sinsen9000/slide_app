using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;

public class image_move : MonoBehaviour
{
    GameObject Model, _text, _caption;

    WavPlay WavObj;
    Animator _anim;
    Text TextObj, CapObj;
    public Record RecObj;
    public Image image;
    private Coroutine _someCoroutine, _CapyionCoroutine, _AnimationCoroutine;
    public static List<Cue_card> notes = new List<Cue_card>();
    public static JsonData json_data;
    public static int now_slide_num;
    public static string file_path;
    private bool caption_bool;

    public class Cue_card{
        public int No { get; set; } //文章番号
        public int Num { get; set; } //スライドページ
        public int Id { get; set; } //動作ID
        public string Pnt { get; set; } //分割記号（何で分割したか？）
        public string Sentence { get; set; } //文章内容
        public string Bracket { get; set; } //字幕
    }

    [System.Serializable] //定義したクラスをJSONデータに変換できるようにする
    public class JsonData{
        public string visualMode;
        public string fileName;
        public string voiceName;
        public float voiceSpeed;
        public float voiceInterval;
        public float voiceIntonation;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        RecObj.slide_lock = false;
        file_path = Application.dataPath+"/../../";

        Model = GameObject.Find("Ailis");
        WavObj = Model.GetComponent<WavPlay>();
        _anim = Model.GetComponent<Animator>();
        image = this.GetComponent<Image>();
        _text = GameObject.Find("Text");
        TextObj = _text.GetComponent<Text>();
        TextObj.text = "";
        _caption = GameObject.Find("Caption");
        CapObj = _caption.GetComponent<Text>();
        CapObj.text = "";
        caption_bool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (RecObj.is_touch){
            StopCoroutine(_someCoroutine);
            WavObj.audio_source[0].Stop();
            TextObj.text = "";
            image.sprite = null;

            RecObj.slide_lock = false;
            RecObj.is_touch = false;
        }
        else if (RecObj.record_start){
            var isFirstLineSkip = true;
            using (StreamReader reader = new StreamReader(file_path+"Setting.json")){ //受け取ったパスのファイルを読み込む
                string datastr = reader.ReadToEnd();//ファイルの中身をすべて読み込む
                json_data = JsonUtility.FromJson<JsonData>(datastr);
            }
            using (StreamReader sr = new StreamReader(file_path+"csv/"+json_data.fileName+".tsv")){
                while (0 <= sr.Peek()){
                    //カンマ区切りで分割して配列で格納する
                    var line = sr.ReadLine().Split('\t');
                    if (line is null) continue;
                    if (isFirstLineSkip)
                    {
                        isFirstLineSkip = false;
                        continue;
                    }
                    //リストにデータを追加する
                    Cue_card m = new Cue_card{No=int.Parse(line[0]),Num=int.Parse(line[1]),Id=int.Parse(line[2]),Pnt=line[3],Sentence=line[4],Bracket=line[5]};
                    notes.Add(m);
                }
            }
            now_slide_num = 0;
            _someCoroutine = StartCoroutine(Slide());
            RecObj.record_start = false;
        }
    }
    IEnumerator Slide(){
        RecObj.slide_lock = true;
        // スライド変更処理（メイン処理）
        foreach(Cue_card line in notes){
            if(now_slide_num != line.Num){
                // 元のスライド番号とcsvファイルのスライド番号が違う時スライド変更
                var image_www = new WWW("file:///" + file_path.Replace("\\","/") + "slide_image/" + json_data.fileName + "/slide" + line.Num.ToString() + ".jpg");
                while (!image_www.isDone) {
                    Debug.Log("Now loading......"); //ロード完了まで待機
                }
                var tex = image_www.texture;
                image.sprite = Sprite.Create(tex, new Rect(0f,0f, tex.width, tex.height), new Vector2());
            }
            // 音声再生処理
            if (line.Bracket != ""){
                if (caption_bool) {
                    StopCoroutine(_CapyionCoroutine);
                    CapObj.text = "";
                }
                _CapyionCoroutine = StartCoroutine(ShowCaption(line.Bracket));
            }
            if (json_data.visualMode == "visual") TextObj.text = line.Sentence;
            var wav_www = new WWW("file:///" + file_path.Replace("\\","/") + "voice/" + json_data.fileName + "/" + line.No.ToString() + ".wav");
            while (!wav_www.isDone) {
                Debug.Log("Now loading......"); //ロード完了まで待機
            }
            WavObj.audio_source[0].clip = wav_www.GetAudioClip(true, true);
            WavObj.audio_source[0].Play();
            if (line.Id != 0){
                _AnimationCoroutine = StartCoroutine(AnimationMove(line.Id));
            }
            yield return new WaitWhile(() => WavObj.audio_source[0].isPlaying);
            now_slide_num = line.Num;
        }
        TextObj.text = "";
        CapObj.text = "";
        yield return new WaitForSeconds(1f);
        RecObj.slide_lock = false;
        RecObj.slide_finish = true;
    }
    
    IEnumerator ShowCaption(string caption_text){
        CapObj.text = caption_text;
        yield return new WaitForSeconds(5f);
        CapObj.text = "";
        caption_bool = false;
    }

    IEnumerator AnimationMove(int motion_id){
        _anim.SetInteger("ActionNo", motion_id);
        yield return new WaitForSeconds(0.1f);
        _anim.SetInteger("ActionNo", 0);
    }
}
