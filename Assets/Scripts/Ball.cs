using System;
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
}
