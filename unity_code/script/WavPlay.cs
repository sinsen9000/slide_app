using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavPlay : MonoBehaviour
{
    public AudioClip audio_target;
    public AudioSource[] audio_source;

    // Start is called before the first frame update
    void Start()
    {
        audio_source = gameObject.GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
