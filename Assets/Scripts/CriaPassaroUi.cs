using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriaPassaroUi : MonoBehaviour
{
    public GameObject[] passaros;

    void Start()
    {
        //A cada "x" tempo o p�ssaro ser� lan�ado
        InvokeRepeating("TiroPassaro", 2f, 2f);
    }

    void TiroPassaro()
    {   //Intanciar seus p�ssaros de forma aleat�ria na posi��o do GameObject em que voc� est� colocando esse c�digo
        Instantiate(passaros[Random.Range(0,3)], transform.position, Quaternion.identity);
    }
}
