﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {
    public AudioSource audioOof;
    public AudioSource audioDeath;
    public AudioSource audioShotWeak;
    public AudioSource audioShotStrong;
    public AudioSource audioThunder;
    public AudioSource audioSlosh1;
    public AudioSource audioSlosh2;
    public AudioSource audioPickup;

    float sloshDistance;
    public static PlayerState playerState;
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
    public float invulnerableTime; //Invulnerable to everything except lightning
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
    public float gameTime;
    public Text textTime;

    public ColourOverlay overlayPain;
    public ColourOverlay overlayDeath;
    public ColourOverlay overlayPickup;
    public ColourOverlay overlayFadein;

    private void Awake()
    {
        playerState = PlayerState.alive;
    }

    void Start () {
        sloshDistance = 3.1f;
        overlayFadein.Go(1);
        headCam = GetComponentInChildren<Camera>();
        momentum = Vector2.zero;
        pitch = 0;
        heading = 0;
        myRigidbody = GetComponent<Rigidbody>();
        transformLeftHandHome = transformLeftHand.localPosition;
        transformRightHandHome = transformRightHand.localPosition;
        accuracy = 1f;
        health = healthMax;
        gameTime = 0;

	}

    private void Update()
    {

        if ( playerState == PlayerState.dead)
        {
            UpdateDead();
            return;
        }

        if (invulnerableTime > 0)
            invulnerableTime -= Time.deltaTime;

        fillBlue.fillAmount = ammoWeak / 100f;
        fillYellow.fillAmount = ammoStrong / 25f;
        fillRed.fillAmount = (float)health / (float)healthMax;
        gameTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        float splits = (gameTime - Mathf.FloorToInt(gameTime))*1000f;
        string timeString = string.Format("{0:#00}:{1:#00}:{2:#000}", minutes, seconds, splits);
        textTime.text = timeString;

    }

    void Die()
    {
        GetComponent<Collider>().enabled = false;
        overlayDeath.enabled = true;
        overlayDeath.Go(2f);
        playerState = PlayerState.dead;
        audioOof.Stop();
        audioDeath.Play();
        if ( TimesTracker.timesTracker != null )
            TimesTracker.timesTracker.SetPrevTime(gameTime);
    }

    void UpdateDead()
    {
        if (transform.position.y > 0.4f)
            transform.position = transform.position + Vector3.down * Time.deltaTime;
        else if (Input.GetButton("LeftHand") || Input.GetAxis("LeftHandAnalogue") > 0.5f)
        {
            SceneManager.LoadScene("Menu");
        }

    }

    void FixedUpdate () {

        if (playerState == PlayerState.dead)
            return;

        if (health <= 0)
            Die();

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
        if (deltaMove.magnitude > 1)
            deltaMove.Normalize();
        deltaMove = transform.TransformVector(deltaMove * 5f) * Time.fixedDeltaTime;
        sloshDistance += deltaMove.magnitude;
        if ( sloshDistance > 3)
        {
            PlaySlosh();
            sloshDistance -= 3f;
        }
            
        myRigidbody.MovePosition(myRigidbody.position + deltaMove);

        //Headbob
        bobTimer += deltaMove.magnitude * 2f;
        headCam.transform.localPosition = new Vector3(0, Mathf.Sin(bobTimer) * bobAmplitude, 0);
        //Handsway
        transformLeftHand.localPosition = transformLeftHandHome + new Vector3(Mathf.Sin(bobTimer) * handMovement, Mathf.Abs(Mathf.Cos(bobTimer) * handMovement), 0);
        transformRightHand.localPosition = transformRightHandHome + new Vector3(Mathf.Sin(bobTimer) * handMovement, Mathf.Abs(Mathf.Cos(bobTimer) * handMovement), 0);
        //Update position for enemies to find us :o
        playerPosition = transform.position;

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
        audioShotWeak.Play();
    }

    void PlaySlosh()
    {
        if (Random.value > 0.5f)
        {
            audioSlosh2.pitch = Random.Range(0.95f, 1.05f);
            audioSlosh2.Play();
        }
        else
        {
            audioSlosh1.pitch = Random.Range(0.95f, 1.05f);
            audioSlosh1.Play();
        }
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
        audioShotStrong.Play();
    }

    public void Zapped() //Smote by hot electric death
    {
        health = 0;
  
    }

    public void Bumped() //Collided with a skull etc
    {
        if (invulnerableTime > 0) //ignore if we're invulnerable
            return;

        overlayPain.Go(0.2f);

        invulnerableTime = 1f;
        health -= 25;
        audioOof.Play();
        
    }

    public void CollectGem()
    {
        ammoStrong = Mathf.Min(ammoStrong + 10, 25);
        health = Mathf.Min(health + 10, healthMax);
        overlayPickup.Go(0.1f);
        audioPickup.Play();
    }

    
}

public enum PlayerState
{
    alive,
    dead
}