using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchCount : MonoBehaviour
{
    public Text txt;

    void Update()
    {
        if(Input.touchCount > 0)
        {
            txt.text = Input.touchCount.ToString(); //Pega seus toques, se voc� bota 3 dedos na tela vai aparecer o n�mero 3 no texto na tela
        }
    }
}
