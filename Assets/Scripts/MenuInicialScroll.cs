using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInicialScroll : MonoBehaviour
{
    public RawImage back, front;

    void Update()
    {
        //valor de x, valor de y, largura, altura
        back.uvRect = new Rect(0.01f * Time.time, 0, 1, 1); //velocidade de back
        front.uvRect = new Rect(0.03f * Time.time, 0, 1, 1); //velocidade de front
    }
}
