using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLifecycle : MonoBehaviour {

    public int health;
    public GameObject prototypeSeed;
    public GameObject prototypeSapling;
    public GameObject prototypeYearling;
    public GameObject prototypeTree;
    public GameObject prototypeSkull;

    GameObject currentPhaseObject;
    PlantPhase currentPhase;
    public PlantPhase phase;
    float time;
    
    
	// Use this for initialization
	void Start () {
        currentPhase = PlantPhase.tree;
	}
	
	// Update is called once per frame
	void Update () {

        time += Time.deltaTime;

        if (currentPhase != phase)
            SetPhase(phase);

        switch(phase)
        {
            case PlantPhase.fallingSeed:
                transform.position += Vector3.down * Time.deltaTime * 3f;
                if (transform.position.y <= 0f)
                    SetPhase(PlantPhase.seed);
                break;

            case PlantPhase.seed:
                float y = Mathf.Sin(time*4f) * 0.1f;
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
                if (time >= 5f)
                    SetPhase(PlantPhase.sapling);
                break;

            case PlantPhase.sapling:
                if (time >= 10f)
                    SetPhase(PlantPhase.yearling);
                break;

            case PlantPhase.yearling:
                if (time >= 10f)
                    SetPhase(PlantPhase.tree);
                break;

            case PlantPhase.tree:
                if (time >= 20f)
                    SetPhase(PlantPhase.tree);
                break;

            default:
                return;

        }
	}

    public void SetPhase(PlantPhase phase)
    {

        time = 0;

        if (currentPhaseObject != null)
            Destroy(currentPhaseObject);
        this.phase = phase;

        switch (phase)
        {

            case PlantPhase.fallingSeed:
                currentPhaseObject = Instantiate(prototypeSeed);
                currentPhaseObject.transform.parent = transform;
                currentPhaseObject.transform.localPosition = new Vector3(0, 0.1f, 0);
                health = 10;
                break;

            case PlantPhase.seed:
                currentPhaseObject = Instantiate(prototypeSeed);
                currentPhaseObject.transform.parent = transform;
                currentPhaseObject.transform.localPosition = new Vector3(0, 0.1f, 0);
                health += 10;
                break;

            case PlantPhase.sapling:
                currentPhaseObject = Instantiate(prototypeSapling);
                currentPhaseObject.transform.parent = transform;
                currentPhaseObject.transform.localPosition = new Vector3(0, 0.5f, 0);
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                health += 20;
                break;

            case PlantPhase.yearling:
                currentPhaseObject = Instantiate(prototypeYearling);
                currentPhaseObject.transform.parent = transform;
                currentPhaseObject.transform.localPosition = new Vector3(0, 1f, 0);
                health += 30;
                break;

            case PlantPhase.tree:
                if (currentPhase != PlantPhase.tree)
                    health += 70;
                currentPhaseObject = Instantiate(prototypeTree);
                currentPhaseObject.transform.parent = transform;
                currentPhaseObject.transform.localPosition = new Vector3(0, 1.5f, 0);
                SpawnWave();
                break;

            default:
                return;
        }

        currentPhase = phase;
    }

    void SpawnWave()
    {
        for (int i = 0; i < 5; ++i)
        {
            float a = i * 360/5;
            GameObject skullObject = Instantiate(prototypeSkull);
            skullObject.transform.position = transform.position + new Vector3(Mathf.Sin(a)/6f, Random.Range(2, 3), Mathf.Cos(a)/6f);
            EnemySkull skull = skullObject.GetComponent<EnemySkull>();
            skull.Spawn();
        }
    }

    public void ReceiveWeakDamage( int damage )
    {
        //Don't care lul
    }

    public void ReceiveStrongDamage( int damage )
    {
        //oh no
        health -= damage;
        if (health <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}

public enum PlantPhase
{
    fallingSeed,
    seed,
    sapling,
    yearling,
    tree
}