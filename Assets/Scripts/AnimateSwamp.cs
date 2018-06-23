using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateSwamp : MonoBehaviour {

    Material mat;

	// Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        float t = Time.time / 4f;
        mat.SetTextureOffset("_MainTex", new Vector2(Mathf.Cos(t)/2f, Mathf.Sin(t * 0.6f)/2f));
	}
}
