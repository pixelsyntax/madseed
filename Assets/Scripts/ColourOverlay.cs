using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ColourOverlay : MonoBehaviour {

    public Color colourEnabled;
    public Color colourDisabled;
    Image image;
    float t = 1;
    float duration = 1;

    private void Awake()
    {
        image = GetComponent<Image>();

    }

    private void Start()
    {

    }
    // Update is called once per frame
    void Update () {
        t = Mathf.Min(t + Time.deltaTime / duration, 1f);
        image.color = Color.Lerp(colourEnabled, colourDisabled, t);
	}

    public void Go(float duration)
    {

        this.duration = duration;
        t = 0;
    }
}
