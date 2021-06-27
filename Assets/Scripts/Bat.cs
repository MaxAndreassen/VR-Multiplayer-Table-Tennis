using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Bat : MonoBehaviour
{
    public float minVelocity = 1;
    public float maxVelocity = 20;
    
    public float hitThreshold = 0.5f;
    public float timeThresholdSeconds = 1f;

    public GameObject ball;

    public long lastHitInSeconds;

    public Transform hitPoint1;
    public Transform hitPoint2;
    public Transform centerPoint;

    public float power = 5f;
    
    DateTime? lastHitAt;

    private void Update()
    {
        if (Vector3.Distance(centerPoint.position, ball.transform.position) < hitThreshold)
        {
            if (lastHitAt == null || (DateTime.Now - lastHitAt.Value).TotalSeconds > timeThresholdSeconds)
            {
                lastHitAt = DateTime.Now;
                lastHitInSeconds = lastHitAt.Value.Ticks / 1000;

                var distanceFromHitPoint1 = Vector3.Distance(hitPoint1.position, ball.transform.position);
                var distanceFromHitPoint2 = Vector3.Distance(hitPoint2.position, ball.transform.position);

                if (distanceFromHitPoint1 < distanceFromHitPoint2)
                {
                    ball.GetComponent<Rigidbody>().velocity = hitPoint1.forward * power;
                }
                else
                {
                    ball.GetComponent<Rigidbody>().velocity = hitPoint2.forward * power;
                }
            }
        }

        Debug.DrawRay(centerPoint.position, ball.transform.position - transform.position, Color.red);

        Debug.DrawRay(centerPoint.position, hitPoint1.forward, Color.blue);
        
        Debug.DrawRay(centerPoint.position, hitPoint2.forward, Color.green);
    }
}
