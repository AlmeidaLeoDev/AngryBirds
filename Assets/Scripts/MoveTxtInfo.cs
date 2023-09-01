using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTxtInfo : MonoBehaviour
{
    private Vector3 pos;
    private RectTransform rt;
    private bool libera = false;
    private GameObject btnBlock, nomeGame;

    private RectTransform uiRect1, uiRect2;

    void Awake()
    {
        uiRect1 = (RectTransform) GameObject.FindWithTag("canvasback").GetComponent<RectTransform>(); 
        uiRect2 = (RectTransform) GameObject.FindWithTag("infotxt").GetComponent<RectTransform>();

        btnBlock = GameObject.FindWithTag("btnblock");
        btnBlock.SetActive(false);
        nomeGame = GameObject.FindWithTag("nomegame");

        rt = GetComponent<RectTransform>();
        pos = rt.anchoredPosition;
    }

    void Update()
    {
        if (libera)
        {
            transform.Translate(0,1 * Time.deltaTime, 0);
        }
        else
        {
            rt.anchoredPosition = pos;
        }

        if(!RectOverLap(uiRect1, uiRect2))
        {
            rt.anchoredPosition = pos;
        }
    }

    public void LiberaMov() 
    {
        btnBlock.SetActive(true);
        nomeGame.SetActive(false);
        libera = true;
        BTN_Confs.instance.liga = false;
        BTN_Confs.instance.animaConf.Play("ConfAnimInvers");
        BTN_Confs.instance.animaEngre.Play("AnimaEngrenagemInvers");
    }
    public void BlockMov()
    {
        btnBlock.SetActive(false);
        nomeGame.SetActive(false);
        libera = false;
    }

    bool RectOverLap (RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect (rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }
}
