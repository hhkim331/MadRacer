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

    //Test
    public Vector3 InputTestTarget { get; set; }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        InputAccel = Input.GetKey(KeyCode.W);
        InputBrake = Input.GetKey(KeyCode.S);
        InputSteer = Input.GetAxis("Horizontal");
        InputBoost = Input.GetKey(KeyCode.LeftShift);   //부스터

        //사격
        if (Input.GetMouseButtonDown(0)) InputFire = true;
        else if (Input.GetMouseButtonUp(0)) InputFire = false;
        //보조무기
        if (Input.GetMouseButtonDown(1)) InputSub = true;
        else if (Input.GetMouseButtonUp(1)) InputSub = false;
        InputShield = Input.GetKey(KeyCode.LeftControl);

        //Test
        if (InputFire)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Ground")))
                InputTestTarget = hit.point;
            else
                InputTestTarget = Vector3.zero;
        }
    }
}
