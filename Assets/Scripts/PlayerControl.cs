using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

    public static Vector3 playerPosition;
    public float handMovement;
    public float inertia;
    public float bobAmplitude;
    public Material matLeftHand;
    public Material matRightHand;
    public Texture2D textureHandOpen;
    public Texture2D textureHandClosed;
    public Texture2D textureHandWeak;
    public Texture2D textureHandStrong;
    public Transform transformLeftHand;
    public Transform transformRightHand;
    public GameObject protoShotWeak;
    public GameObject protoShotStrong;
    public int ammoWeak;
    public int ammoStrong;
    public Image fillBlue;
    public Image fillRed;
    public Image fillYellow;
    public int health;
    public int healthMax;

    Vector3 transformLeftHandHome;
    Vector3 transformRightHandHome;
    float bobTimer;
    float pitch;
    float heading;
    Vector2 momentum;
    Camera headCam;
    Rigidbody myRigidbody;
    public float cooldownDurationWeak;
    public float cooldownDurationStrong;
    public float reloadWeakDuration;
    float reloadWeak;
    float cooldownWeak;
    float cooldownStrong;
    bool showFlashWeak;
    bool showFlashStrong;
    float accuracy; //Less is better

	void Start () {

        headCam = GetComponentInChildren<Camera>();
        momentum = Vector2.zero;
        pitch = 0;
        heading = 0;
        myRigidbody = GetComponent<Rigidbody>();
        transformLeftHandHome = transformLeftHand.localPosition;
        transformRightHandHome = transformRightHand.localPosition;
        accuracy = 1f;
        health = healthMax;

	}

    private void Update()
    {

        fillBlue.fillAmount = ammoWeak / 100f;
        fillYellow.fillAmount = ammoStrong / 25f;
        fillRed.fillAmount = health / 10f;

    }

    void FixedUpdate () {

  
        cooldownWeak -= Time.fixedDeltaTime;
        cooldownStrong -= Time.fixedDeltaTime;
        accuracy = Mathf.Clamp01(accuracy - Time.fixedDeltaTime);
        //Look
        float deltaHeading = Input.GetAxis("LookHorizontal") * 3f;
        heading += deltaHeading;

        myRigidbody.rotation = Quaternion.Euler(0, heading, 0);

        //Move
        float deltaForward = Input.GetAxis("MoveVertical");
        float deltaSideways = Input.GetAxis("MoveHorizontal");
        momentum.x = Mathf.Lerp(momentum.x, deltaSideways, inertia);
        momentum.y = Mathf.Lerp(momentum.y, deltaForward, inertia);
        Vector3 deltaMove = new Vector3(momentum.x, 0, momentum.y);
        deltaMove = transform.TransformVector(deltaMove * 5f);
        myRigidbody.MovePosition(myRigidbody.position + deltaMove * Time.fixedDeltaTime);

        //Headbob
        bobTimer += deltaMove.magnitude * Time.fixedDeltaTime * 2f;
        headCam.transform.localPosition = new Vector3(0, Mathf.Sin(bobTimer) * bobAmplitude, 0);
        //Handsway
        transformLeftHand.localPosition = transformLeftHandHome + new Vector3(Mathf.Sin(bobTimer) * handMovement, Mathf.Abs(Mathf.Cos(bobTimer) * handMovement), 0);
        transformRightHand.localPosition = transformRightHandHome + new Vector3(Mathf.Sin(bobTimer) * handMovement, Mathf.Abs(Mathf.Cos(bobTimer) * handMovement), 0);
        //Update position for enemies to find us :o
        playerPosition = myRigidbody.position;

        if (Input.GetButton("LeftHand") || Input.GetAxis("LeftHandAnalogue") > 0.5f ) {
            ShootWeak();
        }
        else
        {
            matLeftHand.SetTexture("_MainTex", textureHandClosed);
            reloadWeak -= Time.fixedDeltaTime;
            if (reloadWeak <= 0 && ammoWeak < 100)
            {
                reloadWeak = reloadWeakDuration;
                ++ammoWeak;
            }
        }

        if (Input.GetButton("RightHand") || Input.GetAxis("RightHandAnalogue") > 0.5f )
        {
            ShootStrong();
        }
        else
        {
            matRightHand.SetTexture("_MainTex", textureHandClosed);
        }

    }

    void ShootWeak()
    {

        if (cooldownWeak > 0 || ammoWeak <= 0 )
        {
            matLeftHand.SetTexture("_MainTex", textureHandOpen);
            return;
        }
        --ammoWeak;
        cooldownWeak = cooldownDurationWeak;
        matLeftHand.SetTexture("_MainTex", textureHandWeak);
        GameObject shotObject = Instantiate(protoShotWeak);
        shotObject.transform.position = headCam.transform.position + transform.forward * Random.Range(0.4f, 0.6f) - transform.right * Random.Range(0.2f, 0.3f) - transform.up * Random.Range(0.15f, 0.3f);
        Shot shot = shotObject.GetComponent<Shot>();
        shot.SetVelocity(transform.forward * 10f + transform.right * Random.Range(-accuracy, accuracy) + transform.up * Random.Range(-accuracy, accuracy));
        accuracy += 0.125f;
    }

    void ShootStrong()
    {

        if (cooldownStrong > 0 || ammoStrong <= 0)
        {
            matRightHand.SetTexture("_MainTex", textureHandOpen);
            return;
        }
        matRightHand.SetTexture("_MainTex", textureHandStrong);
        cooldownStrong = cooldownDurationStrong;
        --ammoStrong;
        matRightHand.SetTexture("_MainTex", textureHandStrong);
        GameObject shotObject = Instantiate(protoShotStrong);
        shotObject.transform.position = headCam.transform.position + transform.forward * Random.Range(0.4f, 0.6f) + transform.right * Random.Range(0.2f, 0.3f) - transform.up * Random.Range(0.15f, 0.3f);
        Shot shot = shotObject.GetComponent<Shot>();
        shot.SetVelocity(transform.forward * 6f + transform.right * Random.Range(-0.2f, 0.2f) + transform.up * Random.Range(-0.2f, 0.2f));

    }
}
