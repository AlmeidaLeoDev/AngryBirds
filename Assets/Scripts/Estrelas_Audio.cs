using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estrelas_Audio : MonoBehaviour
{
    public AudioSource aSource;
    public AudioClip clip;
    public string nomeEstrela;

    public void TocaAudioEstrela()
    {
        if (!aSource.isPlaying)
        {
            aSource.clip = clip;
            aSource.Play(); 
        }
    }

    public void Verifica_Star()
    {
        switch(nomeEstrela)
        {
            case "Estrela1_win":
                GAMEMANAGER.instance.estrela1Fim = true; 
                break;
            case "Estrela2_win":
                GAMEMANAGER.instance.estrela2Fim = true;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
