using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineR : MonoBehaviour
{
    public Transform p2, p3;

    void Start()
    {

    }

    private void Update()
    {
        gameObject.GetComponent<LineRenderer>().SetPosition(0, transform.position);
        gameObject.GetComponent<LineRenderer>().SetPosition(1, p2.transform.position);
        gameObject.GetComponent<LineRenderer>().SetPosition(2, p3.transform.position);
    }
}
