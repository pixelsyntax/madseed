using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour {

    static Transform playerCameraTransform;

	// Use this for initialization
	void Start () {

        if (playerCameraTransform == null)
            playerCameraTransform = Camera.main.transform;

	}
	
	// Update is called once per frame
	void Update () {

        Vector3 lookTarget = transform.position + ( transform.position - playerCameraTransform.position );
        lookTarget.y = transform.position.y;
        //transform.LookAt(lookTarget);
        transform.rotation = playerCameraTransform.rotation;

	}
}
