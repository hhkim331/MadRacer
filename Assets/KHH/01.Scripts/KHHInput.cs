using UnityEngine;

public class KHHInput : MonoBehaviour
{
    bool active = false;
    public bool Active
    {
        get { return active; }
        set
        {
            active = value;
            if (!active)
            {
                InputAccel = 0f;
                InputBrake = false;
                InputSteer = 0f;
                InputBoost = false;
                InputFire = false;
                InputGrip = false;
                InputShield = false;
                InputReturn = false;
            }
        }
    }

    //input
    public float InputAccel { get; set; }
    public bool InputBrake { get; set; }
    public float InputSteer { get; set; }
    public bool InputBoost { get; set; }
    public bool InputFire { get; set; }
    public bool InputGrip { get; set; }
    public bool InputShield { get; set; }
    public bool InputReturn { get; set; }

    // Update is called once per frame
    void Update()
    {
        InputAccel = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        InputBrake = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch) > 0.2f;
        //InputSteer = Input.GetAxis("Horizontal");
        float steer = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch).eulerAngles.z / 180f - 1f;
        if (steer < 0) steer = Mathf.Abs(steer) - 1f;
        else steer = 1f - Mathf.Abs(steer);
        InputSteer = steer;

        InputBoost = OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch);

        //사격
        InputFire = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.2f;
        //보조무기
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch)) InputGrip = true;
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch)) InputGrip = false;
        InputShield = OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch);
        InputReturn = OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch);
    }
}
