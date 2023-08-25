using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAccelerometer : MonoBehaviour
{


    void Update()
    {
        transform.Translate(Input.acceleration.x, Input.acceleration.x, 0);
    }
}
