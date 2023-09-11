using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHHInput : MonoBehaviour
{
    public static KHHInput instance;

    //input
    public bool InputAccel { get; set; }
    public bool InputBrake { get; set; }
    public float InputSteer { get; set; }
    public bool InputBoost { get; set; }
    public bool InputFire { get; set; }
    public bool InputSub { get; set; }
    public bool InputShield { get; set; }
    public Vector3 InputRightHandPos { get; set; }
    public Quaternion InputRightHandRot { get; set; }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        InputAccel = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.1f;
        InputBrake = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch) > 0.1f;
        //InputSteer = Input.GetAxis("Horizontal");
        float steer = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch).eulerAngles.z / 180f - 1f;
        if (steer < 0) steer = Mathf.Abs(steer) - 1f;
        else steer = 1f - Mathf.Abs(steer);
        InputSteer = steer;

        InputBoost = OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch);

        //사격
        InputFire = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.2f;
        //보조무기
        if (Input.GetMouseButtonDown(1)) InputSub = true;
        else if (Input.GetMouseButtonUp(1)) InputSub = false;
        InputShield = Input.GetKey(KeyCode.LeftControl);
    }
}
