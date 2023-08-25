using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchPhase : MonoBehaviour
{
    public Text txt;
    public Touch toque;

    void Update()
    {
        if(Input.touchCount > 0) //Verifica se estou tocando na tela
        {
            toque = Input.GetTouch(0); //Meu primeiro toque vai ser o índice 0

            switch(toque.phase)
            {
                case UnityEngine.TouchPhase.Began:
                    txt.text = "Began"; //Se eu colocar meu dedo na tela aparecerá esse texto
                    break; 
                case UnityEngine.TouchPhase.Moved:
                    txt.text = "Moved"; //Se eu movo meu dedo na tela aparecerá esse texto
                    break; 
                case UnityEngine.TouchPhase.Ended:
                    txt.text = "Ended"; //Se eu tirar meu dedo na tela aparecerá esse texto
                    break;
                case UnityEngine.TouchPhase.Canceled:
                    txt.text = "Canceled"; //Ex.: Quando estou tocando na tela e giro meu aparelho
                    break;
                case UnityEngine.TouchPhase.Stationary: 
                    txt.text = "Stationary"; //Se eu deixar meu dedo parado na tela aparecerá esse texto
                    break;  
            }
        }
    }
}
