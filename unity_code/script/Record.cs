using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ.FrameCapturer;

public class Record : MonoBehaviour
{
    public GameObject Camera;
    [SerializeField]
    public MovieRecorder m_movieRecorder;
    public bool record_start, record_rock, slide_lock, is_touch, slide_finish;
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
        if (Input.GetMouseButtonDown(0)){
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
