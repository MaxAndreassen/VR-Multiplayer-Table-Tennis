using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float strength = 4;

    public float maxSpeed = 5f;

    public float minVelocity = 3;
    public float maxVelocity = 7;

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, maxSpeed);
    }

    void OnCollisionEnter (Collision other)
    {
        if (other.collider.CompareTag("bat"))
        {
            var speed = Mathf.Max(GetComponent<Rigidbody>().velocity.magnitude, minVelocity);
            var direction = Vector3.Reflect(GetComponent<Rigidbody>().velocity.normalized, other.contacts[0].normal);

            var newVelocity = direction * Mathf.Min(Mathf.Max(speed, minVelocity), maxVelocity);

            GetComponent<Rigidbody>().velocity = new Vector3(
                newVelocity.x,
                Mathf.Max(newVelocity.y, GetComponent<Rigidbody>().velocity.y),
                newVelocity.z
            );

            return;
        }

        Debug.Log(other.collider.name);
        
        GetComponent<Rigidbody>().AddForce(Vector3.up * strength);
    }
}
