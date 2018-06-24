using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimesTracker : MonoBehaviour {

    public static TimesTracker timesTracker;
    public float timeBest;
    public float timePrev;

    private void Awake()
    {
        if (timesTracker != null)
        {
            Destroy(this);
            return;
        }
        timesTracker = this;
        LoadData();   
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadData()
    {
        if ( PlayerPrefs.HasKey("timeBest") )
            timeBest = PlayerPrefs.GetFloat("timeBest");
        if ( PlayerPrefs.HasKey("timePrev") )
            timePrev = PlayerPrefs.GetFloat("timePrev");
           
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("timeBest", timeBest);
        PlayerPrefs.SetFloat("timePrev", timePrev);
    }

    public void SetPrevTime( float time )
    {
        timePrev = time;
        if (time > timeBest)
            timeBest = time;
        SaveData();
        
    }

}
