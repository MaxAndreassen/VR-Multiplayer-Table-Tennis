using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public float speed = 1f;
    public XRNode inputSource;

    private XRRig rig;
    public Vector2 inputAxis;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        var device = InputDevices.GetDeviceAtXRNode(inputSource);

        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    private void FixedUpdate()
    {
        var headRot = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        
        var direction = headRot * new Vector3(inputAxis.x, 0, inputAxis.y);

        transform.Translate(direction * (speed * Time.fixedDeltaTime));
    }
}
