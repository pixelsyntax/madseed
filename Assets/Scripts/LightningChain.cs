using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningChain : MonoBehaviour {

    public GameObject prototypeLightningSegment;
    List<GameObject> segments;
    float duration;

    public void MakeChain(Vector3 start, Vector3 end, float duration)
    {
        segments = new List<GameObject>();
        Vector3 cursor = start;
        Vector3 unit = (end - start);
        unit.Normalize();
        float length = 0;
        while (length < Vector3.Distance(start, end))
        {
            GameObject segment = Instantiate(prototypeLightningSegment);
            segment.transform.position = cursor;
            segment.transform.LookAt(end);
            segments.Add(segment);
            length += 1;
            cursor += unit;
            
        }
        this.duration = duration;
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        duration -= Time.deltaTime;
        if ( duration <= 0)
        {
            while( segments.Count > 0)
            {
                GameObject segment = segments[0];
                segments.RemoveAt(0);
                Destroy(segment);
            }
            Destroy(gameObject);
        }
        
	}
}
