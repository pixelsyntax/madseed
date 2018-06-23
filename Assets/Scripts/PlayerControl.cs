using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public float inertia;
    public float bobAmplitude;

    float bobTimer;
    float pitch;
    float heading;
    Vector2 momentum;
    Camera headCam;
    Rigidbody myRigidbody;

	// Use this for initialization
	void Start () {

        headCam = GetComponentInChildren<Camera>();
        momentum = Vector2.zero;
        pitch = 0;
        heading = 0;
        myRigidbody = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        //Mouselook
        float deltaHeading = Input.GetAxis("LookHorizontal") * 3f;
        heading += deltaHeading;

        myRigidbody.rotation = Quaternion.Euler(0, heading, 0);

        //Movement
        float deltaForward = Input.GetAxis("MoveVertical");
        float deltaSideways = Input.GetAxis("MoveHorizontal");
        momentum.x = Mathf.Lerp(momentum.x, deltaSideways, inertia);
        momentum.y = Mathf.Lerp(momentum.y, deltaForward, inertia);
        Vector3 deltaMove = new Vector3(momentum.x, 0, momentum.y);
        deltaMove = transform.TransformVector(deltaMove * 5f);
        myRigidbody.MovePosition(myRigidbody.position + deltaMove * Time.fixedDeltaTime);

        bobTimer += deltaMove.magnitude * Time.fixedDeltaTime * 2f;
        headCam.transform.localPosition = new Vector3(0, Mathf.Sin(bobTimer) * bobAmplitude, 0);

	}
}
