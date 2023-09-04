using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SalvaDados : MonoBehaviour
{
    //#4
    public Vector3 temp; //usada para armazenar a posi��o atual do objeto quando o jogo come�a

    void Start()
    {
        //#5  atribui a posi��o atual do objeto � vari�vel temp quando o jogo come�a
        temp = transform.position;
    }

    void Update()
    {
        //#3
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Salvar();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            LoadPos();
        }
    }

    void Salvar() //#2
    {
        BinaryFormatter bf = new BinaryFormatter(); //usada para serializar os dados em formato bin�rio
        //cria um arquivo no diret�rio persistente de dados do aplicativo Unity. O nome do arquivo � "posData"
        FileStream fs = File.Create(Application.persistentDataPath + "/posData");

        SaveClass s = new SaveClass();
        s.posx = transform.position.x; //Quero salvar a posi��o do meu GameObject

        bf.Serialize(fs, s);
        fs.Close();
    }

    void LoadPos() //#6
    {
        if(File.Exists(Application.persistentDataPath + "/posData"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/posData", FileMode.Open);

            SaveClass s = (SaveClass)bf.Deserialize(fs);
            fs.Close();

            temp.x = s.posx;
            transform.position = temp;
        }
        else
        {
            print("N�o Existe");
        }
    }
}

[Serializable] //#1
class SaveClass
{
    public float posx;
}