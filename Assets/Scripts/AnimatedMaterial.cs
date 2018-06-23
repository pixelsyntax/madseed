using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMaterial : MonoBehaviour {

    public Texture2D[] texture2Ds;
    public float frameDuration;
    public Material mat;
    float time;
    int textureIndex;

	// Use this for initialization
	void Start () {
        if (frameDuration == 0)
            frameDuration = 0.5f;
        
	}
	
	// Update is called once per frame
	void Update () {

        time += Time.deltaTime;
        while(time > frameDuration)
        {
            time -= frameDuration;
            textureIndex = (textureIndex + 1) % texture2Ds.Length;
            mat.SetTexture("_MainTex", texture2Ds[textureIndex]);
        }

	}
}
