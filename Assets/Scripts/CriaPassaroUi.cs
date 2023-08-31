using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriaPassaroUi : MonoBehaviour
{
    public GameObject[] passaros;

    void Start()
    {
        //A cada "x" tempo o pássaro será lançado
        InvokeRepeating("TiroPassaro", 2f, 2f);
    }

    void TiroPassaro()
    {   //Intanciar seus pássaros de forma aleatória na posição do GameObject em que você está colocando esse código
        Instantiate(passaros[Random.Range(0,3)], transform.position, Quaternion.identity);
    }
}
