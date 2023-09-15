using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHHVRCam : MonoBehaviour
{
    public Transform head;
    public Transform cam;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        yield return null;
        head.transform.localPosition = -cam.transform.localPosition;
    }

    public void GameEnd()
    {
        transform.position = new Vector3(0, 5000, 0);
    }
}
