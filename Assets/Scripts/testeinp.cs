using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testeinp : MonoBehaviour
{
    public Animator animMenu, animE1, animE2, animE3;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Verifique se a tecla Space foi pressionada
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Define o parâmetro "PlayAnimation" como verdadeiro
            animMenu.SetBool("PlayAnimation", true);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            animE1.Play("Estrela1_animada");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            animE2.Play("Estrela2_animada");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            animE3.Play("Estrela3_animada");
        }
    }

    public void IniciarAnimacao()
    {
        animMenu.SetBool("PlayAniamtion", true);
    }
}
