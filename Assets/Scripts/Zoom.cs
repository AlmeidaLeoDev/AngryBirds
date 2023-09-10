using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public float orthoZoomSpeed = 0.5f;
    public bool liberaZoom;
    public int trava = 1;

    public bool um_click = false;
    public float tempoParaDuploCLick;
    public float delay;

    void Update()
    {
        if (GAMEMANAGER.instance.jogoComecou && GAMEMANAGER.instance.pausado == false)
        {
            //MOUSE
            if (Input.GetMouseButtonUp(0))
            {
                if (um_click == false)
                {
                    um_click = true;
                    tempoParaDuploCLick = Time.time; //Controle de tempo para clicar novamente e o zoom funcionar perfeitamente
                }
                else
                {
                    um_click = false;
                    liberaZoom = true;
                }
            }

            if (um_click == true)
            {
                if ((Time.time - tempoParaDuploCLick) > delay)
                {
                    um_click = false;
                }
            }
        }


        if(Camera.main.orthographicSize > 5 && trava == 1) 
        {
            if (liberaZoom)
            {
                Camera.main.orthographicSize -= orthoZoomSpeed;

                if(Camera.main.orthographicSize == 5)
                {
                    liberaZoom = false;
                    trava = 2;
                }
            }
        }
        else if(Camera.main.orthographicSize < 10 && trava == 2)
        {
            if (liberaZoom)
            {
                Camera.main.orthographicSize += orthoZoomSpeed;

                if (Camera.main.orthographicSize == 10)
                {
                    liberaZoom = false;
                    trava = 1;
                }
            }
        }
    }
}
