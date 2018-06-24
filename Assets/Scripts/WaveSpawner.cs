using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class WaveSpawner : MonoBehaviour {

    [SerializeField]
    public List<WaveEvent> waveEvents;
    public float time;
    public LayerMask towerRaycastMask;
    public GameObject prototypeSeed;
    public GameObject protoLightningChain;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        while( waveEvents.Count > 0 && time >= waveEvents[0].time )
        {
            WaveEvent e = waveEvents[0];
            waveEvents.RemoveAt(0);

            switch( e.eventType)
            {
                case WaveEventType.seed:
                    GameObject seedObject = Instantiate(prototypeSeed);
                    seedObject.transform.position = e.position;                  
                    break;
                case WaveEventType.towerAttack:
                    Vector3 direction = PlayerControl.playerPosition - GetTowerPosition(e.tower);
                    RaycastHit hitInfo;
                    Debug.DrawRay(GetTowerPosition(e.tower), direction);
                    if (Physics.Raycast(GetTowerPosition(e.tower), direction, out hitInfo, 200f, towerRaycastMask.value))
                    {
                        PlayerControl player = hitInfo.collider.gameObject.GetComponent<PlayerControl>();

                        Vector3 start = GetTowerPosition(e.tower);
                        Vector3 end = hitInfo.point;
                        if ( player != null)
                        {
                            end += Vector3.down * 0.6f;
                            player.Zapped();
                        } else
                        {
                            Debug.Log("Zapped " + hitInfo.collider.gameObject);
                        }
                        GameObject lightningChainObject = Instantiate(protoLightningChain);
                        LightningChain chain = lightningChainObject.GetComponent<LightningChain>();
                        chain.MakeChain(start, end, 1f);
                    }
                    break;
                case WaveEventType.towerWarmup:
                    GameObject lightningChainObjectW = Instantiate(protoLightningChain);
                    LightningChain chainW = lightningChainObjectW.GetComponent<LightningChain>();
                    chainW.MakeChain(GetTowerPosition(e.tower)-Vector3.up* 3f, GetTowerPosition(e.tower)+Vector3.up*15f, 10f);
                    break;
            }

            return;
        }
	}

    Vector3 GetTowerPosition(Tower tower)
    {
        switch(tower)
        {
            default: //NorthEast
                return new Vector3(25, 18, 25);
            case Tower.SouthEast:
                return new Vector3(25, 18, -25);
            case Tower.SouthWest:
                return new Vector3(-25, 18, -25);
            case Tower.NorthWest:
                return new Vector3(-25, 18, 25);
            
        }
    }
}

[Serializable]
public struct WaveEvent
{
    public Vector3 position;
    public float time;
    public WaveEventType eventType;
    public Tower tower;
}

public enum Tower
{
    NorthEast,
    SouthEast,
    SouthWest,
    NorthWest
}
public enum WaveEventType
{
    seed,
    towerAttack,
    towerWarmup
}