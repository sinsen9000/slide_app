using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ.FrameCapturer;
using System.IO;

public class Record : MonoBehaviour
{
    public GameObject Camera;
    [SerializeField]
    public MovieRecorder m_movieRecorder;
    public bool record_start, record_rock, slide_lock, is_touch, slide_finish;
    private JsonData json_data;

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
        //m_movieRecorder = Camera.GetComponent<MovieRecorder>();
        record_start = false;
        record_rock = false;
        is_touch = false;
        slide_finish = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 画面タッチで録画開始 //
        using (StreamReader reader = new StreamReader(Application.dataPath+"/../../Setting.json")){ //受け取ったパスのファイルを読み込む
                string datastr = reader.ReadToEnd();//ファイルの中身をすべて読み込む
                json_data = JsonUtility.FromJson<JsonData>(datastr);
            }
        if (Input.GetMouseButtonDown(0) && File.Exists(Application.dataPath+"/../../"+"csv/"+json_data.fileName+".tsv")){
            if (!m_movieRecorder.isRecording && !record_rock){
                m_movieRecorder.BeginRecording();
                record_start = true;
                record_rock = true;
            }
            else if (m_movieRecorder.isRecording && slide_lock && record_rock){
                // 録画停止
                m_movieRecorder.EndRecording();
                is_touch = true;
                record_rock = false;
            }
        }
        else if (slide_finish){
            m_movieRecorder.EndRecording();
            record_rock = false;
            slide_finish = false;
        }
    }
}
