using System.Collections;
using System.Collections.Generic;
using UnityEditor.DeviceSimulation;
using UnityEngine;
using UnityEngine.UI;

public class TouchPosition : MonoBehaviour
{
    public Text txt;
    public Touch toque;

    void Update()
    {
        if(Input.touchCount > 0)
        {
            toque = Input.GetTouch(0);

            if (toque.phase == UnityEngine.TouchPhase.Began)
            {
                if(toque.position.x > (Screen.width / 2)) //Se eu tocar do lado direito da minha tela aparecerá escrito "direita"
                {
                    txt.text = "Direita";
                }
                else
                {
                    txt.text = "Esquerda";
                }
            }
        }
    }
}
