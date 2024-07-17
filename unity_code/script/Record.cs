using UnityEngine;
using UTJ.FrameCapturer;
using System.IO;

public class Record : MonoBehaviour
{
    GameObject Model, Camera;
    WavPlay WavObj;
    private AudioSource GuiSE;

    [SerializeField]
    public MovieRecorder m_movieRecorder;
    public bool record_start, record_rock, slide_lock, is_touch, slide_finish, aspect_do;
    private ProjectJson project_data;
    private JsonData json_data;
    public aspect AspectObj;
    public DialogShow GuiObj;
    public static string file_path;

    [System.Serializable] //定義したクラスをJSONデータに変換できるようにする
    private class ProjectJson{
        public string videoAudio;
        public string hosokuAudio;
        public string videoCaption;
        public string captionFont;
        public string motion;
        public string fileName;
        public string voiceName;
        public float voiceSpeed;
        public float voiceInterval;
        public float voiceIntonation;
    }

    [System.Serializable] //定義したクラスをJSONデータに変換できるようにする
    private class JsonData{
        public string targetPath;
    }

    // Start is called before the first frame update
    void Start()
    {
        GuiSE = GetComponent<AudioSource>();
        Model = GameObject.Find("yuina_tpo");
        WavObj = Model.GetComponent<WavPlay>();
        //m_movieRecorder = Camera.GetComponent<MovieRecorder>();

        using (StreamReader reader = new StreamReader(Application.dataPath+"/../../Setting.json")){ //受け取ったパスのファイルを読み込む
                string datastr = reader.ReadToEnd();//ファイルの中身をすべて読み込む
                json_data = JsonUtility.FromJson<JsonData>(datastr);
            }
        file_path = Application.dataPath+"/../../projects/"+json_data.targetPath;

        record_start = false;
        record_rock = false;
        is_touch = false;
        slide_finish = false;
        AspectObj.aspect_do = true;
        GuiObj.is_finish=false;
        using (StreamReader reader = new StreamReader(file_path+"/Project.json")){ //受け取ったパスのファイルを読み込む
                string datastr = reader.ReadToEnd();//ファイルの中身をすべて読み込む
                project_data = JsonUtility.FromJson<ProjectJson>(datastr);
            }
    }

    // Update is called once per frame
    void Update()
    {
        if(GuiObj.is_finish){ return; }
        if (!record_rock && !slide_finish){
            using (StreamReader reader = new StreamReader(file_path+"/Project.json")){ //受け取ったパスのファイルを読み込む
                string datastr = reader.ReadToEnd();//ファイルの中身をすべて読み込む
                project_data = JsonUtility.FromJson<ProjectJson>(datastr);
            }
        }

        // 画面タッチで録画開始 //
        if (Input.GetMouseButtonDown(0) && File.Exists($"{file_path}/use_video.tsv")){
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
