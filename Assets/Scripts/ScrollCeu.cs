using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollCeu : MonoBehaviour
{
    public RawImage back;

    void Start()
    {
        back = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {

        back.uvRect = new Rect(0.01f * Time.time, 0, 1, 1);
    }
}
