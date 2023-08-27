using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBlink : MonoBehaviour
{
    public GameObject face;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    private bool isPlus = true;
    private bool DelayTime = false;
    private float BrinkWeight = 0f;
    private int r_Eye, l_Eye;

    private void Brink(){
        skinnedMeshRenderer.SetBlendShapeWeight(r_Eye, BrinkWeight);
        skinnedMeshRenderer.SetBlendShapeWeight(l_Eye, BrinkWeight);
    }

    // コルーチン本体
    private IEnumerator DelayCoroutine()
    {
        float random_time = Random.Range(0.5f,5.0f);
        yield return new WaitForSeconds(random_time);
        isPlus = true;
        DelayTime = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        skinnedMeshRenderer = face.GetComponent<SkinnedMeshRenderer>();
        r_Eye = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("Facial_BS.eye_closeR");
        l_Eye = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("Facial_BS.eye_closeL");
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlus) BrinkWeight += Time.deltaTime * 750f;
        else       BrinkWeight -= Time.deltaTime * 750f;
        Brink();
        
        if (BrinkWeight < 0){
            BrinkWeight = 0;
            Brink();
            if (!DelayTime){
                DelayTime = true;
                StartCoroutine(DelayCoroutine());
            }
        }
        else if(BrinkWeight > 100){
            BrinkWeight = 100;
            Brink();
            isPlus = false;
        }
        
    }
}
