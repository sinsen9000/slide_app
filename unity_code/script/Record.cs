using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ.FrameCapturer;
using System.IO;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class Record : MonoBehaviour
{
    GameObject Model, Camera;
    WavPlay WavObj;
    private AudioSource GuiSE;

    [SerializeField]
    public MovieRecorder m_movieRecorder;
    public bool record_start, record_rock, slide_lock, is_touch, slide_finish, aspect_do;
    private JsonData json_data;
    public aspect AspectObj;
    public DialogShow GuiObj;

    [System.Serializable] //定義したクラスをJSONデータに変換できるようにする
    private class JsonData{
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
        GuiSE = GetComponent<AudioSource>();
        Model = GameObject.Find("Ailis");
        WavObj = Model.GetComponent<WavPlay>();
        //m_movieRecorder = Camera.GetComponent<MovieRecorder>();
        record_start = false;
        record_rock = false;
        is_touch = false;
        slide_finish = false;
        AspectObj.aspect_do = true;
        GuiObj.is_finish=false;
        using (StreamReader reader = new StreamReader(Application.dataPath+"/../../Setting.json")){ //受け取ったパスのファイルを読み込む
                string datastr = reader.ReadToEnd();//ファイルの中身をすべて読み込む
                json_data = JsonUtility.FromJson<JsonData>(datastr);
            }
    }

    // Update is called once per frame
    void Update()
    {
        if(GuiObj.is_finish){ return; }
        if (!record_rock && !slide_finish){
            using (StreamReader reader = new StreamReader(Application.dataPath+"/../../Setting.json")){ //受け取ったパスのファイルを読み込む
                string datastr = reader.ReadToEnd();//ファイルの中身をすべて読み込む
                json_data = JsonUtility.FromJson<JsonData>(datastr);
            }
        }

        // 画面タッチで録画開始 //
        if (Input.GetMouseButtonDown(0) && File.Exists(Application.dataPath+"/../../"+"csv/"+json_data.fileName+".tsv")){
            if (!m_movieRecorder.isRecording && !record_rock){
                m_movieRecorder.BeginRecording();
                record_start = true;
                record_rock = true;
                AspectObj.aspect_do=false;
            }
            else if (m_movieRecorder.isRecording && slide_lock && record_rock){
                // 録画停止
                m_movieRecorder.EndRecording();
                is_touch = true;
                record_rock = false;
                AspectObj.aspect_do = true;
                GuiSE.Play();
                GuiObj.Open();
                GuiObj.is_finish=true;
            }
        }
        else if (slide_finish){
            m_movieRecorder.EndRecording();
            record_rock = false;
            slide_finish = false;
            AspectObj.aspect_do = true;
            GuiSE.Play();
            GuiObj.Open();
            GuiObj.is_finish=true;
        }
    }
}
