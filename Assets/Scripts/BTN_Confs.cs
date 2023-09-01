using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTN_Confs : MonoBehaviour
{
    public static BTN_Confs instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
    }


    public bool liga = false;
    public Animator animaConf, animaEngre;

    public void ClickBTN()
    {
        liga = !liga;

        if (liga)
        {
            animaConf.Play("ConfAnim");
            animaEngre.Play("AnimaEngrenagem");
        }
        else
        {
            animaConf.Play("ConfAnimInvers");
            animaEngre.Play("AnimaEngrenagemInvers");
        }
    }
}
