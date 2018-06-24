using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningQuad : MonoBehaviour {

    public Material[] materials;
    public float life;
    new Renderer renderer;
	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        life -= Time.deltaTime;
        if (life <= 0)
            Destroy(gameObject);

        renderer.material = materials[Random.Range(0, materials.Length)];
	}
}
