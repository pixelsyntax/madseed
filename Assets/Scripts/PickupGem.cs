using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGem : MonoBehaviour {

    Rigidbody myRigidbody;
    

    // Use this for initialization
    void Start () {
        myRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (PlayerControl.playerState == PlayerState.dead)
            return;
        if (myRigidbody.position.y > 0.5f)
            myRigidbody.MovePosition(myRigidbody.position + Vector3.down * Time.fixedDeltaTime);
	}

    private void OnCollisionEnter(Collision collision)
    {
        PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
        if ( player != null)
        {
            player.CollectGem();
            Destroy(gameObject);
        }
    }
}
