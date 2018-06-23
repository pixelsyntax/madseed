using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

    public ShotType shotType;
    float age;

    private void FixedUpdate()
    {
        age += Time.fixedDeltaTime;
        if (age > 10f)
            Destroy(gameObject);
    }

    public void SetVelocity( Vector3 velocity)
    {
        GetComponent<Rigidbody>().velocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Hit something
        Destroy(gameObject);
    }
}

public enum ShotType
{
    shotWeak,
    shotStrong
}