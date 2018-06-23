using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkull : MonoBehaviour {

    public float spawnHeight = 5f;

    public SkullMode skullMode;
    public int health;
    public float time;
    float speed;
    float targetHeight;
    Vector3 skullDir; //Direction to move
    Rigidbody myRigidbody;

	// Use this for initialization
	void Start () {
        GetComponent<SphereCollider>().enabled = false;

        health = 10;
        time = 0;
        myRigidbody = GetComponent<Rigidbody>();
        Spawn();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        time += Time.fixedDeltaTime;
        Vector3 futurePosition = myRigidbody.position;

        switch(skullMode)
        {
            case SkullMode.spawning:
                speed = 1;
                if (time < 1)
                    targetHeight = spawnHeight;
                else if (time > 2)
                {
                    GetComponent<SphereCollider>().enabled = true;
                    Seek();
                }
                
                break;

            case SkullMode.seeking:
                if (Random.value < 0.015f) //reseek?
                    Seek();
                break;
        }

        futurePosition = myRigidbody.position + skullDir * Time.fixedDeltaTime * speed;
        futurePosition.y = Mathf.Lerp(myRigidbody.position.y, targetHeight + Mathf.Sin(time*10f) * 0.3f, Time.fixedDeltaTime);
        myRigidbody.MovePosition(futurePosition);
    }

    public void Seek() //Start seeking behaviour
    {

        speed = Random.Range(2.5f, 3f);
        skullMode = SkullMode.seeking;
        targetHeight = 0.75f + Random.Range(0, 0.5f);
        Vector3 playerDir = PlayerControl.playerPosition - myRigidbody.position;
        playerDir.Normalize();
        skullDir = Vector3.Slerp(skullDir, playerDir, Random.Range(0.25f, 0.4f));

    }

    public void Spawn() //Call when spawned to start spawning behaviour
    {

        float h = Random.Range(0, 360f);
        skullDir = new Vector3(Mathf.Sin(h), 0, Mathf.Cos(h));

    }
}

public enum SkullMode
{
    spawning,
    seeking
}
