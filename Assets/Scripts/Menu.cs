using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public Text timesText;
    float time;

	// Use this for initialization
	void Start () {
        time = 0;
        float prevTime = TimesTracker.timesTracker.timePrev;
        float bestTime = TimesTracker.timesTracker.timeBest;
        int prevminutes = Mathf.FloorToInt(prevTime / 60f);
        int prevseconds = Mathf.FloorToInt(prevTime % 60);
        int prevsplits = Mathf.FloorToInt((prevTime - Mathf.FloorToInt(prevTime)) * 1000f);
        int bestminutes = Mathf.FloorToInt(prevTime / 60f);
        int bestseconds = Mathf.FloorToInt(prevTime % 60);
        int bestsplits = Mathf.FloorToInt((prevTime - Mathf.FloorToInt(prevTime)) * 1000f);
        string timeString = string.Format("Previous: {0:#00}:{1:#00}:{2:#000}\n\nrecord: {0:#00}:{1:#00}:{2:#000}", prevminutes, prevseconds, prevsplits, bestminutes, bestseconds, bestminutes);
        timesText.text = timeString;
    }

    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;

        if ( time > 0.5f && (Input.GetButton("LeftHand") || Input.GetAxis("LeftHandAnalogue") > 0.5f))
        {
            SceneManager.LoadScene("Gameplay");
        }
	}
}
